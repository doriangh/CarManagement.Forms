using System;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class FinishAddingFormPage : ContentPage
    {
        public FinishAddingFormPage()
        {
            InitializeComponent();
        }

        private async void Submit_Button(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPageForm());
        }
    }
}