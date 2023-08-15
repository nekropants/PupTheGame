using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// A timer that can be started, paused, stopped and reset.
/// </summary>
public class GameTimer
{

    private float? _lastUpdateTime;
    private float _elapsedTime;
    private float _duration;
    private bool _useUnscaledTime;
    private bool _started;
    private bool _paused;

    /// <summary>
    /// Whether or not the timer has completed.
    /// </summary>
    public bool HasFinished
    {
        get
        {
            UpdateTime();
            return _elapsedTime >= _duration;
        }
    }

    /// <summary>
    /// The total amount of time the timer takes to complete.
    /// </summary>
    public float Duration
    {
        get { return _duration; }
    }

    /// <summary>
    /// The amount of time remaining before the timer completes.
    /// </summary>
    public float TimeRemaining
    {
        get
        {
            UpdateTime();
            return Mathf.Max(0, _duration - _elapsedTime);
        }
    }

    /// <summary>
    /// The amount of time that has elapsed while the timer has been tunning.
    /// </summary>
    public float TimeElapsed
    {
        get
        {
            UpdateTime();
            return _elapsedTime;
        }
    }

    /// <summary>
    /// Whether or not the timer is started and not paused.
    /// </summary>
    public bool IsRunning
    {
        get { return _started && !_paused; }
    }

    /// <summary>
    /// Whether or not the timer has been started (will be false if it stopped again).
    /// </summary>
    public bool IsStarted
    {
        get { return _started; }
    }

    /// <summary>
    /// Whether or not the timer has been started (will be false if it stopped again).
    /// </summary>
    public bool IsPaused
    {
        get { return _paused; }
    }

    /// <summary>
    /// What proportion of the timer has already elapsed.
    /// </summary>
    public float ProportionElapsed
    {
        get
        {
            UpdateTime();
            return Mathf.Clamp01(_elapsedTime / _duration);
        }
    }

    /// <summary>
    /// What proportion of the timer is remaining.
    /// </summary>
    public float ProportionRemaining
    {
        get
        {
            UpdateTime();
            return 1f - Mathf.Clamp01(_elapsedTime / _duration);
        }
    }

    /// <summary>
    /// Creates a new GameTimer that can be started, stopped and resumed. Note that the timer must still be started with StartTimer().
    /// </summary>
    /// <param name="duration">The total amount of time the timer takes to complete</param>
    /// <param name="useUnscaledTime">Whether or not the timer should be affected by Time.timeScale</param>
    public GameTimer(float duration, bool useUnscaledTime = false)
    {
        Assert.IsTrue(duration >= 0);
        _duration = duration;
        _useUnscaledTime = useUnscaledTime;
    }

    /// <summary>
    /// Resets the timer back to zero. Does not affect whether or not the timer is running.
    /// </summary>
    public void Reset()
    {
        _elapsedTime = 0;
        _lastUpdateTime = null;
    }

    /// <summary>
    /// Forces the timer to finish right now.
    /// </summary>
    public void Finish()
    {
        _elapsedTime = _duration;
    }

    /// <summary>
    /// Starts the timer. Will also unpause it.
    /// </summary>
    public void Start()
    {
        _started = true;
        _paused = false;
        UpdateTime();
    }

    /// <summary>
    /// Starts the timer and sets the duration.
    /// </summary>
    /// <param name="duration">The timer duration</param>
    public void Start(float duration)
    {
        Assert.IsTrue(duration >= 0);
        _started = true;
        _duration = duration;
        UpdateTime();
    }

    /// <summary>
    /// Stops and then resets the timer.
    /// </summary>
    public void Stop()
    {
        _started = false;
        Reset();
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause()
    {
        _paused = true;
    }

    /// <summary>
    /// Unpauses the timer.
    /// </summary>
    public void Unpause()
    {
        _paused = false;
    }

    /// <summary>
    /// Returns a coroutine that will yield until the timer has finished. The coroutine is started on the CoroutineUtils object.
    /// </summary>
    /// <returns>A coroutine that waits for timer completion</returns>
    public Coroutine WaitForCompletion()
    {
        return CoroutineUtils.StartCoroutine(WaitForCompletionRoutine());
    }

    /// <summary>
    /// Sets the duration. Does not affect whether or not the timer is running.
    /// </summary>
    /// <param name="duration">The new duration</param>
    public void SetDuration(float duration)
    {
        Assert.IsTrue(duration >= 0);
        _duration = duration;
    }

    void UpdateTime()
    {
        float time = _useUnscaledTime ? Time.unscaledTime : Time.time;
        if (_started && !_paused && _lastUpdateTime != null)
        {
            _elapsedTime += time - _lastUpdateTime.Value;
        }
        _lastUpdateTime = time;
    }

    IEnumerator WaitForCompletionRoutine()
    {
        while (!HasFinished) yield return null;
    }


}
