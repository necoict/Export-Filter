using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TestExpNeco
{
    public class DataModel
    {
        public string fileName { get; set; }
        public string Response { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DataModel model = new DataModel
                {
                    fileName = "the file",
                    Response="responses"
                };
                string Json = JsonConvert.SerializeObject(model);
                    //"{\"messageType\":\"response\",\"response\":\"Success: Device reboot successfull\" ,\"destination\":\"3\"}";
                SendScanFileToServer(Json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
             
        }


        public static string SendScanFileToServer(string Json)
        {

            string url = "http://192.168.1.31:80/ManagerApi/api/";
            ///CreateBiodata

            try
            {
                RestClient client = new RestClient();
                client.BaseUrl = new Uri(url);
                RestRequest request = new RestRequest();
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.Timeout = 60 * 60000;//100000;
                //Pass String Data  to the request body
                request.AddBody(Json);
                request.Resource = "Data";

                // Post the Data to the Server.
                RestResponse response = client.Execute(request) as RestResponse;
                //var execTask = client.Execute(request);

                if ((response != null) && ((response.StatusCode == HttpStatusCode.OK) && (response.ResponseStatus == ResponseStatus.Completed)))
                {
                    // dynamic obj = response.Content;
                    string res = response.Content;
                    Console.WriteLine(res);

                    Console.ReadLine();
                    return res;
                }
                else if ((response != null))
                {
                    Console.WriteLine(string.Format("Status code is {0} ({1}); response status is {2}", response.StatusCode, response.StatusDescription, response.ResponseStatus));
                    //System.Windows.Forms.MessageBox.Show("Pin and Biometrics Code Verifcation falied!\n" +
                    //                               "check your Internet connection and try again", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Console.ReadLine();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
                Console.ReadLine();
                return string.Empty;
            }

            return string.Empty;

        }


        [DllImport("advapi32.DLL", SetLastError = true)]
        public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        private static void Button()
        {
            IntPtr admin_token = default(IntPtr);
            WindowsIdentity wid_current = WindowsIdentity.GetCurrent();
            WindowsIdentity wid_admin = null;
            WindowsImpersonationContext wic = null;
            try
            {
               Console.WriteLine("Copying file...");
                if (LogonUser("adahadapato@gmail.com", "DESKTOP-T27RJCI", "emypato4me", 9, 0, ref admin_token) != 0)
                {
                    wid_admin = new WindowsIdentity(admin_token);
                    wic = wid_admin.Impersonate();
                    System.IO.File.Copy("C:\\9611802.001", "\\\\192.168.1.21\\testnew\\9611802.001", true);
                    Console.WriteLine("Copy succeeded");
                }
                else
                {
                    Console.WriteLine("Copy Failed");
                }
            }
            catch (System.Exception se)
            {
                int ret = Marshal.GetLastWin32Error();
                Console.WriteLine(ret.ToString(), "Error code: " + ret.ToString());
                Console.WriteLine(se.Message);
            }
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
            Console.ReadLine();
        }
    }

    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }

    public class ImpersonationHelper
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private extern static bool CloseHandle(IntPtr handle);

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Impersonate(string domainName, string userName, string userPassword, Action actionToExecute)
        {
            SafeTokenHandle safeTokenHandle;
            try
            {

                const int LOGON32_PROVIDER_DEFAULT = 0;
                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_INTERACTIVE = 2;

                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(userName, domainName, userPassword,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    out safeTokenHandle);
                //Facade.Instance.Trace("LogonUser called.");

                if (returnValue == false)
                {
                    int ret = Marshal.GetLastWin32Error();
                    //Facade.Instance.Trace($"LogonUser failed with error code : {ret}");

                    throw new System.ComponentModel.Win32Exception(ret);
                }

                using (safeTokenHandle)
                {
                    //Facade.Instance.Trace($"Value of Windows NT token: {safeTokenHandle}");
                    //Facade.Instance.Trace($"Before impersonation: {WindowsIdentity.GetCurrent().Name}");

                    // Use the token handle returned by LogonUser.
                    using (WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle()))
                    {
                        using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                        {
                            //Facade.Instance.Trace($"After impersonation: {WindowsIdentity.GetCurrent().Name}");
                            //Facade.Instance.Trace("Start executing an action");

                            actionToExecute();

                            //Facade.Instance.Trace("Finished executing an action");
                        }
                    }
                    //Facade.Instance.Trace($"After closing the context: {WindowsIdentity.GetCurrent().Name}");
                }

            }
            catch (Exception ex)
            {
                //Facade.Instance.Trace("Oh no! Impersonate method failed.");
                //ex.HandleException();
                //On purpose: we want to notify a caller about the issue /Pavel Kovalev 9/16/2016 2:15:23 PM)/
                throw;
            }
        }
    }
}
