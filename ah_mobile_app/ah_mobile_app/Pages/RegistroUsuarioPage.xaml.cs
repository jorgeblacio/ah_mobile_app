using ah_mobile_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ah_mobile_app
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistroUsuarioPage : ContentPage
	{
		public RegistroUsuarioPage ()
		{

            var vm = new RegistrarUserViewModel();
            this.BindingContext = vm;
            //CompleteFocusRegister();
            vm.DisplayError += (err) => DisplayAlert("Error", err, "OK");
            vm.SignUpSuccess += () => DisplayAlert("Info", "Registro Exitoso!", "OK");
            InitializeComponent ();
            CompleteFocusRegister();
        }

        private void CompleteFocusRegister()
        {
            nombre_entry.Completed += (s, e) => cédula_entry.Focus();
            cédula_entry.Completed += (s, e) => dirección_entry.Focus();
            dirección_entry.Completed += (s, e) => teléfono_entry.Focus();
            teléfono_entry.Completed += (s, e) => email_entry.Focus();
            email_entry.Completed += (s, e) => contraseña_entry.Focus();
            contraseña_entry.Completed += (s, e) => contraseña2_entry.Focus();
        }
	}
}