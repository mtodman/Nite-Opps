

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;




namespace Nite_Opps
{
    public class phd
    {
        const byte MSG_PAUSE = 1;  //Pause guiding. Camera exposures continue to loop if they are already looping. 0  
        const byte MSG_RESUME = 2;  //Resume guiding if it was posed, otherwise no effect. 0  
        const byte MSG_MOVE1 = 3;  //Dither a random amount, up to +/- 0.5 x dither_scale 
        const byte MSG_MOVE2 = 4;  //Dither a random amount, up to +/- 1.0 x dither_scale Camera exposure time in seconds, but not less than 1 
        const byte MSG_MOVE3 = 5; //Dither a random amount, up to +/- 2.0 x dither_scale Camera exposure time in seconds, but not less than 1 
         const byte MSG_IMAGE =  6; //  (not currently implemented in PHD2)  1 
         const byte MSG_GUIDE = 7; //  (not currently implemented in PHD2) 1 
         const byte MSG_CAMCONNECT = 8;  //(not currently implemented in PHD2) 1 
         const byte MSG_CAMDISCONNECT = 9;  //(not currently implemented in PHD2) 1 
         const byte MSG_REQDIST = 10;  //Request guide error distance The current guide error distance in units of 1/100 pixel. Values > 255 are reported as 255.  
         const byte MSG_REQFRAME = 11;  //(not currently implemented in PHD2) 1 
         const byte MSG_MOVE4 = 12;  //Dither a random amount, up to +/- 3.0 x dither_scale Camera exposure time in seconds, but not less than 1 
         const byte MSG_MOVE5 = 13;  //Dither a random amount, up to +/- 5.0 x dither_scale Camera exposure time in seconds, but not less than 1 
         const byte MSG_AUTOFINDSTAR = 14;  //Auto-select a guide star.  1 if a star was selected, 0 if not  
         const byte MSG_SETLOCKPOSITION = 15;  //Read 2 16-bit integers, x and y,from the socket and set the lock position to (x,y)  0  
         const byte MSG_FLIPRACAL = 16;  //Flip RA calibration data  1 if RA calibration data was flipped, 0 otherwise 
         const byte MSG_GETSTATUS = 17;  //Get a value describing the state of PHD  
        //0: not paused, looping, or guiding 
         //1: capture active and star selected 
         //2: calibrating 
         //3: guiding and locked on star 
         //4: guiding but star lost 
         //100: paused 
         //101: looping but no star selected  
         const byte MSG_STOP = 18;  //Stop looping exposures or guiding 0. Client should poll with MSG_GETSTATUS to check that looping/guiding has actually stopped. 
         const byte MSG_LOOP = 19;  //Start looping exposures 0 if request to start looping was accepted, non-zero otherwise (like when looping was already active). Client should poll with MSG_GETSTATUS to see if looping actually started.  
         const byte MSG_STARTGUIDING = 20;  //Start guiding  0. Client should poll with MSG_GETSTATUS to check that guiding has actually started. 
         const byte MSG_LOOPFRAMECOUNT = 21;  //Get the current frame counter value. 0 if not looping or guiding. Otherwise, the current frame counter value (capped at 255). The frame counter is incremented for each camera exposure when looping or guiding. 
         const byte MSG_CLEARCAL = 22;  //Clear calibration data (force re-calibration)  0 
         const byte MSG_FLIP_SIM_CAMERA = 23;  //When the camera simulator is active, simulate a scope meridian flip 0  
         const byte MSG_DESELECT = 24;  //De-select the currently selected guide star. If subframes are enabled, switch to full frames. This command should be sent before sending MSG_AUTOFINDSTAR to ensure a full frame is captured. For example, the following sequence could be used to select a guide star: MSG_STOP, MSG_DESELECT, MSG_LOOP, MSG_LOOPFRAMECOUNT, MSG_AUTOFINDSTAR.  0  


        private bool _connected;
        private TcpClient oClient;
        private NetworkStream oStream;
        private Thread oListenerThread;
        private bool bListeningActive;
        private ThreadControl oControl;
        public char[] acTerminals = { ',', '}' };               // for JSON parsing
        // Event triggering stuff 
        public delegate void PHDAlarmHandler(AlarmInfo oInfo);      // Define signature of event handler 
        public event PHDAlarmHandler evPHDAlarm;                    // Event to cause or subscribe to

        public struct AlarmInfo
        {
            public eAlarmTypes eType;
            public double Info1;
            public double Info2;
        }

        public enum eAlarmTypes
        {
            eStarLost,
            eExcursion
        }

        Stream s;
        clsSharedData sd;
        
        
        public phd(ref clsSharedData d)
        {
            sd = d;
        }

       //Public Properties
        
        public bool connected
       {
           get
           {
               return _connected;
           }
           set
           {
               Connected(value);
           }
       }

        public byte getStatus
        {
            get
            {
                return requestFromPhd(MSG_GETSTATUS);
            }
        }


        //Private Methods

        private void Connected(bool state)
        {
            switch (state)
            {
                case true:
                    if(oClient == null)
                    {
                        try
                        {
                            oClient = new TcpClient(clsStatics.phd_host, clsStatics.phd_port);
                        }
                        catch (SocketException sex)
                        {
                            _connected = false;
                        }
                        catch(Exception ex)
                        {
                            //Failed to connect
                            _connected = false;
                        }
                        
                        if(oClient != null)
                        {
                            _connected = true;
                        }
                        else
                        {
                            _connected = false;
                        }
                    }
                    else
                    {
                         _connected = false;
                    }

                    break;
                case false:
                    if(oClient != null)
                    {
                        oClient=null;
                    }
                    _connected = false;
                    break;
            }
        }

        private byte requestFromPhd(byte request)
        {
            s = oClient.GetStream();
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true;
            sw.WriteLine(request);
            return Convert.ToByte(sr.ReadLine());
        }

        //public methods


    }


    public class ThreadControl
    {
        public TcpClient oClient;
        public NetworkStream oStream;
        public bool KeepRunning;


        public ThreadControl(TcpClient Client, NetworkStream Stream, bool Running)
        {
            oClient = Client;
            oStream = Stream;
            KeepRunning = Running;
        }
    }
}
