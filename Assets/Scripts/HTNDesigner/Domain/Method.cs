using System;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using UnityEngine;

namespace HTNDesigner.Domain
{
    [Serializable]
    public class Method:ISerializationCallbackReceiver
    {
        [field:SerializeField] public List<ConditionStruct> SerializeConditions { get; set; }
        protected List<Func<WorldStateBlackBoard,bool>> _conditions;
        [SerializeReference]protected List<TaskNode> _subTaskNodesID;
        
        public Method()
        {
            _subTaskNodesID = new List<TaskNode>();
        }

        public Method(List<TaskNode> nodeList,List<ConditionStruct> conditions)
        {
            _subTaskNodesID = new List<TaskNode>();
            _conditions = new List<Func<WorldStateBlackBoard, bool>>();
            SerializeConditions = new List<ConditionStruct>();
            _subTaskNodesID = nodeList;
            SerializeConditions = conditions;
        }

        public bool CheckConditions(WorldStateBlackBoard ws)
        {
            foreach (var condition in _conditions)
            {
                bool conditionStatus = condition(ws);
                if (!conditionStatus)
                    return false;
            }

            return true;
        }

        public List<TaskNode> SubTasks
        {
            get => _subTaskNodesID??= new List<TaskNode>();
        }

        public void OnBeforeSerialize()
        {
            SerializeConditions ??= new List<ConditionStruct>();
        }

        public void OnAfterDeserialize()
        {
            _conditions ??= new List<Func<WorldStateBlackBoard, bool>>();
            _conditions = SerializeConditions.ConstructConditions();
        }
    }
}