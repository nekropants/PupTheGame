using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(BiggerThanZeroAttribute))]
public class BiggerThanZeroDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {

        if (property.propertyType == SerializedPropertyType.Float && property.floatValue <= 0)
        {
            property.floatValue = 0.001f;
        }
        if (property.propertyType == SerializedPropertyType.Integer && property.intValue <= 0)
        {
            property.intValue = 1;
        }
        EditorGUI.PropertyField(rect, property, label);
    }
}
