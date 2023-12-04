using System;
using UnityEngine;

namespace HTNDesigner.Domain
{
    using DataStructure;
    [Serializable]
    public sealed class TaskNode
    {
        //-----------------------字段---------------------------------
        protected TaskStatus _taskStatus;
        
        [Serializable]
        public enum TaskType
        {
            PRIMITIVE,
            COMPOUND
        }
        
        [Serializable]
        public struct ConditionPrmitiveTask
        {
            [SerializeReference] public PrimitiveTask task;
            [SerializeReference] public ConditionContainer container;
        }

        [SerializeField]private TaskType _taskType;
        [SerializeReference]private CompoundTask _compoundTask;
        [SerializeField]private ConditionPrmitiveTask _primitiveTask;

        
        
        
        
        //---------------------------成员函数---------------------------
        public TaskNode(){}
        public TaskNode(CompoundTask compoundTask)
        {
            _taskType =  TaskType.COMPOUND;
            _compoundTask = compoundTask;
        }

        public TaskNode(ConditionPrmitiveTask primitiveTask)
        {
            _taskType = TaskType.PRIMITIVE;
            _primitiveTask = primitiveTask;
        }
        
        
        //---------------------------属性封装--------------------------
        public TaskStatus m_TaskStatus
        {
            get => _taskStatus;
        }

        public TaskType m_type
        {
            get => _taskType;
        }

        public CompoundTask m_CompoundTask
        {
            get => _compoundTask;
        }

        public PrimitiveTask m_PrimitiveTask
        {
            get => _primitiveTask.task;
        }

        public ConditionContainer m_Condition
        {
            get => _primitiveTask.container;
        }
    }
}