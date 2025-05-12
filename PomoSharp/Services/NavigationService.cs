using PomoSharp.ViewModels;

namespace PomoSharp.Services;

public class NavigationService : INavigationService
{
    private readonly List<ViewModelBase> _registeredViewModels = [];

    public event Action<ViewModelBase>? OnNavigationChanged;

    public void Change<TViewModel>() where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = _registeredViewModels.FirstOrDefault(x => x.GetType() == typeof(TViewModel));

        if (viewModel is not null)
        {
            OnNavigationChanged?.Invoke(viewModel);
            return;
        }

        throw new InvalidOperationException($"ViewModel of type {typeof(TViewModel)} has not been registered.");
    }

    public void Register(ViewModelBase viewModel)
    {
        _registeredViewModels.Add(viewModel);
    }
}