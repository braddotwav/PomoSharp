using PomoSharp.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PomoSharp.Messages;

namespace PomoSharp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public override string Name => "Home";

    [ObservableProperty]
    private TimerStates _currentState;

    [ObservableProperty]
    private TimeSpan _remainingTime;

    [ObservableProperty]
    private bool _isTimerRunning;

    private readonly TimerStateMachine _stateMachine;
    private readonly CountdownTimer _countdownTimer;
    private readonly NotificationService _notificationService;

    public HomeViewModel(CountdownTimer timer)
    {
        _notificationService = new NotificationService();

        _countdownTimer = timer;
        _countdownTimer.OnElapsed += OnCountdownElapsed;
        _countdownTimer.OnDurationChanged += OnCountdownDurationChanged;
        _countdownTimer.OnComplete += OnCountdownComplete;

        _stateMachine = new TimerStateMachine(_countdownTimer);
        _stateMachine.OnStateChanged += OnStateChanged;

        WeakReferenceMessenger.Default.Register<SettingsSavedMessage>(this, (r, m) =>
        {
            _stateMachine.RefreshCurrentState();
        });
    }

    private void OnCountdownComplete()
    {
        _notificationService.Push(_stateMachine.CurrentState.CompletedNotificationContext);
    }

    private void OnStateChanged(TimerStates state)
    {
        CurrentState = state;
    }

    private void OnCountdownDurationChanged()
    {
        RemainingTime = _countdownTimer.Duration;
        IsTimerRunning = _countdownTimer.IsRunning;
    }

    private void OnCountdownElapsed(TimeSpan elapsed)
    {
        RemainingTime = elapsed;
    }

    [RelayCommand]
    private void TogglePlay()
    {
        _countdownTimer.TogglePlay();
    }

    [RelayCommand]
    private void TransitionTimerState(object? param)
    {
        if (param is not string state) 
            return;

        _countdownTimer.Stop();

        switch (state)
        {
            case "Pomodoro":
                _stateMachine.TransitionTo(TimerStates.POMODORO);
                break;
            case "Short Break":
                _stateMachine.TransitionTo(TimerStates.SHORT_BREAK);
                break;
            case "Long Break":
                _stateMachine.TransitionTo(TimerStates.LONG_BREAK);
                break;
            default:
                throw new InvalidOperationException("You are trying to set to a unknown state");
        }
    }

    public override void OnViewShow() { }

    public override void OnViewHide() { }
}