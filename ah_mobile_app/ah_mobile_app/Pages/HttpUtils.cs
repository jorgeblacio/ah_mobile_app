using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ah_mobile_app.BaseStructs;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ah_mobile_app
{
    class AHUtils
    {
        private static AHUtils instance;
        public static readonly HttpClient client = new HttpClient();
        public static MasterDetailPage masterDetailPage;
        private AHUtils() { }
        public Cliente loggedUser;
        public static AHUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AHUtils();
                }
                return instance;
            }
        }
        public void UpdateLocalInfo()
        {
            GetInfoFromLogin(loggedUser.email);
        }

        public void GetInfoFromLogin(string _email)
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/AllCitas");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"username\":\"" + _email + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Instance.loggedUser = JsonConvert.DeserializeObject<Cliente>(result);
                    Console.WriteLine(Instance.loggedUser.ToString());
                }
            }
        }        
    }
}
