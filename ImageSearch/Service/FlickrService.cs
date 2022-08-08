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
        public FlickrService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Uri>> GetPhotoUrls(string serchString, int totalPicture)
        {
            var uris = new List<Uri>();
            var client = _httpClientFactory.CreateClient("flickrclient");
            client.BaseAddress = new Uri($"https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=ca370d51a054836007519a00ff4ce59e&text={serchString.Trim()}&per_page={totalPicture}&format=json&extras=url_o");
            var response = await client.GetAsync("");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string res = response.Content.ReadAsStringAsync().Result;
                var cleanResponse = RemoveNoiseFormResponse(res);
                JObject? jsonResult = JObject.Parse(cleanResponse)["photos"] as JObject;

                var responseObj = jsonResult != null ? JsonConvert.DeserializeObject<PhotosResponse>(jsonResult.ToString())
                    : new PhotosResponse();

                if(responseObj.Photo != null)
                {
                    responseObj.Photo.ForEach(photo => {
                        uris.Add(new Uri($"https://live.staticflickr.com/{photo.Server}/{photo.Id}_{photo.Secret}.jpg"));
                    });
                }
            }
            return uris;
        }

        private string RemoveNoiseFormResponse(string response)
        {
            var result = "";
            if (!string.IsNullOrEmpty(response))
            {
                int first = response.IndexOf('(') + 1;
                int last = response.LastIndexOf(')');
                result = response[first..last];
            }
            return result;
        }
    }
}
