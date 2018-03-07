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
    public sealed partial class Favpic : Page
    {
        private UserInfo _user;
        private ObservableCollection<ImageInfo> _images = new ObservableCollection<ImageInfo>();

        public Favpic()
        {
            this.InitializeComponent();
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var image = Images.SelectedItem as ImageInfo;

            //if (image != null)
            //  this.Frame.Navigate(typeof(DetailPage), image);
        }

        private async void Image_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var image = Images.SelectedItem as ImageInfo;
            ImgurApi.FavoriteImage(image.id, _user);
        }

        private async void AddGavView(IEnumerable<IGalleryItem> gallery)
        {
            foreach (var item in gallery)
            {
                if (item is Imgur.API.Models.IGalleryImage)
                {
                    IGalleryImage image = (IGalleryImage)item;
                    ImageInfo _info = new ImageInfo { Name = image.Name, Image = new BitmapImage(new Uri(image.Link)), Item = image, id = image.Id };
                    _images.Add(_info);
                }
                else
                {
                    IGalleryAlbum album = (IGalleryAlbum)item;
                    if (album.Images != null)
                    {
                        IImage firstImage = album.Images.First();
                        ImageInfo info = new ImageInfo
                        {
                            Name = album.Title,
                            Image = new BitmapImage(new Uri(firstImage.Link)),
                            Item = item,
                            id = firstImage.Id
                        };
                        _images.Add(info);
                    }
                }
            }
            Images.ItemsSource = null;
            Images.ItemsSource = _images;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_user == null)
                _user = (UserInfo)e.Parameter;
            _images.Clear();
            var favs = Task.Run(async () => await ImgurApi.GetFav(_user));
            favs.Wait();
            AddGavView(favs.Result);
        }

       private void GoToHome(object send, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), _user);
        }

        private void GotOuP(object send, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Uploadedpic), _user);
        }
    }
}
