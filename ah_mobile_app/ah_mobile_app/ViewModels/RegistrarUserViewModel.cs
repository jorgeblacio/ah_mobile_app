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
using System.Text.RegularExpressions;
using ah_mobile_app.Validators;

namespace ah_mobile_app.ViewModels
{
	public class RegistrarUserViewModel : INotifyPropertyChanged
	{
        public Action<String> DisplayError;
        public Action SignUpSuccess;

        private SignUpValidator validator = new SignUpValidator();


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string nombre;

        private static bool success;

        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Nombre"));
            }
        }
        private string cedula;
        public string Cedula
        {
            get { return cedula; }
            set
            {
                cedula = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Cedula"));
            }
        }
        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Direccion"));
            }
        }
        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Telefono"));
            }
        }
        private string email;
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
        private string password2;
        public string Password2
        {
            get { return password2; }
            set
            {
                password2 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password2"));
            }
        }
        public ICommand RegistrarUserCommand { protected set; get; }
        public RegistrarUserViewModel()
        {
            RegistrarUserCommand = new Command(Registrar);
        }
        public void Registrar()
        {
            try
            {
                if (validator.Validate(this))
                {
                    PostLogin(nombre, cedula, direccion, telefono, email, password, password2).Wait();
                    if(success)
                    {
                        var login = new LoginPage();
                        SignUpSuccess();
                        App.Current.MainPage.Navigation.PopAsync();
                        App.Current.MainPage.Navigation.PushAsync(login);
                        Console.WriteLine("SUCCESS");
                    }
                    else
                    {
                        DisplayError("Error de registro, intente nuevamente.");
                    }
                }

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Registro, Exception caught.", e.Message);
            }
            

        }

        protected virtual void OnComplete(string propertyName)
        {
            
        }

        static async Task PostLogin(string _nombre, string _cedula, string _direccion, string _telefono, string _email, string _password, string _password2)
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/cliente");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                { 
                    string json = "{\"cedula\":\"" + _cedula + "\"," +
                                  "\"nombre\":\"" + _nombre + "\"," +
                                  "\"direccion\":\"" + _direccion + "\"," +
                                  "\"telefono\":\"" + _telefono + "\"," +
                                  "\"email\":\"" + _email + "\"," +
                                  "\"password\":\"" + _password + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    success = result.Contains("success");
                }
            }
        }
    }
}