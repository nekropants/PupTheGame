using UnityEngine;
using System;
using UnityEngine.Assertions;

/// <summary>
/// A timer that will perform some action once the time has elapsed.
/// </summary>
public class DelayedAction
{

    private Action _action;
    private float _duration;
    private float _elapsedTime;
    private bool _useUnscaledTime;
    private bool _started;
    private bool _completed;
    private bool _repeat;

    /// <summary>
    /// The length of the timer.
    /// </summary>
    public float TimerDuration { get { return _duration; } }
    /// <summary>
    /// The amount of time that has elapsed while the timer has been running.
    /// </summary>
    public float TimeElapsed { get { return _elapsedTime; } }
    /// <summary>
    /// The amount of time remaining until the action is performed.
    /// </summary>
    public float TimeRemaining { get { return Mathf.Clamp(_duration - _elapsedTime, 0, _duration); } }
    /// <summary>
    /// The proportion of the total timer duration that has elapsed.
    /// </summary>
    public float ProportionElapsed { get { return Mathf.Clamp01(_elapsedTime / _duration); } }
    /// <summary>
    /// The proportion of the total timer duration is remaining.
    /// </summary>
    public float ProportionRemaining { get { return 1f - Mathf.Clamp01(_elapsedTime / _duration); } }
    /// <summary>
    /// Whether or not the timer is currently running.
    /// </summary>
    public bool TimerStarted { get { return _started; } }
    /// <summary>
    /// Whether or not the timer has completed and the action has been performed.
    /// </summary>
    public bool TimerCompleted { get { return _completed; } }
    /// <summary>
    /// Whether or not the timer will reset and continue running after it has performed the action.
    /// </summary>
    public bool TimerRepeats { get { return _repeat; } }

    /// <summary>
    /// Creates and returns a new delayed action and automatically starts the timer.
    /// </summary>
    /// <param name="delayDuration">How long the timer must last before performing the action</param>
    /// <param name="action">The action to perform when the time has elapsed</param>
    /// <param name="repeat">Whether or not the timer should reset and continue running after the action is performed</param>
    /// <param name="useUnscaledTime">Whether or not the timer will be affected by Time.timeScale</param>
    public static DelayedAction Create(float delayDuration, Action action, bool repeat = false, bool useUnscaledTime = false)
    {
        return new DelayedAction(delayDuration, action, repeat, useUnscaledTime);
    }

    /// <summary>
    /// Creates a new delayed action and automatically starts the timer.
    /// </summary>
    /// <param name="delayDuration">How long the timer must last before performing the action</param>
    /// <param name="action">The action to perform when the time has elapsed</param>
    /// <param name="repeat">Whether or not the timer should reset and continue running after the action is performed</param>
    /// <param name="useUnscaledTime">Whether or not the timer will be affected by Time.timeScale</param>
    public DelayedAction(float delayDuration, Action action, bool repeat, bool useUnscaledTime = false)
    {
        Assert.IsTrue(delayDuration >= 0);

        _action = action;
        _duration = delayDuration;
        _useUnscaledTime = useUnscaledTime;
        _repeat = repeat;

        UpdatePump.Register(Update);
        StartTimer();
    }

    /// <summary>
    /// Starts the timer. Unless the timer has been stopped, this shouldn't be nesseccary as the timer is automatically started when the object is created.
    /// </summary>
    public void StartTimer()
    {
        _started = true;
    }

    /// <summary>
    /// Pauses the timer, but does not reset it.
    /// </summary>
    public void StopTimer()
    {
        _started = false;
    }

    /// <summary>
    /// Sets a new duration and restarts the timer.
    /// </summary>
    /// <param name="newDuration">The new timer length</param>
    public void ResetTimer(float newDuration)
    {
        Assert.IsTrue(newDuration >= 0);
        _duration = newDuration;
        _elapsedTime = 0;
        _completed = false;
    }

    /// <summary>
    /// Restarts the timer.
    /// </summary>
    public void ResetTimer()
    {
        ResetTimer(_duration);
    }

    /// <summary>
    /// Completes the timer and performs the action.
    /// </summary>
    public void PerformAction()
    {
        if (_repeat)
        {
            ResetTimer();
        }
        else
        {
            _completed = true;
        }

        _action();
    }

    void Update()
    {
        if (_started && !_completed)
        {
            _elapsedTime += _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (_elapsedTime >= _duration)
            {
                PerformAction();
            }
        }
    }
}
