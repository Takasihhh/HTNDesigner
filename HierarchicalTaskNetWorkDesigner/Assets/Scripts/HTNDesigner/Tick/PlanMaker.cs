using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
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
        private TaskNode _currentTask;
        private ITaskAgent _agent;

        private List<PrimitiveTask> _recordList;
        private Stack<TaskNode> _recordStack;


        public PlanMaker(ITaskAgent agent)
        {
            _agent = agent;
            _recordList ??= new List<PrimitiveTask>();
            _recordStack ??= new Stack<TaskNode>();
            _planList = new List<PrimitiveTask>();
            _taskToProcess = new Stack<TaskNode>();
            Debug.LogError("最开始"+_agent.m_worldState.GetValue<bool>("需要治疗"));
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
            _workingWorldState.DeepCopy(_agent.m_worldState);
            // Debug.LogError("agent"+_agent.m_worldState.GetValue<bool>("是否到达")+"副本"+_workingWorldState.GetValue<bool>("是否到达"));
            _planList = new List<PrimitiveTask>();
            _taskToProcess.Push(_agent.m_Root);
            
            while (_taskToProcess.Count > 0)
            {
                _currentTask = _taskToProcess.Pop();
                if (_currentTask.m_type == TaskNode.TaskType.COMPOUND)
                {
                    //如果为复合任务
                    var sMethod = _currentTask.m_CompoundTask.SearchForSatisfaiedMethod(_workingWorldState);
                    if (sMethod != null)
                    {
                        RecordStack();
                        foreach (var subTask in sMethod.SubTasks)
                        {
                            _taskToProcess.Push(subTask);
                        }
                    }
                    else
                    {
                        DecordStack();
                    }
                }
                else
                {
                    //如果为原子任务,检查preconditions
                    if (_currentTask.m_Condition == null || _currentTask.m_Condition.CheckCondition(_workingWorldState))
                    {
                        RecordStack();
                        _currentTask.m_PrimitiveTask.ApplyEffect(ref _workingWorldState);
                        _planList.Add(_currentTask.m_PrimitiveTask);
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
            GameObject.Destroy(_workingWorldState);
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