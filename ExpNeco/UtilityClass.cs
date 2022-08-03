using System;
using System.Collections.Generic;
using System.Text;

namespace ExpNeco
{
    class UtilityClass
    {
        public static string GetExams(string examType)
        {
            if (examType.Contains("JUN/JUL")) return "SSCE";
            if (examType.Contains("NOV/DEC")) return "NOV";
            if (examType.Contains("BECE")) return "BECE";
            if (examType.Contains("NCEE")) return "NCEE";
            return null;
        }
        //static string stateCode;
        static int Position;
        /*public static string GetStateCode(string mState)
        {
            try
            {
                string[] state = {"Abia", "Adamawa", "Akwa Ibom", "Anambra", "Bauchi", "Benue", "Borno", "Cross River",
                                 "Delta", "Edo", "Enugu", "Imo", "Jigawa", "Kaduna", "Kano", "Katsina", "Kebbi",
                                 "Kogi", "Kwara", "Lagos", "Niger", "Ogun", "Ondo", "Osun", "Oyo", "Plateau", "Rivers",
                                 "Sokoto", "Taraba", "Yobe", "FCT", "Bayelsa", "Ebonyi", "Ekiti", "Gombe", "Nasarawa", "Zamfara"};

                string[] StateCode ={"01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16",
                                "17","18","19","20","21","22","23","24","25","26","27","28","29","30","31","32","33",
                                "34","35","36","37"};

                for (int i = 0; i < state.Length; i++)
                {
                    if (state[i] == mState)
                    {
                        stateCode = StateCode[i];
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Get State Code, Export Filter");
            }
            return stateCode;
        }*/

        public static  int Instr(string tString, string SearchItem)
        {
            try
            {
                char[] store = new char[tString.Length];
                for (int i = 0; i < tString.Length; i++)
                {
                    store[i] = tString[i];

                }
                for (int j = 0; j < tString.Length; j++)
                {
                    if (store[j].ToString() == SearchItem)
                    {
                        Position = j;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                Position = 0;
            }
            return Position;
        }
        public static string Left(string str, int index)
        {
            string iLeft = "";
            iLeft = str.Substring(0, index);
            return iLeft;
        }
        public static string Right(string str, int index)
        {
            string iRight = "";
            iRight = str.Substring((str.Length - index), index);
            return iRight;
        }
    }
}
