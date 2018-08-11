using ah_mobile_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ah_mobile_app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistroMascotaPage : ContentPage
	{
        RegistroMascotasViewModel vm;
        public RegistroMascotaPage ()
		{
            vm = new RegistroMascotasViewModel();
            this.BindingContext = vm;
            vm.DisplayError += (err) => DisplayAlert("Error", err, "OK");
            vm.RegistrySuccess += () => DisplayAlert("Info", "Registro Exitoso!", "OK");

            InitializeComponent ();
            CompleteFocusRegister();
		}

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                vm.Especie = (string)picker.ItemsSource[selectedIndex];
            }
        }

        void CompleteFocusRegister()
        {
            edad_entry.Completed += (s, e) => nombre_entry.Focus();
            nombre_entry.Completed += (s, e) => raza_entry.Focus();
            raza_entry.Completed += (s, e) => peso_entry.Focus();
        }
    }
}