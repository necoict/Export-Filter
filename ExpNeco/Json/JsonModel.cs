using ExpNeco.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExpNeco.Json
{
    public class JsonModel
    {
        public static string CreateExportJson(ScanDataDTO Model)
        {
          string Json =  JsonConvert.SerializeObject(Model);
          //string result=NetWork.NetWorkClass.SaveToServer(Json);
            return Json;
        }

        public static List<ScanData> GetScanData(string FileName, string BatchNumber, string Operator)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            if (lines.Count == 0)
                return null;

            var scanData = new List<ScanData>();
            foreach(var l in lines)
            {
                scanData.Add(new ScanData
                {
                    CandidateNo = l.Substring(0, 10),
                    SchoolNo = "9999999",
                    Subject = l.Substring(14,4),
                    SerialNo = l.Substring(10,4),
                    Response = l.Substring(18,100),
                    FileNo = BatchNumber,
                    UserId = Operator,
                    DeviceId = BatchNumber.Substring(3,2)
                }) ;
            }
            return scanData;
        }
        public static string ReadFile(string FileName)
        {
            using (StreamReader r = new StreamReader(FileName))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
    }
}
