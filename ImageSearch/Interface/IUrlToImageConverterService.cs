using ImageSearch.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageSearch.Interface
{
    public interface IUrlToImageConverterService
    {
        Task<List<Image>> GetPhoto(List<Uri> photos);
        
    }
}
