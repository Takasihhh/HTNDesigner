using System.Collections;
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using UnityEngine;


namespace HTNDesigner.Data
{
    using Domain;
    using Utilies;
    public class PrimitiveNode_SO : ScriptableObject
    {
        [SerializeField] private TextAsset _conditionBuilder;
        [SerializeField] private TextAsset _primitiveTaskBuilder;


        private TaskNode.ConditionPrmitiveTask ConstructPrimitiveTaskNode()
        {
            TaskNode.ConditionPrmitiveTask ptask = new TaskNode.ConditionPrmitiveTask();
            object containerObj, primitiveTaskObj;
            if(_conditionBuilder!=null && _conditionBuilder.name!="")
                containerObj = ReflectionMethodExtension.CreateInstance(_conditionBuilder.name);
            else
            {
                containerObj = null;
            }
            if (_primitiveTaskBuilder!=null && _primitiveTaskBuilder.name != "")
            {
                primitiveTaskObj = ReflectionMethodExtension.CreateInstance(_primitiveTaskBuilder.name);

                if (primitiveTaskObj != null)
                {
                    if (containerObj != null)
                    {
                        ptask.container = (containerObj as ConditionBuilder)?.Container;
                    }
                    ptask.task = primitiveTaskObj as PrimitiveTask;
                }
                else
                {
                    Debug.LogError("原子任务节点转化失败");
                }
            }

            return ptask;
        }
        
        
        public TaskNode.ConditionPrmitiveTask PrimitiveNode
        {
            get
            {
                return ConstructPrimitiveTaskNode();
            }
        }
    }
}