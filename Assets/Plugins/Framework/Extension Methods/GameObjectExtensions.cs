using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum GameObjectLabelIcon
{
    Gray = 0,
    Blue,
    Teal,
    Green,
    Yellow,
    Orange,
    Red,
    Purple
}

public enum GameObjectShapeIcon
{
    CircleGray = 0,
    CircleBlue,
    CircleTeal,
    CircleGreen,
    CircleYellow,
    CircleOrange,
    CircleRed,
    CirclePurple,
    DiamondGray,
    DiamondBlue,
    DiamondTeal,
    DiamondGreen,
    DiamondYellow,
    DiamondOrange,
    DiamondRed,
    DiamondPurple
}

/// <summary>
/// Extension methods for GameObjects.
/// </summary>
public static class GameObjectExtensions
{

    public static void SetIcon(GameObject go, GameObjectLabelIcon icon)
    {
#if UNITY_EDITOR
        IconUtility.SetIcon(go, icon);
#endif
    }

    public static void SetIcon(GameObject go, GameObjectShapeIcon icon)
    {
#if UNITY_EDITOR
        IconUtility.SetIcon(go, icon);
#endif
    }

    public static Bounds GetBounds(this GameObject go)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            bounds = renderer.bounds;
        }

        if (bounds.extents.x == 0)
        {
            bounds = new Bounds(go.transform.position, Vector3.zero);
            foreach (Transform child in go.transform)
            {
                renderer = child.GetComponent<Renderer>();
                if (renderer)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                else
                {
                    bounds.Encapsulate(GetBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }


    /// <summary>
    /// Sets the layer of the GameObject and all its children
    /// </summary>
    /// <param name="layer">The new layer</param>
    public static void SetLayerRecursively(this GameObject go, int layer)
    {
        go.layer = layer;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).gameObject.SetLayerRecursively(layer);
        }
    }

    /// <summary>
    /// Checks whether this object is on one of the layers contained in a layer mask.
    /// </summary>
    /// <param name="mask">The layer mask to check</param>
    /// <returns>True if the object is on a layer in the mask</returns>
    public static bool IsInLayerMask(this GameObject go, LayerMask mask)
    {
        return ((1 << go.layer) & mask.value) > 0;
    }

    /// <summary>
    /// Finds all the components of a type that are in the GameObject's children, but not on the object itself.
    /// </summary>
    /// <returns>An array of components</returns>
    public static T[] GetComponentsInChildrenOnly<T>(this GameObject go) where T : Component
    {
        List<T> childComponents = new List<T>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            childComponents.AddRange(go.transform.GetChild(i).GetComponentsInChildren<T>());
        }
        return childComponents.ToArray();
    }

    /// <summary>
    /// Finds the first component of a type that is in the GameObject's children, but not on the object itself.
    /// </summary>
    /// <returns>The first child component of the type specified</returns>
    public static T GetComponentInChildrenOnly<T>(this GameObject go) where T : Component
    {

        for (int i = 0; i < go.transform.childCount; i++)
        {
            T component = go.transform.GetChild(i).GetComponentInChildren<T>();
            if (component != null) return component;
        }
        return null;
    }

    /// <summary>
    /// Finds the first component of a type that is in the GameObject's children, but not on the object itself.
    /// </summary>
    /// <returns>The first child component of the type specified</returns>
    public static T GetComponentInParentsOnly<T>(this GameObject go) where T : Component
    {
        if (go.transform.parent != null)
        {
            return go.transform.parent.GetComponentInParent<T>();
        }
        return null;
    }

    /// <summary>
    /// Gets a component if it exists on the object, otherwise adds one.
    /// </summary>
    /// <typeparam name="T">The component type to check</typeparam>
    /// <returns>The component on the object</returns>
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }


    [Conditional("UNITY_EDITOR")]
    public static void ReorderComponents(this GameObject go, IComparer<Component> comparer)
    {
#if UNITY_EDITOR
        List<Component> components = new List<Component>();
        go.GetComponents(components);

        // Bubble sort
        int n = components.Count;
        while (n != 0)
        {
            int newN = 0;
            for (int i = 1; i <= n - 1; i++)
            {
                if (comparer.Compare(components[i - 1], components[i]) < 0)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(components[i]);

                    Component tmp = components[i - 1];
                    components[i - 1] = components[i];
                    components[i] = tmp;

                    newN = i;
                }
            }
            n = newN;
        }
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void ReorderComponents(this GameObject go, IComparer<MonoBehaviour> comparer)
    {
        ReorderComponents(go, new MonoBehaviourComparer(comparer));
    }

    [Conditional("UNITY_EDITOR")]
    public static void ReorderComponents(this GameObject go)
    {
        ReorderComponents(go, new ComponentComparer());
    }



    class MonoBehaviourComparer : IComparer<Component>
    {
        private IComparer<MonoBehaviour> _comparer;

        public MonoBehaviourComparer(IComparer<MonoBehaviour> comparer)
        {
            _comparer = comparer;
        }

        public int Compare(Component x, Component y)
        {
            if (x.IsTypeOf<Transform>()) return 1;
            if (y.IsTypeOf<Transform>()) return -1;

            bool xIsMonoBehaviour = x.IsTypeOf<MonoBehaviour>();
            bool yIsMonoBehaviour = y.IsTypeOf<MonoBehaviour>();

            if (yIsMonoBehaviour && !xIsMonoBehaviour) return 1;
            if (xIsMonoBehaviour && !yIsMonoBehaviour) return -1;

            return _comparer.Compare(x as MonoBehaviour, y as MonoBehaviour);
        }
    }

    class ComponentComparer : IComparer<Component>
    {
        public int Compare(Component x, Component y)
        {
            if (x.IsTypeOf<Transform>()) return 1;
            if (y.IsTypeOf<Transform>()) return -1;

            bool xIsMonoBehaviour = x.IsTypeOf<MonoBehaviour>();
            bool yIsMonoBehaviour = y.IsTypeOf<MonoBehaviour>();

            if (yIsMonoBehaviour && !xIsMonoBehaviour) return 1;
            if (xIsMonoBehaviour && !yIsMonoBehaviour) return -1;

            return y.GetType().Name.CompareTo(x.GetType().Name);
        }
    }

#if UNITY_EDITOR
    static class IconUtility
    {

        private static GUIContent[] _labelIcons;
        private static GUIContent[] _largeIcons;
        private static MethodInfo _setIconMethod;

        public static void SetIcon(GameObject go, GameObjectLabelIcon icon)
        {
            if (_labelIcons == null)
            {
                _labelIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            }

            if (_setIconMethod == null)
            {
                _setIconMethod = typeof(UnityEditor.EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            }

            _setIconMethod.Invoke(null, new object[] { go, _labelIcons[(int)icon].image as Texture2D });
        }

        public static void SetIcon(GameObject go, GameObjectShapeIcon icon)
        {

            if (_largeIcons == null)
            {
                _largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
            }

            if (_setIconMethod == null)
            {
                _setIconMethod = typeof(UnityEditor.EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            }

            _setIconMethod.Invoke(null, new object[] { go, _largeIcons[(int)icon].image as Texture2D });
        }

        private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] guiContentArray = new GUIContent[count];

            var t = typeof(UnityEditor.EditorGUIUtility);
            var mi = t.GetMethod("IconContent", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);

            for (int index = 0; index < count; ++index)
            {
                guiContentArray[index] = mi.Invoke(null, new object[] { baseName + (object)(startIndex + index) + postFix }) as GUIContent;
            }

            return guiContentArray;
        }

    }
#endif
}
