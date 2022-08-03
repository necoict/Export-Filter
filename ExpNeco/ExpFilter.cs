
/*NECO scanning export custom filter
 *This a COM object that must expose all methods in the companion
 please note that all arguments and datatypes must match those in the companion*/

using System;
using System.Runtime.InteropServices;//this namespace allows the COM to be registered and seen by others objects
using System.IO;// this namespace contains the file processing classes
using System.Windows.Forms;
using ExpNeco.Infrastructure;

namespace ExpNeco
{
    /// <summary>
    /// Interface that exposes moethods in the custom Export filter
    /// </summary>
    /// Please do not modify this GUID.

    [Guid("694C1820-04B6-4988-928F-FD858B95C880")]
    public interface IExpNeco
    {
        [DispId(1)]
        void StartBatch(string ExportDir, long BatchNo, string Params, string Operator, string TrackingId, long ExpectedSheets, string Comment, string UserId, string MachineName);
        [DispId(2)]
        void ExportRecord(long SheetNum, string OMRBuffer, int Status, string FormName);
        [DispId(3)]
        void EndBatch(bool Commit, string Statistics);
        //void SheetClipping(string OMRData, SheetStatus ASheetStatus, string FormName);
    }

       
    [Guid("472C2800-572C-4058-8750-A78D42C7CDAE"),ClassInterface(ClassInterfaceType.None)]//Implementing the class interface above
    public class ExpFilter:IExpNeco 
    {
        string ExportFolder,OperatorName;
        //long mBatchNo;
        string ScanFileName;
         string msubjcode;
        string RegFile;
        //string hiddenScanFile;
        
        RegistryHelperClass _rHelper = new RegistryHelperClass();
        //string HiddenBaseDirectory;
      /*  protected void SetParameters()
        {
            try
            {
                if (ReadRegistry("ExamType") == "NCEE" || ReadRegistry("ExamType") == "NEEFUSSC")
                {
                    string sFile = mBatchNo.ToString() + ".txt";
                    RegFile = sFile;
                    string CreateScanFile = mExportDir + @"\" + sFile;
                    newFile = mExportDir + @"\" + sFile;
                    CreateTextFile(CreateScanFile);
                }
                else
                {
                    string code = ReadRegistry("stateCode");//Retrieving state code from the utility class object
                    string sFileExt = UtilityClass.Right(mBatchNo.ToString(), 3);/*extracts the batch file extension from the batchnumber
                                                                       * eg(50102001) the extension becomes ".001"
                                                                       */
                    
                   // string sFile = mBatchNo.ToString().Substring(0, Convert.ToInt32((mBatchNo.ToString().Length - 3)));//Create the file name eg(5102)
                    //newFile = mExportDir + @"\" + sFile + code + "." + sFileExt; //concatenate the name  & extension
                    //RegFile = sFile + code + "." + sFileExt;
                    //string CreateScanFile = mExportDir + @"\" + sFile + code + "." + sFileExt; //create the new scanfile name that includes export dir.
                    //HiddenBaseDirectory = GetDir.FetchMainTempDirectory( mExportDir.Substring(3, mExportDir.Length - 3));
                    //System.Windows.Forms.MessageBox.Show(HiddenBaseDirectory);
                    //newFile = CreateScanFile;
                    //hiddenScanFile = HiddenBaseDirectory  + sFile + code + "." + sFileExt;
                    //System.Windows.Forms.MessageBox.Show(HiddenBaseDirectory);
                    /*if (!Directory.Exists(HiddenBaseDirectory))
                    {
                        Directory.CreateDirectory(HiddenBaseDirectory);
                    }
                    CreateTextFile(CreateScanFile);
                    //CreateTextFile(hiddenScanFile);
                }
              
                WriteBatchnumber(RegFile);// go to file creation module
            }
            catch (Exception ex)// trap any errors here.
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Set Parameters"); //display errors traped
            }
        }*/

        protected void CreateTextFile(string sFile)//Create the scanfile
        {
            try
            {

                if (!File.Exists(sFile)) //check if scan file exist, if no create it else leave it.
                {
                    StreamWriter sw; // declare the stream writer class used to create the file
                    sw = File.CreateText(sFile); //create the file
                    sw.Close(); //close the stream writer class
                }
                
            }
            catch (Exception ex)//trap any errors here
            {
                MessageBox.Show(ex.Message+" "+ sFile, "Create Text File"); // display the errors trapped.
            }
        }

        /*internal string ReadRegistry(string rKey)//reading reistry values
        {
            try{
            
            RegistryKey mICParams = Registry.CurrentUser; //declare registry variable
            mICParams = mICParams.OpenSubKey("software", true);//open the registry subkey(ie Software)
            foreach (string Keyname in mICParams.GetSubKeyNames())//loop through all entries in the CurrentUser/Software key
            {

                if (Keyname == "necoscan")//check if necoscan exists 
                {
                    mICParams = mICParams.OpenSubKey("necoscan", true);//open the key
                    rtValue   = mICParams.GetValue(rKey).ToString();//read value into variable
                    break;//terminate the loop.

                }

            }
            mICParams.Close();//close the reg key.
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,  "Export Filter - Registry");
            }
            
            return rtValue ;//return the value read.
        }*/


        /*internal void WriteBatchnumber(string batchnumber)//write the batch file name to the system registry
        {
            try
            {
                RegistryKey mICParams = Registry.CurrentUser;
                mICParams = mICParams.OpenSubKey("software", true);
                foreach (string Keyname in mICParams.GetSubKeyNames())
                {

                    if (Keyname == "necoscan")
                    {
                        mICParams = mICParams.OpenSubKey("necoscan", true);
                        mICParams.SetValue("batchnumber", batchnumber);
                        break;

                    }

                }
                mICParams.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,  "Export Filter - Write BatchNo.");
            }

        }*/

       


        public void StartBatch(string ExportDir,long BatchNo,string Params,string Operator,string TrackingId,long ExpectedSheets,string Comment,string UserId,string  MachineName)
        {

            try
            {
                
                ExportFolder = ExportDir.ToUpper();
                var BatchFileName = _rHelper.BatchNumber;//  ReadRegistry("batchnumber");
                var SOSInpFileName = _rHelper.SOSInpBatchFileName;// ReadRegistry("SOSInpBatchFileName");
                RegFile = BatchFileName;

                //MessageBox.Show($"{BatchNo.ToString().Trim()} VS {SOSInpFileName.Trim()}");

                if (BatchNo.ToString().Trim() != SOSInpFileName.Trim())
                {
                    var SOSInpFileCounter = UtilityClass.Right(BatchNo.ToString(), 3);
                    var BatchFileCounter = UtilityClass.Right(BatchFileName, 3);
                    BatchFileName = BatchFileName.Replace(BatchFileCounter, SOSInpFileCounter);
                    RegFile = BatchFileName;
                    //MessageBox.Show(BatchFileName);
                    _rHelper.BatchNumber = BatchFileName;
                    //WriteBatchnumber(BatchFileName);

                }
  

                OperatorName = Operator.Trim();
                //mySupervisor =  ReadRegistry("Supervisor");
                msubjcode = _rHelper.SubjCode;// ReadRegistry("subjCode");

                if (string.IsNullOrEmpty(OperatorName))
                {
                    OperatorName = _rHelper.UID;// ReadRegistry("UID");
                }

                ScanFileName = $"{ExportFolder}\\{BatchFileName}";



            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot Start Batch "+ex.Message,"Start Batch");
            }
        }

        public void ExportRecord(long SheetNum, string OMRBuffer, int Status, string FormName)//export read data to scan file
        {
            try
            {
                if (Status == 0) return;
                var _file = Path.GetFileName(ScanFileName);
                string _shrtsubj = _rHelper.ShortSubj;
                var _status = Status;
                if(_rHelper.Job == "Obj" && _rHelper.ExamType == "SSCE")
                {
                    if (_file.Length < 11)
                    {
                        var msg = $"The file name has invalid length : {_file} \n" +
                           "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }


                    if (_shrtsubj == "ORA")
                    {
                        if (_file.Substring(0, 3) != "909")
                        {
                            var msg = $"The file name contains invalid subject code : {_file} \n" +
                                "Please discard this file and reselect the subject to continue";
                            MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _status = 0;
                        }
                    }
                    else
                    {
                        if (_file.Substring(0, 3) != _rHelper.SubjCode.Substring(0, 3))
                        {
                            var msg = $"The file name contains invalid subject code : {_file} \n" +
                                "Please discard this file and reselect the subject to continue";
                            MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _status = 0;
                        }
                    }

                    if (_file.Substring(3, 2) != _rHelper.DeviceId)
                    {
                        var msg = $"The file name contains invalid system number : {_file} \n" +
                               "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }

                    if (_file.Substring(5, 2) != _rHelper.StateCode)
                    {
                        var msg = $"The file name contains invalid state code : {_file} \n" +
                               "Please discard this file and reselect the subject and state to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }
                }


                if (_rHelper.Job == "Obj" && _rHelper.ExamType == "BECE")
                {
                    if (_file.Length < 11)
                    {
                        var msg = $"The file name has invalid length : {_file} \n" +
                           "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }

                    if (_file.Substring(3, 2) != _rHelper.DeviceId)
                    {
                        var msg = $"The file name contains invalid system number : {_file} \n" +
                               "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }

                    var _subjCode = (_rHelper.SubjCode.Substring(1, 1) == "0") ? "9" + _rHelper.SubjCode.Substring(2, 2) :
                        _rHelper.SubjCode.Substring(1, 3);
                    if (_file.Substring(0, 3) != _subjCode)
                    {
                        var msg = $"The file name contains invalid subject code : {_file} \n" +
                            "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }
                }

                if (_rHelper.Job == "Essay")
                {
                    if (_file.Length < 10)
                    {
                        var msg = $"The file name has invalid length : {_file} \n" +
                           "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }

                    if (_file.Substring(0, 4) != _rHelper.SubjCode)
                    {
                        var msg = $"The file name contains invalid subject code : {_file} \n" +
                            "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }

                    if (_file.Substring(4, 2) != _rHelper.DeviceId)
                    {
                        var msg = $"The file name contains invalid system number : {_file} \n" +
                               "Please discard this file and reselect the subject to continue";
                        MessageBox.Show(msg, "Export record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _status = 0;
                    }
                }

               
                
               

               

                if (_status == 1)//check if sheet status is good(ie 1 for good, 0 for bad sheet)
                {
                    using (StreamWriter sw = new StreamWriter(ScanFileName, true))//declare the sream writer class
                    {
                        string OMRData = $"{OMRBuffer} {msubjcode} {RegFile} {OperatorName}";
                        // string.Format("{0} {1} {2} {3}", OMRBuffer, msubjcode, RegFile, myOperator);
                        sw.WriteLine(OMRData);//place content of OMRbuffer into the file.
                        sw.Close();//close the stream writer.
                    }

                  /*  using (StreamWriter sw = new StreamWriter(hiddenScanFile, true))//declare the sream writer class
                    {
                        sw.WriteLine(OMRBuffer + " " + msubjcode + " " + RegFile + " " + myOperator);//place content of OMRbuffer into the file.
                        sw.Close();//close the stream writer.

                    }*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot Export Record "+ex.Message,  "Export Record");
            }
        }

        /// <summary>
        /// This Method is called when a batch has ended.
        /// </summary>
        /// <param name="Commit"></param>
        /// Indicates if the batch was either accepted or discarded.
        /// <param name="Statistics"></param>
        public void EndBatch(bool Commit, string  Statistics)//saves or deletes the scan file created above.
        {
            try
            {
                if (Commit != true)//check if the user accepted the batch
                {
                    string discardDir="";
                    string discardFile = "";
                    discardDir = string.Format($"{_rHelper.SosDir}\\Disc_{UtilityClass.Right(_rHelper.ScanDir,2)}");
                    
                    if (!Directory.Exists(discardDir))
                    {
                        Directory.CreateDirectory(discardDir);
                    }
                     discardFile = string.Format($"{discardDir}\\{RegFile}") ;
                    // string encryptedDiscardFile = string.Format($"{discardDir}\\E{RegFile}");
                    if (!File.Exists(ScanFileName)) return;
                    var file = Path.GetFileName(ScanFileName);
                    //var deviceid = _rHelper.DeviceId;
                    //if no, move scan file to discard folder
                    File.Move(ScanFileName, discardFile);
                    //WriteBatchnumber("000000.000");//reset batch no to 00000
                    //CryptograhyClass.Encrypt(discardFile, encryptedDiscardFile, CryptograhyClass.EncryptionPWD);


                    using (System.Diagnostics.Process P = new System.Diagnostics.Process())
                    {
                        P.StartInfo.UseShellExecute = false;
                        // You can start any process, HelloWorld is a do-nothing example.
                        P.StartInfo.FileName = "c:\\program files\\necoscan\\scan\\CopyHelperApp.exe";
                        P.StartInfo.Arguments = discardFile;
                        //P.StartInfo.RedirectStandardOutput = true;
                        P.StartInfo.CreateNoWindow = true;
                        P.Start();
                    }

                    //var examType = ReadRegistry("examType").ToString();
                    //var Exams = UtilityClass.GetExams(examType);
                    //var operatorId = ReadRegistry("OperatorId").ToString();
                    //var response = NetWorkClass.SaveToServer(Exams, discardFile, RegFile, operatorId, true);
                    /*if (response == null)
                    {
                        MessageBox.Show("File does not contain any data to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<ResponseModel>(response.Content);
                        MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to save data to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }*/

                }

                if (Commit == true)
                {
                    //var fileName = RegFile;
                    //var scanDir = ReadRegistry("scanDir");
                    //var tempfile = Path.Combine(scanDir, "temp.neco");
                    //CryptograhyClass.Encrypt(newFile, tempfile, CryptograhyClass.EncryptionPWD);
                    //MessageBox.Show(myOperator);
                    //MessageBox.Show(newFile);
                    //MessageBox.Show(RegFile);

                    //var JString = JsonModel.CreateExportJson(scanData);
                    //MessageBox.Show(JString);

                    //var examType = ReadRegistry("examType").ToString();
                    //var Exams = UtilityClass.GetExams(examType);
                    //var operatorId = ReadRegistry("OperatorId").ToString();
                    //var response = NetWorkClass.SaveToServer(Exams, newFile, RegFile, operatorId);
                    /*if (response == null)
                    {
                        MessageBox.Show("File does not contain any data to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<ResponseModel>(response.Content);
                        MessageBox.Show(result.Message,"Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to save data to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }*/
                    try
                    {
                        using (System.Diagnostics.Process P = new System.Diagnostics.Process())
                        {
                            P.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            P.StartInfo.FileName = "c:\\program files\\necoscan\\scan\\CopyHelperApp.exe";
                            P.StartInfo.Arguments = ScanFileName;
                            //P.StartInfo.RedirectStandardOutput = true;
                            P.StartInfo.CreateNoWindow = true;
                            P.Start();
                            //P.WaitForExit();
                            //var exitCode = P.ExitCode;
                        }
                       
                        //p.StandardOutput.ReadToEnd().Dump();
                        //System.Diagnostics.Process.Start("c:\\program files\\necoscan\\scan\\CopyHelperApp.exe", newFile);
                    }
                    catch(Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error" + ex.Message, "End Batch");
                    }
                   
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Cannot End Batch " + ex.Message, "End Batch");
            }
            
        }

        internal void LockScanFile(string fileIn, string fileOut)
        {

        }
        public void SetupParams(string Params)
        {
            //System.Windows.Forms.MessageBox.Show(Params);
        }

        
    }
}
