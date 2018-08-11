using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net;
using System.IO;
using ah_mobile_app.Pages;

namespace ah_mobile_app.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action<String> DisplayInvalidLoginPrompt;
        public Action DisplayServerErrorPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string email;
        
        private static bool success;

        public bool Success
        {
            get
            {
                return success;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
           }
        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public void OnSubmit()
        {
            if (Password == null || Email == null)
            {
                DisplayInvalidLoginPrompt("Login inválido, los campos no pueden estar vacios");
                return;
            }
            try
            {
                PostLogin(email, password, this).Wait();
                if (!success)
                {
                    DisplayInvalidLoginPrompt("Login inválido.");
                }
                else
                {
                    Console.WriteLine("SUCCESS");
                    var inicio = new InicioPage();
                    inicio.email = email;
                    Application.Current.MainPage = inicio;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} login,  Exception caught.", e);
            }


        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async Task PostLogin(string _email, string _password, LoginViewModel model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/login");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"username\":\"" + _email + "\"," +
                                  "\"password\":\"" + _password + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        success = result.Contains("success");
                        if (success)
                            AHUtils.Instance.GetInfoFromLogin(_email);
                    }
                }
                catch(WebException e)
                {
                    model.DisplayServerErrorPrompt();
                }
                
            }
        }
    }
}