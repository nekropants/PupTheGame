using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RichardPieterse
{
    public static class RichardUtilities
    {
        public static void DestroyChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                parent.GetChild(i).gameObject.SmartDestroy();
            }
        }

        public static void SmartDestroy(this UnityEngine.Object target)
        {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEngine.Object.Destroy(target);
        else
            UnityEngine.Object.DestroyImmediate(target);
#else
            UnityEngine.Object.Destroy(target);
#endif
        }

        static void SetAngularXDrive(this ConfigurableJoint joint, float springForce)
        {
            JointDrive d = joint.angularXDrive;

            d.positionSpring = springForce;

            joint.angularXDrive = d;
        }

        static void SetAngularYZDrive(this ConfigurableJoint joint, float springForce)
        {
            JointDrive d = joint.angularYZDrive;

            d.positionSpring = springForce;

            joint.angularXDrive = d;
        }
    }
    
    
}