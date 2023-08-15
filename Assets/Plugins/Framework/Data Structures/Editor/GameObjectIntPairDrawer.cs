using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(GameObjectIntPair))]
public class GameObjectIntPairDrawer : PropertyDrawer
{

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, label, property);

        Rect amountRect = new Rect(rect.x, rect.y, 60, rect.height);
        Rect objectRect = new Rect(rect.x + 60, rect.y, rect.width - 60, rect.height);

        EditorGUI.PropertyField(objectRect, property.FindPropertyRelative("_gameObject"), GUIContent.none);
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("_number"), GUIContent.none);

        EditorGUI.EndProperty();
    }

}



