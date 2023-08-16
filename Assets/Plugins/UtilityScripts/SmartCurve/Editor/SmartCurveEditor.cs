// using NightTools.Utility;
//
// namespace FrogtownIK
// {
//     using UnityEngine;
//     using UnityEditor;
//
//     [CustomPropertyDrawer(typeof(SmartCurve))]
//     public class SmartCurveDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect totalRect, SerializedProperty property, GUIContent label)
//         {
//             float labelWidth = EditorGUIUtility.labelWidth;
//
//             SerializedProperty curveProperty = property.FindPropertyRelative("_curve");
//             SerializedProperty phaseProperty = property.FindPropertyRelative("_phaseShift");
//             SerializedProperty amplitudeProperty = property.FindPropertyRelative("_amplitudeM");
//             SerializedProperty frequencyProperty = property.FindPropertyRelative("_frequencyM");
//
//             SerializedProperty lastSampleProperty = property.FindPropertyRelative("_normalizedSample");
//
//             SerializedProperty lastSampleTimeProperty = property.FindPropertyRelative("_lastSampleTime");
//             SerializedProperty lastSampleResultProperty = property.FindPropertyRelative("_lastResult");
//
//             
//             EditorGUI.BeginProperty(totalRect, label, property);
//
//             Rect rect = EditorGUI.PrefixLabel(totalRect, GUIUtility.GetControlID(FocusType.Passive), label);
//
//             float curveWidth = (rect.width * 0.4f);
//
//             int fields = 4;
//             float valueInterval = 0.6f / fields;
//             float valueWidth = (rect.width * valueInterval);
//
//             Rect curveValueRect = new Rect(rect.xMin, rect.y, curveWidth - 6f, rect.height);
//             Rect ampValueRect = new Rect(curveValueRect.x + curveWidth, rect.y, valueWidth - 6f, rect.height);
//             Rect freqValueRect = new Rect(ampValueRect.x + valueWidth, rect.y, valueWidth - 6f, rect.height);
//             Rect phaseValueRect = new Rect(freqValueRect.x + valueWidth, rect.y, valueWidth - 6f, rect.height);
//             Rect outputRect = new Rect(phaseValueRect.x + valueWidth, rect.y, valueWidth - 6f, rect.height);
//
//             Rect markerRect = curveValueRect;
//             markerRect.width = curveValueRect.width * lastSampleProperty.floatValue;
//
//             EditorGUIUtility.labelWidth = 30f;
//             EditorGUI.PropertyField(curveValueRect, curveProperty, new GUIContent(""));
//
//             // EditorGUI.LabelField(markerRect,"I");
//             EditorGUI.DrawRect(markerRect, Color.white.WithAlpha(0.1f));
//             EditorGUIUtility.labelWidth = 10;
//             EditorGUI.PropertyField(ampValueRect, amplitudeProperty, new GUIContent("a", "Amplitude Multiplier"));
//             EditorGUIUtility.labelWidth = 9;
//             EditorGUI.PropertyField(freqValueRect, frequencyProperty, new GUIContent("f", "Frequency Multiplier"));
//             EditorGUIUtility.labelWidth = 10;
//             EditorGUI.PropertyField(phaseValueRect, phaseProperty, new GUIContent("p", "Phase Shift"));
//             EditorGUIUtility.labelWidth = 10;
//
//             EditorGUI.LabelField(outputRect, $"({lastSampleTimeProperty.floatValue},{lastSampleResultProperty.floatValue})", EditorStyles.miniLabel);
//
//             EditorGUIUtility.labelWidth = labelWidth;
//
//
//
//
//             EditorGUI.EndProperty();
//
//             Event e = Event.current;
//
//             if (e.type == EventType.MouseDown && e.button == 1 && totalRect.Contains(e.mousePosition))
//             {
//                 GenericMenu context = new GenericMenu();
//                 SmartCurve smartCurve = this.GetSerializedValue<SmartCurve>( property);
//                 context.AddItem(new GUIContent("Freeze", "Apply constants to curve"), false, () =>
//                 {
//                     UnityEditor.Undo.RecordObject(property.serializedObject.targetObject, "Freeze");
//                     smartCurve.Freeze();
//                 });
//                 context.AddItem(new GUIContent("Normalize", "Remap curve between [0,1]"), false, () =>
//                 {
//                     UnityEditor.Undo.RecordObject(property.serializedObject.targetObject, "Normalize");
//                     smartCurve.Normalize();
//                 });
//
//                 context.ShowAsContext();
//             }
//         }
//     }
// }