using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtensions
{
    public static Rect WithWidth(this Rect rect, float width)
    {
        Rect newRect = new Rect(rect);
        newRect.width = width;
        return newRect;
    }

    public static Rect WithHeight(this Rect rect, float height)
    {
        Rect newRect = new Rect(rect);
        newRect.height = height;
        return newRect;
    }

    public static Rect WithPosition(this Rect rect, Vector2 position)
    {
        Rect newRect = new Rect(rect);
        newRect.position = position;
        return newRect;
    }

    public static Rect WithCenter(this Rect rect, Vector2 center)
    {
        Rect newRect = new Rect(rect);
        newRect.center = center;
        return newRect;
    }

    public static Rect WithSize(this Rect rect, Vector2 size)
    {
        Rect newRect = new Rect(rect);
        newRect.size = size;
        return newRect;
    }

    public static Rect WithX(this Rect rect, float x)
    {
        Rect newRect = new Rect(rect);
        newRect.x = x;
        return newRect;
    }

    public static Rect WithY(this Rect rect, float y)
    {
        Rect newRect = new Rect(rect);
        newRect.y = y;
        return newRect;
    }

    public static bool Contains(this Rect rect, Rect other, bool includeEqual = true)
    {
        if (includeEqual && rect == other) return true;
        if (other.xMin < rect.xMin) return false;
        if (other.yMin < rect.yMin) return false;
        if (other.xMax > rect.xMax) return false;
        if (other.yMax > rect.yMax) return false;
        return true;
    }
}
