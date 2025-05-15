using System.Windows;
using Timer = System.Timers.Timer;

namespace PomoSharp.Services;

public class CountdownTimer : IDisposable
{
    public event Action? OnComplete;
    public event Action<TimeSpan>? OnDurationChanged;
    public event Action<TimeSpan>? OnElapsed;
    public TimeSpan Duration;
    public bool IsRunning { get; private set; } = false;

    private readonly TimeSpan _oneSecondInterval = TimeSpan.FromSeconds(1);
    private readonly Timer _timer;
    private TimeSpan _remainingTime;

    public CountdownTimer()
    {
        _timer = new Timer(TimeSpan.FromSeconds(1));
        _timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? sender, EventArgs e)
    {
        _remainingTime = _remainingTime.Subtract(_oneSecondInterval);

        Application.Current.Dispatcher.Invoke(() =>
        {
            OnElapsed?.Invoke(_remainingTime);
        });

        if (RemainingTimeHasReachedZero())
        {
            OnComplete?.Invoke();
        }
    }

    public void TogglePlay()
    {
        if (IsRunning)
        {
            Stop();
        }
        else
        {
            Play();
        }
    }

    public void SetDuration(TimeSpan duration)
    {
        Duration = duration;
        _remainingTime = Duration;
        OnDurationChanged?.Invoke(Duration);
    }

    public void Play()
    {
        if (!IsRunning) 
        {
            _timer.Start();
            IsRunning = true;
        }
    }

    public void Play(TimeSpan duration)
    {
        SetDuration(duration);
        Play();
    }

    public void Stop()
    {
        if (IsRunning) 
        {
            _timer.Stop();
            IsRunning = false;
        }
    }

    private bool RemainingTimeHasReachedZero() 
    {
        return _remainingTime == TimeSpan.Zero;
    }

    public void Dispose()
    {
        _timer.Elapsed -= OnTimerElapsed;
    }
}