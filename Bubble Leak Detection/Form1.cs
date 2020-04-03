using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TIS.Imaging;
using AForge.Vision.Motion;
using System.Linq;
using System.Threading.Tasks;

namespace Bubble_Leak_Detection
{
    public partial class Form1 : Form
    {
        string AssetPath { get; set; } = @"C:\Galiso Data\Bubble Detection";
        string Device1SettingsPath { get; set; }
        string Device2SettingsPath { get; set; }
        string ImagePath { get; set; }

        //Device 1
        Thread ImageProcessingThread1;
        //Device 2
        Thread ImageProcessingThread2;
        //Ends threads on close
        bool keepImageProcessingThreadsAlive = true;
        bool processImages1 = false;
        bool processImages2 = false;
        bool updateOverlay1 = false;
        bool updateOverlay2 = false;

        TensorObjectDetection LeakLocator;
        TensorClassificationModel LeakClassifier;

        TestPosition cyl1;
        TestPosition cyl2;
        TestPosition cyl3;
        TestPosition cyl4;
        TestPosition cyl5;
        TestPosition cyl6;
        TestPosition cyl7;
        TestPosition cyl8;

        Opto22Comm PAC;
        string PACIP;

        int threadDelay = 10;

        int frameIndex1 = 0;
        int frameIndex2 = 1;

        public Form1(TensorObjectDetection leak, TensorClassificationModel classifier, string pacip)
        {
            InitializeComponent();
            LeakLocator = leak;
            LeakClassifier = classifier;
            PACIP = pacip;
            this.TopMost = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAssets();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sectors.WriteROIs();
            if (PAC != null) PAC.CloseDispose();
            keepImageProcessingThreadsAlive = false;
            CloseDisposeCamera();
            e.Cancel = true;
        }

        private void CloseDisposeCamera()
        {
            new Thread(() =>
            {
                if (icImagingControl1.LiveVideoRunning) icImagingControl1.LiveStop();
                if (icImagingControl2.LiveVideoRunning) icImagingControl2.LiveStop();
                this.Invoke(new Action(() =>
                {
                    icImagingControl1.Dispose();
                    icImagingControl2.Dispose();
                }));
                if (ImageProcessingThread1 != null && ImageProcessingThread1.IsAlive)
                {
                    Console.WriteLine("join1");
                    ImageProcessingThread1.Join();
                }
                if (ImageProcessingThread2 != null && ImageProcessingThread2.IsAlive)
                {
                    Console.WriteLine("join2");

                    ImageProcessingThread2.Join();
                }
                Environment.Exit(0);
            }).Start();
        }

        private void LoadAssets()
        {
            BuildStrings();
            PAC = new Opto22Comm(AssetPath);
            InitTestPositions();
            LoadSettings();
            InitializeImaging(icImagingControl1);
            InitializeImaging(icImagingControl2);
            Sectors.Initialize();
            tbRadius.Text = Sectors.RegionRadius.ToString();
            StartCameras();
            StartImageThreads();
            PAC.StopTakeThreshold += PAC_StopTakeThreshold;
            PAC.StartTakeThreshold += PAC_StartTakeThreshold;
            PAC.StopTest += PAC_StopTest;
            PAC.StartTest += PAC_StartTest;
            PAC.TestCompleted += PAC_TestCompleted;
            icImagingControl1.ImageAvailable += icImagingControl1_ImageAvailable;
            icImagingControl2.ImageAvailable += icImagingControl2_ImageAvailable;
            icImagingControl1.OverlayUpdate += icImagingControl1_OverlayUpdate;
            icImagingControl2.OverlayUpdate += icImagingControl2_OverlayUpdate;
        }

        private void PAC_StartTest(object source, EventArgs e)
        {
            cyl1.Reset();
            cyl2.Reset();
            cyl3.Reset();
            cyl4.Reset();
            cyl5.Reset();
            cyl6.Reset();
            cyl7.Reset();
            cyl8.Reset();
            var s = (string[])source;
            cyl1.SerialText = s[0];
            cyl2.SerialText = s[1];
            cyl3.SerialText = s[2];
            cyl4.SerialText = s[3];
            cyl5.SerialText = s[4];
            cyl6.SerialText = s[5];
            cyl7.SerialText = s[6];
            cyl8.SerialText = s[7];

            cyl1.LotNoText = s[8];
            cyl2.LotNoText = s[9];
            cyl3.LotNoText = s[10];
            cyl4.LotNoText = s[11];
            cyl5.LotNoText = s[12];
            cyl6.LotNoText = s[13];
            cyl7.LotNoText = s[14];
            cyl8.LotNoText = s[15];
        }

        private void PAC_StartTakeThreshold(object source, EventArgs e)
        {
            cyl1.Reset();
            cyl2.Reset();
            cyl3.Reset();
            cyl4.Reset();
            cyl5.Reset();
            cyl6.Reset();
            cyl7.Reset();
            cyl8.Reset();
        }

        private void PAC_StopTest(object source, EventArgs e)
        {

        }

        private void PAC_StopTakeThreshold(object source, EventArgs e)
        {

        }

        private void PAC_TestCompleted(object source, EventArgs e)
        {
            cyl1.Fail = PAC.bFail1 == 1;
            cyl2.Fail = PAC.bFail2 == 1;
            cyl3.Fail = PAC.bFail3 == 1;
            cyl4.Fail = PAC.bFail4 == 1;
            cyl5.Fail = PAC.bFail5 == 1;
            cyl6.Fail = PAC.bFail6 == 1;
            cyl7.Fail = PAC.bFail7 == 1;
            cyl8.Fail = PAC.bFail8 == 1;
        }

        private void BuildStrings()
        {
            Device1SettingsPath = Path.Combine(AssetPath, "device1.dat");
            Device2SettingsPath = Path.Combine(AssetPath, "device2.dat");
            ImagePath = CheckPath(Path.Combine(AssetPath, "Images"));
        }

        private string CheckPath(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                return path;
            }
            else
            {
                MessageBox.Show(path + " not found! Exiting application...");
                Environment.Exit(0);
            }
            return null;
        }

        private void InitTestPositions()
        {
            cyl1 = new TestPosition();
            cyl2 = new TestPosition();
            cyl3 = new TestPosition();
            cyl4 = new TestPosition();
            cyl5 = new TestPosition();
            cyl6 = new TestPosition();
            cyl7 = new TestPosition();
            cyl8 = new TestPosition();
        }

        public void InitializeImaging(TIS.Imaging.ICImagingControl ic)
        {
            ic.LiveDisplayDefault = false;
            ic.LiveDisplayHeight = icImagingControl1.Height;
            ic.LiveDisplayWidth = icImagingControl1.Width;
            ic.LiveCaptureContinuous = true;
            ic.LiveDisplay = true;
            ic.OverlayBitmapPosition = TIS.Imaging.PathPositions.Display;
            ic.OverlayBitmapAtPath[PathPositions.Display].ColorMode = OverlayColorModes.Color;
        }

        private void icImagingControl1_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            if (e.bufferIndex == frameIndex2)
            {
                processImages1 = true;
            }
        }

        private void icImagingControl2_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            if (e.bufferIndex == frameIndex2)
            {
                processImages2 = true;
            }
        }

        private void StartImageThreads()
        {

            ImageProcessingThread1 = new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();
                while (keepImageProcessingThreadsAlive)
                {
                    //Process images every time ring buffer is iterated through completely
                    if (processImages1)
                    {
                        sw.Restart();
                        processImages1 = false;
                        Bitmap frame1 = null;
                        Bitmap frame2 = null;
                        Bitmap drop1 = null;
                        Bitmap drop2 = null;
                        Bitmap drop5 = null;
                        Bitmap drop6 = null;
                        int frame1Index = icImagingControl1.ImageRingBufferSize / 2;
                        int frame2Index = icImagingControl1.ImageRingBufferSize - 2;

                        lock (Sectors._lock1)
                        {
                            frame1 = (Bitmap)icImagingControl1.ImageBuffers[frameIndex1].Bitmap.Clone();
                            frame2 = (Bitmap)icImagingControl1.ImageBuffers[frameIndex2].Bitmap.Clone();
                            Sectors.ExtractRegionImages1(frame1, frame2);
                            drop1 = (Bitmap)Sectors.ROI1Diff.Clone();
                            drop2 = (Bitmap)Sectors.ROI2Diff.Clone();
                            drop5 = (Bitmap)Sectors.ROI5Diff.Clone();
                            drop6 = (Bitmap)Sectors.ROI6Diff.Clone();
                        }

                        cyl1.LeakObjectProbability = LeakLocator.DetectObjects(drop1, Sectors.Region1, out cyl1.detections)[0];
                        cyl2.LeakObjectProbability = LeakLocator.DetectObjects(drop2, Sectors.Region2, out cyl2.detections)[0];
                        cyl5.LeakObjectProbability = LeakLocator.DetectObjects(drop5, Sectors.Region5, out cyl5.detections)[0];
                        cyl6.LeakObjectProbability = LeakLocator.DetectObjects(drop6, Sectors.Region6, out cyl6.detections)[0];
                        drop1.Dispose();
                        drop2.Dispose();
                        drop5.Dispose();
                        drop6.Dispose();
                        if (cyl1.LeakLocated) cyl1.GetDetectionImages(frame1);
                        if (cyl2.LeakLocated) cyl2.GetDetectionImages(frame1);
                        if (cyl5.LeakLocated) cyl5.GetDetectionImages(frame1);
                        if (cyl6.LeakLocated) cyl6.GetDetectionImages(frame1);
                        if (cyl1.LeakLocated) cyl1.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl1.detectionImages[0]);
                        if (cyl2.LeakLocated) cyl2.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl2.detectionImages[0]);
                        if (cyl5.LeakLocated) cyl5.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl5.detectionImages[0]);
                        if (cyl6.LeakLocated) cyl6.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl6.detectionImages[0]);

                        if (PAC.fPercTargetPressure > 80 || PAC.fPressure > 500)
                        {
                            cyl1.TestActive = true;
                            cyl2.TestActive = true;
                            cyl5.TestActive = true;
                            cyl6.TestActive = true;
                            if (cyl1.Fail) PAC.pac.Write32BitIntegerVariable("bFail1", false, 1);
                            if (cyl2.Fail) PAC.pac.Write32BitIntegerVariable("bFail2", false, 1);
                            if (cyl5.Fail) PAC.pac.Write32BitIntegerVariable("bFail5", false, 1);
                            if (cyl6.Fail) PAC.pac.Write32BitIntegerVariable("bFail6", false, 1);
                        }
                        else
                        {
                            cyl1.TestActive = false;
                            cyl2.TestActive = false;
                            cyl5.TestActive = false;
                            cyl6.TestActive = false;
                        }
                        cyl1.DisplayText = cyl1.LeakObjectProbability.ToString("N2") + $" ({cyl1.LeakVerifiedCount})";
                        cyl2.DisplayText = cyl2.LeakObjectProbability.ToString("N2") + $" ({cyl2.LeakVerifiedCount})";
                        cyl5.DisplayText = cyl5.LeakObjectProbability.ToString("N2") + $" ({cyl5.LeakVerifiedCount})";
                        cyl6.DisplayText = cyl6.LeakObjectProbability.ToString("N2") + $" ({cyl6.LeakVerifiedCount})";
                        updateOverlay1 = true;
                        frame1.Dispose();
                        frame1 = null;
                        Thread.Sleep(threadDelay);
                    }
                }
            });
            ImageProcessingThread2 = new Thread(() =>
            {
                while (keepImageProcessingThreadsAlive)
                {
                    if (processImages2)
                    {
                        processImages2 = false;
                        Bitmap frame1 = null;
                        Bitmap frame2 = null;
                        Bitmap drop3 = null;
                        Bitmap drop4 = null;
                        Bitmap drop7 = null;
                        Bitmap drop8 = null;
                        lock (Sectors._lock1)
                        {
                            frame1 = (Bitmap)icImagingControl2.ImageBuffers[frameIndex1].Bitmap.Clone();
                            frame2 = (Bitmap)icImagingControl2.ImageBuffers[frameIndex2].Bitmap.Clone();
                            Sectors.ExtractRegionsDevice2(frame1, frame2);
                            drop3 = (Bitmap)Sectors.ROI3Diff.Clone();
                            drop4 = (Bitmap)Sectors.ROI4Diff.Clone();
                            drop7 = (Bitmap)Sectors.ROI7Diff.Clone();
                            drop8 = (Bitmap)Sectors.ROI8Diff.Clone();
                        }
                        //this.Invoke(new Action(() => { pictureBox1.Image = drop7; }));
                        cyl3.LeakObjectProbability = LeakLocator.DetectObjects(drop3, Sectors.Region3, out cyl3.detections)[0];
                        cyl4.LeakObjectProbability = LeakLocator.DetectObjects(drop4, Sectors.Region4, out cyl4.detections)[0];
                        cyl7.LeakObjectProbability = LeakLocator.DetectObjects(drop7, Sectors.Region7, out cyl7.detections)[0];
                        cyl8.LeakObjectProbability = LeakLocator.DetectObjects(drop8, Sectors.Region8, out cyl8.detections)[0];
                        drop3.Dispose();
                        drop4.Dispose();
                        drop7.Dispose();
                        drop8.Dispose();
                        if (cyl3.LeakLocated) cyl3.GetDetectionImages(frame1);
                        if (cyl4.LeakLocated) cyl4.GetDetectionImages(frame1);
                        if (cyl7.LeakLocated) cyl7.GetDetectionImages(frame1);
                        if (cyl8.LeakLocated) cyl8.GetDetectionImages(frame1);

                        if (cyl3.LeakLocated) cyl3.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl3.detectionImages[0]);
                        if (cyl4.LeakLocated) cyl4.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl4.detectionImages[0]);
                        if (cyl7.LeakLocated) cyl7.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl7.detectionImages[0]);
                        if (cyl8.LeakLocated) cyl8.LeakClassificationProbability = LeakClassifier.ClassifyImage(cyl8.detectionImages[0]);

                        if (PAC.fPercTargetPressure > 80 || PAC.fPressure > 500)
                        {
                            cyl3.TestActive = true;
                            cyl4.TestActive = true;
                            cyl7.TestActive = true;
                            cyl8.TestActive = true;
                            if (cyl3.Fail) PAC.pac.Write32BitIntegerVariable("bFail3", false, 1);
                            if (cyl4.Fail) PAC.pac.Write32BitIntegerVariable("bFail4", false, 1);
                            if (cyl7.Fail) PAC.pac.Write32BitIntegerVariable("bFail7", false, 1);
                            if (cyl8.Fail) PAC.pac.Write32BitIntegerVariable("bFail8", false, 1);
                        }
                        else
                        {
                            cyl3.TestActive = false;
                            cyl4.TestActive = false;
                            cyl7.TestActive = false;
                            cyl8.TestActive = false;
                        }

                        cyl3.DisplayText = cyl3.LeakObjectProbability.ToString("N2") + $" ({cyl3.LeakVerifiedCount})";
                        cyl4.DisplayText = cyl4.LeakObjectProbability.ToString("N2") + $" ({cyl4.LeakVerifiedCount})";
                        cyl7.DisplayText = cyl7.LeakObjectProbability.ToString("N2") + $" ({cyl7.LeakVerifiedCount})";
                        cyl8.DisplayText = cyl8.LeakObjectProbability.ToString("N2") + $" ({cyl8.LeakVerifiedCount})";
                        updateOverlay2 = true;
                        frame1.Dispose();
                        frame1 = null;
                        Thread.Sleep(threadDelay);
                    }
                }
            });
            ImageProcessingThread1.Start();
            ImageProcessingThread2.Start();
        }

        //Write Video
        private void button1_Click(object sender, EventArgs e)
        {
            WriteAvi WriteAviDlg = new WriteAvi(icImagingControl1);
            WriteAviDlg.ShowDialog();
            WriteAviDlg.Dispose();
            WriteAviDlg = null;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            icImagingControl1.ShowPropertyDialog();
            icImagingControl1.SaveDeviceStateToFile(Device1SettingsPath);
        }

        private void LoadSettings()
        {
            icImagingControl1.Device = icImagingControl1.Devices[0];
            icImagingControl2.Device = icImagingControl2.Devices[1];
            if (File.Exists(Device1SettingsPath))
            {
                try
                {
                    icImagingControl1.LoadDeviceStateFromFile(Device1SettingsPath, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    icImagingControl1.ShowPropertyDialog();
                    icImagingControl1.SaveDeviceStateToFile(Device1SettingsPath);
                }
            }
            else
            {
                icImagingControl1.ShowPropertyDialog();
                icImagingControl1.SaveDeviceStateToFile(Device1SettingsPath);
            }
            if (File.Exists(Device2SettingsPath))
            {
                try
                {
                    icImagingControl2.LoadDeviceStateFromFile(Device2SettingsPath, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    icImagingControl2.ShowPropertyDialog();
                    icImagingControl2.SaveDeviceStateToFile(Device2SettingsPath);
                }
            }
            else
            {
                icImagingControl2.ShowPropertyDialog();
                icImagingControl2.SaveDeviceStateToFile(Device2SettingsPath);
            }
        }

        private void SetupOverlay1(TIS.Imaging.OverlayBitmap overlay)
        {
            overlay.Enable = true;
            // Set magenta as dropout color.
            overlay.DropOutColor = Color.Magenta;
            // Fill the overlay bitmap with the dropout color.
            overlay.Fill(overlay.DropOutColor);

            overlay.Font = new Font("Consolas Bold", 28);
            overlay.FontTransparent = true;

            var g = overlay.GetGraphics();
            Pen noleak = new Pen(Color.Green, 4);
            Pen leak = new Pen(Color.Red, 4);
            Pen highlight = new Pen(Color.Orange, 4);
            var textColor = Color.Black;

            //g.DrawRectangle(cyl1.Fail ? leak : noleak, Sectors.Region1);
            //g.DrawRectangle(cyl2.Fail ? leak : noleak, Sectors.Region2);
            //g.DrawRectangle(cyl5.Fail ? leak : noleak, Sectors.Region5);
            //g.DrawRectangle(cyl6.Fail ? leak : noleak, Sectors.Region6);

            //g.DrawEllipse(cyl1.Fail ? leak : noleak, Sectors.Region1);
            //g.DrawEllipse(cyl2.Fail ? leak : noleak, Sectors.Region2);
            //g.DrawEllipse(cyl5.Fail ? leak : noleak, Sectors.Region5);
            //g.DrawEllipse(cyl6.Fail ? leak : noleak, Sectors.Region6);

            g.DrawEllipse(cyl1.Fail ? leak : noleak, Sectors.Region1.X + (int)(Sectors.Region1.Width * 0.5) - Sectors.RegionRadius, Sectors.Region1.Y + (int)(Sectors.Region1.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);
            g.DrawEllipse(cyl2.Fail ? leak : noleak, Sectors.Region2.X + (int)(Sectors.Region2.Width * 0.5) - Sectors.RegionRadius, Sectors.Region2.Y + (int)(Sectors.Region2.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);
            g.DrawEllipse(cyl5.Fail ? leak : noleak, Sectors.Region5.X + (int)(Sectors.Region5.Width * 0.5) - Sectors.RegionRadius, Sectors.Region5.Y + (int)(Sectors.Region5.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);
            g.DrawEllipse(cyl6.Fail ? leak : noleak, Sectors.Region6.X + (int)(Sectors.Region6.Width * 0.5) - Sectors.RegionRadius, Sectors.Region6.Y + (int)(Sectors.Region6.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);

            if (cyl1.TestActive && cyl1.detections != null && cyl1.LeakLocated) g.DrawRectangle(cyl1.LeakVerified ? leak : highlight, cyl1.detections[0]);
            if (cyl2.TestActive && cyl2.detections != null && cyl2.LeakLocated) g.DrawRectangle(cyl2.LeakVerified ? leak : highlight, cyl2.detections[0]);
            if (cyl5.TestActive && cyl5.detections != null && cyl5.LeakLocated) g.DrawRectangle(cyl5.LeakVerified ? leak : highlight, cyl5.detections[0]);
            if (cyl6.TestActive && cyl6.detections != null && cyl6.LeakLocated) g.DrawRectangle(cyl6.LeakVerified ? leak : highlight, cyl6.detections[0]);

            overlay.DrawText(textColor, Sectors.Region1.X + 3, Sectors.Region1.Y + 10 + (Sectors.Region1.Height), cyl1.DisplayText);
            overlay.DrawText(textColor, Sectors.Region2.X + 3, Sectors.Region2.Y + 10 + (Sectors.Region2.Height), cyl2.DisplayText);
            overlay.DrawText(textColor, Sectors.Region5.X + 3, Sectors.Region5.Y + 10, cyl5.DisplayText);
            overlay.DrawText(textColor, Sectors.Region6.X + 3, Sectors.Region6.Y + 10, cyl6.DisplayText);
            overlay.DrawText(textColor, Sectors.Region1.X + 3, Sectors.Region1.Y - 35 + (Sectors.Region1.Height), cyl1.SerialText);
            overlay.DrawText(textColor, Sectors.Region2.X + 3, Sectors.Region2.Y - 35 + (Sectors.Region2.Height), cyl2.SerialText);
            overlay.DrawText(textColor, Sectors.Region5.X + 3, Sectors.Region5.Y - 35, cyl5.SerialText);
            overlay.DrawText(textColor, Sectors.Region6.X + 3, Sectors.Region6.Y - 35, cyl6.SerialText);
            var w = icImagingControl1.ImageActiveBuffer.Bitmap.Width;
            var h = icImagingControl1.ImageActiveBuffer.Bitmap.Height;
            g.DrawLine(new Pen(Color.White, 5), 0, (h / 2.0f), w, (h / 2.0f));
            g.DrawLine(new Pen(Color.White, 5), (w / 2.0f), 0, (w / 2.0f), h);
            overlay.ReleaseGraphics(g);
        }

        private void SetupOverlay2(TIS.Imaging.OverlayBitmap overlay)
        {
            overlay.Enable = true;
            // Set magenta as dropout color.
            overlay.DropOutColor = Color.Magenta;
            // Fill the overlay bitmap with the dropout color.
            overlay.Fill(overlay.DropOutColor);

            overlay.Font = new Font("Consolas Bold", 28);
            overlay.FontTransparent = true;

            var g = overlay.GetGraphics();
            Pen noleak = new Pen(Color.Green, 4);
            Pen leak = new Pen(Color.Red, 4);
            Pen highlight = new Pen(Color.Orange, 4);
            var textColor = Color.Black;

            //g.DrawRectangle(cyl3.Fail ? leak : noleak, Sectors.Region3);
            //g.DrawRectangle(cyl4.Fail ? leak : noleak, Sectors.Region4);
            //g.DrawRectangle(cyl7.Fail ? leak : noleak, Sectors.Region7);
            //g.DrawRectangle(cyl8.Fail ? leak : noleak, Sectors.Region8);

            //g.DrawEllipse(cyl3.Fail ? leak : noleak, Sectors.Region3);
            //g.DrawEllipse(cyl4.Fail ? leak : noleak, Sectors.Region4);
            //g.DrawEllipse(cyl7.Fail ? leak : noleak, Sectors.Region7);
            //g.DrawEllipse(cyl8.Fail ? leak : noleak, Sectors.Region8);

            g.DrawEllipse(cyl3.Fail ? leak : noleak, Sectors.Region3.X + (int)(Sectors.Region3.Width * 0.5) - Sectors.RegionRadius, Sectors.Region3.Y + (int)(Sectors.Region3.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);
            g.DrawEllipse(cyl4.Fail ? leak : noleak, Sectors.Region4.X + (int)(Sectors.Region4.Width * 0.5) - Sectors.RegionRadius, Sectors.Region4.Y + (int)(Sectors.Region4.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);
            g.DrawEllipse(cyl7.Fail ? leak : noleak, Sectors.Region7.X + (int)(Sectors.Region7.Width * 0.5) - Sectors.RegionRadius, Sectors.Region7.Y + (int)(Sectors.Region7.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);
            g.DrawEllipse(cyl8.Fail ? leak : noleak, Sectors.Region8.X + (int)(Sectors.Region8.Width * 0.5) - Sectors.RegionRadius, Sectors.Region8.Y + (int)(Sectors.Region8.Height * 0.5) - Sectors.RegionRadius, Sectors.RegionRadius * 2, Sectors.RegionRadius * 2);


            if (cyl3.TestActive && cyl3.detections != null && cyl3.LeakLocated) g.DrawRectangle(cyl3.LeakVerified ? leak : highlight, cyl3.detections[0]);
            if (cyl4.TestActive && cyl4.detections != null && cyl4.LeakLocated) g.DrawRectangle(cyl4.LeakVerified ? leak : highlight, cyl4.detections[0]);
            if (cyl7.TestActive && cyl7.detections != null && cyl7.LeakLocated) g.DrawRectangle(cyl7.LeakVerified ? leak : highlight, cyl7.detections[0]);
            if (cyl8.TestActive && cyl8.detections != null && cyl8.LeakLocated) g.DrawRectangle(cyl8.LeakVerified ? leak : highlight, cyl8.detections[0]);

            overlay.DrawText(textColor, Sectors.Region3.X + 3, Sectors.Region3.Y + 10 + (Sectors.Region3.Height), cyl3.DisplayText);
            overlay.DrawText(textColor, Sectors.Region4.X + 3, Sectors.Region4.Y + 10 + (Sectors.Region4.Height), cyl4.DisplayText);
            overlay.DrawText(textColor, Sectors.Region7.X + 3, Sectors.Region7.Y + 10, cyl7.DisplayText);
            overlay.DrawText(textColor, Sectors.Region8.X + 3, Sectors.Region8.Y + 10, cyl8.DisplayText);
            overlay.DrawText(textColor, Sectors.Region3.X + 3, Sectors.Region3.Y - 35 + (Sectors.Region3.Height), cyl3.SerialText);
            overlay.DrawText(textColor, Sectors.Region4.X + 3, Sectors.Region4.Y - 35 + (Sectors.Region4.Height), cyl4.SerialText);
            overlay.DrawText(textColor, Sectors.Region7.X + 3, Sectors.Region7.Y - 35, cyl7.SerialText);
            overlay.DrawText(textColor, Sectors.Region8.X + 3, Sectors.Region8.Y - 35, cyl8.SerialText);
            var w = icImagingControl1.ImageActiveBuffer.Bitmap.Width;
            var h = icImagingControl1.ImageActiveBuffer.Bitmap.Height;
            g.DrawLine(new Pen(Color.White, 5), 0, (h / 2.0f), w, (h / 2.0f));
            g.DrawLine(new Pen(Color.White, 5), (w / 2.0f), 0, (w / 2.0f), h);
            overlay.ReleaseGraphics(g);
        }

        bool mouseHold1 = false;
        bool mouseHold2 = false;
        bool upperLeft = false;
        bool upperRight = false;
        bool lowerLeft = false;
        bool lowerRight = false;
        private void icImagingControl1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseHold1 = true;
            mouseHold2 = false;
            int x = e.X * Sectors.ImageSize.Width / icImagingControl1.Width;
            int y = e.Y * Sectors.ImageSize.Height / icImagingControl1.Height;
            SetQuadrant(x, y);
            ChangeROI(x, y);
        }

        private void icImagingControl2_MouseDown(object sender, MouseEventArgs e)
        {
            mouseHold1 = false;
            mouseHold2 = true;
            int x = e.X * Sectors.ImageSize.Width / icImagingControl1.Width;
            int y = e.Y * Sectors.ImageSize.Height / icImagingControl1.Height;
            SetQuadrant(x, y);
            ChangeROI(x, y);
        }

        private void SetQuadrant(double x, double y)
        {
            if (x < (Sectors.ImageSize.Width / 2) && y < (Sectors.ImageSize.Height / 2))
            {
                upperLeft = true;
            }
            if (x >= (Sectors.ImageSize.Width / 2) && y < (Sectors.ImageSize.Height / 2))
            {
                upperRight = true;
            }
            if (x < (Sectors.ImageSize.Width / 2) && y >= (Sectors.ImageSize.Height / 2))
            {
                lowerLeft = true;
            }
            if (x >= (Sectors.ImageSize.Width / 2) && y >= (Sectors.ImageSize.Height / 2))
            {
                lowerRight = true;
            }
        }

        private void icImagingControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseHold1 || mouseHold2)
            {
                int x = e.X * Sectors.ImageSize.Width / icImagingControl1.Width;
                int y = e.Y * Sectors.ImageSize.Height / icImagingControl1.Height;
                ChangeROI(x, y);
            }
        }

        private void icImagingControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseHold1 = false;
            mouseHold2 = false;
            upperLeft = false;
            upperRight = false;
            lowerLeft = false;
            lowerRight = false;
        }

        private void ChangeROI(double x, double y)
        {
            double halfWidth = (Sectors.ImageSize.Width / 2.0);
            double halfHeight = (Sectors.ImageSize.Height / 2.0);

            //get corner from center
            double x0 = 0;
            double y0 = 0;

            if (upperLeft)
            {
                //constrain left
                x = Math.Max(x, 0);
                //constrain right
                x = Math.Min(x, halfWidth);
                //constrain top
                y = Math.Max(y, 0);
                //constrain bottom
                y = Math.Min(y, halfHeight);
                //get corner from center
                x0 = x - (Sectors.RegionSize.Width / 2.0);
                y0 = y - (Sectors.RegionSize.Height / 2.0);

                if (mouseHold1) Sectors.Region1 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
                if (mouseHold2) Sectors.Region3 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
            }
            if (upperRight)
            {
                //constrain left
                x = Math.Max(x, halfWidth);
                //constrain right
                x = Math.Min(x, Sectors.ImageSize.Width);
                //constrain top
                y = Math.Max(y, 0);
                //constrain bottom
                y = Math.Min(y, (Sectors.ImageSize.Height / 2));
                //get corner from center
                x0 = x - (Sectors.RegionSize.Width / 2.0);
                y0 = y - (Sectors.RegionSize.Height / 2.0);
                if (mouseHold1) Sectors.Region2 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
                if (mouseHold2) Sectors.Region4 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);

            }
            if (lowerLeft)
            {
                //constrain right
                x = Math.Max(x, 0);
                //constrain right
                x = Math.Min(x, (Sectors.ImageSize.Width / 2));
                //constrain top
                y = Math.Max(y, halfHeight);
                //constrain bottom
                y = Math.Min(y, Sectors.ImageSize.Height);
                //get corner from center
                x0 = x - (Sectors.RegionSize.Width / 2.0);
                y0 = y - (Sectors.RegionSize.Height / 2.0);
                if (mouseHold1) Sectors.Region5 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
                if (mouseHold2) Sectors.Region7 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
            }
            if (lowerRight)
            {
                //constrain left
                x = Math.Max(x, halfWidth);
                //constrain right
                x = Math.Min(x, Sectors.ImageSize.Width);
                //constrain top
                y = Math.Max(y, halfHeight);
                //constrain bottom
                y = Math.Min(y, Sectors.ImageSize.Height);
                //get corner from center
                x0 = x - (Sectors.RegionSize.Width / 2.0);
                y0 = y - (Sectors.RegionSize.Height / 2.0);
                if (mouseHold1) Sectors.Region6 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
                if (mouseHold2) Sectors.Region8 = new Rectangle((int)x0, (int)y0, Sectors.RegionSize.Width, Sectors.RegionSize.Height);

            }
            if (mouseHold1) updateOverlay1 = true;
            if (mouseHold2) updateOverlay2 = true;
        }

        private Rectangle ResizeRectOnCenter(Rectangle original)
        {
            //get center x,y
            int centerX = original.X + (original.Width / 2);
            int centerY = original.Y + (original.Height / 2);
            int newX = centerX - (Sectors.RegionSize.Width / 2);
            int newY = centerY - (Sectors.RegionSize.Height / 2);
            return new Rectangle(newX, newY, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
        }

        private Rectangle ResizeRectOnCenter(Rectangle[] original)
        {
            //get center x,y
            int centerX = 0;
            int centerY = 0;
            foreach (var r in original)
            {
                centerX += r.X + (r.Width / 2);
                centerY += r.Y + (r.Height / 2);
            }
            centerX = centerX / original.Length;
            centerY = centerY / original.Length;
            int newX = centerX - (Sectors.RegionSize.Width / 2);
            int newY = centerY - (Sectors.RegionSize.Height / 2);
            return new Rectangle(newX, newY, Sectors.RegionSize.Width, Sectors.RegionSize.Height);
        }

        // Show/hide controls for capturing images
        private void showCaptureControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gbCaptureImages.Visible)
            {
                gbCaptureImages.Visible = false;
                showCaptureControlsToolStripMenuItem.Text = "Show Capture Controls";
            }
            else
            {
                gbCaptureImages.Visible = true;
                showCaptureControlsToolStripMenuItem.Text = "Hide Capture Controls";
            }
        }

        private void rbSector_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSector.Checked)
            {
                tbSector1.Text = "Sector1";
                tbSector2.Text = "Sector2";
                tbSector3.Text = "Sector3";
                tbSector4.Text = "Sector4";
                tbSector5.Text = "Sector5";
                tbSector6.Text = "Sector6";
                tbSector7.Text = "Sector7";
                tbSector8.Text = "Sector8";
            }
            else
            {
                tbSector1.Text = "dt_ROI1";
                tbSector2.Text = "dt_ROI2";
                tbSector3.Text = "dt_ROI3";
                tbSector4.Text = "dt_ROI4";
                tbSector5.Text = "dt_ROI5";
                tbSector6.Text = "dt_ROI6";
                tbSector7.Text = "dt_ROI7";
                tbSector8.Text = "dt_ROI8";
            }
        }

        private void btnCaptureSingle_Click(object sender, EventArgs e)
        {
            SaveImages();
        }

        // For producing unique image file names
        int imgN1 = 0;
        int imgN2 = 0;
        int imgN3 = 0;
        int imgN4 = 0;
        int imgN5 = 0;
        int imgN6 = 0;
        int imgN7 = 0;
        int imgN8 = 0;
        private void SaveImages()
        {

            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }

            string img1 = tbSector1.Text;
            string img2 = tbSector2.Text;
            string img3 = tbSector3.Text;
            string img4 = tbSector4.Text;
            string img5 = tbSector5.Text;
            string img6 = tbSector6.Text;
            string img7 = tbSector7.Text;
            string img8 = tbSector8.Text;

            string[] names = { img1, img2, img3, img4, img5, img6, img7, img8 };
            int[] nums = { imgN1, imgN2, imgN3, imgN4, imgN5, imgN6, imgN7, imgN8 };

            //iterate over file names
            for (int i = 0; i < 8; i++)
            {
                string fileName = names[i] + "_" + nums[i] + ".bmp";
                while (File.Exists(Path.Combine(ImagePath, fileName)))
                {
                    nums[i]++;
                    fileName = names[i] + "_" + nums[i] + ".bmp";
                }
                names[i] = fileName;
            }
            var a = new Action(() =>
            {
                lock (Sectors._lock1)
                {
                    lock (Sectors._lock2)
                    {
                        if (rbSector.Checked)
                        {
                            if (tbSector1.Text != "") Sectors.Quadrant1.Save(Path.Combine(ImagePath, names[0]));
                            if (tbSector2.Text != "") Sectors.Quadrant2.Save(Path.Combine(ImagePath, names[1]));
                            if (tbSector3.Text != "") Sectors.Quadrant3.Save(Path.Combine(ImagePath, names[2]));
                            if (tbSector4.Text != "") Sectors.Quadrant4.Save(Path.Combine(ImagePath, names[3]));
                            if (tbSector5.Text != "") Sectors.Quadrant5.Save(Path.Combine(ImagePath, names[4]));
                            if (tbSector6.Text != "") Sectors.Quadrant6.Save(Path.Combine(ImagePath, names[5]));
                            if (tbSector7.Text != "") Sectors.Quadrant7.Save(Path.Combine(ImagePath, names[6]));
                            if (tbSector8.Text != "") Sectors.Quadrant8.Save(Path.Combine(ImagePath, names[7]));
                        }
                        else
                        {
                            Directory.CreateDirectory(Path.Combine(ImagePath, "ROIDIFF"));
                            Directory.CreateDirectory(Path.Combine(ImagePath, "ROIDropDIFF"));
                            Directory.CreateDirectory(Path.Combine(ImagePath, "boxes"));
                            try
                            {
                                if (tbSector1.Text != "")
                                {
                                    Sectors.ROI1Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[0]));
                                    Sectors.ROI1DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[0]));
                                    Sectors.ROI1.Save(Path.Combine(ImagePath, names[0]));
                                    //cyl1.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[0]));
                                }
                                if (tbSector2.Text != "")
                                {
                                    Sectors.ROI2.Save(Path.Combine(ImagePath, names[1]));
                                    Sectors.ROI2Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[1]));
                                    Sectors.ROI2DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[1]));
                                    //cyl2.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[1]));

                                }
                                if (tbSector3.Text != "")
                                {
                                    Sectors.ROI3.Save(Path.Combine(ImagePath, names[2]));
                                    Sectors.ROI3DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[2]));
                                    Sectors.ROI3Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[2]));
                                    //cyl3.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[2]));

                                }
                                if (tbSector4.Text != "")
                                {
                                    Sectors.ROI4.Save(Path.Combine(ImagePath, names[3]));
                                    Sectors.ROI4DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[3]));
                                    Sectors.ROI4Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[3]));
                                    //cyl4.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[3]));

                                }
                                if (tbSector5.Text != "")
                                {
                                    Sectors.ROI5.Save(Path.Combine(ImagePath, names[4]));
                                    Sectors.ROI5DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[4]));
                                    Sectors.ROI5Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[4]));
                                    //cyl5.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[4]));

                                }
                                if (tbSector6.Text != "")
                                {
                                    Sectors.ROI6.Save(Path.Combine(ImagePath, names[5]));
                                    Sectors.ROI6DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[5]));
                                    Sectors.ROI6Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[5]));
                                    //cyl6.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[5]));

                                }
                                if (tbSector7.Text != "")
                                {
                                    Sectors.ROI7.Save(Path.Combine(ImagePath, names[6]));
                                    Sectors.ROI7DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[6]));
                                    Sectors.ROI7Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[6]));
                                    //cyl7.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[6]));

                                }
                                if (tbSector8.Text != "")
                                {
                                    Sectors.ROI8.Save(Path.Combine(ImagePath, names[7]));
                                    Sectors.ROI8DropDiff.Save(Path.Combine(ImagePath, "ROIDropDIFF", "drop_" + names[7]));
                                    Sectors.ROI8Diff.Save(Path.Combine(ImagePath, "ROIDIFF", "diff_" + names[7]));
                                    //cyl8.detectionImages[0]?.Save(Path.Combine(ImagePath, "boxes", "box_" + names[7]));
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            });
            var t = new Task(a);
            t.Start();
        }

        bool multipleCapture = false;
        private void btnCaptureMultiple_Click(object sender, EventArgs e)
        {
            if (!multipleCapture)
            {
                multipleCapture = true;
                btnCaptureMultiple.Text = "Stop Multiple Capture";
                timerCapture.Start();
                showCaptureControlsToolStripMenuItem.Enabled = false;
            }
            else
            {
                multipleCapture = false;
                btnCaptureMultiple.Text = "Start Multiple Capture";
                timerCapture.Stop();
                showCaptureControlsToolStripMenuItem.Enabled = true;
            }
        }

        private void timerCapture_Tick(object sender, EventArgs e)
        {
            SaveImages();
        }

        // Set icImagingControl1 Device settings
        private void device1SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (icImagingControl1.LiveVideoRunning)
            {
                MessageBox.Show("Must stop devices!");
                return;
            }
            icImagingControl1.ShowDeviceSettingsDialog();
            icImagingControl1.SaveDeviceStateToFile(Device1SettingsPath);
        }

        // Set icImagingControl1 image properties
        private void properties1TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            icImagingControl1.ShowPropertyDialog();
            icImagingControl1.SaveDeviceStateToFile(Device1SettingsPath);
        }

        // Set icImagingControl2 Device settings
        private void device2SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (icImagingControl2.LiveVideoRunning)
            {
                MessageBox.Show("Must stop devices!");
                return;
            }
            icImagingControl2.ShowDeviceSettingsDialog();
            icImagingControl2.SaveDeviceStateToFile(Device2SettingsPath);
        }

        // Set icImagingControl2 image properties
        private void properties2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            icImagingControl2.ShowPropertyDialog();
            icImagingControl2.SaveDeviceStateToFile(Device2SettingsPath);
        }

        // Stop cameras
        private void stopLiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            icImagingControl1.LiveStop();
            icImagingControl2.LiveStop();
        }

        private void startLiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartCameras();
        }

        // Try to start cameras
        private void StartCameras()
        {
            if (icImagingControl1.Device == icImagingControl2.Device)
            {
                MessageBox.Show("Camera1 and Camera2 must be set to different devices!");
                return;
            }
            try
            {
                icImagingControl1.LiveStart();
                icImagingControl2.LiveStart();
                updateOverlay1 = true;
                updateOverlay2 = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void icImagingControl1_OverlayUpdate(object sender, ICImagingControl.OverlayUpdateEventArgs e)
        {
            try
            {
                if (updateOverlay1)
                {
                    updateOverlay1 = false;
                    SetupOverlay1(e.overlay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void icImagingControl2_OverlayUpdate(object sender, ICImagingControl.OverlayUpdateEventArgs e)
        {
            try
            {
                if (updateOverlay2)
                {
                    updateOverlay2 = false;
                    SetupOverlay2(e.overlay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tbRadius_Leave(object sender, EventArgs e)
        {
            int radius = 0;
            if (int.TryParse(tbRadius.Text, out radius))
            {
                if (radius >= 30 && radius <= 160)
                {
                    Sectors.RegionRadius = radius;
                    Sectors.SetCircleFilterBackgroungImages();
                    return;
                }
            }
            tbRadius.Text = Sectors.RegionRadius.ToString();
        }

        private void btnFail1_Click(object sender, EventArgs e)
        {
            cyl1.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail1", false, 1);
        }

        private void btnFail2_Click(object sender, EventArgs e)
        {
            cyl2.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail2", false, 1);
        }

        private void btnFail3_Click(object sender, EventArgs e)
        {
            cyl3.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail3", false, 1);
        }

        private void btnFail4_Click(object sender, EventArgs e)
        {
            cyl4.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail4", false, 1);
        }

        private void btnFail5_Click(object sender, EventArgs e)
        {
            cyl5.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail5", false, 1);
        }

        private void btnFail6_Click(object sender, EventArgs e)
        {
            cyl6.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail6", false, 1);
        }

        private void btnFail7_Click(object sender, EventArgs e)
        {
            cyl7.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail7", false, 1);
        }

        private void btnFail8_Click(object sender, EventArgs e)
        {
            cyl8.Fail = true;
            PAC.pac.Write32BitIntegerVariable("bFail8", false, 1);
        }
    }
}
