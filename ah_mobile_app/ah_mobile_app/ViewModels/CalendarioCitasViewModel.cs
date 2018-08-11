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
using ah_mobile_app.BaseStructs;
using System.Collections.ObjectModel;
using System.Linq;

namespace ah_mobile_app.ViewModels
{
	public class CalendarioCitasViewModel : ContentView
	{
        public Action DeleteSuccess;
        public Action DisplayError;
        private static bool success;
        public bool Success { get { return success; } }
        public ICommand DeleteAppointment { protected set; get; }
        public int Cita_ID;
        static ObservableCollection<Cita> citas = new ObservableCollection<Cita>();
        public ObservableCollection<Cita> Citas => citas;
        static ObservableCollection<CitaMascota> citasMascota = new ObservableCollection<CitaMascota>();
        public ObservableCollection<CitaMascota> CitasMascota => citasMascota;

        public CalendarioCitasViewModel ()
		{
            citasMascota.Clear();
            citas.Clear();
            foreach (var mascota in AHUtils.Instance.loggedUser.mascotas)
            {
                foreach (var cita in mascota.citas)
                {
                    citas.Add(cita);
                    citasMascota.Add(new CitaMascota(cita.id, cita.fecha, cita.duracion, 
                        mascota.id, mascota.nombre));
                    
                }
            }
            citasMascota = new ObservableCollection<CitaMascota>(
                citasMascota.OrderBy(citaMascota => citaMascota));
            DeleteAppointment = new Command(OnDelete);
        }

        public void OnDelete()
        {
            try
            {
                DeleteAppointmentPost(AHUtils.Instance.loggedUser.cedula, Cita_ID).Wait();
                if (!success)
                {
                    DisplayError();
                }
                else
                {                    
                    DeleteSuccess();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Delete Cita,  Exception caught.", e);
            }


        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async Task DeleteAppointmentPost(string _user_id, int _cita_id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/DeleteCita");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"propietario\":\"" + _user_id + "\"," +
                                  "\"cita_id\":\"" + _cita_id + "\"}";

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
                        AHUtils.Instance.UpdateLocalInfo();
                    }
                }
            }
        }
    }
}