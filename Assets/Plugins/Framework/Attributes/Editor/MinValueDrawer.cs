using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinValueAttribute))]
public class MinValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        MinValueAttribute minValue = attribute as MinValueAttribute;
        if (property.propertyType == SerializedPropertyType.Float) property.floatValue = Mathf.Max(property.floatValue, minValue.Value);
        if (property.propertyType == SerializedPropertyType.Integer) property.intValue = Mathf.Max(property.intValue, (int)minValue.Value);

        EditorGUI.PropertyField(rect, property, label);
    }
}
