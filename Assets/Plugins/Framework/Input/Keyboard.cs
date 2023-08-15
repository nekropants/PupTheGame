using UnityEngine;
using System.Collections;

/// <summary>
/// Static wrapper for keyboard input.
/// </summary>
public static class Keyboard
{
    /// <summary>
    /// Whether or not any key is currently held down.
    /// </summary>
    public static bool AnyKeyDown { get { return Input.anyKeyDown; } }

    /// <summary>
    /// Whether or not any key was pressed since the last frame.
    /// </summary>
    public static bool AnyKeyPressed { get { return Input.anyKey; } }

    /// <summary>
    /// Checks whether or not a key is currently pressed.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key is currently pressed</returns>
    public static bool GetKey(KeyCode key)
    {
        return Input.GetKey(key);
    }

    /// <summary>
    /// Checks whether or not a key was pressed since the last frame.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key was pressed since the last frame</returns>
    public static bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    /// <summary>
    /// Checks whether or not a key was released since the last frame.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key was released since the last frame</returns>
    public static bool GetKeyUp(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }
}
