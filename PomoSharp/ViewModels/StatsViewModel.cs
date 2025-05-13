using PomoSharp.Models;
using PomoSharp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace PomoSharp.ViewModels;

public partial class StatsViewModel : ViewModelBase
{
    public override string Name => "Stats";

    [ObservableProperty]
    private Report _report;

    public StatsViewModel()
    {
        _report = Ioc.Default.GetRequiredService<JsonStorageProvider<Report>>().Data;
    }

    public override void OnViewShow() { }

    public override void OnViewHide() { }
}