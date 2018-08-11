using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ah_mobile_app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioPageMaster : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public InicioPageMaster()
        {
            InitializeComponent();
        }
    }
}