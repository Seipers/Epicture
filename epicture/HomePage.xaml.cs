using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace epicture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private UserInfo _user;
        bool singleTap;
        private ObservableCollection<ImageInfo> _images = new ObservableCollection<ImageInfo>();
        public HomePage()
        {
            this.InitializeComponent();
            _user = null;
        }
        private void AddGalleryView(IEnumerable<IGalleryItem> gallery)
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
                    if (album.Images != null && album.Images.Count() > 0)
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
            _images.Clear();
            if (_user == null)
                _user = (UserInfo)e.Parameter;
            var gallery = Task.Run(async () => await ImgurApi.GetGallery());
            gallery.Wait();

            AddGalleryView(gallery.Result);
        }

        private async void UploadImage_OnClick(object sender, TappedRoutedEventArgs e)
        {
            // Opening the file explorer
            var picker =
                new Windows.Storage.Pickers.FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                // Application now has read/write access to the picked file
                using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    await ImgurApi.UploadImage(fileStream, _user);
                }
            }
            else
            {
                // cancell action
            }
        }

        private async void TextBox_CharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            IEnumerable<IGalleryItem> gallery;

            if (Search.Text == "")
                gallery = await ImgurApi.GetGallery();
            else
                gallery = await ImgurApi.SearchGallery(Search.Text, _user);

            _images.Clear();
            AddGalleryView(gallery);
        }

        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.singleTap = true;
            await Task.Delay(200);
            if (this.singleTap)
            {
                var image = Images.SelectedItem as ImageInfo;
                DetailsStruc tmp = new DetailsStruc();
                tmp.Image = image;
                tmp.user = _user;
                if (image != null)
                    this.Frame.Navigate(typeof(DetailPage), tmp);
            }
        }

        private async void Image_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.singleTap = false;

            var image = Images.SelectedItem as ImageInfo;
            if (image != null)
                ImgurApi.FavoriteImage(image.id, _user);
        }

        private void test(object send, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Favpic), _user);
        }

        private void GotOuP(object send, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Uploadedpic), _user);
        }
    }
}
