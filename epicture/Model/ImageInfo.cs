using Imgur.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace epicture.Model
{
    class ImageInfo
    {
        public IGalleryItem Item { get; set; }
        public BitmapImage Image { get; set; }
        public string Name { get; set; }
        public string id { get; set; }
    }
}
