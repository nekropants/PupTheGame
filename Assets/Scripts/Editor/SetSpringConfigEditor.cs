using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetSpringConfig))]
public class SetSpringConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SetSpringConfig springConfig = (target as SetSpringConfig);
        GUILayout.Space(5);

        if (springConfig.springConfig)
        {
            if(GUILayout.Button("Set " + springConfig.springConfig.name.Replace("SpringConfig_","")))
            {
                springConfig.ApplyConfig();
            }
        }
    }
}
