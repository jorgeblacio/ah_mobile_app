using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ah_mobile_app.ViewModels
{
    class AboutUsViewModel : ContentView
    {
        public AboutUsViewModel()
        {
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://www.animalhouse.com.ec/")));
            OpenWebCommandMap = new Command(() => Device.OpenUri(new Uri("https://wego.here.com/directions/mix//Animal-House,-Cdla-Miraflores-Av.-Ignacio-Cuesta-307-y-Enrique-D%C3%ADaz,-Guayaquil:e-eyJuYW1lIjoiQW5pbWFsIEhvdXNlIiwiYWRkcmVzcyI6IkNkbGEgTWlyYWZsb3JlcyBBdi4gSWduYWNpbyBDdWVzdGEgMzA3IHkgRW5yaXF1ZSBEXHUwMGVkYXosIEd1YXlhcXVpbCwgRWN1YWRvciIsImxhdGl0dWRlIjotMi4xNjU0MDAyOTQ0NzksImxvbmdpdHVkZSI6LTc5LjkyMTAwOTU0MDU1OCwicHJvdmlkZXJOYW1lIjoiZmFjZWJvb2siLCJwcm92aWRlcklkIjozMzIxODg0NDM5MTIwMTJ9?map=-2.1654,-79.92101,15,normal&fb_locale=en_US")));
        }

        /// <summary>
        /// Command to open browser to xamarin.com
        /// </summary>
        public ICommand OpenWebCommand { get; }
        public ICommand OpenWebCommandMap { get; }
    }

    
}
