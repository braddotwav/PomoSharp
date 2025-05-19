using System.Windows;
using Timer = System.Timers.Timer;

namespace PomoSharp.Services;

public class CountdownTimer : IDisposable
{
    public TimeSpan Duration { get; private set; }
    public bool IsRunning { get; private set; } = false;

    public event Action? OnComplete;
    public event Action<TimeSpan>? OnDurationChanged;
    public event Action<TimeSpan>? OnElapsed;
    
    private readonly TimeSpan _oneSecondInterval = TimeSpan.FromSeconds(1);
    private readonly Timer _timer;
    private TimeSpan _remainingTime;

    public CountdownTimer()
    {
        _timer = new Timer(_oneSecondInterval);
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
        _timer.Start();
        IsRunning = true;
    }

    public void Stop()
    {
        _timer.Stop();
        IsRunning = false;
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