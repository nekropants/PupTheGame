using System;
using UnityEngine;

public enum MonoBehaviourMethodType
{
    Update,
    LateUpdate,
    FixedUpdate,
    OnApplicationFocus,
    OnApplicationPause,
    OnApplicationQuit,
    OnDrawGizmos,
    OnGUI,
    OnPreCull,
    OnPreRender,
    OnPostRender,
}

/// <summary>
/// A utility for registering methods to be called during frame updates, this allows classes that are not MonoBehaviours to hook into MonoBehaviour methods.
/// </summary>
public class UpdatePump : AutoSingletonBehaviour<UpdatePump>
{

    private static Action[] _delegates = new Action[EnumUtils.GetCount<MonoBehaviourMethodType>()];

    /// <summary>
    /// Register a method to be called during update. DON'T FORGET TO DEREGISTER.
    /// </summary>
    /// <param name="method">The method to call</param>
    /// <param name="type">The update type to call the method in</param>
    public static void Register(Action method, MonoBehaviourMethodType type = MonoBehaviourMethodType.Update)
    {
        EnsureInstanceExists();
        _delegates[(int)type] += method;
    }

    /// <summary>
    /// Deregisters a previously registered update method.
    /// </summary>
    /// <param name="method">The method to deregister</param>
    public static void Deregister(Action method, MonoBehaviourMethodType type = MonoBehaviourMethodType.Update)
    {
        _delegates[(int)type] -= method;

    }

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        gameObject.hideFlags = HideFlags.HideAndDontSave;
    }

    void Update()
    {
        if (_delegates[(int)MonoBehaviourMethodType.Update] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.Update].Invoke();
        }
    }

    void LateUpdate()
    {

        if (_delegates[(int)MonoBehaviourMethodType.LateUpdate] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.LateUpdate].Invoke();
        }
    }

    void OnApplicationFocus()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnApplicationFocus] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnApplicationFocus].Invoke();
        }
    }

    void OnApplicationPause()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnApplicationPause] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnApplicationPause].Invoke();
        }
    }
    void OnApplicationQuit()
    {

        if (_delegates[(int)MonoBehaviourMethodType.OnApplicationQuit] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnApplicationQuit].Invoke();
        }
    }

    void OnDrawGizmos()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnDrawGizmos] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnDrawGizmos].Invoke();
        }
    }

    void OnGUI()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnGUI] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnGUI].Invoke();
        }
    }


    void OnPreCull()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnPreCull] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnPreCull].Invoke();
        }
    }

    void OnPreRender()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnPreRender] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnPreRender].Invoke();
        }
    }

    void OnPostRender()
    {
        if (_delegates[(int)MonoBehaviourMethodType.OnPostRender] != null)
        {
            _delegates[(int)MonoBehaviourMethodType.OnPostRender].Invoke();
        }
    }





}
