using System.Windows;
using PomoSharp.Models;
using PomoSharp.Services;
using PomoSharp.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace PomoSharp;

public partial class App : Application
{
    private readonly AppStorage _storage;
    private readonly MainViewModel _mainViewModel;

    public App()
    {
        JsonStorageProvider<Settings> settingsStorage = new("settings");
        JsonStorageProvider<Stats> statsStorage = new("stats");
        
        _storage = new AppStorage(settingsStorage, statsStorage);

        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddSingleton(_settingsStorage)
            .AddSingleton(_statsStorage)
            .AddSingleton<IAppStorage>(_storage)
            .AddSingleton<MainViewModel>()
            .BuildServiceProvider()
        );

        _mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Window mainView = new MainWindow
        {
            DataContext = _mainViewModel
        };

        mainView.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _storage.Save();
        base.OnExit(e);
    }
}
