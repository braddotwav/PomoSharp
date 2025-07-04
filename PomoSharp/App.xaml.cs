﻿using System.Windows;
using PomoSharp.Models;
using PomoSharp.Services;
using PomoSharp.ViewModels;
using Microsoft.Toolkit.Uwp.Notifications;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace PomoSharp;

public partial class App : Application
{
    private readonly AppStorage _storage;
    private readonly MainViewModel _mainViewModel;

    public App()
    {
        SingleInstance.Make("PomoSharp");

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
        ToastArguments args = ToastArguments.Parse(e.Argument);

        if (args["action"] == "openApp")
        {
            Current.Dispatcher.Invoke(() =>
            {
                SingleInstance.ShowCurrentMainWindow();
            });
        }
    }
}
