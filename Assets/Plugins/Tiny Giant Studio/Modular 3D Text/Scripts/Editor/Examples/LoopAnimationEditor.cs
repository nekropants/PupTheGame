﻿/// Created by Ferdowsur Asif @ Tiny Giant Studios

using UnityEditor;
using UnityEngine;

namespace TinyGiantStudio.Text.Example
{
    [CustomEditor(typeof(LoopAnimation))]
    public class LoopAnimationEditor : Editor
    {
        LoopAnimation myTarget;
        SerializedObject soTarget;

        void OnEnable()
        {
            myTarget = (LoopAnimation)target;
            soTarget = new SerializedObject(target);
        }
        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();
            WarningCheck();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
        }

        private void WarningCheck()
        {
            EditorGUI.indentLevel = 0;
            if (!myTarget.GetComponent<Modular3DText>()) { EditorGUILayout.HelpBox("Modular 3D Text is needed for this to work", MessageType.Error); }
            else
            {
                if (myTarget.GetComponent<Modular3DText>().combineMeshInEditor || myTarget.GetComponent<Modular3DText>().combineMeshDuringRuntime)
                {
                    EditorGUILayout.HelpBox("Turn off single mesh in 3D Text component for animation to work.", MessageType.Warning);
                }
            }
            GUILayout.Space(5);
        }
    }
}