using ah_mobile_app.Pages;
using ah_mobile_app.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ah_mobile_app.ViewModels
{
    public class RegistroMascotasViewModel : INotifyPropertyChanged
    {
        private static bool success;
        public Action<String> DisplayError;
        public Action RegistrySuccess;

        private MascotaRegistroValidator validator = new MascotaRegistroValidator();

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private int edad;
        public string Edad
        {
            get
            {
                return edad.ToString();
            }
            set
            {
                try
                {
                    edad = int.Parse(value);
                    PropertyChanged(this, new PropertyChangedEventArgs("Edad"));
                }
                catch
                {

                }
            }
        }
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Nombre"));
            }
        }

        private string especie;
        public string Especie
        {
            get { return especie; }
            set
            {
                especie = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Especie"));
            }
        }
        private float peso;
        public string Peso
        {
            get
            {
                return peso.ToString();
            }
            set
            {
                try
                {
                    peso = float.Parse(value);
                    PropertyChanged(this, new PropertyChangedEventArgs("Peso"));
                }
                catch
                {

                }
            }
        }

        private string raza;
        public string Raza
        {
            get { return raza; }
            set
            {
                raza = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Raza"));
            }
        }


        public ICommand RegistrarMascotaCommand { protected set; get; }



        public RegistroMascotasViewModel()
        {

            RegistrarMascotaCommand = new Command(Registrar);


        }


        public void Registrar()
        {


            try
            {
                if (validator.Validate(this))
                {
                    RegisterPet(nombre, raza, Edad, Peso).Wait();
                    if (success)
                    {
                        var inicio = new InicioPageDetail();
                        RegistrySuccess();
                        AHUtils.masterDetailPage.Detail.Navigation.PopAsync();

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
                Console.WriteLine("{0} Registro, Exception caught.", e);
            }


        }

        static async Task RegisterPet(string _nombre, string _raza, string _edad, string _peso)
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/mascota");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"nombre\":\"" + _nombre + "\"," +
                                  "\"propietario\":\"" + AHUtils.Instance.loggedUser.cedula + "\"," +
                                  "\"raza\":\"" + _raza + "\"," +

                                  "\"peso\":\"" + _peso + "\"," +
                                  "\"edad\":\"" + _edad + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    success = result.Contains("success");
                    if (success)
                        AHUtils.Instance.UpdateLocalInfo();
                }
            }
        }


    }
}
