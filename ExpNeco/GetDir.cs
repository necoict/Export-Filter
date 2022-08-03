using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace ExpNeco
{
   public class GetDir
    {
       
        public static string FetchMainTempDirectory(string Dir)
        {
           
                return @"c:\program files\necoscan\scan\data\"+Dir+@"\";
            
        }

        public static string FetchDiscardTempDirectory(string Dir)
        {
            // ReadRegistry("sosDir") + @"\Disc_" + UtilityClass.Right(ReadRegistry("scanDir"), 2);
            //Dir.Substring(3, mExportDir.Length - 3));
            return @"c:\program files\necoscan\scan\data\" + Dir.Substring(3, Dir.Length - 3) +  @"\Disc_" + UtilityClass.Right(ReadRegistry("scanDir"), 2); ;

        }

        internal static string ReadRegistry(string rKey)//reading registry values
        {
            var rtValue="";
            try
            {

                RegistryKey mICParams = Registry.CurrentUser; //declare registry variable
                mICParams = mICParams.OpenSubKey("software", true);//open the registry subkey(ie Software)
                foreach (string Keyname in mICParams.GetSubKeyNames())//loop through all entries in the CurrentUser/Software key
                {

                    if (Keyname == "necoscan")//check if necoscan exists 
                    {
                        mICParams = mICParams.OpenSubKey("necoscan", true);//open the key
                        rtValue = mICParams.GetValue(rKey).ToString();//read value into variable
                        break;//terminate the loop.

                    }

                }
                mICParams.Close();//close the reg key.
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Export Filter - Registry");
            }

            return rtValue;//return the value read.
        }

    }
}
