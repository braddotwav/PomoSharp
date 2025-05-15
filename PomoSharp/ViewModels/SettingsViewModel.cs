using PomoSharp.Models;
using PomoSharp.Services;
using PomoSharp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

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
        if (UserHasUnsavedChanges()) 
        {
            Ioc.Default.GetRequiredService<JsonStorageProvider<Settings>>().Save();
            WeakReferenceMessenger.Default.Send<SettingsSavedMessage>();
        }
    }

    private bool UserHasUnsavedChanges() 
    {
        return _cachedSettings != null && !_cachedSettings.Equals(Settings);
    }
}