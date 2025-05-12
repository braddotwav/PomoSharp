using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PomoSharp.Models;
using PomoSharp.Services;

namespace PomoSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            JsonStorageProvider<Settings> settingsStorage = new("settings");
            JsonStorageProvider<Report> reportStorage = new("report");

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton(settingsStorage)
                .AddSingleton(reportStorage)
                .BuildServiceProvider()
            );
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Ioc.Default.GetRequiredService<JsonStorageProvider<Settings>>().Save();
            Ioc.Default.GetRequiredService<JsonStorageProvider<Report>>().Save();

            base.OnExit(e);
        }
    }
}
