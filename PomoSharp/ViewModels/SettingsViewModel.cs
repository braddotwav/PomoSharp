using PomoSharp.Models;
using PomoSharp.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PomoSharp.ViewModels;

public partial class SettingsViewModel(IAppStorage storage) : ViewModelBase
{
    public override string Name => "Settings";

    [ObservableProperty]
    private Settings _settings = storage.Settings;

    [ObservableProperty]
    private Settings _defaultSettings = new();

    private Settings? _cachedSettings;

    public override void OnViewShow()
    {
        _cachedSettings = new(Settings);
    }

    public override void OnViewHide()
    {
        if (!UserHasUnsavedChanges()) return;
        
        storage.Save();
    }

    private bool UserHasUnsavedChanges() 
    {
        return _cachedSettings != null && !_cachedSettings.Equals(Settings);
    }
}