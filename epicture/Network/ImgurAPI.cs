using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using epicture.Model;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;

namespace epicture.Network
{
    public class ImgurApi
    {

        public static async Task ConnectImgurApi()
        {
            var client = new ImgurClient("3f8dfa6f3541120");

            var endpoint = new OAuth2Endpoint(client);
            var authorizationUrl = endpoint.GetAuthorizationUrl(OAuth2ResponseType.Pin);

            await Windows.System.Launcher.LaunchUriAsync(new Uri(authorizationUrl));
        }

        public static async Task<IOAuth2Token> RequestImgurTokenFromPin(String code)
        {
            var client = new ImgurClient("3f8dfa6f3541120", "dcb972ae2f6952efb8e9a156fe0c65b8136b3010");
            var endpoint = new OAuth2Endpoint(client);
            var token = await endpoint.GetTokenByPinAsync(code);
            return token;
        }

        public static async Task<IEnumerable<IGalleryItem>> GetGallery()
        {
            var client = new ImgurClient("3f8dfa6f3541120");
            var endpoint = new GalleryEndpoint(client);
            var images = await endpoint.GetRandomGalleryAsync();
            return images;
            
        }

        public static async Task<IEnumerable<IGalleryItem>> GetFav(UserInfo user)
        {
            var client = new ImgurClient("3f8dfa6f3541120", "dcb972ae2f6952efb8e9a156fe0c65b8136b3010");
            client.SetOAuth2Token(user.tokenObject);
            var endpoint = new AccountEndpoint(client);
            var favs = await endpoint.GetAccountFavoritesAsync();
            return favs;
        }

        public static async Task<IEnumerable<IImage>> GetUpl(UserInfo user)
        {
            var client = new ImgurClient("3f8dfa6f3541120", "dcb972ae2f6952efb8e9a156fe0c65b8136b3010");
            client.SetOAuth2Token(user.tokenObject);
            var endpoint = new AccountEndpoint(client);
            var favs = await endpoint.GetImagesAsync();
            return favs;
        }

        public static async void FavoriteImage(String id, UserInfo user)
        {
            var client = new ImgurClient("3f8dfa6f3541120", "dcb972ae2f6952efb8e9a156fe0c65b8136b3010");
            client.SetOAuth2Token(user.tokenObject);
            var endpoint = new ImageEndpoint(client);
            var favorited = await endpoint.FavoriteImageAsync(id);
        }
        public static async Task<IEnumerable<IGalleryItem>> SearchGallery(String query, UserInfo user)
        {
            var client = new ImgurClient("3f8dfa6f3541120", "dcb972ae2f6952efb8e9a156fe0c65b8136b3010");
            client.SetOAuth2Token(user.tokenObject);
            var endpoint = new GalleryEndpoint(client);
            var images = await endpoint.SearchGalleryAsync(query);
            return images;
        }

        public static async Task UploadImage(IRandomAccessStream imageLocation, UserInfo user)
        {
            try
            {
                var client = new ImgurClient("3f8dfa6f3541120", "dcb972ae2f6952efb8e9a156fe0c65b8136b3010");
                client.SetOAuth2Token(user.tokenObject);
                var endpoint = new ImageEndpoint(client);

                IImage image;

                image = await endpoint.UploadImageStreamAsync(imageLocation.AsStream());
                Debug.Write("Image uploaded. Image Url: " + image.Link);
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
        }
    }
}
