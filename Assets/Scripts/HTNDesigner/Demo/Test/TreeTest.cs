using System;
using System.Collections;
using System.Collections.Generic;
using HTNDesigner.Data;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEngine;

public class TreeTest : MonoBehaviour
{
    [SerializeField]private TaskTreeBuilder _tree;
    string result = "树：";
    private void Awake()
    {
        // DebugTree(_tree.m_RootNode.m_CompoundTask);
        // string path = "Assets/Resources/TreeAssets/m_TreeData.json";
        // var treeData = JsonUtility.FromJson<TaskTree>(path);
        Debug.LogWarning(result);
    }
    
    [ContextMenu("测试树")]
    private void DebugThis()
    {
        DebugTree(_tree.m_RootNode.m_CompoundTask);
        Debug.LogWarning(result);
    }    
    
    
    
        private void DebugTree(CompoundTask task)
        {
            if (task != null)
            {
                if (task.m_Methods != null)
                {
                    foreach (var mth in task.m_Methods)
                    {
                        foreach (var node in mth.SubTasks)
                        {
                            if (node.m_type == TaskNode.TaskType.PRIMITIVE)
                            {
                                result +=
                                    $"----------\n----------{node.m_PrimitiveTask.TaskName}+{node.m_PrimitiveTask.TestName()}";
                            }
                            else
                            {
                                result += "-----------\n";
                                DebugTree(node.m_CompoundTask);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var node in task.m_Method.SubTasks)
                    {
                        if (node.m_type == TaskNode.TaskType.PRIMITIVE)
                        {
                            result +=
                                $"----------\n----------{node.m_PrimitiveTask.TaskName}+{node.m_PrimitiveTask.TestName()}";
                        }
                        else
                        {
                            result += "-----------\n";
                            DebugTree(node.m_CompoundTask);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("任务不存在");
                return;
            }
        }
}
