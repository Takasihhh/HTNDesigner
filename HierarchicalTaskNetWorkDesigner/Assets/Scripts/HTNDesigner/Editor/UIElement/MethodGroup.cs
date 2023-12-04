

using System.Collections.Generic;
using System.Linq;
using HTNDesigner.DataStructure;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    using Domain;
    using Data;
    public class MethodGroup : Group
    {
        private MethodNode_SO _node;
        private SerializedProperty nodeProperty;
        private SerializedObject serializedNode;
        public VisualElement root;
        public MethodGroup()
        {
            title = "方法组";
            _node = ScriptableObject.CreateInstance<MethodNode_SO>();
            serializedNode = new SerializedObject(_node);
            nodeProperty = serializedNode.GetIterator();
            nodeProperty.Next(true);
            Port input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,typeof(Node));
            input.portName = "输入";
            //input.title = "方法输入";
            Add(input);
        }

        public override void OnSelected()
        {
            var container = root.Q<VisualElement>("ContextContainer");
            container.Clear();
            while (nodeProperty.NextVisible(false))
            {
                //构造一个PropertyField用于显示
                PropertyField field = new PropertyField(nodeProperty);
                //与实际的节点数据绑定
                field.Bind(serializedNode);
                //加入到Inspector
                container.Add(field);
            }
            nodeProperty.Reset();
            nodeProperty.Next(true);
        }

        private List<Node> GetTaskGroup()
        {
            List<Node> nodeList = new List<Node>();
            if (containedElements != null)
            {
                var orderContainedElements = containedElements.OrderByDescending(child => child.resolvedStyle.top);
                var iterator = orderContainedElements?.GetEnumerator();
                iterator.MoveNext();
                while (iterator.Current != null)
                {
                    nodeList.Add(iterator.Current as Node);
                    iterator.MoveNext();
                }
            }
            return nodeList;
        }

        /// <summary>
        /// 获取方法组
        /// </summary>
        /// <returns></returns>
        public CompoundTask.ConditionMethod CreateMethod()
        {
            List<Node> nodeList = new List<Node>();
            List<TaskNode> taskList = new List<TaskNode>();
            nodeList = GetTaskGroup();
            
            if (nodeList != null)
            {
                foreach (var node in nodeList)
                {
                    //节点分类
                    if (node is PrimitiveNodeGraph)
                    {
                        var pnode = (node as PrimitiveNodeGraph);
                        TaskNode tmp = new TaskNode(pnode.m_NodeInfo.PrimitiveNode);
                        taskList.Add(tmp);
                    }

                    if (node is CompoundNodeGraph)
                    {
                        var cnode = (node as CompoundNodeGraph);
                        TaskNode tmp = cnode.SearchChildMethod();
                        taskList.Add(tmp);
                    }
                }
            }

            if (taskList.Count>0)
            {
                Method method = new Method(taskList);
                _node.method = method;
            }
            return _node.m_Method;
        }
    }
}