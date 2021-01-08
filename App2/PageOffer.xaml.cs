using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageOffer : ContentPage
    {
        public PageOffer()
        {
            InitializeComponent();
        }

        public PageOffer(String sirilizedOffer)
        {
            InitializeComponent();
            this.label1.Text = sirilizedOffer;
        }

        private async void button1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}