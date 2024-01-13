
using HTNDesigner.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    public class PrimitiveNodeGraph : Node
    {
        private PrimitiveNode_SO _node;
        private SerializedProperty nodeProperty;
        private SerializedObject serializedNode;
        public VisualElement root;
        
        public PrimitiveNodeGraph()
        {
            title = "原子节点";
            _node = ScriptableObject.CreateInstance<PrimitiveNode_SO>();
            serializedNode = new SerializedObject(_node);
            nodeProperty = serializedNode.GetIterator();
            nodeProperty.Next(true);
        }

        public override void OnSelected()
        {
            if (_node.PrimitiveNode.task != null)
            {
                title = _node.PrimitiveNode.task.TaskName;
            }
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
    
        public PrimitiveNode_SO m_NodeInfo { get=>_node; }
    }
}
