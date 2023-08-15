using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ClampAttribute))]
public class ClampDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty (rect, label, property);
        ClampAttribute clamp = attribute as ClampAttribute;
        if (property.propertyType == SerializedPropertyType.Float) property.floatValue = Mathf.Clamp(property.floatValue, clamp.min, clamp.max);
        if (property.propertyType == SerializedPropertyType.Integer) property.intValue = Mathf.Clamp(property.intValue, (int)clamp.min, (int)clamp.max);

        EditorGUI.PropertyField(rect, property, label);
        EditorGUI.EndProperty ();
    }
}
