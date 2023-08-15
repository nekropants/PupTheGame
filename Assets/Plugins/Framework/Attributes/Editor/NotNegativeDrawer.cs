using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(NotNegativeAttribute))]
public class NotNegativeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {

        if (property.propertyType == SerializedPropertyType.Float && property.floatValue < 0)
        {
            property.floatValue = 0;
        }
        if (property.propertyType == SerializedPropertyType.Integer && property.intValue < 0)
        {
            property.intValue = 0;
        }
        EditorGUI.PropertyField(rect, property, label);
    }
}
