using ah_mobile_app.Pages;
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
using ah_mobile_app.BaseStructs;
using ah_mobile_app.Validators;

namespace ah_mobile_app.ViewModels
{
    class EditarMascotaModelView
    {
        private static bool success;
        public Action<String> DisplayError;
        public Action EditSuccess;
        int mascota_id;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private EditarMascotaValidator validator = new EditarMascotaValidator();

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


        public ICommand EditarMascotaCommand { protected set; get; }



        public EditarMascotaModelView(Mascota _mascota)
        {
            mascota_id = _mascota.id;
            this.edad = _mascota.edad;
            this.raza = _mascota.raza;
            this.nombre = _mascota.nombre;
            this.peso = _mascota.peso;
            EditarMascotaCommand = new Command(Editar);
        }


        public void Editar()
        {
            try
            {
                if (validator.Validate(this))
                {
                    EditPet(mascota_id, nombre, raza, Edad, Peso).Wait();
                    if(success)
                    {
                        var inicio = new InicioPageDetail();
                        EditSuccess();
                        AHUtils.masterDetailPage.Detail.Navigation.PopAsync();

                        Console.WriteLine("SUCCESS");
                    }else
                    {
                        DisplayError("Ocurrio un error al editar la mascota, intentelo nuevamente.");
                    }
                }
                

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Registro, Exception caught.", e);
            }


        }

        static async Task EditPet(int _id, string _nombre, string _raza, string _edad, string _peso)
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/EditMascota");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"mascota_id\":\"" + _id + "\"," +
                                  "\"nombre\":\"" + _nombre + "\"," +
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
