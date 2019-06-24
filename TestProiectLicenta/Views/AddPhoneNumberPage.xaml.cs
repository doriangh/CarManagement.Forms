using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            Regex re = new Regex(@"^0[0-9]{9}", RegexOptions.Compiled);

            if (re.IsMatch(phoneNumber.Text))
            {
                _user.PhoneNumber = Convert.ToInt32(phoneNumber.Text);
                await App.UserManager.UpdateUser(_user);
                await Navigation.PopPopupAsync();
            }
            else
            {
                await DisplayAlert("Error", "Invalid number", "OK");
            }
        }
    }
}
