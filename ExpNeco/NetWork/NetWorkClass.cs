
using ExpNeco.Model;
using RestSharp;
using System;
using System.Net;
using System.Windows.Forms;

namespace ExpNeco.NetWork
{
    public class NetWorkClass
    {
        public static bool GetApproval(string fileNO, string PWD)
        {
            var client = new RestClient("192.168.1.2/ManagerApi/api/");
            RestRequest request = new RestRequest("accounts/approvediscard", Method.POST);
            return false;
        }
        private static RestClient client = new RestClient(BaseAddressLayer.BaseAddress);
        
        public static IRestResponse SaveToServer(string ExamType, string newFile, string RegFile, string OperatorId, bool IsDiscard=false)
        {
            IRestResponse response = null;
            var scanData = JsonModel.GetScanData(newFile, RegFile, OperatorId);


            if (scanData == null)
                return response;

            var Data = new ScanDataDTO()
            {
                IsDiscard = IsDiscard,
                Subject = GetDir.ReadRegistry("shortsubj").ToString(),
                Paper = Convert.ToInt32(GetDir.ReadRegistry("Paper")),
                Data = scanData
            };
            //MessageBox.Show(JsonConvert.SerializeObject(Data));
            switch (ExamType)
            {
                case "BECE":
                    RestRequest request = new RestRequest("scanning/bece/save", Method.POST);
                    request.AddJsonBody(Data);
                    response = client.Execute(request);
                    break;
                case "SSCE":
                    break;
                case "NOV":
                    break;
                case "NCEE":
                    break;
            }
          
            //"scanning/bece/save"
            
            return response;
            //return result;
        }
    }
}
