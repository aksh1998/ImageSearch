using ImageSearch.Interface;
using ImageSearch.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSearch.Service
{
    public class UrlToImageConverterService : IUrlToImageConverterService
    {
        public List<Image> GetPhoto(List<Photo> photos)
        {
            var images = new List<Image>();
            photos?.ForEach(photo =>
            {
                Image img = new Image
                {
                    Source = DownloadImage(photo),
                    Stretch = Stretch.UniformToFill,
                    StretchDirection = StretchDirection.DownOnly,
                };
                images.Add(img);
            });
            return images;
        }

        private BitmapImage DownloadImage(Photo photo)
        {
            return byteArrayToImage(new WebClient().DownloadData($"https://live.staticflickr.com/{photo.Server}/{photo.Id}_{photo.Secret}.jpg"));
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
