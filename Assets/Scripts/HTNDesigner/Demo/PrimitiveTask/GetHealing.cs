using System;
using System.Collections;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class GetHealing : PrimitiveTask
{
    [SerializeField] private  string taskName = "治疗";

    public override void OnStart()
    {
        base.OnStart();
        _agent.TaskInstance.GetComponent<NavMeshAgent>().isStopped = true;
        _agent.TaskInstance.GetComponent<NavMeshAgent>().speed = 0f;
        _agent.TaskInstance.GetComponent<NavMeshAgent>().velocity =Vector3.zero;
    }

    public override TaskStatus OnUpdate()
    {
        if (_agent.m_worldState.GetValue<int>("健康值") >= 10)
        {
            Debug.LogWarning("治疗完毕");
            _agent.TaskInstance.GetComponent<NavMeshAgent>().speed = 3f;
            _taskStatus = TaskStatus.TASK_SUCCESS;
        }

        return _taskStatus;
    }

    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        worldState.SetValue("健康值", 10);
        worldState.SetValue("需要治疗", false);
    }

    public override string TestName()
    {
        return TaskName;
    }

    public override string TaskName {
        get
        {
            taskName = "治疗";
            return taskName;
        }
    }
}
