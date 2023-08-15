using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


public static class MenuItems
{

    [MenuItem("GameObject/Remove Prefab Connection")]
    public static void RemovePrefabConnection()
    {
        List<GameObject> newObjects = new List<GameObject>();

        while (Selection.gameObjects.Length > 0)
        {
            GameObject oldGameObject = Selection.gameObjects[0];

            string name = oldGameObject.name;
            int index = oldGameObject.transform.GetSiblingIndex();
            Vector3 scale = oldGameObject.transform.localScale;

            GameObject newGameObject = Object.Instantiate(oldGameObject, oldGameObject.transform.position, oldGameObject.transform.rotation, oldGameObject.transform.parent) as GameObject;
            Object.DestroyImmediate(oldGameObject);

            newGameObject.name = name;
            newGameObject.transform.localScale = scale;
            newGameObject.transform.SetSiblingIndex(index);

            newObjects.Add(newGameObject);
        }

        Selection.objects = newObjects.ToArray();
    }

    [MenuItem("Assets/Create/Scriptable Object Instance")]
    static void CreateScriptableObjectInstance()
    {
        MonoScript script = Selection.activeObject as MonoScript;

        if (script != null)
        {
            if (typeof(ScriptableObject).IsAssignableFrom(script.GetClass()))
            {
                //string path = AssetDatabase.GetAssetPath(script);
                //path =  Path.GetDirectoryName(path);
                //Debug.Log(path);

                ScriptableObject obj = ScriptableObject.CreateInstance(script.name);
                if (obj == null)
                {
                    EditorUtility.DisplayDialog("Scriptable Object Creator", "Unable to instantiate class: " + script.name, "Ok");
                    return;
                }

                Debug.Log("Created scriptable object instance: " + script.name);


                string path = AssetDatabase.GetAssetPath(script);
                 Debug.Log(path);
                 Debug.Log(Application.dataPath);
                path = Directory.GetParent(path) + "";
                 Debug.Log(path);


                AssetDatabase.CreateAsset(obj, path+ "/" + script.name + ".asset");
                AssetDatabase.SaveAssets();

                Selection.activeObject = obj;
            }
            else
            {
                EditorUtility.DisplayDialog("Scriptable Object Creator", "Selected object is not a ScriptableObject class.", "Ok");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Scriptable Object Creator", "Selected object is not a MonoScript", "Ok");

        }
    }




    [MenuItem("CONTEXT/MonoBehaviour/Validate")]
    private static void ValidateMonoBehaviour(MenuCommand command)
    {
        MethodInfo methodInfo = command.context.GetType().GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (methodInfo != null)
        {
            methodInfo.Invoke(command.context, new object[0]);
        }
    }



   //[MenuItem("GameObject/Group #%G")]
    static void Group2()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel );


        if (transforms.Length > 0)
        {
            GameObject group = new GameObject(transforms.Length > 1 ? "Group" : transforms[0].name);

            Undo.RegisterCreatedObjectUndo(group, "Group");
            Transform commonParent = transforms[0].parent;
            Vector3 totalPosition = Vector3.zero;

            for (int i = 1; i < transforms.Length; i++)
            {
                if (commonParent != transforms[i].parent)
                {
                    commonParent = null;
                    break;
                }
            }

            if (commonParent != null)
            {
                group.transform.parent = commonParent;
            }

            for (int i = 0; i < transforms.Length; i++)
            {
                totalPosition += transforms[i].position;
            }

            group.transform.position = totalPosition / transforms.Length;
            group.transform.rotation = transforms[0].rotation;
            group.transform.localScale = Vector3.one;

            for (int i = 0; i < transforms.Length; i++)
            {
                Undo.SetTransformParent(transforms[i], group.transform, "Group");
            }


            Selection.activeGameObject = group;
        }
    }


   //[MenuItem("GameObject/Group %G")]

    [MenuItem("GameObject/Group %g")]
    static void Group()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel );


        if (transforms.Length > 0)
        {
            GameObject group = new GameObject(transforms.Length > 1 ? "Group" : transforms[0].name);

            Undo.RegisterCreatedObjectUndo(group, "Group");
            Transform commonParent = transforms[0].parent;
            Vector3 totalPosition = Vector3.zero;

            for (int i = 1; i < transforms.Length; i++)
            {
                if (commonParent != transforms[i].parent)
                {
                    commonParent = null;
                    break;
                }
            }

            if (commonParent != null)
            {
                group.transform.parent = commonParent;
            }

            for (int i = 0; i < transforms.Length; i++)
            {
                totalPosition += transforms[i].position;
            }

            group.transform.position = totalPosition / transforms.Length;

            for (int i = 0; i < transforms.Length; i++)
            {
                Undo.SetTransformParent(transforms[i], group.transform, "Group");
            }


            Selection.activeGameObject = group;
        }
    }

    [MenuItem("Window/Collapse Hierarchy %#C")]
    public static void CollapseHierarchy()
    {
        EditorApplication.ExecuteMenuItem("Window/Hierarchy");
        EditorWindow hierarchyWindow = EditorWindow.focusedWindow;
        MethodInfo expandMethodInfo = hierarchyWindow.GetType().GetMethod("SetExpandedRecursive");
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            expandMethodInfo.Invoke(hierarchyWindow, new object[] { root.GetInstanceID(), false });
        }
    }

    [MenuItem("Tools/Framework/Update Generated Constants")]
    private static void UpdateGeneratedConstants()
    {

        string[] sortingLayerNames = (string[])(typeof(UnityEditorInternal.InternalEditorUtility)).GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, new object[0]); ;
        string[] layerNames = UnityEditorInternal.InternalEditorUtility.layers;
        List<string> sceneNames = new List<string>();
        List<string> santitizedSceneNames = new List<string>();
        int[] sortingLayerValues = (int[])(typeof(UnityEditorInternal.InternalEditorUtility)).GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, new object[0]);
        int[] layerValues = new int[layerNames.Length];
        string[] tagNames = UnityEditorInternal.InternalEditorUtility.tags;
        string[] tagValues = new string[tagNames.Length];

        for (int i = 0; i < layerNames.Length; i++)
        {
            layerValues[i] = LayerMask.NameToLayer(layerNames[i]);
            layerNames[i] = StringUtils.Santise(layerNames[i], false);
        }

        for (int i = 0; i < sortingLayerNames.Length; i++)
        {
            sortingLayerNames[i] = StringUtils.Santise(sortingLayerNames[i], false);
        }

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            string path = EditorBuildSettings.scenes[i].path;
            int slashIndex = path.LastIndexOf('/') + 1;
            string sceneName = path.Substring(slashIndex, path.Length - slashIndex - 6);
            string sanitizedName = StringUtils.Santise(StringUtils.Titelize(sceneName), false);

            if (!sceneNames.Contains(sceneName) && File.Exists(path))
            {
                sceneNames.Add(sceneName);
                santitizedSceneNames.Add(sanitizedName);
            }
        }

        for (int i = 0; i < tagNames.Length; i++)
        {
            tagValues[i] = tagNames[i];
            tagNames[i] = StringUtils.Santise(tagNames[i], false);
        }

        CodeGenerator.CodeDefintion layers = CodeGenerator.CreateEnumDefinition("Layer", layerNames, layerValues);
        CodeGenerator.CodeDefintion sortingLayers = CodeGenerator.CreateEnumDefinition("SortingLayer", sortingLayerNames, sortingLayerValues);
        CodeGenerator.CodeDefintion tags = CodeGenerator.CreateConstantStrings(tagNames, tagValues);
        CodeGenerator.CodeDefintion tagClass = CodeGenerator.CreateClass("Tag", tags);
        CodeGenerator.CodeDefintion layerMasks = CodeGenerator.CreateConstantLayerMasks(layerNames, layerValues);
        CodeGenerator.CodeDefintion layerMaskClass = CodeGenerator.CreateClass("LayerMasks", layerMasks);
        CodeGenerator.CodeDefintion scenes = CodeGenerator.CreateConstantStrings(santitizedSceneNames, sceneNames);
        CodeGenerator.CodeDefintion sceneClass = CodeGenerator.CreateClass("SceneNames", scenes);

        CodeGenerator.CreateSourceFile("AutoGeneratedConstants", layers, sortingLayers, layerMaskClass, tagClass, sceneClass);
    }

    [MenuItem("Tools/Framework/Clear PlayerPrefs")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


}
