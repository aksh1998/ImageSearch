using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Model
{
    public class PhotosResponse
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Total { get; set; }
        public List<Photo> Photo { get; set; }
    }
}
