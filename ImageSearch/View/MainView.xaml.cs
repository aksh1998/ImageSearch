using System.Windows;
using ImageSearch.Interface;
using ImageSearch.ViewModel;

namespace ImageSearch
{
    public partial class MainView : Window
    {
        private readonly IFlickrService _flickrService;
        public MainView(IFlickrService flickrService, IDialogService dialogService)
        {
            _flickrService = flickrService;
            InitializeComponent();
            this.DataContext = new MainViewModel(flickrService, dialogService);
        }
    }
}
