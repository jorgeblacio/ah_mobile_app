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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using ah_mobile_app.Validators;

namespace ah_mobile_app.ViewModels
{
	public class ReservaCitaViewModel : INotifyPropertyChanged
    {
        public Action RegistrySuccess;
        public Action DisplayInvalidDay;
        public Action<String> DisplayError;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        List<Mascota> mascotas = new List<Mascota>();
        List<String> mascotaNombres = new List<String>();
        public int Mascota_ID_Index = -1;
        public int Available_Time_Index = -1;
        public List<Mascota> Mascotas => mascotas;
        public List<String> MascotaNombres => mascotaNombres;
        static ObservableCollection<String> available = new ObservableCollection<String>();
        public ObservableCollection<String> Available => available;
        private static bool success;

        private AgendarCitaValidator validator = new AgendarCitaValidator();

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set
            {
                fecha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Fecha"));
            }
        }
        public ICommand RegistrarCitaCommand { protected set; get; }
        public ReservaCitaViewModel()
        {
            Fecha = DateTime.Now;
            mascotas = AHUtils.Instance.loggedUser.mascotas;
            foreach (var mascota in mascotas)
            {
                mascotaNombres.Add(mascota.nombre);
            }
                
            UpdateAvailableHours();
            RegistrarCitaCommand = new Command(Registrar);
        }

        public void Registrar()
        {


            try
            {
                if (validator.Validate(this))
                {
                    var index = Available[Available_Time_Index].IndexOf(':');
                    Fecha = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, 
                        Int32.Parse(Available[Available_Time_Index].Substring(0, index)), 0, 0);
                    RegisterDate(Fecha, Mascotas[Mascota_ID_Index].id).Wait();
                    if(success)
                    {
                        var inicio = new InicioPageDetail();
                        RegistrySuccess();
                        AHUtils.masterDetailPage.Detail.Navigation.PopToRootAsync();

                        Console.WriteLine("SUCCESS");
                    }else
                    {
                        DisplayError("Ocurrio un error al resgitrar la cita, intentelo nuevamente.");
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Registro, Exception caught.", e);
            }


        }

        public void UpdateAvailableHours()
        {
            getAvailableHours(fecha).Wait();
        }

        async Task getAvailableHours(DateTime date)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/Available");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"day\":\"" + date.Date + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                if (result.Contains("fail_invalid_day"))
                {
                    DisplayInvalidDay();
                    return;
                }
                JObject jObject = JObject.Parse(result);
                JToken jToken = jObject.GetValue("available");
                Available.Clear();
                var currentTime = DateTime.Now;
                foreach (var value in jToken.Values<int>())
                {
                    if(value + 1 == currentTime.Hour && currentTime.Minute <= 30)
                        Available.Add(value + ":00");
                    else if(value > currentTime.Hour && value != currentTime.Hour + 1)
                        Available.Add(value + ":00");
                }
            }
        }

        static async Task RegisterDate(DateTime _fecha, int _mascota_id)
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/cita");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"propietario\":\"" + AHUtils.Instance.loggedUser.cedula + "\"," +
                                  "\"fecha\":" + JsonConvert.ToString(_fecha) + "," +
                                  "\"mascota_id\":\"" + _mascota_id + "\"}";

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
                    {
                        if (success)
                            AHUtils.Instance.UpdateLocalInfo();
                    }

                }
            }
        }





    }
}
