using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class VinPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public VinPopupPage()
        {
            InitializeComponent();
        }

        private async void Cancel_Button(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}
