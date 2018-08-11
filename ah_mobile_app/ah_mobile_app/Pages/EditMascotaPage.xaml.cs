using ah_mobile_app.BaseStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ah_mobile_app.ViewModels;

namespace ah_mobile_app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditMascotaPage : ContentPage
	{
        EditarMascotaModelView vm;
		public EditMascotaPage (Mascota _mascota)
		{
            vm = new EditarMascotaModelView(_mascota);
            this.BindingContext = vm;
            vm.DisplayError += (err) => DisplayAlert("Error", err, "OK");
            vm.EditSuccess += () => DisplayAlert("Info", "Edición de Mascota Exitoso!", "OK");
            InitializeComponent ();
		}
	}
}