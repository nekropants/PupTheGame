﻿
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LABColour))]
public class LABColourDrawer : PropertyDrawer
{

    public override void OnGUI(Rect totalRect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(totalRect, label, property);

        SerializedProperty lProperty = property.FindPropertyRelative("_l");
        SerializedProperty aProperty = property.FindPropertyRelative("_a");
        SerializedProperty bProperty = property.FindPropertyRelative("_b");
        SerializedProperty alphaProperty = property.FindPropertyRelative("_alpha");

        LABColour labColour = new LABColour(EditorGUI.ColorField(totalRect, label, new LABColour(lProperty.floatValue, aProperty.floatValue, bProperty.floatValue, alphaProperty.floatValue).ToRGB()));

        lProperty.floatValue = labColour.L;
        aProperty.floatValue = labColour.A;
        bProperty.floatValue = labColour.B;
        alphaProperty.floatValue = labColour.Alpha;

        EditorGUI.EndProperty();
    }

}