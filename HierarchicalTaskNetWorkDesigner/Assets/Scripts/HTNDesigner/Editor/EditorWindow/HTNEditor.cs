using System;
using System.IO;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{

    using Data;
    public class HTNEditor : EditorWindow
    {
        public static HTNEditor _htnEditor;
        public TaskTreeBuilder _treeAssets;
        private string result = "树：";
        private TwoPanelExtensionHorizontal mainContainer;
        private VisualElement leftBoradContainer;
        private HTNGraphView _graphView;
        CompoundNodeGraph rootNode;
        
        [MenuItem("Window/AI/HTN Editor")]
        public static void OpenEditorWindow()
        {
            _htnEditor = EditorWindow.GetWindow<HTNEditor>();
            _htnEditor.titleContent = new GUIContent("HTN Editor");
            _htnEditor.minSize = new Vector2(540, 480);
            _htnEditor.maxSize = new Vector2(1280, 720);
        }
        
        private void CreateGUI()
        {
            TaskTreeBuilder existingAsset = AssetDatabase.LoadAssetAtPath<TaskTreeBuilder>("Assets/Resources/TreeAssets/TreeData.asset");
            if (existingAsset != null)
            {
                _treeAssets = existingAsset;
                Debug.Log("覆盖老文件");
            }
            else
            {
                _treeAssets = CreateInstance<TaskTreeBuilder>();
                Debug.Log("创建新文件");
            }
            //加载主界面
            DrawWindow();
            RegisterBtn();
            TryBuildTree();
        }
        private void DrawWindow()
        {
            DrawMainContainer();
            DrawGraphView();
        }

        private void DrawMainContainer()
        {
            // mainContainer = new TwoPanelExtensionHorizontal();
            // mainContainer.Q<VisualElement>("RightPanel").Add(_graphView);
            // rootVisualElement.Add(mainContainer);
            //加载主界面
            VisualTreeAsset editorViewTree = (VisualTreeAsset)EditorGUIUtility.Load("MainWnd.uxml");
            TemplateContainer editorInstance = editorViewTree.CloneTree();
            editorInstance.StretchToParentSize();
            rootVisualElement.Add(editorInstance);
        }

        private void DrawGraphView()
        {
            _graphView = new HTNGraphView();
            _graphView.root = rootVisualElement;
            rootNode = new CompoundNodeGraph();
            rootNode.title = "根节点";
            _graphView.AddElement(rootNode);
            _graphView.style.minHeight = 1000;
            _graphView.style.minWidth = 500;
            _graphView.StretchToParentSize();
            rootVisualElement.Q<VisualElement>("RightPanel").Add(_graphView);
        }


        private void TryBuildTree()
        {
        }

        private void RegisterBtn()
        {
            var ele = rootVisualElement.Q<Button>("SaveBtn");
            var lele = rootVisualElement.Q<Button>("LoadBtn");
            lele.clicked += DebugThis;
            ele.clicked += SaveTree;
        }
        
        
        private void SaveTree()
        {
            Debug.Log("保存");
            if (rootNode != null)
            {
                _treeAssets.m_RootNode = rootNode.SearchChildMethod();
                if (_treeAssets.m_RootNode.m_CompoundTask == null)
                {
                    Debug.LogError("保存的时候就是没有的");
                }

            }
            else
            {
                Debug.Log("不存在rootNode");
            }
            _treeAssets.SaveScriptableObject();
        }


        #region Debug

        
        private void DebugThis()
        {
            Debug.Log("Debug");
            
            // string path = "Assets/Resources/TreeAssets/m_TreeData.json";
            // string treeJson = Resources.Load<TextAsset>("TreeAssets/m_TreeData").text;
            // var tNode = JsonUtility.FromJson<TaskNode>(treeJson);
            
            if (_treeAssets.m_Tree.m_RootNode != null)
            {
                result = "树: ";
                DebugTree(_treeAssets.m_Tree.m_RootNode.m_CompoundTask);
            }
            else
            {
                Debug.LogError("没有东西");
            }
            Debug.Log(result);
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
        
        #endregion
    }
}