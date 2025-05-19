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

    private void OnDurationChanged(TimeSpan span)
    {
        ResetCountdownPercentage();
    }

    private void OnCountdownElapsed(TimeSpan elapsed)
    {
        CountdownPercentage = CalculateRemainingTimeToPercentage(elapsed);
    }

    private void OnNavigationChanged(ViewModelBase view)
    {
        CurrentViewModel =  view;
        CurrentViewTitle = view.Name;
        CanShowHome = view is not HomeViewModel;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteChangeNavigation))]
    private void ChangeNavigation(object? param)
    {
        try
        {
            switch (param as string)
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
                    throw new ArgumentException($"Unknown navigation target: '{param}");
            }
        }
        catch (Exception)
        {
            // todo: log exception
        }
    }

    private bool CanExecuteChangeNavigation(object? param)
    {
        return param is string text && !string.IsNullOrEmpty(text);
    }

    private double CalculateRemainingTimeToPercentage(TimeSpan elapsed) 
    {
        if (elapsed.TotalSeconds == 0)
            return 0;

        double progress = elapsed.TotalSeconds / _countdownTimer.Duration.TotalSeconds;
        return Math.Clamp(progress, 0, 1);
    }

    private void ResetCountdownPercentage()
    {
        CountdownPercentage = 0;
    }
}