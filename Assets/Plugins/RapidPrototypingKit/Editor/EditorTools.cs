using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class EditorTools 
{
    // public class SelectNameWindow : EditorWindow {
    //
    //     static string name = "_group";
    //     
    //     
    //     
    //     void Awake()
    //     {
    //
    //         ((Object)this).name = "Group";
    //         name = Selection.activeObject.name + " group";
    //         //   minSize =  maxSize = new Vector2(300, 200);
    //
    //
    //         position = new Rect(Screen.width / 2, Screen.height / 2, 200, 50);
    //         minSize =  maxSize = new Vector2(300, 50);
    //     }
    //
    //
    //     void OnGUI() {
    //         name = EditorGUILayout.TextField("name", name);
    //
    //
    //         GUILayout.Space(10);
    //         if (GUILayout.Button("Group")) {
    //             OnClickSavePrefab();
    //             GUIUtility.ExitGUI();
    //         }
    //     }
    //
    //     void OnClickSavePrefab() {
    //
    //
    //
    //         Object[] files = Selection.objects;
    //         string originalPath = AssetDatabase.GetAssetPath(Selection.activeObject);
    //
    //         Debug.Log(Selection.activeObject + " ]"+ originalPath+"]");
    //         string originalDirectory = Path.GetDirectoryName(originalPath);
    //         string newDirectory = originalDirectory + "/" + name;
    //
    //         for (int i = 0; i < files.Length; i++)
    //         {
    //             originalPath = AssetDatabase.GetAssetPath(files[i]);   
    //             originalDirectory = Path.GetDirectoryName(originalPath);
    //             string newPath = originalPath.Replace(originalDirectory, newDirectory);
    //
    //
    //
    //             Debug.Log(newDirectory);
    //             if (Directory.Exists(newDirectory) == false)
    //             {
    //                 Directory.CreateDirectory(newDirectory);
    //                 AssetDatabase.ImportAsset(newDirectory);
    //             }
    //
    //             AssetDatabase.MoveAsset(originalPath, newPath);
    //             Debug.Log(AssetDatabase.GetAssetPath(files[i]));
    //         }
    //
    //         AssetDatabase.SaveAssets();
    //      
    //         // You may also want to check for illegal characters :)
    //         // Save your prefab
    //
    //         Close();
    //     }
    //
    // }
    //
    [MenuItem("Tools/Framework/Duplicate Material %w")]
    static void DuplicateMaterial()
    {
        Dictionary<Material, Material> alreadyDuplicated = new Dictionary<Material, Material>();
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                Material duplicateMaterial = DuplicateMaterial(meshRenderer.sharedMaterial, alreadyDuplicated);
                Selection.activeObject = duplicateMaterial;
                meshRenderer.sharedMaterial = duplicateMaterial;
            }
        }
    }


    public static Material DuplicateMaterial(Material material, Dictionary<Material, Material> alreadyDuplicated)
    {
        if (alreadyDuplicated.ContainsKey(material))
            return alreadyDuplicated[material];
        
        string assetPath = AssetDatabase.GetAssetPath(material);

        string directoryName = Path.GetDirectoryName(assetPath);
        Debug.Log(directoryName);
        Debug.Log(assetPath);
        string newPath = assetPath.Replace(".mat", "_Duplicate.mat");
        if (AssetDatabase.CopyAsset(assetPath, newPath))
        {
            Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(newPath);
            alreadyDuplicated.Add(material, newMaterial);
            return newMaterial;
        }

        return null;
    }
}
