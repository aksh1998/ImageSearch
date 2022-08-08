using ImageSearch.Interface;
using ImageSearch.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageSearch.Service
{
    public class FlickrService : IFlickrService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUrlToImageConverterService _urlToImageConverter;
        public FlickrService(IHttpClientFactory httpClientFactory, IUrlToImageConverterService urlToImageConverter)
        {
            _httpClientFactory = httpClientFactory;
            _urlToImageConverter = urlToImageConverter;
        }

        public async Task<List<Image>> GetPhotoUrls(string serchString, int totalPicture)
        {
            var resultPhoto = new List<Image>();
            var client = _httpClientFactory.CreateClient("flickrclient");
            client.BaseAddress = new Uri($"https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=ca370d51a054836007519a00ff4ce59e&text={serchString.Trim()}&per_page={totalPicture}&format=json&extras=url_o");
            var response = await client.GetAsync("");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string res = response.Content.ReadAsStringAsync().Result;
                    int first = res.IndexOf('(') + 1;
                    int last = res.LastIndexOf(')');
                var expect = res[first..last];
                JObject? jsonResult = JObject.Parse(expect)["photos"] as JObject;

                var responseObj = jsonResult != null ? JsonConvert.DeserializeObject<PhotosResponse>(jsonResult.ToString())
                    : new PhotosResponse();
                resultPhoto = _urlToImageConverter.GetPhoto(responseObj.Photo);
            }
            return resultPhoto;
        }
    }
}
