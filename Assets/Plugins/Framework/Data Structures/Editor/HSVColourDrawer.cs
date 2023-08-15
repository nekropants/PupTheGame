
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HSVColour))]
public class HSVColourDrawer : PropertyDrawer
{

    public override void OnGUI(Rect totalRect, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(totalRect, label, property);

        SerializedProperty hueProperty = property.FindPropertyRelative("_h");
        SerializedProperty saturationProperty = property.FindPropertyRelative("_s");
        SerializedProperty valueProperty = property.FindPropertyRelative("_v");
        SerializedProperty alphaProperty = property.FindPropertyRelative("_a");


        HSVColour hsvColour = EditorGUI.ColorField(totalRect, label, new HSVColour(hueProperty.floatValue, saturationProperty.floatValue, valueProperty.floatValue, alphaProperty.floatValue).ToRGB()).ToHSV();

        hueProperty.floatValue = hsvColour.H;
        saturationProperty.floatValue = hsvColour.S;
        valueProperty.floatValue = hsvColour.V;
        alphaProperty.floatValue = hsvColour.A;

        EditorGUI.EndProperty();
    }

}