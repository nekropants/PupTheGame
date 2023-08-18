using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConnectChildJointChain))]
public class ConnectChildJointChainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SetSpringConfig springConfig = (target as SetSpringConfig);
        GUILayout.Space(5);

        {
            if(GUILayout.Button("Connect "  ))
            {
                ConnectChildJointChain childJointChain = target as ConnectChildJointChain;
                List<Joint> joints = new List<Joint>();
                foreach (Transform t in childJointChain.transform)
                {
                    Joint joint = t.GetComponent<Joint>();

                    if (joint)
                        joints.Add(joint);
                }

                for (int i = 1; i < joints.Count; i++)
                {
                    // joints[i]
                }
                
            }
        }
    }
}
