using System;
using System.Collections.Generic;
using UnityEngine;

namespace HTNDesigner.Domain
{
    using BlackBoard;
    [Serializable]
    public abstract class PrimitiveTask:ISerializationCallbackReceiver
    {
        [field:SerializeField] public List<ConditionStruct> SerializeConditions { get; set; }

        protected ITaskAgent _agent;
        protected List<Func<WorldStateBlackBoard,bool>> _conditions;
        
        #region 生命周期
        public void Initialize(ITaskAgent agent){_agent=agent;}
        
        public virtual void OnAwake(){}
        public virtual void OnStart(){}

        public virtual TaskStatus OnUpdate() => TaskStatus.TASK_RUNNIGN;

        public virtual void OnFixedUpdate(){}

        #endregion

        public bool CheckConditions(WorldStateBlackBoard ws)
        {
            foreach (var condition in _conditions)
            {
                if (!condition(ws))
                    return false;
            }

            return true;
        }
        
        public virtual void ApplyEffect(ref WorldStateBlackBoard worldState){}
        
        
        //public List<Func<WorldStateBlackBoard,bool>> Conditions { get=>_conditions??=new List<Func<WorldStateBlackBoard, bool>>(); set=>_conditions = value; }
        [field:SerializeField]public string TaskName { get; set; }

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
