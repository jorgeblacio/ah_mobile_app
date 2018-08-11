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
using System.Collections.ObjectModel;
using ah_mobile_app.BaseStructs;

namespace ah_mobile_app.ViewModels
{
	public class InicioPageViewModel : ContentView
	{
        public Action DeleteSuccess;
        public Action DisplayError;
        private static bool success;
        public bool Success { get { return success; } }
        public ICommand DeletePet { protected set; get; }
        public int Mascota_ID;
        static ObservableCollection<Mascota> mascotas = new ObservableCollection<Mascota>();
        public ObservableCollection<Mascota> Mascotas => mascotas;
        public InicioPageViewModel()
        {
            mascotas.Clear();
            foreach (var mascota in AHUtils.Instance.loggedUser.mascotas)
            {                
                mascotas.Add(mascota);
            }
            DeletePet = new Command(OnDelete);
        }

        public void OnDelete(Object _mascota)
        {
            try
            {
                DeletePetPost(AHUtils.Instance.loggedUser.cedula, Mascota_ID).Wait();
                if (!success)
                {
                    DisplayError();
                }
                else
                {
                    DeleteSuccess();
                    Mascotas.Remove((Mascota)_mascota);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} login,  Exception caught.", e);
            }


        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async Task DeletePetPost(string _user_id, int _mascota_id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            using (var client = AHUtils.client)
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ec2-18-191-1-231.us-east-2.compute.amazonaws.com:5000/api/user/DeleteMascota");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"propietario\":\"" + _user_id + "\"," +
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
                        AHUtils.Instance.UpdateLocalInfo();
                    }
                }
            }
        }
    }
}