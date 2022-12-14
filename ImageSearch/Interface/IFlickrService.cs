using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageSearch.Interface
{
    public interface IFlickrService
    {
        Task<List<Uri>> GetPhotoUrls(string serchString, int totalPicture);
    }
}
