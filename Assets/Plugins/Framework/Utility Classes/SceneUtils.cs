using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtils
{

    public static T[] FindObjectsOfTypeIncludingInactive<T>() where T : Component
    {
        List<T> components = new List<T>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                for (int j = 0; j < rootObjects.Length; j++)
                {
                    components.AddRange(rootObjects[j].GetComponentsInChildren<T>(true));
                }
            }
        }

        return components.ToArray();
    }

    public static T FindObjectOfTypeIncludingInactive<T>() where T : Component
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                for (int j = 0; j < rootObjects.Length; j++)
                {
                    T component = rootObjects[j].GetComponentInChildren<T>(true);
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }

        return null;
    }

    public static Transform GetCommonAncestor(Transform[] transforms)
    {
        if (transforms.Length > 0)
        {
            Transform currentTransform = transforms[0].parent;
            while (currentTransform != null)
            {
                if (IsCommonAncestor(currentTransform, transforms))
                {
                    return currentTransform;
                }

                currentTransform = currentTransform.parent;
            }
        }

        return null;
    }

    public static bool IsCommonAncestor(Transform parent, Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] == parent) return false;
            if (!transforms[i].IsChildOf(parent)) return false;
        }

        return true;
    }

    public static bool IsAncestor(Transform parent, Transform child)
    {

        Transform current = child.parent;
        while (current != null)
        {
            if (current == parent)
            {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

}
