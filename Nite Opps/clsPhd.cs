using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using guider;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Nite_Opps
{
    public class phd2 : Guider
    {
        Guider guider;
        public phd2()
        {
            guider = Guider.Factory("localhost");
            //guider.Connect();
        }

        public async Task<bool> StartPHD2Process()
        {
            // if phd2 is not running start it
            try
            {
                if (Process.GetProcessesByName("phd2").Length == 0)
                {
                    var defaultPHD2Path = Environment.ExpandEnvironmentVariables(@"%programfiles(x86)%\PHDGuiding2\phd2.exe");
                    if (!File.Exists(defaultPHD2Path))
                    {
                        throw new FileNotFoundException();
                    }

                    var process = Process.Start(defaultPHD2Path);
                    process?.WaitForInputIdle();

                    await Task.Delay(1500);

                    return true;
                }
            }
            catch (FileNotFoundException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }

            return false;
        }
        public override void Dispose()
        {
            throw new NotSupportedException(
                "The Dispose() method was called on a class which does not support that method.");
        }

        public override bool IsGuiding()
        {
            return guider.IsGuiding();
           // throw new NotSupportedException(
           //     "The IsGuiding() method was called on a class which does not support that method.");
        }

        public override void Close()
        {
            throw new NotSupportedException(
                "The Close() method was called on a class which does not support that method.");
        }

        public override void Unpause()
        {
            throw new NotSupportedException(
                "The Unpause() method was called on a class which does not support that method.");
        }

        public override JObject Call(string method, JToken param)
        {
            throw new NotSupportedException(
                "The Call() method was called on a class which does not support that method.");
        }

        public override JObject Call(string method)
        {
            throw new NotSupportedException(
                "The Call() method was called on a class which does not support that method.");
        }

        public override List<string> GetEquipmentProfiles()
        {
            throw new NotSupportedException(
                "The GetEquipmentProfiles() method was called on a class which does not support that method.");
        }

        public override void StopCapture(uint timeoutSeconds)
        {
            guider.StopCapture(timeoutSeconds);
           // throw new NotSupportedException(
            //    "The StopCapture() method was called on a class which does not support that method.");
        }

        public override SettleProgress CheckSettling()
        {
            throw new NotSupportedException(
                "The CheckSettling() method was called on a class which does not support that method.");
        }

        public override void Dither(double ditherPixels, double settlePixels, double settleTime, double settleTimeout)
        {
            throw new NotSupportedException(
                "The Dither() method was called on a class which does not support that method.");
        }

        public override void ConnectEquipment(string profileName)
        {
            guider.ConnectEquipment(profileName);
           
        }

        public override void GetStatus(out string appState, out double avgDist)
        {
            throw new NotSupportedException(
                "The GetStatus() method was called on a class which does not support that method.");
        }

        public override string SaveImage()
        {
            throw new NotSupportedException(
                "The SaveImage() method was called on a class which does not support that method.");
        }

        public override double PixelScale()
        {
            throw new NotSupportedException(
                "The PixelScale() method was called on a class which does not support that method.");
        }

        public override void DisconnectEquipment()
        {
            throw new NotSupportedException(
                "The DisconnectEquipment() method was called on a class which does not support that method.");
        }

        public override bool IsSettling()
        {
            throw new NotSupportedException(
                "The IsSettling() method was called on a class which does not support that method.");
        }

        //public override bool IsConnected()
        //{
       //     return guider.IsConnected();
            //throw new NotSupportedException(
               // "The IsSettling() method was called on a class which does not support that method.");
       // }

        public override void Pause()
        {
            throw new NotSupportedException(
                "The Pause() method was called on a class which does not support that method.");
        }

        public override GuideStats GetStats()
        {
            throw new NotSupportedException(
                "The GetStats() method was called on a class which does not support that method.");
        }

        public override void Loop(uint timeoutSeconds)
        {
            throw new NotSupportedException(
                "The Loop() method was called on a class which does not support that method.");
        }

        public override void Guide(double settlePixels, double settleTime, double settleTimeout, bool forceCalibration)
        {
            
            guider.Guide(settlePixels, settleTime, settleTimeout, forceCalibration);
        }

        public override void Connect()
        {
            guider.Connect();
            //throw new NotSupportedException(
            //    "The Connect() method was called on a class which does not support that method.");
            
        }

    }
    
    public class clsPhd
    {
        private TcpClient oClient;
        private NetworkStream oStream;
        public bool bConnected { get; set; }
        private const int iPhdPortNumber = 4400;
        private bool bListeningActive;
        private Thread oListenerThread;
        private ThreadControl oControl;
        private frmMain oParent;
        // Statistical stuff
        public RunningVariance oRAFrameSteps = new RunningVariance();
        public RunningVariance oRASessionSteps = new RunningVariance();
        public RunningVariance oDecFrameSteps = new RunningVariance();
        public RunningVariance oDecSessionSteps = new RunningVariance();
        public double dImageScale;
        public char[] acTerminals = { ',', '}' };               // for JSON parsing
        // Event triggering stuff 
        public delegate void PHDAlarmHandler(clsStatics.AlarmInfo oInfo);      // Define signature of event handler 
        public event PHDAlarmHandler evPHDAlarm;                    // Event to cause or subscribe to

        // Support classes
        //public enum eAlarmTypes
        //    {
        //    eStarLost, 
        //    eExcursion
        //    }

        //public struct AlarmInfo
        //    {
        //    public eAlarmTypes eType;
        //    public double Info1;
        //    public double Info2;
        //    }

        public class ThreadControl 
            {
            public TcpClient oClient;
            public NetworkStream oStream;
            public bool KeepRunning;


            public ThreadControl (TcpClient Client, NetworkStream Stream, bool Running) 
                {
                oClient = Client;
                oStream = Stream;
                KeepRunning = Running;
                }
            }
        public class RunningVariance
            {
            double SumXSquared;
            double SumX;
            double Count;

            public void ClearVals()
                {
                SumXSquared = 0;
                SumX = 0;
                Count = 0;
                }

            public RunningVariance()
                {
                ClearVals();
                }
            public void Increment (double NewVal) 
                {
                SumXSquared += NewVal * NewVal;
                SumX += NewVal;
                Count++;
                }
            public double Sigma()
                {
                double Variance = 0;
                if (Count > 1)
                    {
                    Variance = (Count * SumXSquared - SumX * SumX) / (Count * (Count - 1));
                    if (Variance >= 0)
                        return (Math.Sqrt (Variance));
                    else
                        return (double.NaN);
                    }
                else
                    if (Count == 1)
                        {
                        return (0);
                        }
                    else
                        return (double.NaN);
                }
            }

        public clsPhd(ref clsSharedData d, frmMain Parent)
            {
            bConnected = false;
            oParent = Parent;
            
            }
        // Public methods and properties
        // The only public client interface for starting/stopping PHD monitoring
        public bool Listening
            {
            get {return (bListeningActive);} 
            set
                {
                if (value && !bListeningActive) 
                    {
                    if (!bConnected)
                        ConnectToPHD(ref oClient, ref oStream);
                    if (bConnected)
                        {
                        oControl = new ThreadControl(oClient, oStream, true);
                        bListeningActive = true;
                        StartListener(ref oControl);
                        }
                    }
                else
                    if (!value && bConnected)
                        {
                        int iCounter = 0;
                        oControl.KeepRunning = false;
                        while (oListenerThread.IsAlive && iCounter++ < 10)
                            Thread.Sleep(1000);
                        // Ask nicely but carry a cudgel...
                        if (oListenerThread.IsAlive)
                            oListenerThread.Abort();
                        oStream.Close();
                        oClient.Close();
                        bListeningActive = false;
                        bConnected = false;
                        }
                }
            }

        public void DecStats(ref double FrameSigma, ref double SessionSigma)
            {
            FrameSigma = oDecFrameSteps.Sigma() * dImageScale;
            SessionSigma = oDecSessionSteps.Sigma() * dImageScale;
            }
        public void RAStats(ref double FrameSigma, ref double SessionSigma)
            {
            FrameSigma = oRAFrameSteps.Sigma() * dImageScale;
            SessionSigma = oRASessionSteps.Sigma() * dImageScale;
            }
        public void ClearFrameStats()
            {
            oRAFrameSteps.ClearVals();
            oDecFrameSteps.ClearVals();
            }
        public void ClearSessionStats()
            {
            oRASessionSteps.ClearVals();
            oDecSessionSteps.ClearVals();
            }


        private bool ConnectToPHD(ref TcpClient oClient, ref NetworkStream oStream)
            {
            IPAddress oIpAddr = IPAddress.Parse("127.0.0.1");       // Localhost
            oClient = new TcpClient();

            try
                {
                oClient.Connect(oIpAddr, iPhdPortNumber);
                oStream = oClient.GetStream();              
                bConnected = true;
                }
            catch (Exception oEx)
                {
                MessageBox.Show("Could not connect to PHD...");
                bConnected = false;
                }
            return (bConnected);
            }

        private void StartListener(ref ThreadControl oControl)
            {

            oListenerThread = new Thread(new ParameterizedThreadStart(Listener));
            oListenerThread.Start(oControl);
            }


        // Ad-hoc method for extracting a string value from the json stream
        private string ExtractString(string sTarget, string sBuff)
            {
            int iOffset = sBuff.IndexOf(sTarget);
            char [] acTerminals = {',', '}'};
            string sTemp = "";

            if (iOffset >= 0)
                {
                sTemp = sBuff.Substring(iOffset);
                sTemp = sTemp.Substring (sTemp.IndexOf(':') + 1);       // now starts with value
                // Get rid of quoted strings as values
                if (sTemp.StartsWith ("\""))
                    {
                    sTemp = sTemp.Substring(1, sTemp.IndexOfAny(acTerminals) - 1);
                    sTemp = sTemp.Substring(0, sTemp.Length - 1);           // get rid of trailing quote
                    }
                else
                    {
                    sTemp = sTemp.Substring(0, sTemp.IndexOfAny(acTerminals) - 1);
                    }
                }
            return (sTemp);
            }

        // Ad-hoc method to retrieve a double value from the json string
        private double ExtractDouble(string sTarget, string sBuff)
            {
            string sTemp = ExtractString (sTarget, sBuff);

            double dVal;
            if (double.TryParse(sTemp, out dVal))
                return (dVal);
            else
                return (double.NaN);
            }

        // Extract the RA and Dec raw displacements from a GuideStep entry.  We can't safely use index offsets here 
        // because the GuideStep string has variable length, with variable offsets because of numerical signs and 
        // numerical values
        private bool GetDisplacements(string sBuff, ref double RADelta, ref double DecDelta)
            {
            string sVal;
            int iLen;
            bool bOk = false;

            sVal = sBuff.Substring(sBuff.IndexOf("RADistanceRaw"));
            sVal = sVal.Substring(sVal.IndexOf (':') + 1);
            iLen = sVal.IndexOfAny(acTerminals);
            sVal = sVal.Substring(0, iLen);
            if (double.TryParse(sVal, out RADelta))
                {
                sVal = sBuff.Substring(sBuff.IndexOf("DECDistanceRaw"));
                sVal = sVal.Substring(sVal.IndexOf(':') + 1);
                iLen = sVal.IndexOfAny(acTerminals);
                sVal = sVal.Substring(0, iLen);
                if (double.TryParse(sVal, out DecDelta))
                    bOk = true;

                }
            if (!bOk)
                {
                RADelta = double.NaN; 
                DecDelta = double.NaN;
                }
            return (bOk);
            }
        // Get the event id from the JSON event notification string
        private bool GetEventName(string sBuff, int iEventLoc, ref string sId)
            {
            string sVal;
            int iLen;

            sVal = sBuff.Substring(iEventLoc);
            iLen = sVal.IndexOf('"');
            sId = sVal.Substring(0, iLen);
            return (iLen > 0);
            }

        // Get the image scale from PHD
        private double PHDImageScale(ThreadControl oControl, StreamReader oReader, StreamWriter oDebug)
            {
            string sBuff;
            string sCommand = "{\"method\": \"get_pixel_scale\", \"id\": 1}";
            double dVal;
            int iCounter = 0;
            StreamWriter oSw = new StreamWriter(oControl.oStream);

            oSw.WriteLine(sCommand);
            oSw.Flush();
            sBuff = oReader.ReadLine();
            while (!sBuff.Contains("jsonrpc") && iCounter < 10)
                {
                sBuff = oReader.ReadLine();
                iCounter++;
                }
            if (iCounter >= 10)
                return (double.NaN);
            // Now get the bloody result 
            dVal = ExtractDouble("result", sBuff);
            if (oParent.bLogging)
                writetolog("PHD image scale: " + dVal.ToString(), true);
                //oParent.oLog.Trace("PHD image scale: " + dVal.ToString());
            return (dVal);
            }

        // Mule function for incrementing the stats 
        private void AccumDisplacements(ref ThreadControl oControl, double RADelta, double DecDelta)
            {
            oRAFrameSteps.Increment(RADelta);
            oRASessionSteps.Increment(RADelta);
            oDecFrameSteps.Increment(DecDelta);
            oDecSessionSteps.Increment(DecDelta);
            }

        // This is where the work gets done - fielding asynchronous messages from PHD, computing stats, etc. 
        // It runs on a separate thread for obvious reasons
        // MessageBox calls are ok on worker thread - not related to the main UI, MessabeBox class is static
        private void Listener(object oCtrl)
            {
            ThreadControl oControl = (ThreadControl) oCtrl;
            StreamReader oSr = new StreamReader (oControl.oStream);

            string sBuff;
            string sEventId = "";
            double dRADelta = 0;            // position shifts in GuideStep entry
            double dDecDelta = 0;
            const int iEventLoc = 10;       // Optimized access in JSON entry for event id
            bool bCloudy = false;
            int iClearCounter = 0;           // Number of consecutive non-cloudy guide steps we've gotten
            int iSteadyCounter = 0;             // Damping for guider woof alarms

            StreamWriter Writer = new StreamWriter("Displacements.csv");        // Debug text file


            while (oControl.KeepRunning)
                {
                if (oControl.oStream.DataAvailable)                  // avoid blocking until data is available
                    {
                    sBuff = oSr.ReadLine();
                    if (oParent.bLogging)
                        writetolog("PHD: " + sBuff, true);
                    if (sBuff.Length > 0)
                        {
                        GetEventName(sBuff, iEventLoc, ref sEventId);
                        switch (sEventId)
                            {

                        case "GuideStep":
                                {
                                if (GetDisplacements(sBuff, ref dRADelta, ref dDecDelta))
                                    {
                                    AccumDisplacements(ref oControl, dRADelta, dDecDelta);
                                    Writer.WriteLine(dRADelta.ToString("#0.000") + ", " +
                                            dDecDelta.ToString("#0.000"));
                                    Writer.Flush();
                                    if (oParent.bLogging)

                                        writetolog("Displacements: " +
                                            dRADelta.ToString("#0.000") + ", " +
                                            dDecDelta.ToString("#0.000"), true);
                                    if (Math.Abs(dRADelta * dImageScale) > oParent.flMaxGuideDelta ||
                                        Math.Abs(dDecDelta * dImageScale) > oParent.flMaxGuideDelta)
                                        {
                                        if (iSteadyCounter > 10)
                                            {
                                            iSteadyCounter = 0;
                                            clsStatics.AlarmInfo oInfo = new clsStatics.AlarmInfo();
                                            oInfo.eType = clsStatics.eAlarmTypes.eExcursion;
                                            oInfo.Info1 = Math.Abs(dRADelta * dImageScale);
                                            oInfo.Info2 = Math.Abs(dDecDelta * dImageScale);
                                            evPHDAlarm(oInfo);              // Fire the event
                                            }
                                        }
                                    else
                                        iSteadyCounter++;
                                    if (bCloudy)
                                        {
                                        if (iClearCounter++ > 10)
                                            {
                                            bCloudy = false;            // Alarms may occur again
                                            iClearCounter = 0;
                                            }
                                        }
                                    }
                                else
                                    if (oParent.bLogging)
                                        writetolog("Broken pick on displacements...", true);
                                break;
                                }
                          case "StartGuiding":
                                    {
                                    dImageScale = PHDImageScale(oControl, oSr, Writer);
                                    ClearFrameStats();
                                    ClearSessionStats();
                                    if (oParent.bLogging)
                                        writetolog("PHD guiding started", true);
                                    break;
                                    }
                          case "GuidingStopped":
                                    {
                                    if (oParent.bLogging)
                                        writetolog("PHD guiding stopped", true);
                                    break;
                                    }
                          case "StarLost":
                                    {
                                    if (oParent.bLogging)
                                        writetolog("PHD star lost notice...", true);
                                    if (!bCloudy)           // Don't bury user in alarms
                                        {
                                        bCloudy = true;
                                        clsStatics.AlarmInfo oInfo = new clsStatics.AlarmInfo();
                                        oInfo.eType = clsStatics.eAlarmTypes.eStarLost;
                                        evPHDAlarm(oInfo);              // Fire the event
                                        }
                                    break;
                                    }
                            }


                        }
                    else
                        {
                        MessageBox.Show("PHD timeout");
                        }
                    }
                else
                    Thread.Sleep(1000);
                }
            Writer.WriteLine("RA Frame, RA Session, Dec Frame, Dec Session");
            Writer.WriteLine(oRAFrameSteps.Sigma().ToString("#0.000") + ", " +
                oRASessionSteps.Sigma().ToString("#0.000") + ", " +
                oDecFrameSteps.Sigma().ToString("#0.000") + ", " +
                oDecSessionSteps.Sigma().ToString("#0.000") + ", ");
            Writer.Flush();
            Writer.Close();
            }

        // Code to create the event which sends a string message to 
        // the Main form's Status Box (txtStatusBox).
        public delegate void LogHandler(string message, bool displaytime);
        public event LogHandler Log;
        public void writetolog(string message, bool displaytime)
        {
            Log(message, displaytime);
        }
    }
}
