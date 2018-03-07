using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using epicture.Model;
using epicture.Network;
using Imgur.API.Models;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace epicture
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Uploadedpic : Page
    {
        private UserInfo _user;
        private ObservableCollection<ImageInfo> _images = new ObservableCollection<ImageInfo>();

        public Uploadedpic()
        {
            this.InitializeComponent();
        }

        private async void AddUplView(IEnumerable<IImage> gallery)
        {
            foreach (var item in gallery)
            {
                IImage firstImage = item;
                ImageInfo info = new ImageInfo
                {
                    Name = item.Title,
                    Image = new BitmapImage(new Uri(firstImage.Link)),
                    id = firstImage.Id
                };
                _images.Add(info);
            }
            Images.ItemsSource = null;
            Images.ItemsSource = _images;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_user == null)
                _user = (UserInfo)e.Parameter;
            _images.Clear();
            var upl = Task.Run(async () => await ImgurApi.GetUpl(_user));
            upl.Wait();
            AddUplView(upl.Result);
        }

        private void GoToFav(object send, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Favpic), _user);
        }

        private void GoToHome(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), _user);
        }
    }
}
