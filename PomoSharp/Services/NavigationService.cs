using PomoSharp.ViewModels;

namespace PomoSharp.Services;

public class NavigationService : INavigationService
{
    private readonly List<ViewModelBase> _registeredViewModels = [];

    public event Action<ViewModelBase>? OnNavigationChanged;

    private ViewModelBase? _currentViewmodel;

    public void Change<TViewModel>() where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = _registeredViewModels.FirstOrDefault(x => x.GetType() == typeof(TViewModel));

        if (viewModel is null)
            throw new InvalidOperationException($"ViewModel of type {typeof(TViewModel)} has not been registered.");
        
        _currentViewmodel?.OnViewHide();
        _currentViewmodel = viewModel;
        _currentViewmodel?.OnViewShow();
        OnNavigationChanged?.Invoke(viewModel);
    }

    public void Register(ViewModelBase viewModel)
    {
        _registeredViewModels.Add(viewModel);
    }
}