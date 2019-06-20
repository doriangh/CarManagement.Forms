using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddPhoneNumberPage : PopupPage
    {
        private User _user;

        public AddPhoneNumberPage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        async void Cancel_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        async void Confirm_Clicked(object sender, System.EventArgs e)
        {
            _user.PhoneNumber = Convert.ToInt32(phoneNumber.Text);
            await App.UserManager.UpdateUser(_user);
            await Navigation.PopPopupAsync();
        }
    }
}
