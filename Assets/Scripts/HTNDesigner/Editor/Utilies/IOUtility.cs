using System;
using System.Collections.Generic;
using System.Linq;
using HTNDesigner.BlackBoard;
using HTNDesigner.Data;
using HTNDesigner.DataStructure;
using HTNDesigner.Editor.Data;
using HTNDesigner.Editor.GraphicsElements;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor.Utilies
{
    using Domain;
    using HTNDesigner.Utilies;
    using DataStructure.Variable;
    public static partial class IOUtility
    {
        private static HTNGraphView  graphView;
        private static List<Sensor> _variables;
        private static List<string> _primitiveTaskName;
        public static event Action<VisualElement> DrawInspectorAct;
        public static event Action ClearInspectorAct;
        
        private static string graphFileName;
        private static string graphDataFolderPath;
        
        
        private static Dictionary<string, HTNNode> loadedNodes;
        private static List<string> loadedCompoundNodes;
        private static Dictionary<string, List<string>> loadedConnections;
        
        private static List<HTNNode> nodes;
        private static HTNNode rootNode;

        
        public static void InitializeData(HTNGraphView dsGraphView, string graphName)
        {
            graphView = dsGraphView;
            graphFileName = graphName;

            nodes = new List<HTNNode>();
            graphDataFolderPath = $"Assets/HTNDesigner/HTNmaps/{graphName}";

            loadedNodes ??= new Dictionary<string, HTNNode>();
            loadedCompoundNodes ??= new List<string>();
            loadedConnections ??= new Dictionary<string, List<string>>();
            
            
            _variables ??= new List<Sensor>();
            _primitiveTaskName ??= new List<string>();
            if(_primitiveTaskName == null || _primitiveTaskName.Count ==0)
                FindAllPrimitiveTaskClass();
        }


        #region 保存
        
        /// <summary>
        /// 存取：先保存到本地的树文件里面，然后保存到图标的数据里面
        /// </summary>
        public static void Save()
        {
            CreateDefaultFolders();
            
            GetElementsFromGraphView();
            

            
            TaskGraphData_SO localGraphData = CreateAsset<TaskGraphData_SO>(graphDataFolderPath, graphFileName);
            WorldStateBlackBoard localBlackBoard = CreateAsset<WorldStateBlackBoard>(graphDataFolderPath, graphFileName + "BlackBoard");
            
            
            if (SaveToScriptableObject(localGraphData,localBlackBoard))
            {
                GraphSaveData_SO graphData = CreateAsset<GraphSaveData_SO>("Assets/Editor/HTNDesigner/Graphs", $"{graphFileName}Graph");
            
                graphData.Initialize(graphFileName);
                
                SaveToGraph(graphData);

                SaveAsset(graphData);
                SaveAsset(localGraphData);
                SaveAsset(localBlackBoard);
            }
            else
            {
                Debug.LogError("保存失败");
            }
        }
        
        private static void SaveToGraph(GraphSaveData_SO graphData)
        {
            //保存黑板
            BlackBoardSaveData bbData = new BlackBoardSaveData
            {
                BBValueList = new List<BBValueSaveStruct>()
            };
            foreach (var sensor in _variables)
            {
                BBValueSaveStruct saveStruct;
                saveStruct.typeName = sensor.GetType().Name;
                saveStruct.Name = sensor.Name;
                bbData.BBValueList.Add(saveStruct);
            }
            graphData.BBValue = bbData;
            //保存节点
            foreach (var gNode in nodes)
            {
                List<string> connections = new List<string>();
                var venumrable = gNode.GetOutputConnection();
                var iterator = venumrable?.GetEnumerator();

                // iterator.MoveNext();
                while (iterator.MoveNext())
                {
                    if (iterator.Current == null)
                    {
                        Debug.LogError("iterator为空");
                        break;
                    }
                    var tnode = iterator.Current.input.node;
                    // iterator.Current.in
                    if (tnode is HTNNode node)
                    {
                        connections.Add(node.ID);
                        Debug.LogWarning($"{node.ID}");
                    }
                    else
                    {
                        Debug.LogError("node不是HTNNode类型的" + $"{tnode.name}");
                    }
                    //iterator.MoveNext();
                }

                List<ConditionStruct> conditionStructs = new List<ConditionStruct>();
                string description ="";
                if (gNode is HTNPrimitiveNode gpNode)
                {
                    conditionStructs = gpNode.BBConditionConstructList;
                    description = gpNode.Text;
                }
                
                NodeSaveData data = new NodeSaveData
                {
                    ID = gNode.ID,
                    Name = gNode.Name,
                    NodeType = gNode.NodeType,
                    ConnectionID = connections,
                    Position = gNode.GetPosition().position,
                    Conditions = conditionStructs,
                    Description = description,
                    isRootNode = gNode.IsRootNode,
                };
                
                graphData.Nodes.Add(data);
            }          
        }

        //TODO:保存到本地文件
        private static bool SaveToScriptableObject(TaskGraphData_SO localGraphData,
            WorldStateBlackBoard localBlackBoard)
        {
            loadedCompoundNodes = new List<string>();
            loadedNodes = new Dictionary<string, HTNNode>();
            rootNode.BuildGraph();
            localBlackBoard.InitBlackBoard();
            //保存黑板
            foreach (var variable in _variables)
            {
                localBlackBoard.AddItem(variable);
            }

            TaskGraph graph = new TaskGraph();


            //得到所有的原子节点
            SavePrimitiveTask(graph);
            //得到所有的复合节点
            SaveCompoundTask(graph);
            graph.m_RootNode = new TaskNode(TaskNode.TaskType.COMPOUND,rootNode.ID);
            localGraphData.Initialize(graph);
            return true;
        }

        private static void SavePrimitiveTask(TaskGraph graph)
        {
            foreach (var graphNode in nodes)
            {
                //如果是原子节点
                if (graphNode is HTNPrimitiveNode pNode)
                {
                    PrimitiveTask task = (PrimitiveTask)ReflectionMethodExtension.CreateInstance(pNode.Name);
                    task.TaskName = pNode.Name;
                    // foreach (var conditionStruct in pNode.BBConditionConstructList)
                    // {
                    //     // if (conditionStruct.PropertyType == "Boolean")
                    //     // {
                    //     //     task.Conditions.Add((ws) =>
                    //     //     {
                    //     //         return  bool.Parse(conditionStruct.PropertyValue) == ws.GetValue<bool>(conditionStruct.PropertyName);
                    //     //     });
                    //     // }
                    //     //
                    //     // if (conditionStruct.PropertyType == "String")
                    //     // {
                    //     //     task.Conditions.Add((ws) =>
                    //     //     {
                    //     //         return Enum.Parse(Type.GetType(conditionStruct.PropertyName),conditionStruct.PropertyValue) ==
                    //     //                Enum.Parse(Type.GetType(conditionStruct.PropertyName),ws.GetValue<string>(conditionStruct.PropertyName));
                    //     //     });
                    //     // }
                    //     task.SerializeConditions.Add(conditionStruct);
                    // }
                    task.SerializeConditions = new List<ConditionStruct>();
                    task.SerializeConditions = pNode.BBConditionConstructList;
                    graph.AddTask(task,pNode.ID);
                }
            }
        }

        private static void SaveCompoundTask(TaskGraph graph)
        {
            //遍历所有复合节点然后BuildMethod
            foreach (var cnode in loadedCompoundNodes)
            {
                string id = cnode;
                List<Method> methods = new List<Method>();
                methods = BuildMethods(id, ref graph);
                CompoundTask task = new CompoundTask(methods);
                graph.AddTask(task,id);
            }
        }


        private static List<Method> BuildMethods(string id,ref TaskGraph graph)
        {
            List<Method> result = new List<Method>();
            var methodHeaders = loadedConnections[id];
            for (int i = methodHeaders.Count-1; i >=0; i--)
            {
                var nodeID = methodHeaders[i];
                Method method;
                List<TaskNode> taskQueue = buildTaskQueue(nodeID,ref graph);
                if (taskQueue.Count <= 0)return null;
                
                switch (loadedNodes[nodeID].NodeType)
                {
                    case HTNNodeType.Primitive:
                        method = new Method(taskQueue,
                            graph.SearchForPrimitiveTask(nodeID).SerializeConditions);
                        result.Add(method);
                        break;
                    case HTNNodeType.Composite:
                        method = new Method(taskQueue, null);
                        result.Add(method);
                        break;
                    case HTNNodeType.Hub:
                        return null;
                }
            }
            
            // foreach (var nodeID in methodHeaders)
            // {
            //     Method method;
            //     List<TaskNode> taskQueue = buildTaskQueue(nodeID,ref graph);
            //     if (taskQueue.Count <= 0)return null;
            //     
            //     switch (loadedNodes[nodeID].NodeType)
            //     {
            //         case HTNNodeType.Primitive:
            //             method = new Method(taskQueue,
            //                 graph.SearchForPrimitiveTask(nodeID).SerializeConditions);
            //         result.Add(method);
            //             break;
            //         case HTNNodeType.Composite:
            //             method = new Method(taskQueue, null);
            //         result.Add(method);
            //             break;
            //         case HTNNodeType.Hub:
            //             return null;
            //     }
            // }
            return result;
        }

        private static List<TaskNode> buildTaskQueue(string id,ref TaskGraph graph)
        {
            List<TaskNode> result = new List<TaskNode>();
            Stack<string> idStack = new Stack<string>();
            string curid = id;
            while (true)
            {
                //如果是复合节点就寻找到下一个匹配的Hub的下一个id处
                if (loadedNodes[curid].NodeType == HTNNodeType.Composite)
                {
                    result.Add(new TaskNode(TaskNode.TaskType.COMPOUND,curid));
                    idStack.Push(curid);
                    while (true)
                    {
                        if (loadedConnections[curid].Count >= 1)
                        {
                            var nextNode = loadedNodes[loadedConnections[curid][0]];
                            if (nextNode.NodeType == HTNNodeType.Composite)
                                idStack.Push(curid);
                            if (nextNode.NodeType == HTNNodeType.Hub)
                            {
                                idStack.Pop();
                                if (idStack.Count == 0)
                                {
                                    //找到了对应的hub，将节点指向下一个节点
                                    if (loadedConnections[nextNode.ID].Count >= 0)
                                    {
                                         curid = loadedNodes[loadedConnections[nextNode.ID][0]].ID;
                                    }
                                    else
                                        return null;
                                    break;
                                }
                            }
                            curid = nextNode.ID;
                        }
                        else
                        {
                            return null;    
                        }
                    }
                }
                else if (loadedNodes[curid].NodeType == HTNNodeType.Primitive)
                {
                    result.Add(new TaskNode(TaskNode.TaskType.PRIMITIVE,curid,loadedNodes[curid].Name));
                    if (loadedConnections[curid].Count >= 1)
                    {
                        var nextNode = loadedNodes[loadedConnections[curid][0]];
                        if (nextNode.NodeType == HTNNodeType.Hub)
                            return result;
                        curid = nextNode.ID;
                    }
                    else //下一个节点为空
                        break;
                }
                else
                    return null;
            }

            return result;
        }
        #endregion



        #region 加载
        
        public static void Load()
        {
            loadedConnections = new Dictionary<string, List<string>>();
            loadedNodes = new Dictionary<string, HTNNode>();
            GraphSaveData_SO graphData = LoadAsset<GraphSaveData_SO>("Assets/Editor/HTNDesigner/Graphs", graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "找不到文件喵!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"Assets/Editor/DialogueSystem/Graphs/{graphFileName}\".\n\n" +
                    "找不到文件捏.",
                    "好的喵!"
                );

                return;
            }

            HTNEditor.UpdateFileName(graphData.FileName);

            LoadBlackBoard(graphData.BBValue);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }


        private static void LoadBlackBoard(BlackBoardSaveData loadBlackBoard)
        {
            _variables = new List<Sensor>();
            foreach (var loadStruct in loadBlackBoard.BBValueList)
            {
                if (loadStruct.typeName == "SensorBool")
                {
                    SensorBool vBool = new SensorBool();
                    vBool.Name = loadStruct.Name;
                    _variables.Add(vBool);
                }
                
                if (loadStruct.typeName == "SensorEnum")
                {
                    SensorEnum vEnum = new SensorEnum();
                    vEnum.Name = loadStruct.Name;
                    _variables.Add(vEnum);
                }
                
            }
            graphView.UpdateBlackBoard();
        }
        
        
        private static void LoadNodes(List<NodeSaveData> loadNodes)
        {
            foreach (NodeSaveData nodeData in loadNodes)
            {

                HTNNode node = graphView.CreateNode(nodeData.Name, nodeData.NodeType, nodeData.Position, false);

                node.ID = nodeData.ID;
                node.Draw();
                if (node is HTNPrimitiveNode gpNode)
                {
                    gpNode.InitConditions(nodeData.Conditions);
                    gpNode.Text = nodeData.Description;
                }
                if (nodeData.isRootNode)
                {
                    node.InitRootNode();
                }

                graphView.AddElement(node);

                loadedNodes.Add(node.ID, node);
                loadedConnections.Add(node.ID,nodeData.ConnectionID);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, HTNNode> loadedNode in loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    // DSChoiceSaveData choiceData = (DSChoiceSaveData) choicePort.userData;
                    List<string> ids = loadedConnections[loadedNode.Value.ID];
                    
                    foreach (var id in ids)
                    {
                        if (string.IsNullOrEmpty(id))
                        {
                            continue;
                        }                        
                        
                        HTNNode nextNode = loadedNodes[id];

                        Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                        Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                        graphView.AddElement(edge);

                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
        }
        #endregion
        

        public static void ClearData()
        {
            _variables = null;
            // nodeDataDic = null;
            loadedNodes = null;
            nodes = null;
        }
        
        #region 数据结构管理

        public static void SetRootNode(this HTNNode node)
        {
            rootNode = node;
        }
        #endregion
        
        //========属性封装
        public static List<Sensor> Variables { get=>_variables??=new List<Sensor>(); }
        public static List<string> PrimitiveTaskName{get => _primitiveTaskName ??= new List<string>();}
    }
}