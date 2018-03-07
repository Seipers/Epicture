using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using epicture.Model;
using epicture.Network;
using Imgur.API.Models;
using Imgur.API.Models.Impl;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace epicture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private UserInfo _user;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;

            _user = new UserInfo();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
        }

        private async void ConnectToImgur_OnClick(object sender, RoutedEventArgs e)
        {
            await ImgurApi.ConnectImgurApi();
            ButtonConnectImgur.Visibility = Visibility.Collapsed;
            LeaveBtn.Visibility = Visibility.Collapsed;
            PinFormGrid.Visibility = Visibility.Visible;
        }

        private async void ButtonValidatePin_OnClick(object sender, RoutedEventArgs e)
        {
            if (PinTextBox.Text.Length != 0)
            {
                try
                {
                    var res = await ImgurApi.RequestImgurTokenFromPin(PinTextBox.Text);
                    _user = new UserInfo()
                    {
                        tokenObject = (OAuth2Token)res,
                        Id = res.AccountId,
                        UserName = res.AccountUsername,
                        AccessToken = res.AccessToken,
                        ExpiresAt = res.ExpiresAt,
                        RefreshToken = res.RefreshToken
                    };
                    Frame.Navigate(typeof(HomePage), _user);
                }
                catch (Imgur.API.ImgurException)
                {
                    ErrorTextBlockPin.Visibility = Visibility.Visible;
                }
            } else ErrorTextBlockPin.Visibility = Visibility.Visible;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}
