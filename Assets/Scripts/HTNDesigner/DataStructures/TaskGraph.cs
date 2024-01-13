using System;
using System.Collections.Generic;
using HTNDesigner.Domain;
using UnityEngine;
namespace HTNDesigner.DataStructure
{
    [Serializable]
    public class TaskGraph
    {
        [SerializeField]private TaskNode rootTask;
        [SerializeReference] private PassWordDictionary _primitiveTaskTable;
        [SerializeReference] private PassWordDictionary _compoundTaskTable;
        [SerializeReference] private List<PrimitiveTask> _primitiveTasks; //节点存储表
        [SerializeReference] private List<CompoundTask> _compoundTasks;


        public TaskGraph()
        {
            _primitiveTasks = new List<PrimitiveTask>();
            _compoundTasks = new List<CompoundTask>();
            _compoundTaskTable = new PassWordDictionary();
            _primitiveTaskTable = new PassWordDictionary();
        }

        public void AddTask(CompoundTask task,string id)
        {
            _compoundTasks.Add(task);
            _compoundTaskTable.Add(id,_compoundTasks.Count-1);
        }

        public void AddTask(PrimitiveTask task,string id)
        {
            _primitiveTasks.Add(task);
            _primitiveTaskTable.Add(id,_primitiveTasks.Count-1);
        }

        
        public PrimitiveTask SearchForPrimitiveTask(string id)
        {
            if (_primitiveTaskTable.ContainsKey(id))
            {
                return _primitiveTasks[_primitiveTaskTable[id]];
            }

            return null;
        }
        
        public CompoundTask SearchForCompoundTask(string id)
        {
            if (_compoundTaskTable.ContainsKey(id))
            {
                return _compoundTasks[_compoundTaskTable[id]];
            }
            return null;
        }

        public void Initialize()
        {
            foreach (var compoundTask in _compoundTasks)
            {
                compoundTask.Initialize();
            }
        }
        
        public TaskNode m_RootNode
        {
            get => rootTask;
            set => rootTask = value;
        }
    }
}