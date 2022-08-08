using ImageSearch.Interface;
using ImageSearch.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;
using System.Windows;

namespace ImageSearch
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices((context, service) =>
            {
                ConfigureService(service);
            }).Build();
        }

        private void ConfigureService(IServiceCollection services)
        {
            services.AddHttpClient("flickrclient", client =>
            {
                client.BaseAddress = new Uri("https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=ca370d51a054836007519a00ff4ce59e");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(10000));
            });
            services.AddSingleton<MainView>();
            services.AddScoped<IUrlToImageConverterService, UrlToImageConverterService>();
            services.AddScoped<IFlickrService, FlickrService>();
            services.AddScoped<IDialogService, DialogService>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainwindow = _host.Services.GetRequiredService<MainView>();
            mainwindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
            base.OnExit(e);
        }
    }
}