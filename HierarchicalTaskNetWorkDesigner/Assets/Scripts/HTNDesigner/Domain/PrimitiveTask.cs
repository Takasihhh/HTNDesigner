using System;
using UnityEngine;

namespace HTNDesigner.Domain
{
    using BlackBoard;
    [Serializable]
    public class PrimitiveTask
    {
        protected TaskStatus _taskStatus;
        protected ITaskAgent _agent;
        [SerializeField] public string taskName = "默认节点";
        [SerializeField]private string _testName = "原子任务";
        public virtual void OnStart(){_taskStatus = TaskStatus.TASK_RUNNIGN;}

        public virtual TaskStatus OnUpdate() => _taskStatus;
        
        public virtual void ApplyEffect(ref WorldStateBlackBoard worldState){}

        public virtual string TaskName { get=>taskName; }

        public virtual string TestName()
        {
            return "默认";
        }
        public ITaskAgent Agent{get => _agent; set => _agent = value;}
    }   
}