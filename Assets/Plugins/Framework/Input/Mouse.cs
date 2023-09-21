using UnityEngine;
using System.Collections;

/// <summary>
/// Static wrapper for mouse input.
/// </summary>
public static class MouseInput
{
    public enum Button
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    /// <summary>
    /// Whether or not there is a mouse connected and available for use.
    /// </summary>
    public static bool IsAvailable
    {
        get { return Input.mousePresent; }
    }

    /// <summary>
    /// The position of the mouse in screen coordinates.
    /// </summary>
    public static Vector2 Position
    {
        get { return Input.mousePosition; }
    }

    /// <summary>
    /// The position of the mouse in world space (Projected using Camera.main).
    /// </summary>
    public static Vector3 WorldPosition
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }

    /// <summary>
    /// The mouse movement in this frame.
    /// </summary>
    public static Vector2 PositionDelta
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }

    /// <summary>
    /// The mouse wheel movement this frame.
    /// </summary>
    public static float ScrollwheelDelta
    {
        get { return Input.GetAxis("Mouse ScrollWheel"); }
    }

    /// <summary>
    /// Whether or not the mouse wheel has been scrolled up this frame.
    /// </summary>
    public static bool HasScrolledUp
    {
        get { return Input.GetAxis("Mouse ScrollWheel") > 0; }
    }

    /// <summary>
    /// Whether or not the mouse wheel has been scrolled down this frame.
    /// </summary>
    public static bool HasScrolledDown
    {
        get { return Input.GetAxis("Mouse ScrollWheel") < 0; }
    }

    /// <summary>
    /// Checks whether or not a mouse button is currently pressed.
    /// </summary>
    /// <param name="button">The button to check</param>
    /// <returns>True if the button is currently pressed</returns>
    public static bool GetButton(Button button)
    {
        return Input.GetMouseButton((int)button);
    }

    /// <summary>
    /// Checks whether or not a mouse button was pressed since the last frame.
    /// </summary>
    /// <param name="button">The button to check</param>
    /// <returns>True if the button was pressed since the last frame</returns>
    public static bool GetButtonDown(Button button)
    {
        return Input.GetMouseButtonDown((int)button);
    }

    /// <summary>
    /// Checks whether or not a mouse button was released since the last frame.
    /// </summary>
    /// <param name="button">The button to check</param>
    /// <returns>True if the button was released since the last frame</returns>
    public static bool GetButtonUp(Button button)
    {
        return Input.GetMouseButtonUp((int)button);
    }

}
