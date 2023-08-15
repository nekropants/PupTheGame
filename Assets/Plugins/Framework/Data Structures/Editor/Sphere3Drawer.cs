﻿using System;
using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomPropertyDrawer(typeof(Sphere3))]
public class Sphere3Drawer : PropertyDrawer
{

    private bool _expanded;
    private float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        _height = rect.y;

        SerializedProperty positionProperty = property.FindPropertyRelative("_center");
        SerializedProperty radiusProperty = property.FindPropertyRelative("_radius");

        EditorGUI.BeginProperty(rect, label, property);

        rect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
        _expanded = EditorGUI.Foldout(rect, _expanded, label, true);

        if (_expanded)
        {
            EditorGUI.indentLevel++;

            rect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, positionProperty);
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(rect, radiusProperty);
        }

        EditorGUI.EndProperty();
        _height = rect.y - _height + EditorGUIUtility.singleLineHeight;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return _height;
    }

}
