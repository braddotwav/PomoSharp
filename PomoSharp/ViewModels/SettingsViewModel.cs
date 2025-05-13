using PomoSharp.Models;
using PomoSharp.Services;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using PomoSharp.Messages;

namespace PomoSharp.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    public override string Name => "Settings";

    [ObservableProperty]
    private Settings _settings;

    [ObservableProperty]
    private Settings _defaultSettings;

    private Settings? _cachedSettings;

    public SettingsViewModel()
    {
        _defaultSettings = new Settings();
        _settings = Ioc.Default.GetRequiredService<JsonStorageProvider<Settings>>().Data;
    }

    public override void OnViewShow() 
    {
        _cachedSettings = new(Settings);
    }

    public override void OnViewHide()
    {
        if (_cachedSettings == null) return;
        if (_cachedSettings.Equals(Settings)) return;

        Ioc.Default.GetRequiredService<JsonStorageProvider<Settings>>().Save();
        WeakReferenceMessenger.Default.Send<SettingsSavedMessage>();
    }
}