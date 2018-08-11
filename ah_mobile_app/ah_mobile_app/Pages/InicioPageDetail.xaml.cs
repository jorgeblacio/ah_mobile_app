using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ah_mobile_app.BaseStructs;
using ah_mobile_app.ViewModels;

namespace ah_mobile_app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioPageDetail : ContentPage
    {
        InicioPageViewModel vm;
        //public static ObservableCollection<Mascota> items { get; set; }

        public InicioPageDetail()
        {
            vm = new InicioPageViewModel();
            this.BindingContext = vm;
            vm.DisplayError += () => DisplayAlert("Error", "No se pudo completar la acción.", "OK");
            vm.DeleteSuccess += () => DisplayAlert("Info", "Borrado de Mascota Exitoso!", "OK");
            //items = new ObservableCollection<Mascota>();
            
            InitializeComponent();
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            string citas = "";
            foreach (var cita in ((Mascota)e.SelectedItem).citas)
            {
                citas += cita.ToString() + "\n";
            }
            if(citas == "")
                DisplayAlert("Citas de " + ((Mascota)e.SelectedItem).nombre, "No tiene ninguna cita pendiente.", "Ok");
            else
                DisplayAlert("Citas de " + ((Mascota)e.SelectedItem).nombre, citas, "Ok");
            //comment out if you want to keep selections
            ListView lst = (ListView)sender;
            lst.SelectedItem = null;
        }

        void OnMore(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            DisplayAlert("Información completa de: " + ((Mascota)item.CommandParameter).nombre, ((Mascota)item.CommandParameter).ToString(), "OK");
        }

        void OnDelete(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            ConfirmDelete((Mascota)item.CommandParameter);         
        }

        void OnEdit(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            var editPage = new EditMascotaPage((Mascota)item.CommandParameter);
            Navigation.PopAsync();
            Navigation.PushAsync(editPage);
        }

        async void ConfirmDelete(Mascota _mascota)
        {
            var confirmed = await DisplayAlert("Confirmación", "¿Seguro que desea borrar esta mascota?", "Si", "No");
            if (confirmed)
            {
                vm.Mascota_ID = _mascota.id;
                vm.DeletePet.Execute(_mascota);

            }
        }
    }
}