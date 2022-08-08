using ImageSearch.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageSearch.Interface
{
    public interface IUrlToImageConverterService
    {
        List<Image> GetPhoto(List<Photo> photos);
        
    }
}
