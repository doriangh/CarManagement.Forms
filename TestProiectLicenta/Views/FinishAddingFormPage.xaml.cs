using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class FinishAddingFormPage : ContentPage
    {
        public FinishAddingFormPage()
        {
            InitializeComponent();
        }

        private async void Submit_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserPageForm());
        }
    }
}
