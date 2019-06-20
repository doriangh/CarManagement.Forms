using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddUserAddressPage : PopupPage
    {
        private readonly User _newUser;

        public AddUserAddressPage(User user)
        {
            InitializeComponent();
            _newUser = user;
        }

        private async void Cancel_Button(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void Add_Address_Button(object sender, System.EventArgs e)
        {
            _newUser.Address = street.Text + ' ' + number.Text + ' ' + zip.Text + ' ' + city.Text + ' ' + country.Text;
            await App.UserManager.UpdateUser(_newUser);
            await Navigation.PopPopupAsync();
        }
    }
}
