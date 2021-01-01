using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;


namespace Nite_Opps
{
    public class objectDb
    {

        string[] data_array = null;
        int count = 0;
        int total_num_lines = 0;
        string[, ,] object_file_array = null;
        string _engine;
        



        public objectDb()
        {
            //Assume Generic
            read_files();
        }

       

        /// <summary>
        /// Reads all generic db files and loads the data into a 3 dimensional array (object_file_array).
        /// </summary>
        public void read_files()
        {
            
            string[] num_lines = null;
            int line_count = 0;
            int max_lines = 0;

            string[] DB_Name_new = {
            Properties.Resources.NGC_Database1_999,
            Properties.Resources.NGC_Database1000_1999,
            Properties.Resources.NGC_Database2000_2999,
            Properties.Resources.NGC_Database3000_3999,
            Properties.Resources.NGC_Database4000_4999,
            Properties.Resources.NGC_Database5000_5999,
            Properties.Resources.NGC_Database6000_6999,
            Properties.Resources.NGC_Database7000_7840,
            Properties.Resources.Messier_Database1_110,
            Properties.Resources.IC_Database1_1529
            };
            string[] File_Name_new = new string[DB_Name_new.Length];

            // We need to determine the number of lines in the largest database file 
            // so that we can ReDim object_file_array correctly below
            for (int i = 0; i <= DB_Name_new.Length - 1; i++)
            {
                File_Name_new[i] = DB_Name_new[i]; //**
                num_lines = File_Name_new[i].Split(new string[] { "\r\n" }, StringSplitOptions.None); //**

                line_count = num_lines.Length;
                if (line_count > max_lines)
                {
                    max_lines = line_count;
                }
            }

            object_file_array = new string[File_Name_new.Length, max_lines + 1, 3];

            for (int file_count = 0; file_count <= File_Name_new.Length - 1; file_count++)
            {
                count = 0;
                num_lines = File_Name_new[file_count].Split(new string[] { "\r\n" }, StringSplitOptions.None); //**
                total_num_lines = num_lines.Length;
                int i = 0;
                for (int x = 0; x < num_lines.Length-1; x++ )
                {
                    string LineIn_new = num_lines[x];
                    data_array = LineIn_new.Split(new Char[] { ',' });
                    int LastNonEmpty_new = -1;
                    for (i = 0; i <= data_array.Length - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(data_array[i]))
                        {
                            LastNonEmpty_new += 1;
                            data_array[LastNonEmpty_new] = data_array[i];
                        }
                    }
                    Array.Resize(ref data_array, LastNonEmpty_new + 1);
                    if (data_array[0] != "Messier #")
                    {
                        for (int y = 0; y <= 2; y++)
                        {
                            object_file_array[file_count, count, y] = data_array[y];
                        }
                        count += 1;
                    }
                }






            }//

        }

        public double GetObjectRA(string Obj)
        {
            double ra = 0;
                //double functionReturnValue = 0;
                int column = 0;
                int DB_Ref = 0;
                if ((Obj.StartsWith("m") | Obj.StartsWith("M")))
                {
                    column = 0;
                    DB_Ref = 8;
                    Obj = Obj.Replace("m", "");
                    Obj = Obj.Replace("M", "");
                }
                else
                {
                    if ((Obj.StartsWith("ngc") | Obj.StartsWith("NGC")))
                    {
                        column = 0;
                        Obj = Obj.Replace("ngc", "");
                        Obj = Obj.Replace("NGC", "");
                        int xx = Convert.ToInt32(Obj);
                        if (xx >= 0 & xx <= 999) DB_Ref = 0;
                        else if (xx >= 1000 & xx <= 1999) DB_Ref = 1;
                        else if (xx >= 2000 & xx <= 2999) DB_Ref = 2;
                        else if (xx >= 3000 & xx <= 3999) DB_Ref = 3;
                        else if (xx >= 4000 & xx <= 4999) DB_Ref = 4;
                        else if (xx >= 5000 & xx <= 5999) DB_Ref = 5;
                        else if (xx >= 6000 & xx <= 6999) DB_Ref = 6;
                        else if (xx >= 7000 & xx <= 7840) DB_Ref = 7;

                    }
                    else
                    {
                        if ((Obj.StartsWith("ic") | Obj.StartsWith("IC")))
                        {
                            column = 0;
                            DB_Ref = 9;
                            Obj = Obj.Replace("ic", "");
                            Obj = Obj.Replace("IC", "");
                        }
                    }
                }

                string strObj = null;
                double result = 0;
                try
                {
                    for (int i = 0; i <= total_num_lines; i++)
                    {
                        strObj = object_file_array[DB_Ref, i, column];
                        if (Obj == strObj.Replace(" ", ""))
                        {
                            result = Convert.ToDouble(object_file_array[DB_Ref, i, 1]);
                            ra = result;
                            return ra;
                        }
                        else ra = 0;
                    }
                }
                catch
                {
                    ra = 0;
                }
                return ra;
            return ra;
        }



        public double GetObjectDEC(string Obj)
        {
                double functionReturnValue = 0;
                int column = 0;
                int DB_Ref = 0;
                if ((Obj.StartsWith("m") | Obj.StartsWith("M")))
                {
                    column = 0;
                    DB_Ref = 8;
                    Obj = Obj.Replace("m", "");
                    Obj = Obj.Replace("M", "");
                }
                else
                {
                    if ((Obj.StartsWith("ngc") | Obj.StartsWith("NGC")))
                    {
                        column = 0;
                        Obj = Obj.Replace("ngc", "");
                        Obj = Obj.Replace("NGC", "");
                        int xx = Convert.ToInt32(Obj);
                        if (xx >= 0 & xx <= 999) DB_Ref = 0;
                        else if (xx >= 1000 & xx <= 1999) DB_Ref = 1;
                        else if (xx >= 2000 & xx <= 2999) DB_Ref = 2;
                        else if (xx >= 3000 & xx <= 3999) DB_Ref = 3;
                        else if (xx >= 4000 & xx <= 4999) DB_Ref = 4;
                        else if (xx >= 5000 & xx <= 5999) DB_Ref = 5;
                        else if (xx >= 6000 & xx <= 6999) DB_Ref = 6;
                        else if (xx >= 7000 & xx <= 7840) DB_Ref = 7;

                    }
                    else
                    {
                        if ((Obj.StartsWith("ic") | Obj.StartsWith("IC")))
                        {
                            column = 0;
                            DB_Ref = 9;
                            Obj = Obj.Replace("ic", "");
                            Obj = Obj.Replace("IC", "");
                        }
                    }
                }
                string strObj = null;
                double result = 0;
                try
                {
                    for (int i = 0; i <= total_num_lines; i++)
                    {
                        strObj = object_file_array[DB_Ref, i, column];
                        if (Obj == strObj.Replace(" ", ""))
                        {
                            result = Convert.ToDouble(object_file_array[DB_Ref, i, 2]);
                            functionReturnValue = result;
                            return functionReturnValue;
                        }
                    }
                }
                catch
                {
                    //Object does not exist
                    return 0;
                }
                return 0;
            
        }

        public bool IsConnected
        {
            get
            {
                
                switch (_engine)
                {
                    case "Generic":
                        if (object_file_array[0, 0, 0] == "1 ")
                        {
                            return true;
                        } else return false;
                        break;
                }
                return false;
            }
        }


    }
}

