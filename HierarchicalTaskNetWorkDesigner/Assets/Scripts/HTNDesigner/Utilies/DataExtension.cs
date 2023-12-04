using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DataExtension 
{
    public static void SaveScriptableObject(this ScriptableObject data)
    {
        // 将 ScriptableObject 保存到本地
        SaveScriptableObjectAsset(data);
        // 刷新 AssetDatabase，确保保存操作生效
        AssetDatabase.Refresh();
    }

    static void SaveScriptableObjectAsset(ScriptableObject data)
    {
        string path = $"Assets/Resources/TreeAssets/TreeData.asset";
        
            // 创建或覆盖现有的 ScriptableObject 文件
            ScriptableObject existingAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (existingAsset == null)
            {
                if (data != null)
                {
                    AssetDatabase.CreateAsset(data, path);
                    EditorUtility.SetDirty(data);
                }
            }

            AssetDatabase.SaveAssets();
    }
}
