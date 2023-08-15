﻿using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(ExposeIfAttribute), true)]
public class ExposeIfDrawer : PropertyDrawer
{


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {


        return (attribute as ExposeIfAttribute).EvaluateCondition(property.serializedObject.targetObject) ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {


        if ((attribute as ExposeIfAttribute).EvaluateCondition(property.serializedObject.targetObject)) EditorGUI.PropertyField(position, property, label, true);
    }




}
