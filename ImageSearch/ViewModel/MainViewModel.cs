using ImageSearch.Command;
using ImageSearch.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace ImageSearch.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IFlickrService _flickrService;
        private readonly IDialogService _dialogService;
        private string _searchString;
        private string _imageCount;
        private List<Image> _images;
        public MainViewModel(IFlickrService flickrService, IDialogService dialogService)
        {
            _flickrService = flickrService;
            _dialogService = dialogService;
            ImageCount = "0";
            OnBtnSubmit = new RelayCommand(OnSubmit);
        }

        public RelayCommand OnBtnSubmit { get; set; }
        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                OnPropertyChanged(nameof(SearchString));
            }
        }
        public string ImageCount
        {
            get => _imageCount;
            set
            {
                _imageCount = value;
                OnPropertyChanged(nameof(ImageCount));
            }
        }

        public List<Image> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
            }
        }

        public async void OnSubmit(object parameter = null)
        {
            try
            {
                ValidateInput(SearchString);
                var response = await _flickrService.GetPhotoUrls(SearchString, int.Parse(ImageCount));
                if (response != null)
                {
                    Images = response;
                }
            }
            catch (ArgumentException ex)
            {
                _dialogService.ShowMessageBox(ex.Message, "Warning");
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message, "Error");
            }
        }

        private void ValidateInput(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException("Nothing to search, Please type something.");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
