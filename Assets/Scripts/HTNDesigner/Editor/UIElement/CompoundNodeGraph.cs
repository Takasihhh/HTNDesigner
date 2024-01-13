

using System.Collections.Generic;
using System.Linq;
using HTNDesigner.DataStructure;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    
    using Domain;
    public class CompoundNodeGraph : Node
    {
        private Port output;
        public CompoundNodeGraph()
        {
            title = "复合节点";
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Node));
            output.portName = "输出";
            outputContainer.Add(output);
            outputContainer.style.color = new StyleColor(Color.red);
            RefreshPorts();
            RefreshExpandedState();
        }

        public TaskNode SearchChildMethod()
        {
            TaskNode rtNode = new TaskNode();
            List<CompoundTask.ConditionMethod> methods = new List<CompoundTask.ConditionMethod>();
            var enumrator = output.connections?.OrderBy((edge) => edge.resolvedStyle.top);
            var iterator = enumrator?.GetEnumerator();
            iterator.MoveNext();
            while (iterator.Current!=null)
            {
                if(iterator.Current.input.parent is MethodGroup)
                {
                    if (iterator.Current.input.parent is MethodGroup mGroup)
                    {
                        CompoundTask.ConditionMethod method = mGroup.CreateMethod();
                        methods.Add(method);
                    }
                }
                else
                {
                    Debug.Log(iterator.Current.input.portName);
                    Debug.Log(iterator.Current.output.node.title);
                    Debug.LogError("找不到方法节点捏");
                }
                iterator.MoveNext();
            }

            if (methods.Count > 0)
            {
                CompoundTask ctask;
                if (methods.Count == 1)
                {
                     ctask = new CompoundTask(methods[0]);
                     rtNode = new TaskNode(ctask);
                }
                else
                {
                     ctask = new CompoundTask(methods);
                     rtNode = new TaskNode(ctask);
                }
            }
            else
            {
                Debug.LogError("没有复合任务被创建，因为方法组为0");
            }
            return rtNode;
        }
    }
}