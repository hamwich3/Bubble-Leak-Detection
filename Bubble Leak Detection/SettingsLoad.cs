using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIS.Imaging;

namespace Bubble_Leak_Detection
{
    public partial class SettingsLoad : Form
    {
        bool model1OK = false;
        bool model2OK = false;
        bool PACOK = false;
        bool device1OK = false;
        bool device2OK = false;

        string model1SettingsPath;
        string model2SettingsPath;
        string PACSettingsPath;

        bool autostart = true;
        bool loading = false;

        object _lock = new object();

        bool okToClose = true;
        string dir = "";
        
        TensorObjectDetection LeakLocator;
        TensorClassificationModel LeakClassifier;
        ICImagingControl ic = new ICImagingControl();

        int pBarValue = 0;

        public SettingsLoad()
        {
            InitializeComponent();
            dir = Directory.GetCurrentDirectory();
            model1SettingsPath = Path.Combine(dir, "model1.path");
            model2SettingsPath = Path.Combine(dir, "model2.path");
            PACSettingsPath = Path.Combine(dir, "opto22.ip");
            LoadSettingsFiles();
            timer1.Start();
        }

        private void LoadSettingsFiles()
        {
            if (File.Exists(model1SettingsPath))
            {
                tbModel1.Text = File.ReadAllText(model1SettingsPath);
            }
            else
            {
                string path1 = Path.Combine(dir, "Tensorflow", "LeakLocator i3", "model.pb");
                File.WriteAllText(model1SettingsPath, path1);
                tbModel1.Text = path1;
            }

            if (File.Exists(model2SettingsPath))
            {
                tbModel2.Text = File.ReadAllText(model2SettingsPath);
            }
            else
            {
                string path2 = Path.Combine(dir, "Tensorflow", "LeakClassifier i3", "model.pb");
                File.WriteAllText(model2SettingsPath, path2);
                tbModel2.Text = path2;
            }

            if (File.Exists(PACSettingsPath))
            {
                tbIP.Text = File.ReadAllText(PACSettingsPath);
            }
            else
            {
                string ip = "192.168.0.200";
                File.WriteAllText(PACSettingsPath, ip);
                tbIP.Text = ip;
            }
        }

        private async Task<bool> PerformPreChecks()
        {
            if (tbModel1.Text == "" || !File.Exists(tbModel1.Text))
            {
                AddStatus("Leak Detector model.pb not found!");
                model1OK = false;
            }
            else model1OK = true;
            if (tbModel2.Text == "" || !File.Exists(tbModel2.Text))
            {
                AddStatus("Leak Classifier model.pb not found!");
                model2OK = false;
            }
            else model2OK = true;
            if (ic.Devices.Length < 2)
            {
                AddStatus($"{ic.Devices.Length} camera(s) detected!");
                device1OK = false;
                device2OK = false;
            }
            else
            {
                AddStatus($"Device detected: {ic.Devices[0].Name}");
                AddStatus($"Device detected: {ic.Devices[1].Name}");
                device1OK = true;
                device2OK = true;
            }
            PACOK = await PingPAC();
            return (model1OK && model2OK && PACOK && device1OK && device2OK);
        }

        private void LoadBubbleDetector()
        {
            this.Enabled = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool thread1complete = false;
            bool thread2complete = false;
            pBarValue = 10;
            Thread initLeakLocator = new Thread(() =>
            {
                AddStatus("Loading LeakLocator...");
                LeakLocator = new TensorObjectDetection();
                LeakLocator.LoadModel(tbModel1.Text);
                AddStatus($"Finished Loading LeakLocator in {sw.ElapsedMilliseconds}ms");
                lock (_lock) pBarValue += 30;
                thread1complete = true;
            });
            Thread initLeakClassifier = new Thread(() =>
            {
                AddStatus("Loading LeakClassifier...");
                LeakClassifier = new TensorClassificationModel();
                LeakClassifier.LoadModel(tbModel2.Text);
                AddStatus($"Finished Loading LeakClassifier in {sw.ElapsedMilliseconds}ms");
                lock (_lock) pBarValue += 30;
                thread2complete = true;
            });
            Thread waitforload = new Thread(() =>
            {
                while (!(thread1complete && thread2complete))
                {
                    Thread.Sleep(100);
                }
                lock (_lock) pBarValue = 100;
                Thread.Sleep(1000);
                initLeakLocator.Join();
                initLeakClassifier.Join();
                AddStatus("Opening Bubble Detector...");
                this.Invoke(new Action(() =>
                {
                    var app = new Form1(LeakLocator, LeakClassifier, tbIP.Text);
                    app.StartPosition = FormStartPosition.Manual;
                    try
                    {
                        var screenBounds = Screen.AllScreens[0].Bounds;
                        app.Bounds = screenBounds;
                    }
                    catch
                    {
                        //2nd monitor not connected?
                    }
                    app.Show();
                    this.Hide();
                }));
            });
            waitforload.Start();
            initLeakLocator.Start();
            initLeakClassifier.Start();
        }

        private void AddStatus(string statusMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    rtbStatus.AppendText(statusMessage);
                    rtbStatus.AppendText(Environment.NewLine);
                }));
            }
            else
            {
                rtbStatus.AppendText(statusMessage);
                rtbStatus.AppendText(Environment.NewLine);
            }
        }

        private async Task<bool> PingPAC()
        {
            Ping ping = new Ping();
            IPAddress ip;
            if (!IPAddress.TryParse(tbIP.Text, out ip))
            {
                AddStatus("PAC IPAddress not valid!");
                return false;
            }
            AddStatus("Pinging PAC...");
            okToClose = false;
            var result = await ping.SendPingAsync(ip);
            if (result.Status == IPStatus.Success)
            {
                AddStatus($"Pinged PAC {tbIP.Text} successfully");
                okToClose = true;
                return true;
            }
            else
            {
                AddStatus("PAC ping unsuccessful: " + result.Status.ToString());
                okToClose = true;
                return false;
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (loading) return;
            loading = true;
            bool load = await PerformPreChecks();
            if (load)
            {
                LoadBubbleDetector();
            }
            else
            {
                loading = false;
                MessageBox.Show("Could not load, check data");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(model1SettingsPath, tbModel1.Text);
            File.WriteAllText(model2SettingsPath, tbModel2.Text);
            File.WriteAllText(PACSettingsPath, tbIP.Text);
            cbEdit.Checked = false;
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            LoadSettingsFiles();
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            PingMessageBox();
        }

        private async void PingMessageBox()
        {
            btnPing.Text = "Pinging...";
            bool success = await PingPAC();
            if (success)
            {
                btnPing.Text = "Ping";
                MessageBox.Show("Ping Successful");
            }
            else
            {
                btnPing.Text = "Ping";
                MessageBox.Show("Ping Unsuccessful");
            }
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "tensorflow models (*.pb)|*.pb";
            fd.InitialDirectory = dir;
            string tf = Path.Combine(dir, "Tensorflow");
            if (Directory.Exists(tf)) fd.InitialDirectory = tf;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                tbModel1.Text = fd.FileName;
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "tensorflow models (*.pb)|*.pb";
            fd.InitialDirectory = dir;
            string tf = Path.Combine(dir, "Tensorflow");
            if (Directory.Exists(tf)) fd.InitialDirectory = tf;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                tbModel2.Text = fd.FileName;
            }
        }

        private void btnBrowse3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "tensorflow models (*.pb)|*.pb";
            fd.InitialDirectory = dir;
            string tf = Path.Combine(dir, "Tensorflow");
            if (Directory.Exists(tf)) fd.InitialDirectory = tf;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                tbModel3.Text = fd.FileName;
            }
        }

        private void cbEdit_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowse1.Enabled = cbEdit.Checked;
            btnBrowse2.Enabled = cbEdit.Checked;
            btnBrowse3.Enabled = cbEdit.Checked;
            btnSave.Enabled = cbEdit.Checked;
            btnUndo.Enabled = cbEdit.Checked;
            tbIP.Enabled = cbEdit.Checked;
            tbModel1.Enabled = cbEdit.Checked;
            tbModel2.Enabled = cbEdit.Checked;
            tbModel3.Enabled = cbEdit.Checked;
        }

        bool mouseHold = false;
        private void rtbStatus_MouseDown(object sender, MouseEventArgs e)
        {
            mouseHold = true;
        }

        private void rtbStatus_MouseUp(object sender, MouseEventArgs e)
        {
            mouseHold = false;
        }

        private void rtbStatus_TextChanged(object sender, EventArgs e)
        {
            if (!mouseHold)
            {
                rtbStatus.SelectionStart = rtbStatus.Text.Length;
                rtbStatus.ScrollToCaret();
            }
        }

        private void SettingsLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!okToClose) e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (_lock) progressBar1.Value = pBarValue;
        }

        private void SettingsLoad_Click(object sender, EventArgs e)
        {
            autostart = false;
        }

        private void SettingsLoad_Load(object sender, EventArgs e)
        {
            Action start = new Action(() =>
            {
                Thread.Sleep(10000);

                if (autostart)
                {
                    Invoke(new Action(() =>
                    {
                        btnLoad.PerformClick();
                    }));
                }
            });
            Task t = new Task(start);
            t.Start();
        }
    }
}
