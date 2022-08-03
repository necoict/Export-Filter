

using System;

namespace ExpNeco.NetWork
{
    public class HttpClientInstance
    {
        private const string BaseUri = "http://192.168.1.2/";
        /*public static HttpClient Instance
        {
            get { return new HttpClient { BaseAddress = new Uri(BaseUri) }; }
        }*/
    }
    public class BaseAddressLayer
    {
        public static string BaseAddress
        {
            get
            {
                return "http://192.168.1.2/ManagerApi/api/";
            }
        }
    }
}
