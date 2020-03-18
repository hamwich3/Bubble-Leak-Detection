using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Opto22.Controller;

namespace Bubble_Leak_Detection
{
    class Opto22Comm
    {
        public Opto22.Controller.Controller pac;
        public bool Connected = false;

        private Controller.Item[] tagReadArray;
        private Controller.Item[] stringReadArray;
        private Thread readThread;
        private bool keepReadThreadAlive;
        private System.Timers.Timer watchdogTimer;
        private string settingsFileName = "opto22.ini";
        private string IP = String.Empty;

        //write
        public Int32 bFail1 = 0;
        public Int32 bFail2 = 0;
        public Int32 bFail3 = 0;
        public Int32 bFail4 = 0;
        public Int32 bFail5 = 0;
        public Int32 bFail6 = 0;
        public Int32 bFail7 = 0;
        public Int32 bFail8 = 0;

        //read
        public Int32 bPressurize = 0;
        public Int32 bAirFill = 0;
        public Int32 iHeadDown = 0;
        public Int32 bTesting = 0;
        public float fPressure = 0;
        public float fPercTargetPressure = 0;
        public Int32 bTareVisionSystem = 0;
        public Int32 bTestComplete = 0;

        int iHeadDownLast = 0;
        int bTestingLast = 0;
        int bTareVisionSystemLast = 0;
        int bPressurizeLast = 0;
        int bTestCompleteLast = 0;

        public delegate void EventHandler(object source, EventArgs e);

        public event EventHandler HeadDown;
        public event EventHandler StartTakeThreshold;
        public event EventHandler StopTakeThreshold;
        public event EventHandler StartTest;
        public event EventHandler StopTest;
        public event EventHandler TestCompleted;

        public Opto22Comm(string settingsPath)
        {
            pac = new Controller();
            GetSettings(settingsPath);
            InitReadTags();
            if (OpenSession())
            {
                Connected = true;
                StartReadThread();
            }
        }

        private void GetSettings(string settingsPath)
        {
            string path = Path.Combine(settingsPath, settingsFileName);
            if (File.Exists(path))
            {
                var ip = File.ReadAllText(path);
                IPAddress tempAddress;
                if (IPAddress.TryParse(ip, out tempAddress))
                {
                    IP = ip;
                }
                else
                {
                    File.Delete(path);
                    File.WriteAllText(path, "192.168.0.200");
                    System.Windows.Forms.MessageBox.Show("Opto22.ini created, IPAddress set to 192.168.0.200");
                    IP = "192.168.0.200";
                }
            }
            else
            {
                File.WriteAllText(path, "192.168.0.200");
                System.Windows.Forms.MessageBox.Show("Opto22.ini created, IPAddress set to 192.168.0.200");
            }
        }

        private void StartReadThread()
        {
            if (readThread != null) System.Windows.Forms.MessageBox.Show("PAC read thread already assigned!");
            readThread = new Thread(() =>
            {
                while (keepReadThreadAlive)
                {
                    pac.Write32BitIntegerVariable("bVisionRunning", false, 1);
                    var eMethodResponse = pac.ReadGroupItems(tagReadArray, 0, tagReadArray.Length);
                    tagReadArray[0].GetData(out bPressurize);
                    tagReadArray[1].GetData(out bAirFill);
                    tagReadArray[2].GetData(out iHeadDown);
                    tagReadArray[3].GetData(out bTesting);
                    tagReadArray[4].GetData(out fPressure);
                    tagReadArray[5].GetData(out bTareVisionSystem);
                    tagReadArray[6].GetData(out fPercTargetPressure);
                    tagReadArray[7].GetData(out bTestComplete);
                    tagReadArray[8].GetData(out bFail1);
                    tagReadArray[9].GetData(out bFail2);
                    tagReadArray[10].GetData(out bFail3);
                    tagReadArray[11].GetData(out bFail4);
                    tagReadArray[12].GetData(out bFail5);
                    tagReadArray[13].GetData(out bFail6);
                    tagReadArray[14].GetData(out bFail7);
                    tagReadArray[15].GetData(out bFail8);


                    if (bTesting == 1 && bTestingLast == 0)
                    {
                        eMethodResponse = pac.ReadGroupItems(stringReadArray, 0, stringReadArray.Length);
                        string[] s = Enumerable.Repeat<string>("null", stringReadArray.Length).ToArray();
                        for (int i = 0; i < stringReadArray.Length; i++)
                        {
                            stringReadArray[i].GetData(out s[i]);
                        }
                        StartTest?.Invoke(s, new EventArgs());
                    }
                    bTestingLast = bTesting;

                    if (iHeadDown == 1 && iHeadDownLast == 0)
                    {
                        HeadDown?.Invoke(this, new EventArgs());
                    }
                    iHeadDownLast = iHeadDown;

                    if (bTareVisionSystem == 1 && bTareVisionSystemLast == 0)
                    {
                        StartTakeThreshold?.Invoke(this, new EventArgs());
                    }
                    if (bTareVisionSystem == 0 && bTareVisionSystemLast == 1 && bTesting == 1)
                    {
                        StopTakeThreshold?.Invoke(this, new EventArgs());
                    }
                    bTareVisionSystemLast = bTareVisionSystem;

                    if (bPressurize == 0 && bPressurizeLast == 1)
                    {
                        StopTest?.Invoke(this, new EventArgs());
                    }
                    bPressurizeLast = bPressurize;

                    if (bTestComplete == 1 && bTestCompleteLast == 0)
                    {
                        TestCompleted?.Invoke(this, new EventArgs());
                    }
                    bTestCompleteLast = bTestComplete;

                    Thread.Sleep(250);
                }
            });
            keepReadThreadAlive = true;
            readThread.Start();
        }

        private bool OpenSession()
        {
            Controller.ErrorResponse eMethodResponse;
            eMethodResponse = pac.OpenSession(IP, 22001, 1000);
            // validate the attempt
            if (eMethodResponse != Controller.ErrorResponse.Success)
            {
                System.Windows.Forms.MessageBox.Show("Error Connecting to PLC!: " + eMethodResponse);
            }

            return (eMethodResponse == Controller.ErrorResponse.Success);
        }

        private void InitReadTags()
        {
            tagReadArray = Controller.NewItemList(16);
            tagReadArray[0].SetRead("bPressurize", Controller.Item.eItemTypes.integer32);
            tagReadArray[1].SetRead("bAirFill", Controller.Item.eItemTypes.integer32);
            tagReadArray[2].SetRead("iHeadDown", Controller.Item.eItemTypes.integer32);
            tagReadArray[3].SetRead("bTesting", Controller.Item.eItemTypes.integer32);
            tagReadArray[4].SetRead("fPressure", Controller.Item.eItemTypes.float32);
            tagReadArray[5].SetRead("bTareVisionSystem", Controller.Item.eItemTypes.integer32);
            tagReadArray[6].SetRead("fPercTargetPressure", Controller.Item.eItemTypes.float32);
            tagReadArray[7].SetRead("bTestComplete", Controller.Item.eItemTypes.integer32);

            tagReadArray[8].SetRead("bFail1", Controller.Item.eItemTypes.integer32);
            tagReadArray[9].SetRead("bFail2", Controller.Item.eItemTypes.integer32);
            tagReadArray[10].SetRead("bFail3", Controller.Item.eItemTypes.integer32);
            tagReadArray[11].SetRead("bFail4", Controller.Item.eItemTypes.integer32);
            tagReadArray[12].SetRead("bFail5", Controller.Item.eItemTypes.integer32);
            tagReadArray[13].SetRead("bFail6", Controller.Item.eItemTypes.integer32);
            tagReadArray[14].SetRead("bFail7", Controller.Item.eItemTypes.integer32);
            tagReadArray[15].SetRead("bFail8", Controller.Item.eItemTypes.integer32);

            stringReadArray = Controller.NewItemList(16);

            int i = 0;
            stringReadArray[i++].SetRead("sSerial1", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial2", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial3", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial4", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial5", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial6", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial7", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sSerial8", Controller.Item.eItemTypes.stringvariable);

            stringReadArray[i++].SetRead("sLotNo1", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo2", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo3", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo4", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo5", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo6", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo7", Controller.Item.eItemTypes.stringvariable);
            stringReadArray[i++].SetRead("sLotNo8", Controller.Item.eItemTypes.stringvariable);



        }

        public void CloseDispose()
        {
            if (watchdogTimer != null)
            {
                watchdogTimer.Stop();
            }
            keepReadThreadAlive = false;
            while (readThread != null && readThread.IsAlive)
            {
                readThread.Join();
            }
            if (pac != null)
            {
                try
                {
                    pac.CloseSession();
                }
                catch { }
                try
                {
                    pac.Dispose();
                }
                catch { }
            }
        }
    }
}
