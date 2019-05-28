using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class ChooseMethodFormPage : ContentPage
    {
        public ChooseMethodFormPage()
        {
            InitializeComponent();
        }

        private async void Add_Manually_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FillExtraFormPage());
        }

        private async void Add_VIN_Button(object sender, System.EventArgs e)
        {
            //await Navigation.PushPopupAsync(new VINPopupPage());

            var result = await Plugin.DialogKit.CrossDiaglogKit.Current.GetInputTextAsync("Enter VIN", "Please enter VIN");

            if (result != null)
                ((Button) sender).Text = result;
        }
    }
}
