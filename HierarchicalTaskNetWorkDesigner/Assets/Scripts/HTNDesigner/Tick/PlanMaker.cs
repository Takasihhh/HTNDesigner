using System.Collections.Generic;
using System.Linq;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using HTNDesigner.Utilies;
using Unity.VisualScripting;
using UnityEngine;

namespace HTNDesigner.Tick
{
    /// <summary>
    /// 任务的制定者类
    /// </summary>
    public sealed class PlanMaker
    {
        private List<PrimitiveTask> _planList;
        private Stack<TaskNode> _taskToProcess;
        private WorldStateBlackBoard _workingWorldState;
        private TaskNode _currentTaskNode;
        private ITaskAgent _agent;
        private TaskGraph _graph;
        private List<PrimitiveTask> _recordList;
        private Stack<TaskNode> _recordStack;


        public PlanMaker(ITaskAgent agent)
        {
            _agent = agent;
            _graph = agent.TaskGraph;
            _recordList ??= new List<PrimitiveTask>();
            _recordStack ??= new Stack<TaskNode>();
            _planList = new List<PrimitiveTask>();
            _taskToProcess = new Stack<TaskNode>();
        }

        /// <summary>
        /// 制定计划
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public List<PrimitiveTask> MakePlan()
        {
            _workingWorldState = ScriptableObject.CreateInstance<WorldStateBlackBoard>();
           // Debug.LogError(_agent.m_worldState.GetValue<bool>("需要治疗"));
            _workingWorldState.DeepCopy(_agent.WorldState_BB);
            // _workingWorldState = ReflectionMethodExtension.DeepCopy(_agent.WorldState_BB);
            _graph.Initialize();
            // Debug.LogError("agent"+_agent.m_worldState.GetValue<bool>("是否到达")+"副本"+_workingWorldState.GetValue<bool>("是否到达"));
            _planList = new List<PrimitiveTask>();
            _taskToProcess.Push(_graph.m_RootNode);
            
            while (_taskToProcess.Count > 0)
            {
                _currentTaskNode = _taskToProcess.Pop();
                if (_currentTaskNode.taskType == TaskNode.TaskType.COMPOUND)
                {
                    //如果为复合任务
                    CompoundTask curTask =  _graph.SearchForCompoundTask(_currentTaskNode.ID);
                    var sMethod = curTask.SearchForSatisfaiedMethod(_workingWorldState);
                    if (sMethod != null)
                    {
                        RecordStack();
                        sMethod.SubTasks.Reverse();
                        foreach (var subTask in sMethod.SubTasks)
                        {
                            _taskToProcess.Push(subTask);
                        }
                        sMethod.SubTasks.Reverse();
                    }
                    else
                    {
                        DecordStack();
                    }
                }
                else
                {
                    PrimitiveTask curTask = _graph.SearchForPrimitiveTask(_currentTaskNode.ID);
                    //如果为原子任务,检查preconditions
                    if (curTask.CheckConditions(_workingWorldState))
                    {
                        RecordStack();
                        curTask.ApplyEffect(ref _workingWorldState);
                        _planList.Add(curTask);
                    }
                    else
                    {
                        DecordStack();
                    }
                }
            }

#region Debug
            
            if (_planList.Count > 0)
            {
                string ListName = "";
                foreach (var list in _planList)
                {
                    ListName += " " + "+" +list.TaskName;
                }
                Debug.LogWarning("规划任务完毕"+ ListName);
            }
            else
            {
                Debug.LogError("当前任务列表为空，没有可执行的任务");
            }
#endregion
            //GameObject.Destroy(_workingWorldState);
            _workingWorldState = null;
            return _planList;
        }

        
        /// <summary>
        /// 记录堆栈和当前任务列表的数据
        /// </summary>
        private void RecordStack()
        {
            _recordStack = _taskToProcess;
            _recordList = _planList;
        }
        
        
        /// <summary>
        /// 回溯堆栈和当前任务列表的数据
        /// </summary>
        private void DecordStack()
        {
            _taskToProcess = _recordStack;
            _planList = _recordList;
        }
    }
}