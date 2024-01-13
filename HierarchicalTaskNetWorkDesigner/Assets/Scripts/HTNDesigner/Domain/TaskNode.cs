using System;
using UnityEngine;

namespace HTNDesigner.Domain
{
    using DataStructure;
    [Serializable]
    public sealed class TaskNode
    {
        [Serializable]
        public enum TaskType
        {
            PRIMITIVE,
            COMPOUND
        }
        [SerializeField]private TaskType _taskType;
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        public TaskNode(TaskType taskType, string id,string name = "")
        {
            _id = id;
            _taskType = taskType;
            _name = name;
        }


        public TaskType taskType
        {
            get => _taskType;
            private set => _taskType = value;
        }
        
        public string ID
        {
            get => _id;
        }
    }
}