using System;
using System.Collections.Generic;
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
	public partial class ReservarCitaPage : ContentPage
	{
        ReservaCitaViewModel vm;

        public ReservarCitaPage ()
		{
            vm = new ReservaCitaViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidDay += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            vm.RegistrySuccess += () => DisplayAlert("Info", "Registro Exitoso!", "OK");
            vm.DisplayError += (err) => DisplayAlert("Error", err, "OK");
            InitializeComponent ();
		}

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                vm.Mascota_ID_Index = selectedIndex;
            }
        }

        void OnAvailablePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                vm.Available_Time_Index = selectedIndex;
            }
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            vm.Fecha = startDatePicker.Date;
            vm.UpdateAvailableHours();
        }

        private void PickerLabel_OnTapped(object sender, EventArgs e)
        {
            picker.Focus();
        }
    }
}