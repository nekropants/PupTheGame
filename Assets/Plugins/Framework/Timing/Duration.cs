using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A simple class that will remember when it was instantiated and can be polled to see if a specific amount of time has elapsed since then.
/// </summary>
public struct Duration
{
    public bool UsingUnscaledTime { get { return _useUnscaledTime; } }

    private float _startTime;
    private float _duration;
    private bool _useUnscaledTime;

    /// <summary>
    /// The amount of time remaining until the duration has elapsed
    /// </summary>
    public float TimeRemaining
    {
        get { return Mathf.Max(0, (_startTime + _duration) - CurrentTime); }
    }

    /// <summary>
    /// The amount of time that has elapsed since the duration began
    /// </summary>
    public float TimeElapsed
    {
        get { return CurrentTime - _startTime; }
    }

    /// <summary>
    /// The proportion of the duration that has already elapsed
    /// </summary>
    public float ProportionElapsed
    {
        get
        {
            return Mathf.Clamp01((CurrentTime - _startTime) / _duration);
        }
    }

    /// <summary>
    /// The proportion of the duration that has not yet elapsed
    /// </summary>
    public float ProportionRemaining
    {
        get
        {
            return 1f - Mathf.Clamp01((CurrentTime - _startTime) / _duration);
        }
    }

    /// <summary>
    /// Whether or not the duration has elapsed
    /// </summary>
    public bool HasElapsed
    {
        get
        {
            return (CurrentTime - _startTime) >= _duration;
        }
    }

    private float CurrentTime
    {
        get { return _useUnscaledTime ? Time.unscaledTime : Time.time; }
    }

    /// <summary>
    /// Eases a value of the course of a duration
    /// </summary>
    /// <param name="startValue">The starting value</param>
    /// <param name="endvalue">The finishing value</param>
    /// <param name="easingType">The easing function to use</param>
    /// <returns>The eased value based on the porportion of the duration that has elapsed</returns>
    public float EaseValueOverDuration(float startValue, float endvalue, EasingType easingType = EasingType.Linear)
    {
        return Easing.Ease(ProportionElapsed, startValue, endvalue, easingType);
    }

    /// <summary>
    /// An amount of time that begins to elapse from the moment it is created.
    /// </summary>
    /// <param name="duration">The length of the duration</param>
    /// <param name="useUnscaledTime">Whether or not the duration should be affected by Time.timeScale</param>
    public Duration(float duration, bool useUnscaledTime = false)
    {
        Assert.IsTrue(duration >= 0);
        _startTime = 0;
        _duration = duration;
        _useUnscaledTime = useUnscaledTime;
        Reset();
    }

    /// <summary>
    /// Sets the duration to a new length of time.
    /// </summary>
    /// <param name="duration"></param>
    public void Set(float duration)
    {
        Assert.IsTrue(duration >= 0);
        _duration = duration;
    }

    /// <summary>
    /// Resets the amount that the duration has elapsed
    /// </summary>
    public void Reset()
    {
        _startTime = CurrentTime;
    }

    /// <summary>
    /// Sets the duration to a new length of time. Also resets the amount that the duration has elapsed.
    /// </summary>
    /// <param name="duration"></param>
    public void Reset(float duration)
    {
        Assert.IsTrue(duration >= 0);
        _duration = duration;
        _startTime = CurrentTime;
    }



}
