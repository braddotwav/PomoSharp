using System.Windows;
using PomoSharp.Models;
using PomoSharp.Services;
using PomoSharp.ViewModels;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;

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
            .AddSingleton<CountdownTimer>()
            .AddSingleton<IAppStorage>(_storage)
            .AddSingleton<MainViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<StatsViewModel>()
            .AddSingleton<SettingsViewModel>()
            .AddSingleton<INavigationService, NavigationService>()
            .BuildServiceProvider()
        );

        INavigationService navigationService = Ioc.Default.GetRequiredService<INavigationService>();
        navigationService.Register(Ioc.Default.GetRequiredService<HomeViewModel>());
        navigationService.Register(Ioc.Default.GetRequiredService<StatsViewModel>());
        navigationService.Register(Ioc.Default.GetRequiredService<SettingsViewModel>());

        _mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;

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
    
    private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        if (e.Argument == "pomosharp")
        {
            try
            {
                BringMainWindowToFront();
            }
            catch (Exception)
            {
                // todo: log
            }
        }
    }

    private void BringMainWindowToFront()
    {
        Current.Dispatcher.Invoke(() =>
        {
            var window = Current.MainWindow ?? throw new InvalidOperationException("Main window is not set.");

            if (window.WindowState != WindowState.Normal)
                window.WindowState = WindowState.Normal;

            window.Activate();
            window.Focus();
        });
    }
}
