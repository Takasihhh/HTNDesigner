using System;
using System.Collections;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
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

    public static List<Func<WorldStateBlackBoard, bool>> ConstructConditions(this List<ConditionStruct> SerializeConditions)
    {
        List<Func<WorldStateBlackBoard, bool>> conditions = new List<Func<WorldStateBlackBoard, bool>>();
        foreach (var vaCondition in SerializeConditions)
        {
            if (vaCondition.PropertyType == "Boolean")
            {
                conditions.Add((ws) =>
                {
                    bool blackBoardVal = ws.GetValue<bool>(vaCondition.PropertyName);
                    bool currentVal = bool.Parse(vaCondition.PropertyValue);
                    return blackBoardVal == currentVal;
                });
            }

            if (vaCondition.PropertyType == "String")
            {
                conditions.Add((ws) =>
                {
                    return ws.GetValue<string>(vaCondition.PropertyName) == vaCondition.PropertyValue;
                });   
            }
        }

        return conditions;
    }
}
