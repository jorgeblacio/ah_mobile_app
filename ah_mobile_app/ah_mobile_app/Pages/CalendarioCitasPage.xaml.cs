using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ah_mobile_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ah_mobile_app.BaseStructs;


namespace ah_mobile_app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarioCitasPage : ContentPage
	{
        CalendarioCitasViewModel vm;
        
        public CalendarioCitasPage ()
		{

            Title = "Calendario de Citas";
            vm = new CalendarioCitasViewModel();
            this.BindingContext = vm;
            vm.DisplayError += () => DisplayAlert("Error", "No se pudo completar la acción.", "OK");
            vm.DeleteSuccess += () => DisplayAlert("Info", "Borrado de Cita Exitoso!", "OK");
            InitializeComponent();
        }        

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            Mascota mascota_select = new Mascota();
            
            foreach (var mascota in AHUtils.Instance.loggedUser.mascotas)
            {
                if (((CitaMascota)e.SelectedItem).mascota_id == mascota.id)
                    mascota_select = mascota;
            }
            DisplayAlert("Detalles de Cita", e.SelectedItem.ToString(), "Ok");
            //comment out if you want to keep selections
            ListView lst = (ListView)sender;
            lst.SelectedItem = null;
        }

        void OnMore(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            DisplayAlert("Información completa de cita con ID: " + ((CitaMascota)item.CommandParameter).cita_id, ((CitaMascota)item.CommandParameter).ToString(), "OK");
        }

        void OnDelete(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            ConfirmDelete((Cita)item.CommandParameter);
        }

        async void ConfirmDelete(Cita _cita)
        {
            var confirmed = await DisplayAlert("Confirmación", "¿Seguro que desea borrar esta cita?", "Si", "No");
            if (confirmed)
            {
                vm.Cita_ID = _cita.id;
                vm.DeleteAppointment.Execute(null);
                if (vm.Success)
                {
                    var index = vm.Citas.IndexOf(_cita);
                    vm.Citas.Remove(_cita);
                    vm.CitasMascota.RemoveAt(index);
                }
            }
        }
    }
}