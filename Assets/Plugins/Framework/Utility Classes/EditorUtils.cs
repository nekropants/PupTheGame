
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

public static class EditorUtils
{
#if UNITY_EDITOR

    private static Mesh[] _defaultPrimitiveMeshes;
    private static AudioSource _audioSource;

    public static ReorderableList CreateReorderableList(SerializedProperty property, bool displayFoldout, bool displayAddButton = true, bool displayRemoveButton = true)
    {
        return CreateReorderableList(property, property.displayName, displayFoldout, displayAddButton, displayRemoveButton);
    }

    public static ReorderableList CreateReorderableList(SerializedProperty property, string header, bool displayFoldout, bool displayAddButton = true, bool displayRemoveButton = true)
    {
        ReorderableList list = new ReorderableList(property.serializedObject, property, true, true, displayAddButton, displayRemoveButton);

        list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            if (!displayFoldout || property.isExpanded)
            {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), list.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
            }
        };

        list.drawFooterCallback = (rect) =>
        {
            if (!displayFoldout || property.isExpanded)
            {
                list.footerHeight = 13f;
                list.draggable = true;
                ReorderableList.defaultBehaviours.DrawFooter(rect, list);
            }
            else
            {
                list.footerHeight = 0;
                list.draggable = false;
            }
        };

        list.drawHeaderCallback = rect =>
        {
            if (displayFoldout)
            {
                property.isExpanded = EditorGUI.Foldout(new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height), property.isExpanded, header, true);
            }
            else
            {
                EditorGUI.LabelField(rect, header);
            }
        };

        list.elementHeightCallback = (index) =>
        {
            return EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(index)) + 2;
        };

        //   if (displayFoldout)
        //  {
        //      list.elementHeightCallback = index =>
        //       {
        //           return property.isExpanded ? list.elementHeight : 0;
        //       };
        //   }

        return list;
    }


    public static Material GetDefaultMaterial()
    {
        return AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
    }

    public static Mesh GetDefaultPrimitiveMesh(PrimitiveType primitiveType)
    {
        if (_defaultPrimitiveMeshes == null)
        {
            _defaultPrimitiveMeshes = new Mesh[EnumUtils.GetCount<PrimitiveType>()];
            Object[] objects = AssetDatabase.LoadAllAssetsAtPath("Library/unity default resources");

            for (int i = 0; i < objects.Length; i++)
            {
                for (int j = 0; j < _defaultPrimitiveMeshes.Length; j++)
                {
                    if (objects[i].name == ((PrimitiveType)j).ToString())
                    {
                        _defaultPrimitiveMeshes[j] = (Mesh)objects[i];
                    }
                }
            }
        }

        return _defaultPrimitiveMeshes[(int)primitiveType];
    }

    public static AudioSource PlayAudio(AudioClip clip, bool stopOnSelectionChange = true, float volume = 0.5f, float pitch = 1f)
    {
        if (_audioSource == null)
        {
            if (stopOnSelectionChange)
            {
                Selection.selectionChanged += StopAudio;
            }
            GameObject editorAudioObject = new GameObject();
            editorAudioObject.hideFlags = HideFlags.HideAndDontSave;
            _audioSource = editorAudioObject.AddComponent<AudioSource>();
        }

        _audioSource.volume = volume;
        _audioSource.pitch = 1f;
        _audioSource.spatialBlend = 0f;

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        return _audioSource;
    }

    public static void StopAudio()
    {
        if (_audioSource != null)
        {
            Selection.selectionChanged -= StopAudio;
            _audioSource.Stop();
        }
    }

    public static bool IsPlayingAudioClip(AudioClip clip)
    {
        return _audioSource != null && _audioSource.isPlaying && _audioSource.clip == clip;
    }

    public static bool IsPlayingAnyAudio()
    {
        return _audioSource != null && _audioSource.isPlaying;
    }

    public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        if (parent != null)
        {
            go.transform.parent = parent;
        }
        go.transform.position = position;
        go.transform.rotation = rotation;

        return go;
    }

    public static LayerMask LayerMaskField(GUIContent label, LayerMask mask)
    {
        return LayerMaskField(EditorGUILayout.GetControlRect(), label, mask);
    }

    public static LayerMask LayerMaskField(Rect rect, GUIContent label, LayerMask mask)
    {
        List<string> layerNames = new List<string>();
        List<int> layerMasks = new List<int>();
        for (int i = 0; i < 32; i++)
        {

            string name = LayerMask.LayerToName(i);
            if (name != "")
            {
                layerNames.Add(name);
                layerMasks.Add(1 << i);

            }

        }

        int val = mask;
        int maskVal = 0;
        for (int i = 0; i < layerNames.Count; i++)
        {
            if (layerMasks[i] != 0)
            {
                if ((val & layerMasks[i]) == layerMasks[i])
                {
                    maskVal |= 1 << i;
                }
            }
            else if (val == 0)
            {
                maskVal |= 1 << i;
            }
        }
        int newMaskVal = EditorGUI.MaskField(rect, label, maskVal, layerNames.ToArray());
        int changes = maskVal ^ newMaskVal;

        for (int i = 0; i < layerMasks.Count; i++)
        {
            if ((changes & (1 << i)) != 0)            // has this list item changed?
            {
                if ((newMaskVal & (1 << i)) != 0)     // has it been set?
                {
                    if (layerMasks[i] == 0)           // special case: if "0" is set, just set the val to 0
                    {
                        val = 0;
                        break;
                    }
                    val |= layerMasks[i];
                }
                else                                  // it has been reset
                {
                    val &= ~layerMasks[i];
                }
            }
        }
        return val;
    }

    public static Quaternion QuaternionField(GUIContent label, Quaternion quaternion)
    {
        return QuaternionField(EditorGUILayout.GetControlRect(), label, quaternion);
    }

    public static Quaternion QuaternionField(Rect rect, GUIContent label, Quaternion quaternion)
    {
        return Quaternion.Euler(EditorGUI.Vector3Field(rect, label, quaternion.eulerAngles));
    }

    public static void QuaternionPropertyField(Rect rect, GUIContent label, SerializedProperty property)
    {
        property.quaternionValue = Quaternion.Euler(EditorGUI.Vector3Field(rect, label, property.quaternionValue.eulerAngles));
    }


    /// <summary>
    /// Checks wether a file with a certain name exists in the Assets directory, outs the path if it does.
    /// </summary>
    /// <param name="filename">The file name to check</param>
    /// <param name="filepath">The local file path (if a match is found, otherwise null)</param>
    /// <returns>Whether or not a file with that name was found in the Assets directory</returns>
    public static bool GetAssetFilePath(string filename, out string filepath)
    {
        string[] files = Directory.GetFiles(Application.dataPath, filename, SearchOption.AllDirectories);
        if (files.Length > 0)
        {
            filepath = files[0].Replace('\\', '/');
            filepath = filepath.Replace(Application.dataPath + "/", String.Empty);
            return true;
        }
        filepath = null;
        return false;
    }

    public static bool AssetFileExists(string filename)
    {
        return Directory.GetFiles(Application.dataPath, filename, SearchOption.AllDirectories).Length > 0;
    }

    /// <summary>
    /// Creates/changes a text file and imports it.
    /// </summary>
    /// <param name="path">The path of the file to create</param>
    /// <param name="text">The contents of the text file</param>
    public static void CreateTextFile(string localPath, string text)
    {
        bool fileExists = AssetFileExists(localPath.Substring(localPath.LastIndexOf("/") + 1));

        File.WriteAllText(Application.dataPath + "/" + localPath, text.Replace('\r', '\n'));

        AssetDatabase.Refresh();
        Debug.Log((fileExists ? "Updating" : "Creating") + " file: " + localPath, AssetDatabase.LoadMainAssetAtPath("Assets/" + localPath));
    }

    public static T GetValue<T>(this SerializedProperty property)
    {
        string[] separatedPaths = property.propertyPath.Split('.');
        object reflectionTarget = property.serializedObject.targetObject;

        Type targetType = reflectionTarget.GetType();
        FieldInfo fieldInfo;

        foreach (var path in separatedPaths)
        {
            targetType = reflectionTarget.GetType();

            do
            {
                fieldInfo = targetType.GetField(path, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (fieldInfo != null)
                {
                    reflectionTarget = fieldInfo.GetValue(reflectionTarget);
                    break;
                }

                targetType = targetType.BaseType;

                if (targetType == null)
                {
                    return default(T);
                }

            } while (fieldInfo == null);
        }

        return (T)reflectionTarget;
    }
#endif
}

