using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SnapToGrid))]
public class SnapToGridEditor : Editor
{
    
    [InitializeOnLoadMethod]
    private  static void Initialize()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;

    }

    private static void OnSceneGUI(SceneView sceneview)
    {
        
        if (Event.current.isMouse == false || Event.current.type != EventType.MouseUp)
        {
            return;
        }
        // Undo.SetCurrentGroupName("Snap");
        int group = Undo.GetCurrentGroup();
        Debug.Log("OnHierarchyChanged");
        List<Transform> snapChildren = new List<Transform>();

        for (int i = 0; i <  Selection.transforms.Length; i++)
        {
            Transform[] children = Selection.transforms[i].GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (snapChildren.Contains(child) == false)
                    snapChildren.Add(child);
            }
        }

        for (var index = 0; index < snapChildren.Count; index++)
        {
            Transform transform = snapChildren[index];
            MeshRenderer snapToGrid = transform.GetComponent<MeshRenderer>();

            List<Transform> children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }

            for (int i = 0; i < children.Count; i++)
            {
                Undo.SetTransformParent(   children[i], null, true, "Snap");
                Debug.Log("Set Parent " + children[i]);
            }

            Undo.RecordObject(transform, "Snap");
            if (snapToGrid)
            {
                SnapBasedOnBounds(snapToGrid, 0.1f);
            }
            else
            {
                transform.position = Snapping.Snap(transform.position, Vector3.one * 0.1f);
            }
            
            for (int i = 0; i < children.Count; i++)
            {
                
                Debug.Log("Set Parent " + children[i]);
                Undo.SetTransformParent(   children[i], transform, true, "Snap");
            }
        }
        
        Undo.CollapseUndoOperations( group );

    }
    private void OnSceneGUI()
    {
        return;
        // if (Event.current.isMouse && Event.current.type == EventType.MouseUp)
        // {
        //     ((SnapToGrid)target).Snap();
        //     Debug.Log("Mouse Up");
        // }
    }
    
    
    private static void SnapBasedOnBounds( MeshRenderer renderer, float snapIncrement)
    {
        Transform transform = renderer.transform;
        Quaternion rotation = transform.rotation;
        transform.parent = null;
        transform.rotation = Quaternion.identity;

        Bounds bounds = renderer.bounds;

        Vector3 min = Snapping.Snap(bounds.min, Vector3.one * snapIncrement);
        Vector3 max = Snapping.Snap(bounds.max, Vector3.one * snapIncrement);

        bounds.min = min;
        bounds.max = max;

        Vector3 scale = max - min;

        transform.position = bounds.center;
        transform.localPosition = Snapping.Snap(transform.localPosition, Vector3.one * snapIncrement/2f);

        transform.localScale = scale;
        transform.rotation = rotation;

    }
}