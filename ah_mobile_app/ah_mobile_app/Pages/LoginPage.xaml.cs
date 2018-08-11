using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net;
using System.IO;

using ah_mobile_app.ViewModels;

namespace ah_mobile_app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += (err) => DisplayAlert("Error", err, "OK");
            vm.DisplayServerErrorPrompt += () => DisplayAlert("Error", "El servidor no se encuentra disponible, inténtelo nuevamente en unos momentos.", "OK");
            InitializeComponent();

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };


        }


        private async Task ButtonRegistrar_ClickedAsync(object sender, EventArgs e)
        {

            var registro = new RegistroUsuarioPage();
            await App.Current.MainPage.Navigation.PushAsync(registro);
        }
    }
}