using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Nite_Opps
{
    public static class AstroCalculations
    {
        public static double Deg(int degree, int min, double sec)
        {
            //converts degree,min,sec (sexagesimal) to degrees (double)
            return degree + (min / 60) + (sec / 3600);
        }

        public static double JulianDay(int yy, int mm, int dd)
        {
            //Calculates Julian Day 1900 < Year < 2099
            //double x, y, z;
            double tulos = 0;

            tulos = 367 * yy - Math.Floor(7 * (yy + Math.Floor((double)(mm + 9) / 12)) / 4)
- Math.Floor(3 * (Math.Floor((double)(yy + (mm - 9) / 7) / 100) + 1) / 4)
+ Math.Floor((double)275 * mm / 9) + dd + 1721028.5;


            //x = (mm + 9) / 12;
            //y = (yy + (mm - 9) / 7) / 100;
            //z = 275 * mm / 9;
            //tulos = 367 * yy - Math.Floor(7 * (yy + Math.Floor (x) / 4) - Math.Floor(3 * (Math.Floor(y) + 1) / 4) + Math.Floor(z) + dd + 1721028.5);
            return tulos;
        }

        public static double JDNow()
        {
            //Calculates Julian for current date
            double x, y, z, tulos = 0;
            int yy = 0;
            int mm = 0;
            int dd = 0;
            yy = DateTime.UtcNow.Year;
            mm = DateTime.UtcNow.Month;
            dd = DateTime.UtcNow.Day;

            x = (mm + 9) / 12;
            y = (yy + (mm - 9) / 7) / 100;
            z = 275 * mm / 9;

            tulos = 367 * yy - Math.Floor(7 * (yy + Math.Floor(x)) / 4) - Math.Floor(3 * (Math.Floor(y) + 1) / 4) + Math.Floor(z) + dd + 1721028.5;
            return tulos;
        }

        public static double GetLST(int intLonDeg, int intLongMin, int dblLongSec, string strLonew)
        {
            double LST = 0;
            double longitudedeg = 0;
            longitudedeg = CalcLongitude(intLonDeg, intLongMin, dblLongSec, strLonew);
            //LST is Local Sidereal Time in hours, differs from GMST by longitude
            LST = GMSTime() + longitudedeg / 15;
            if ((LST < 0))
            {
                LST = LST + 24;
            }

            if ((LST >= 24))
            {
                LST = LST - 24;
            }

            return LST;
        }

        public static double GetHA(double LST, double RA)
        {
            double hourangle = 0;
            hourangle = LST - RA;
            return hourangle;
        }

        public static double Greenwich(int yy, int mm, int dd, double time)
        {
            //Calculates the Greenwich Sidereal time in Hours
            double T = 0;
            double JD = 0;
            double GWT0 = 0;
            double GW = 0;
            double GMST = 0;
            //JD is the Julian Day
            //JD = JulianDay(yy, mm, dd)
            //JD = GetJD()
            JD = JDNow();
            T = (JD - 2451545.0) / 36525;
            //GWT0 is Greenwich Sidereal Time in hours
            GWT0 = (24110.54841 + 8640184.812866 * T + 0.093104 * T * T - 6.2E-06 * T * T * T) / 3600;
            if ((GWT0 >= 24))
            {
                GW = GWT0 - 24 * Math.Floor(GWT0 / 24);
            }
            if ((GWT0 <= -24))
            {
                GW = 24 - (Math.Abs(GWT0) - 24 * Math.Floor(Math.Abs(GWT0) / 24));
            }
            GMST = GW + 1.00273790935 * time;
            if ((GMST >= 24))
            {
                GMST = GMST - 24;
            }
            if ((GMST < 0))
            {
                GMST = GMST + 24;
            }

            return GMST;

        }

        public static double GMSTime()
        {
            //Calculates the Greenwich Sidereal time in Hours
            double T = 0;
            double JD = 0;
            double GWT0 = 0;
            double GW = 0;
            double _GMST = 0;
            //JD is the Julian Day
            JD = JDNow();
            T = (JD - 2451545.0) / 36525;
            //GWT0 is Greenwich Sidereal Time in hours
            GWT0 = (24110.54841 + 8640184.812866 * T + 0.093104 * T * T - 6.2E-06 * T * T * T) / 3600;
            if ((GWT0 >= 24))
            {
                GW = GWT0 - 24 * Math.Floor(GWT0 / 24);
            }
            if ((GWT0 <= -24))
            {
                GW = 24 - (Math.Abs(GWT0) - 24 * Math.Floor(Math.Abs(GWT0) / 24));
            }
            _GMST = GW + 1.00273790935 * Deg(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
            if ((_GMST >= 24))
            {
                _GMST = _GMST - 24;
            }
            if ((_GMST < 0))
            {
                _GMST = _GMST + 24;
            }

            return _GMST;

        }

        //public void Calculate()
        //{
        //    double JD = 0;
        //    double LST = 0;
        //    double RA = 0;
        //    double SHA = 0;
        //    double x = 0;
        //    double asx = 0;
        //    double minx = 0;
        //    double sekx = 0;
        //    double Dec = 0;
        //    double time = 0;
        //    double latitude = 0;
        //    double longitudedeg = 0;
        //    double hourangle = 0;
        //    double GHA = 0;
        //    double sinAlt = 0;
        //    double altitude = 0;
        //    double cosAz = 0;
        //    double azimuth = 0;

        //    Dec = Deg(intDecDeg, intDecMin, dblDecSec) * Math.PI / 180;
        //    RA = Deg(intRAhh, intRAmm, dblRAss);
        //    //SHA = Sidereal Hour Angle of object
        //    SHA = 360 - RA * 15;
        //    x = Math.Floor(SHA);
        //    minx = Math.Floor(60 * (SHA - x));
        //    sekx = (SHA - x - minx / 60) * 3600;

        //    time = Deg(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
        //    latitude = Deg(intLatDeg, intLatMin, dblLatSec) * Math.PI / 180;
        //    if (strLatns == "S")
        //    {
        //        latitude = -latitude;
        //    }

        //    longitudedeg = Deg(intLonDeg, intLongMin, dblLongSec);
        //    if (strLonew == "W")
        //    {
        //        longitudedeg = -longitudedeg;
        //    }

        //    JD = GetJD();

        //    //hourangle is local hourangle in hours
        //    hourangle = LST - RA;
        //    if ((hourangle < 0))
        //    {
        //        hourangle = 24 + hourangle;
        //    }


        //    //GHA is Greenwich Hour Angle
        //    GHA = hourangle - longitudedeg / 15;
        //    if ((GHA >= 24))
        //    {
        //        GHA = GHA - 24;
        //    }

        //    if ((GHA < 0))
        //    {
        //        GHA = GHA + 24;
        //    }

        //    x = Math.Floor(GHA);
        //    minx = Math.Floor(60 * (GHA - x));
        //    sekx = (GHA - x - minx / 60) * 3600;

        //    hourangle = hourangle * Math.PI / 12;

        //    //Calculate the Altitude
        //    RA = RA * Math.PI / 12;
        //    sinAlt = Math.Cos(hourangle) * Math.Cos(Dec) * Math.Cos(latitude) + Math.Sin(Dec) * Math.Sin(latitude);
        //    altitude = Math.Asin(sinAlt);

        //    x = Math.Abs(altitude * 180 / Math.PI);
        //    asx = Math.Floor(x);
        //    minx = Math.Floor(60 * (x - asx));
        //    sekx = (x - asx - minx / 60) * 3600;
        //    if ((altitude < 0))
        //    {
        //        asx = -asx;
        //    }

        //    //Form1.txtaltDeg.Text = asx
        //    //Form1.txtAltMin.Text = minx
        //    //Form1.txtAltSec.Text = sekx
        //    AltDeg = asx;

        //    cosAz = (Math.Sin(Dec) - (Math.Sin(latitude) * Math.Sin(altitude))) / (Math.Cos(latitude) * Math.Cos(altitude));

        //    azimuth = Math.Acos(cosAz);

        //    x = Math.Abs(azimuth * 180 / Math.PI);
        //    asx = Math.Floor(x);
        //    minx = Math.Floor(60 * (x - asx));
        //    sekx = (x - asx - minx / 60) * 3600;
        //    if ((azimuth < 0))
        //    {
        //        asx = -asx;
        //    }


        //}

        public static Decimal2hhmmss Dec2hhmmss(double x)
        {
            //Converts from hh.mm format to hh mm ss.ss format
            Decimal2hhmmss ret_value = default(Decimal2hhmmss);
            if (x < 0)
            {
                x = -x;
                ret_value.hh = (int)Math.Floor(x);
                ret_value.mm = (int)Math.Floor(60 * (x - ret_value.hh));
                ret_value.ss = (x - ret_value.hh - ret_value.mm / 60) * 3600;
                ret_value.hh = -ret_value.hh;
            }
            else
            {
                ret_value.hh = (int)Math.Floor(x);
                ret_value.mm = (int)Math.Floor(60 * (x - ret_value.hh));
                ret_value.ss = (x - ret_value.hh - ret_value.mm / 60) * 3600;
            }

            return ret_value;

        }

        public static  RAandDEC2hhmmss Dec2hhmmss(double x, double y)
        {
            //Converts from hh.mm format to hh mm ss.ss format
            RAandDEC2hhmmss ret_value = default(RAandDEC2hhmmss);
            if (x < 0)
            {
                x = -x;
                ret_value.RAhh = (int)Math.Floor(x);
                decimal minutesdecimal = (decimal)((x - ret_value.RAhh) * 60);
                int minutes = (int)Math.Floor(minutesdecimal);
                double seconds = (double)((minutesdecimal - minutes) * 60);
                ret_value.RAmm = minutes;
                ret_value.RAss = seconds;
                ret_value.RAhh = -ret_value.RAhh;
            }
            else
            {
                ret_value.RAhh = (int)Math.Floor(x);
                decimal minutesdecimal = (decimal)((x - ret_value.RAhh) * 60);
                int minutes = (int)Math.Floor(minutesdecimal);
                double seconds = (double)((minutesdecimal - minutes) * 60);
                ret_value.RAmm = minutes;
                ret_value.RAss = seconds;
            }

            if (y < 0)
            {
                y = -y;
                ret_value.DECdeg = (int)Math.Floor(y);
                decimal minutesdecimal = (decimal)((y - ret_value.DECdeg) * 60);
                int minutes = (int)Math.Floor(minutesdecimal);
                double seconds = (double)((minutesdecimal - minutes) * 60);
                ret_value.DECmm = minutes;
                ret_value.DECss = seconds;
                ret_value.DECdeg = -ret_value.DECdeg;
            }
            else
            {
                ret_value.DECdeg = (int)Math.Floor(y);
                decimal minutesdecimal = (decimal)((y - ret_value.DECdeg) * 60);
                int minutes = (int)Math.Floor(minutesdecimal);
                double seconds = (double)((minutesdecimal - minutes) * 60);
                ret_value.DECmm = minutes;
                ret_value.DECss = seconds;
            }


            return ret_value;

        }


        public static Decimal2ddmmss Dec2ddmmss(double x)
        {
            //Converts from dd.mmmmmm format to dd mm ss.ss format
            Decimal2ddmmss ret_value = default(Decimal2ddmmss);
            double degx = 0;
            double minx = 0;
            double secx = 0;
            x = Math.Abs(x * 180 / Math.PI);
            degx = Math.Floor(x);
            minx = Math.Floor(60 * (x - degx));
            secx = (x - degx - minx / 60) * 3600;
            if ((x < 0))
            {
                degx = -degx;
            }

            ret_value.dd = (int)degx;
            ret_value.mm = (int) minx;
            ret_value.ss = secx;

            return ret_value;

        }

        public static double Dec2secs(double x)
        {
            //Converts from dd.mmmmmm format to secs
            double total_secs;
            double degx = 0;
            double minx = 0;
            double secx = 0;
            //x = Math.Abs(x * 180 / Math.PI);
            degx = Math.Floor(x);
            minx = Math.Floor(60 * (x - degx));
            secx = (x - degx - minx / 60) * 3600;
            if ((x < 0))
            {
                degx = -degx;
            }

            total_secs = (degx * 3600) + (minx * 60) + secx;

            return total_secs;
        }

        public static double CalcLatitude(int deg, int mm, int ss, string ns)
        {
            double rawdeg = 0;
            rawdeg = (deg + (mm / 60) + (ss / 3600));
            if (ns == "S")
            {
                rawdeg = -rawdeg;
            }
            return rawdeg;
        }

        public static double CalcLongitude(int deg, int mm, int ss, string ew)
        {
            double rawdeg = 0;
            rawdeg = (deg + (mm / 60) + (ss / 3600));
            if (ew == "W")
            {
                rawdeg = -rawdeg;
            }
            return rawdeg;
        }

        public static double ArcSecsPerPixelX(User_Profile Prof)
        {
            return ((clsStatics.imaging_camera_pixel_width / Properties.Settings.Default.imaging_telescope_focal_length) * 0.206 * 1000);
        }

        public static double ArcSecsPerPixelY(User_Profile Prof)
        {
            return ((clsStatics.imaging_camera_pixel_height / Properties.Settings.Default.imaging_telescope_focal_length) * 0.206 * 1000);
        }


        public struct Decimal2ddmmss
        {
            public int dd;
            public int mm;
            public double ss;
        }

        public struct Decimal2hhmmss
        {
            public int hh;
            public int mm;
            public double ss;
        }

        public struct RAandDEC2hhmmss
        {
            public int RAhh;
            public int RAmm;
            public double RAss;

            public int DECdeg;
            public int DECmm;
            public double DECss;

        }

        public struct solvedCoordinates
        {
            public double RA;
            public double DEC;
        }

        public struct structAltAz
        {
            public double Alt;
            public double Az;
        }

        /// <summary>
        /// Returns a struct with alt & az coordinates of an object given it's Ra & Dec from a specific lat & lon on a specific date (jd) 
        /// </summary>
        /// <param name="Lat"></param>
        /// <param name="Lon"></param>
        /// <param name="Ra"></param>
        /// <param name="Dec"></param>
        /// <param name="Prof"></param>
        /// <param name="jd"></param>
        /// <returns></returns>
        public static structAltAz GetAltAz(double Lat, double Lon, double Ra, double Dec, clsSharedData Prof, double jd)
        {
            //Lat = Current Observing Latitude
            //Lon = Current Observing Longitude
            //Ra = Right Ascention of Object
            //Dec = Declination of object
            //jd = Julian Date
            //GSTime = GMT Sidereal Time


            structAltAz ret_value = default(structAltAz);
            double ASCOMAlt = 0;
            double ASCOMAz = 0;
            ASCOM.Astrometry.NOVASCOM.Site Site = default(ASCOM.Astrometry.NOVASCOM.Site);
            ASCOM.Utilities.Util utl = default(ASCOM.Utilities.Util);
            ASCOM.Astrometry.NOVASCOM.Star Obj = default(ASCOM.Astrometry.NOVASCOM.Star);
            ASCOM.Astrometry.NOVASCOM.PositionVector PosVector = default(ASCOM.Astrometry.NOVASCOM.PositionVector);
            //ASCOM.Astrometry.SiteInfo Site = default(ASCOM.Astrometry.SiteInfo);
            //ASCOM.Astrometry.PosVector PVector = default(ASCOM.Astrometry.PosVector);
            ASCOM.Astrometry.NOVAS.NOVAS31 N = new ASCOM.Astrometry.NOVAS.NOVAS31();
            ASCOM.Astrometry.SOFA.SOFA S = new ASCOM.Astrometry.SOFA.SOFA();
            ASCOM.Astrometry.OnSurface onSurface = default(ASCOM.Astrometry.OnSurface);
            onSurface = new ASCOM.Astrometry.OnSurface();
            onSurface.Latitude = Lat;
            onSurface.Longitude = Lon;
            onSurface.Height = Properties.Settings.Default.site_altitude;
            onSurface.Pressure = Properties.Settings.Default.site_pressure;
            onSurface.Pressure = Properties.Settings.Default.site_temperature;




            Site = new ASCOM.Astrometry.NOVASCOM.Site();
            utl = new ASCOM.Utilities.Util();
            Obj = new ASCOM.Astrometry.NOVASCOM.Star();
            PosVector = new ASCOM.Astrometry.NOVASCOM.PositionVector();
            //Site = new ASCOM.Astrometry.SiteInfo();
            //PVector = new ASCOM.Astrometry.PosVector();



            //double jd = JDNow();
            double GSTime = GMSTime();

            Site.Latitude = Lat;
            Site.Longitude = Lon;
            Site.Height = Properties.Settings.Default.site_altitude;
            Site.Pressure = Properties.Settings.Default.site_pressure;
            Site.Temperature = Properties.Settings.Default.site_temperature;

            PosVector.SetFromSite(Site, GSTime);
            Obj.Set(Ra, Dec, 0.0, 0.0, 0.0, 0.0);
            PosVector = Obj.GetTopocentricPosition(jd, Site, false);

            ASCOMAlt = PosVector.Elevation;
            ASCOMAz = PosVector.Azimuth;

            ret_value.Alt = ASCOMAlt;
            ret_value.Az = ASCOMAz;

            return ret_value;

        }




    }
}

