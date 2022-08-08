using ImageSearch.Interface;
using ImageSearch.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSearch.Service
{
    public class UrlToImageConverterService : IUrlToImageConverterService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UrlToImageConverterService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<Image>> GetPhoto(List<Uri> uris)
        {
            var images = new List<Image>();
            uris?.ForEach(uri =>
            {
                Image img = new Image
                {
                    Source = DownloadImage(uri),
                    Stretch = Stretch.UniformToFill,
                    StretchDirection = StretchDirection.DownOnly,
                };
                images.Add(img);
            });
            return images;
        }

        private BitmapImage DownloadImage(Uri uri)
        {
            return byteArrayToImage(_httpClientFactory.CreateClient().GetByteArrayAsync(uri).Result);
        }

        private BitmapImage byteArrayToImage(byte[] byteArrayIn)
        {
            BitmapImage myBitmapImage;
            using (MemoryStream stream = new MemoryStream(byteArrayIn))
            {
                myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = stream;
                myBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                myBitmapImage.EndInit();
            }
            return myBitmapImage;
        }
    }
}
