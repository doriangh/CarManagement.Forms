using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class FillExtraFormPage : ContentPage
    {
        List<string> errors = new List<string>();

        public FillExtraFormPage()
        {
            InitializeComponent();
        }

        private async void Continue_button(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FinishAddingFormPage());
        }

        //void Handle_Completed(object sender, System.EventArgs e)
        //{
        //    //if (Convert.ToInt32(year.Text) < 1970 || Convert.ToInt32(year.Text) > DateTime.Today.Year)
        //    //{
        //    //} 
        //}

        private void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var selectedItem = fuel.SelectedItem.ToString();

            if (selectedItem == "Electric")
            {
            }
        }
    }
}