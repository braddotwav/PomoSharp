using PomoSharp.Models;
using PomoSharp.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PomoSharp.ViewModels;

public partial class StatsViewModel(IAppStorage storage) : ViewModelBase
{
    public override string Name => "Stats";

    [ObservableProperty]
    private Stats _stats = storage.Stats;

    public override void OnViewShow() { }

    public override void OnViewHide() { }
}