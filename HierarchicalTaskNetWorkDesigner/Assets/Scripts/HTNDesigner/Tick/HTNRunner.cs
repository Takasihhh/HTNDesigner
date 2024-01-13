using System.Collections.Generic;
using HTNDesigner.Domain;
using UnityEditor;
using UnityEngine;

namespace HTNDesigner.Tick
{
    /// <summary>
    /// 任务列表运行类
    /// </summary>
    public sealed class HTNRunner
    {
        private PrimitiveTask _curTask;
        private TaskStatus _curStatus;
        private readonly ITaskAgent _agent;
        private List<PrimitiveTask>.Enumerator _listEnumrator;
        private List<PrimitiveTask> _list;
        public HTNRunner(ITaskAgent agent)
        {
            _agent = agent;
        }
        
        /// <summary>
        /// 开始执行任务列表
        /// </summary>
        /// <param name="planList"></param>
        public void StartRun(List<PrimitiveTask> planList)
        {
            _list = new List<PrimitiveTask>();
            _list = planList;
            // _listEnumrator = new List<PrimitiveTask>.Enumerator();
            _listEnumrator = planList.GetEnumerator();
            _listEnumrator.MoveNext();
            StartNewTask();
        }

        public TaskStatus Update()
        {
            if (_curTask != null)
            {
                _curStatus = _curTask.OnUpdate();
                if (_curStatus == TaskStatus.TASK_SUCCESS)
                {
                    RunNextTask();
                }
            }
            else
            {
                _curStatus = TaskStatus.TASK_FALIURE;
                Debug.LogError("当前没有任务");
            }
            return _curStatus;
        }

        public void FixedUpdate()
        {
            if (_curTask != null)
            {
                _curTask.OnFixedUpdate();
            }
        }

        /// <summary>
        /// 开始执行下一个任务
        /// </summary>
        public void RunNextTask()
        {
            var so = _agent.WorldState_BB;
            _curTask.ApplyEffect(ref so);
            //_agent.WorldState_BB.DeepCopy(so);
            if (_listEnumrator.MoveNext())
            {
                StartNewTask();
            }
            else
            {
                _curStatus = TaskStatus.TASK_FALIURE;
            }
        }

        /// <summary>
        /// 开始一个新的任务
        /// </summary>
        private void StartNewTask()
        {
            _curTask = _listEnumrator.Current;
            _curTask.Initialize(_agent);
#region Debug
#if UNITY_EDITOR
            if (_curTask == null)
            {
                Debug.LogError("当前任务为空");
                Debug.LogError(_listEnumrator.Current.TaskName);
                string listString = "";
                foreach (var item in _list)
                {
                    listString += item.TaskName;
                }
                Debug.LogError("当前列表内容" + listString);
                EditorApplication.isPaused = true;
            }
#endif
#endregion


            _curTask.OnStart();
            Debug.Log($"当前进入状态: ({_curTask.TaskName })。");
            _curStatus = TaskStatus.TASK_RUNNIGN;
        }
        
    }
}