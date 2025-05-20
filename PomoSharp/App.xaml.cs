using System.Windows;
using PomoSharp.Models;
using PomoSharp.Services;
using PomoSharp.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace PomoSharp;

public partial class App : Application
{
    private readonly JsonStorageProvider<Settings> _settingsStorage;
    private readonly JsonStorageProvider<Stats> _statsStorage;
    private readonly AppStorage _storage;
    private readonly MainViewModel _mainViewModel;

    public App()
    {
        _settingsStorage = new("settings");
        _statsStorage = new("stats");

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
        _settingsStorage.Save();
        _statsStorage.Save();

        _storage.Save();
        base.OnExit(e);
    }
}
