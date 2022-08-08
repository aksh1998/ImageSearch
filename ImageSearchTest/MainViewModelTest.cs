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
        private List<Image> images;
        private Mock<IFlickrService> _mockFlickerService;
        private Mock<IDialogService> _mockDialogService;
        private Mock<IUrlToImageConverterService> _mockUrlToImageConverterService;
        [TestInitialize]
        public void Setup()
        {
            _mockFlickerService = new Mock<IFlickrService>();
            _mockDialogService = new Mock<IDialogService>();
            _mockUrlToImageConverterService = new Mock<IUrlToImageConverterService>();
            _vm = new MainViewModel(_mockFlickerService.Object, _mockDialogService.Object);

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

        private void SetupData()
        {
            _vm.SearchString = "India";
            _vm.ImageCount = "5";
            images = new List<Image>();
        }

        private void SetupMockMethods()
        {
            _mockUrlToImageConverterService.Setup(x => x.GetPhoto(It.IsAny<List<Photo>>())).Returns(images);
            _mockFlickerService.Setup(x => x.GetPhotoUrls(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(images);
        }
    }
}
