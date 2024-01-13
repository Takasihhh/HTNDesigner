using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    using GraphicsElements;
    using Utilies;
    public class HTNGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<HTNGraphView, UxmlTraits>{ }

        public HTNEditor _editorWnd;
        private BlackBoardGraphView _blackBoard;
        private HTNSearchWindow _searchWindow;
        public HTNGraphView(){}
        
        public HTNGraphView(HTNEditor editorWnd)
        {
            _editorWnd = editorWnd;
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            AddGridBackground();
            AddStyles();
            AddManipulator();
            AddSearchWindow();
            AddBlackBoard();
            
            OnElementsDeleted();
            OnGraphViewChanged();
        }

        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            //存储符合条件的兼容的端口
            List<Port> compatiblePorts = new List<Port>();
            //遍历Graphview中所有的Port 从中寻找
            ports.ForEach(
                (port) =>
                {
                    if (startPort.node != port.node && startPort.direction != port.direction)
                    {
                        compatiblePorts.Add(port);
                    }
                }
            );
            return compatiblePorts;
        }


        private void AddManipulator()
        {

            //添加拖拽、选择、框选Manipulator 固定搭配
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
        
        private void AddGridBackground()
        {
            //加载背景网格的USS文件
            styleSheets.Add((StyleSheet)EditorGUIUtility.Load("NodeGraphGridBackground.uss"));
            // GridBackground gridBackground = new GridBackground();
            // gridBackground.StretchToParentSize();
            // Insert(0, gridBackground);
            //设置视图滚轮缩放
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //为视图添加背景网格
            var grid = new GridBackground();
            Insert(0, grid);    
        }


        #region 添加组件
        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<HTNSearchWindow>();
            }

            _searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        
        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
            );
        }
        #endregion
        #region 黑板
        private void AddBlackBoard()
        {
            _blackBoard = new BlackBoardGraphView(this);
            _blackBoard.SetPosition(new Rect(0, 0, 200, 180));
            Add(_blackBoard);
        }

        public void UpdateBlackBoard()
        {
            ClearBlackBoard();
            _blackBoard.UpdateBlackBoard();
        }

        public void ClearBlackBoard()
        {
            _blackBoard.ClearField();
        }
        
        public void ToggleBlackBoard()
        {
            _blackBoard.visible = !_blackBoard.visible;
        }
        #endregion

        #region 创建函数
        public HTNNode CreateNode(string nodeName, HTNNodeType htnNodeType, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"HTNDesigner.Editor.GraphicsElements.HTN{htnNodeType}Node");

            HTNNode node = (HTNNode) Activator.CreateInstance(nodeType);

            node.Initialize(nodeName, this, position);

            if (shouldDraw)
            {
                node.Draw();
            }

            return node;
        }
        
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type edgeType = typeof(Edge);

                List<HTNNode> nodesToDelete = new List<HTNNode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is HTNNode node)
                    {
                        nodesToDelete.Add(node);

                        continue;
                    }

                    if (selectedElement.GetType() == edgeType)
                    {
                        Edge edge = (Edge) selectedElement;

                        edgesToDelete.Add(edge);

                        continue;
                    }
                }

                DeleteElements(edgesToDelete);

                foreach (HTNNode nodeToDelete in nodesToDelete)
                {
                    nodeToDelete.DisconnectAllPorts();
                    RemoveElement(nodeToDelete);
                }
            };
        }
        
        private void OnGraphViewChanged()
        {
            //TODO：图标变化时更新树
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        HTNNode nextNode = (HTNNode) edge.input.node;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge) element;
                    }
                }

                return changes;
            };
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            ClearBlackBoard();
        }

        #endregion
        
        #region 功能函数
        
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = _editorWnd.rootVisualElement.ChangeCoordinatesTo(_editorWnd.rootVisualElement.parent, mousePosition - _editorWnd.position.position);
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
        #endregion
    }
}