using PomoSharp.Services;

namespace PomoSharp.Tests;

public class CountdownTimerTests
{
    private readonly CountdownTimer _countdownTimer;

    public CountdownTimerTests()
    {
        _countdownTimer = new CountdownTimer();
    }

    [Fact]
    private void Play_StartingPlay_SetIsRunningToTrue()
    {
        _countdownTimer.Play();

        Assert.True(_countdownTimer.IsRunning);
    }

    [Fact]
    private void Stop_StoppingPlay_SetIsRunningToFalse()
    {
        _countdownTimer.Play();
        _countdownTimer.Stop();

        Assert.False(_countdownTimer.IsRunning);
    }

    [Fact]
    private void SetDuration_ValidDuration_SetDurationAndFiresEvent() 
    {
        var duration = TimeSpan.FromMinutes(25);
        TimeSpan eventDuration = TimeSpan.Zero;
        _countdownTimer.OnDurationChanged += x => eventDuration = x;

        _countdownTimer.SetDuration(duration);

        Assert.Equal(duration, _countdownTimer.Duration);
        Assert.Equal(duration, eventDuration);
    }
}