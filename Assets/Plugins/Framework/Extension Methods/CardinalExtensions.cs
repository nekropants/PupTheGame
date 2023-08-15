using UnityEngine;
using System.Collections;

/// <summary>
/// Extension methods for cardinal enums.
/// </summary>
public static class CardinalExtensions
{
    /// <summary>
    /// Returns this cardinal rotated clockwise by a certain number of 90 degree rotations.
    /// </summary>
    /// <param name="rotations">The number of 90 degree rotations to perform</param>
    /// <returns>The rotated cardinal</returns>
    public static Cardinal4 RotatedClockwise(this Cardinal4 cardinal, int rotations = 1)
    {
        return (Cardinal4)MathUtils.Wrap((int)cardinal + rotations, 0, 3);
    }

    /// <summary>
    /// Returns this cardinal rotated clockwise by a certain number of 45 degree rotations.
    /// </summary>
    /// <param name="rotations">The number of 90 degree rotations to perform</param>
    /// <returns>The rotated cardinal</returns>
    public static Cardinal8 RotatedClockwise(this Cardinal8 cardinal, int rotations = 1)
    {
        return (Cardinal8)MathUtils.Wrap((int)cardinal + rotations, 0, 7);
    }

    /// <summary>
    /// Returns this cardinal rotated anti-clockwise by a certain number of 90 degree rotations.
    /// </summary>
    /// <param name="rotations">The number of 90 degree rotations to perform</param>
    /// <returns>The rotated cardinal</returns>
    public static Cardinal4 RotatedAntiClockwise(this Cardinal4 cardinal, int rotations = 1)
    {
        return (Cardinal4)MathUtils.Wrap((int)cardinal - rotations, 0, 3);
    }

    /// <summary>
    /// Returns this cardinal rotated anti-clockwise by a certain number of 45 degree rotations.
    /// </summary>
    /// <param name="rotations">The number of 90 degree rotations to perform</param>
    /// <returns>The rotated cardinal</returns>
    public static Cardinal8 RotatedAntiClockwise(this Cardinal8 cardinal, int rotations = 1)
    {
        return (Cardinal8)MathUtils.Wrap((int)cardinal - rotations, 0, 7);
    }

    /// <summary>
    /// Returns the vector representation of this cardinal.
    /// </summary>
    /// <returns>This cardinal as a Vector2</returns>
    public static Vector2 ToVector(this Cardinal4 cardinal)
    {
        switch (cardinal)
        {
            case Cardinal4.North: return Vector2.up;
            case Cardinal4.South: return -Vector2.up;
            case Cardinal4.West: return -Vector2.right;
            case Cardinal4.East: return Vector2.right;
        }

        return Vector2.zero;
    }

    /// <summary>
    /// Returns the vector representation of this cardinal.
    /// </summary>
    /// <returns>This cardinal as a Vector2</returns>
    public static Vector2 ToVector(this Cardinal8 cardinal)
    {
        switch (cardinal)
        {
            case Cardinal8.North: return Vector2.up;
            case Cardinal8.South: return -Vector2.up;
            case Cardinal8.East: return Vector2.right;
            case Cardinal8.West: return -Vector2.right;

            case Cardinal8.NorthEast: return new Vector2(1, 1).normalized;
            case Cardinal8.NorthWest: return new Vector2(1, -1).normalized;
            case Cardinal8.SouthEast: return new Vector2(1, -1).normalized;
            case Cardinal8.SouthWest: return new Vector2(-1, -1).normalized;
        }

        return Vector2.zero;
    }

    /// <summary>
    /// Returns this Cardinal8 as a Cardinal4, 45 degree cardinals are snapped to the cardinal that is 45 degrees anti-clockwise.
    /// </summary>
    /// <returns>This Cardinal8 as a Cardinal4</returns>
    public static Cardinal4 ToCardinal4(this Cardinal8 cardinal)
    {
        switch (cardinal)
        {
            case Cardinal8.North: return Cardinal4.North;
            case Cardinal8.South: return Cardinal4.South;
            case Cardinal8.East: return Cardinal4.East;
            case Cardinal8.West: return Cardinal4.West;

            case Cardinal8.NorthEast: return Cardinal4.North;
            case Cardinal8.NorthWest: return Cardinal4.West;
            case Cardinal8.SouthEast: return Cardinal4.East;
            case Cardinal8.SouthWest: return Cardinal4.South;
        }

        return Cardinal4.North;
    }

    /// <summary>
    /// Returns this Cardinal4 as a Cardinal8.
    /// </summary>
    /// <returns>This Cardinal4 as a Cardinal8</returns>
    public static Cardinal8 ToCardinal8(this Cardinal4 cardinal)
    {
        switch (cardinal)
        {
            case Cardinal4.North: return Cardinal8.North;
            case Cardinal4.South: return Cardinal8.South;
            case Cardinal4.West: return Cardinal8.West;
            case Cardinal4.East: return Cardinal8.East;
        }

        return Cardinal8.North;
    }




}
