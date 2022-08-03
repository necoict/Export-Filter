

using System.Collections.Generic;

namespace ExpNeco.Model
{
    public class ScanData
    {
        public string CandidateNo { get; set; }
        public string SchoolNo { get; set; }
        public string Subject { get; set; }
        public string SerialNo { get; set; }
        public string Response { get; set; }
        public string FileNo { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }


    public class ScanDataDTO
    {
        public bool IsDiscard { get; set; }
        public string Subject { get; set; }
        public int Paper { get; set; }
        public List<ScanData> Data { get; set; }
    }
    public class ResponseModel
    {
        public string Message { get; set; }
    }
}
