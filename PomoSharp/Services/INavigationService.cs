using PomoSharp.ViewModels;

namespace PomoSharp.Services;

public interface INavigationService
{
    public event Action<ViewModelBase>? OnNavigationChanged;
    public void Register(ViewModelBase viewModel);
    public void Change<TViewModel>() where TViewModel : ViewModelBase;
}