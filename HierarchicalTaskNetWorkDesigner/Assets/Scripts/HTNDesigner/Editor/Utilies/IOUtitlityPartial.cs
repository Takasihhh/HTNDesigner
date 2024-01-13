using System.Collections.Generic;
using System.Linq;
using HTNDesigner.Domain;
using HTNDesigner.Editor.GraphicsElements;
using HTNDesigner.Utilies;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor.Utilies
{
    /// <summary>
    /// 功能函数部分
    /// </summary>
    public static partial class IOUtility
    {

        #region 文件
        private static void CreateDefaultFolders()
        {
            CreateFolder("Assets/Editor/HTNDesigner", "Graphs");

            CreateFolder("Assets", "HTNDesigner");
            CreateFolder("Assets/HTNDesigner", "HTNmaps");

            CreateFolder("Assets/HTNDesigner/HTNmaps", graphFileName);
            
        }

        private static void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is HTNNode node)
                {
                    nodes.Add(node);
                    return;
                }
            });
        }

        public static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }
        #endregion

        #region 编辑器
        private static void FindAllPrimitiveTaskClass()
        {
            var classofTasks = ReflectionMethodExtension.FindSubclassesOf(typeof(PrimitiveTask));
            var classNames = FileMethodExtension.GetAllFilesNameByfileExtension("Assets", ".cs");
            if (classNames != null)
            {
                foreach (var classTask in classofTasks)
                {
                    if (classNames.Contains(classTask.Name+".cs"))
                    {
                        _primitiveTaskName.Add(classTask.Name);
                    }
                }
            }
            else
            {
                Debug.LogError("找不到类文件");
            }
        }

        public static void CallForUpdateInspector(VisualElement element)
        {
            DrawInspectorAct?.Invoke(element);
        }

        public static void ClearInspector()
        {
            ClearInspectorAct?.Invoke();
        }


        public static bool BuildGraph(this HTNNode node)
        {
            List<string> connections = new List<string>();
            IOrderedEnumerable<Edge> vEnumrator;

            vEnumrator = node.GetOutputConnection()?
                .OrderByDescending((edge) => edge.resolvedStyle.top);
            if (vEnumrator == null)
                return false;
            using var iterator = vEnumrator.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current == null)
                    break;
                var cnode = (HTNNode)iterator.Current.input.node;
                connections.Add(cnode.ID);
                cnode.BuildGraph();
            }

            loadedNodes.TryAdd(node.ID, node);
            if (connections.Count > 1)
            {
                loadedCompoundNodes.Add(node.ID);
            }
            if (loadedConnections.ContainsKey(node.ID))
            {
                loadedConnections[node.ID] = connections;
            }
            else
            {
                loadedConnections.Add(node.ID, connections);
            }

            return true;
        }
        #endregion
    }
}