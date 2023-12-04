using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    public class HTNGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<HTNGraphView, UxmlTraits>{ }

        public VisualElement root;
        public HTNGraphView()
        {
            //加载背景网格的USS文件
            styleSheets.Add((StyleSheet)EditorGUIUtility.Load("NodeGraphGridBackground.uss"));
            //设置视图滚轮缩放
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //添加拖拽、选择、框选Manipulator 固定搭配
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            //为视图添加背景网格
            var grid = new GridBackground();
            Insert(0, grid);    
            AddSearchWnd();
        }

        private void AddSearchWnd()
        {
            HTNSearchWindow searchWindow = ScriptableObject.CreateInstance<HTNSearchWindow>();
            searchWindow._graphView = this;
            nodeCreationRequest = context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            };
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


        public CompoundNodeGraph AddNewCompoundNode()
        {
            CompoundNodeGraph node = new CompoundNodeGraph();
            
            AddElement(node);
            return node;
        }
    }
}