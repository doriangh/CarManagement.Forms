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
        User newUser;

        public AddUserAddressPage(User user)
        {
            InitializeComponent();
            newUser = user;
        }

        async void Cancel_Button(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        async void Add_Address_Button(object sender, System.EventArgs e)
        {
            newUser.Address = street.Text + ' ' + number.Text + ' ' + zip.Text + ' ' + city.Text + ' ' + country.Text;
            await App.UserManager.UpdateUser(newUser);
            await Navigation.PopPopupAsync();
        }
    }
}
