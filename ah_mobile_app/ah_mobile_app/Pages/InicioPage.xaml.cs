using Android.App;
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
    public partial class InicioPage : MasterDetailPage
    {
        InicioPageMaster masterPage;
        public string email; //placeholder for token
        private bool _canClose = true;

        public InicioPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            masterPage = new InicioPageMaster();
            AHUtils.masterDetailPage = this;
            Master = masterPage;
            Detail = new NavigationPage(new InicioPageDetail());


            //InitializeComponent();

            masterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as InicioPageMenuItem;
            if (item != null)
            {
                if (item.Title == "Página de Inicio" && Detail.Navigation.NavigationStack.Count > 1)
                {
                    Detail.Navigation.PopToRootAsync();
                }
                else if (item.Title != "Página de Inicio")
                {
                    Detail.Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));
                }

                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }

        }

        protected override bool OnBackButtonPressed()
        {
            if (_canClose && Detail.Navigation.NavigationStack.Count == 1)
            {
                ShowExitDialog();
            }
            else
            {
                base.OnBackButtonPressed();
            }
            return _canClose;
        }

        private async void ShowExitDialog()
        {
            var answer = await DisplayAlert("Salir", "¿Desea salir de la aplicación?", "Si", "No");
            if (answer)
            {
                _canClose = false;
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                //OnBackButtonPressed();
            }
        }
    }
}