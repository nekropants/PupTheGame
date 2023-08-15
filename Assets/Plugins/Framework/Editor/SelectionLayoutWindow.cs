using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectionLayoutWindow : EditorWindow
{
    enum Tab
    {
        Line,
        Grid
    }

    enum Axis
    {
        X,
        Y,
        Z
    }

    enum Plane
    {
        XY,
        XZ,
        ZY
    }

    enum Space
    {
        Local,
        World
    }

    enum SpacingMode
    {
        FixedInterval,
        BoundsAndSpacing
    }

    private const float WIDTH = 300;
    private const float HEIGHT = 155;

    private static Space _space;
    private static Axis _axis;
    private static Plane _plane;
    private static Tab _tab;
    private static SpacingMode _spacingMode;
    private static float _lineInterval = 1f;
    private static float _lineSpacing = 0f;
    private static int _rows = 1;
    private static Vector2 _gridSpacing = new Vector2(1f, 1f);

    [MenuItem("GameObject/Selection/Layout", false, 0)]
    static void ShowWindow()
    {
        SelectionLayoutWindow window = GetWindow<SelectionLayoutWindow>(true, "Layout Selection", true);

        Rect rect = new Rect();
        rect.width = WIDTH;
        rect.height = HEIGHT;
        rect.center = new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);

        window.position = rect;
        window.ShowUtility();
    }

    void OnGUI()
    {
        _tab = (Tab)GUILayout.Toolbar((int)_tab, Enum.GetNames(typeof(Tab)));

        EditorGUILayout.Space();

        switch (_tab)
        {
            case Tab.Line:
                LineTab();
                break;

            case Tab.Grid:
                GridTab();
                break;
        }
    }

    void LineTab()
    {
        _axis = (Axis)EditorGUILayout.EnumPopup("Axis", _axis);
        _space = (Space)EditorGUILayout.EnumPopup("Space", _space);
        _spacingMode = (SpacingMode)EditorGUILayout.EnumPopup("Spacing Mode", _spacingMode);

        if (_spacingMode == SpacingMode.FixedInterval)
        {
            _lineInterval = EditorGUILayout.FloatField("Interval", _lineInterval);
        }
        if (_spacingMode == SpacingMode.BoundsAndSpacing)
        {
            _lineSpacing = EditorGUILayout.FloatField("Spacing", _lineSpacing);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(Selection.transforms.Length <= 1);
        if (GUILayout.Button("Layout"))
        {
            LineLayout(GetOrderedSelection());
        }
        EditorGUI.EndDisabledGroup();
    }

    void GridTab()
    {
        _plane = (Plane)EditorGUILayout.EnumPopup("Plane", _plane);
        _space = (Space)EditorGUILayout.EnumPopup("Space", _space);
        _rows = Mathf.Max(EditorGUILayout.IntField("Rows", _rows), 1);
        _gridSpacing.x = EditorGUILayout.FloatField("Horizontal Spacing", _gridSpacing.x);
        _gridSpacing.y = EditorGUILayout.FloatField("Vertical Spacing", _gridSpacing.y);

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(Selection.transforms.Length <= 1);
        if (GUILayout.Button("Layout"))
        {
            GridLayout(GetOrderedSelection());
        }
        EditorGUI.EndDisabledGroup();
    }

    void GridLayout(Transform[] transforms)
    {
        Undo.RecordObjects(transforms, "Layout Selection");

        Vector3 horizontalDirection = Vector3.zero;
        Vector3 verticalDirection = Vector3.zero;

        switch (_plane)
        {
            case Plane.XY:
                horizontalDirection = Vector3.right;
                verticalDirection = Vector3.up;
                break;

            case Plane.XZ:
                horizontalDirection = Vector3.right;
                verticalDirection = Vector3.forward;
                break;

            case Plane.ZY:
                horizontalDirection = Vector3.forward;
                verticalDirection = Vector3.up;
                break;
        }

        int columns = Mathf.CeilToInt((float)transforms.Length / _rows);
        int index = 0;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (index < transforms.Length)
                {
                    Vector3 offset = (horizontalDirection * x * _gridSpacing.x) + (verticalDirection * y * _gridSpacing.y);
                    if (_space == Space.Local)
                    {
                        transforms[index].position = transforms[0].position + (transforms[0].rotation * offset);
                    }
                    else
                    {
                        transforms[index].position = transforms[0].position + offset;
                    }

                    transforms[index].rotation = transforms[0].rotation;

                    index++;
                }
            }



        }
    }

    void LineLayout(Transform[] transforms)
    {
        Undo.RecordObjects(transforms, "Layout Selection");

        Vector3 direction = _axis == Axis.X ? Vector3.right : _axis == Axis.Y ? Vector3.up : Vector3.forward;
        float distance = 0;

        for (int i = 1; i < transforms.Length; i++)
        {
            if (_spacingMode == SpacingMode.FixedInterval)
            {
                distance += _lineInterval;
            }
            else
            {
                switch (_axis)
                {
                    case Axis.X:
                        distance += transforms[i].gameObject.GetBounds().size.x + _lineSpacing;
                        break;
                    case Axis.Y:
                        distance += transforms[i].gameObject.GetBounds().size.y + _lineSpacing;
                        break;
                    case Axis.Z:
                        distance += transforms[i].gameObject.GetBounds().size.z + _lineSpacing;
                        break;
                }
            }

            if (_space == Space.Local)
            {
                transforms[i].position = transforms[0].position + (transforms[0].rotation * direction * distance);
            }
            else
            {
                transforms[i].position = transforms[0].position + (direction * distance);
            }

            transforms[i].rotation = transforms[0].rotation;
        }
    }

    Transform[] GetOrderedSelection()
    {
        List<Transform> transforms = new List<Transform>(Selection.transforms);
        transforms.Sort((x, y) => x.GetSiblingIndex().CompareTo(y.GetSiblingIndex()));
        return transforms.ToArray();
    }


}
