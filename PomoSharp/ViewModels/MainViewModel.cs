using PomoSharp.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PomoSharp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _canShowHome;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    [ObservableProperty]
    private double _countdownPercentage;

    [ObservableProperty]
    private string _currentViewTitle = string.Empty;

    private readonly CountdownTimer _countdownTimer;
    private readonly NavigationService _navigationService;

    public MainViewModel()
    {
        _countdownTimer = new CountdownTimer();
        _countdownTimer.OnElapsed += OnCountdownElapsed;
        _countdownTimer.OnDurationChanged += OnDurationChanged;

        _navigationService = new();
        _navigationService.OnNavigationChanged += OnNavigationChanged;

        _navigationService.Register(new HomeViewModel(_countdownTimer));
        _navigationService.Register(new StatsViewModel());
        _navigationService.Register(new SettingsViewModel());

        _navigationService.Change<HomeViewModel>();
    }

    private void OnDurationChanged()
    {
        CountdownPercentage = 0;
    }

    private void OnCountdownElapsed(TimeSpan elapsed)
    {
        CountdownPercentage = RemaningTimeToPercentage(elapsed);
    }

    private void OnNavigationChanged(ViewModelBase view)
    {
        CurrentViewModel =  view;
        CurrentViewTitle = view.Name;
        CanShowHome = view is not HomeViewModel;
    }

    [RelayCommand]
    private void ChangeNavigation(object? param)
    {
        if (param is not string navigate) return;
        if (string.IsNullOrEmpty(navigate)) return;

        switch (navigate)
        {
            case "Home":
                _navigationService.Change<HomeViewModel>();
                break;
            case "Stats":
                _navigationService.Change<StatsViewModel>();
                break;
            case "Settings":
                _navigationService.Change<SettingsViewModel>();
                break;
            default:
                throw new InvalidOperationException($"Something went wrong trying to navigate to {navigate}");
        }
    }

    private double RemaningTimeToPercentage(TimeSpan elapsed) 
    {
        if (elapsed.TotalSeconds == 0)
            return 0;

        double progress = (elapsed.TotalSeconds / _countdownTimer.Duration.TotalSeconds);
        return Math.Clamp(progress, 0, 1);
    }
}