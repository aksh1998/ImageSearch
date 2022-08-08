using ImageSearch.Interface;
using ImageSearch.Model;
using ImageSearch.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ImageSearchTest
{
    [TestClass]
    public class MainViewModelTest
    {
        private MainViewModel _vm;
        private Mock<IFlickrService> _mockFlickerService;
        private Mock<IDialogService> _mockDialogService;
        private Mock<IUrlToImageConverterService> _mockUrlToImageConverterService;
        [TestInitialize]
        public void Setup()
        {
            _mockFlickerService = new Mock<IFlickrService>();
            _mockDialogService = new Mock<IDialogService>();
            _mockUrlToImageConverterService = new Mock<IUrlToImageConverterService>();
            _vm = new MainViewModel(_mockFlickerService.Object, _mockDialogService.Object,
                _mockUrlToImageConverterService.Object);

            SetupData();
            SetupMockMethods();
        }

        [TestMethod]
        public void OnSubmit_Valid_SearchString_FlickerService_CalledOnce()
        {
            _vm.OnSubmit();
            _mockFlickerService.Verify(
                x => x.GetPhotoUrls(It.IsAny<string>(), It.IsAny<int>()),
                Times.Once);
        }

        [TestMethod]
        public void OnSubmit_Valid_SearchString_DialogService_NeverCalled()
        {
            _vm.SearchString = "Google";
            _vm.OnSubmit();
            _mockDialogService.Verify(
                x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [TestMethod]
        public void OnSubmit_InValid_SearchString_DialogService_CalledOnce()
        {
            _vm.SearchString = " ";
            _vm.OnSubmit();
            _mockDialogService.Verify(
                x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        public void OnSubmit_Valid_SearchString_UrlToImageConverterService_CalledOnce()
        {
            _vm.SearchString = "Google";
            _vm.OnSubmit();
            _mockUrlToImageConverterService.Verify(
                x => x.GetPhoto(It.IsAny<List<Uri>>()),
                Times.Once);
        }

        [TestMethod]
        public void OnSubmit_InValid_SearchString_UrlToImageConverterService_NeverCalled()
        {
            _vm.SearchString = " ";
            _vm.OnSubmit();
            _mockUrlToImageConverterService.Verify(
                x => x.GetPhoto(It.IsAny<List<Uri>>()),
                Times.Never);
        }

        private void SetupData()
        {
            _vm.SearchString = "India";
            _vm.ImageCount = "5";
        }

        private void SetupMockMethods()
        {
            _mockUrlToImageConverterService.Setup(x => x.GetPhoto(It.IsAny<List<Uri>>())).ReturnsAsync(new List<Image>());
            _mockFlickerService.Setup(x => x.GetPhotoUrls(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new List<Uri>());
        }
    }
}
