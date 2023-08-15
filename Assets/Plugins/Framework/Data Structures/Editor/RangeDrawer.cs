using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect totalRect, SerializedProperty property, GUIContent label)
    {
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUI.BeginProperty(totalRect, label, property);

        Rect rect = EditorGUI.PrefixLabel(totalRect, GUIUtility.GetControlID(FocusType.Passive), label);
        SerializedProperty minProperty = property.FindPropertyRelative("_min");
        SerializedProperty maxProperty = property.FindPropertyRelative("_max");

        float valueWidth = (rect.width * 0.5f);

        Rect minValueRect = new Rect(rect.xMin, rect.y, valueWidth - 3f, rect.height);
        Rect maxValueRect = new Rect(rect.x + (rect.width * 0.5f), rect.y, valueWidth, rect.height);

        EditorGUIUtility.labelWidth = 30f;
        EditorGUI.PropertyField(minValueRect, minProperty, new GUIContent("Min"));
        EditorGUIUtility.labelWidth = 33f;
        EditorGUI.PropertyField(maxValueRect, maxProperty, new GUIContent("Max"));
        EditorGUIUtility.labelWidth = labelWidth;

        if (minProperty.intValue > maxProperty.intValue)
        {
            maxProperty.intValue = minProperty.intValue;
        }

        EditorGUI.EndProperty();
    }

}

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect totalRect, SerializedProperty property, GUIContent label)
    {
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUI.BeginProperty(totalRect, label, property);

        Rect rect = EditorGUI.PrefixLabel(totalRect, GUIUtility.GetControlID(FocusType.Passive), label);
        SerializedProperty minProperty = property.FindPropertyRelative("_min");
        SerializedProperty maxProperty = property.FindPropertyRelative("_max");


        float valueWidth = (rect.width * 0.5f);

        Rect minValueRect = new Rect(rect.xMin, rect.y, valueWidth - 3f, rect.height);
        Rect maxValueRect = new Rect(rect.x + (rect.width * 0.5f), rect.y, valueWidth, rect.height);

        EditorGUIUtility.labelWidth = 30f;
        EditorGUI.PropertyField(minValueRect, minProperty, new GUIContent("Min"));
        EditorGUIUtility.labelWidth = 33f;
        EditorGUI.PropertyField(maxValueRect, maxProperty, new GUIContent("Max"));
        EditorGUIUtility.labelWidth = labelWidth;

        if (minProperty.floatValue > maxProperty.floatValue)
        {
            maxProperty.floatValue = minProperty.floatValue;
        }

        EditorGUI.EndProperty();
    }

}
