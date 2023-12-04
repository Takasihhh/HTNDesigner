
using System;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
public class Escape : PrimitiveTask
{
    [SerializeField] private  string taskName = "逃跑";

    private NavMeshAgent _meshAgent;
    private Vector3 _destination;
    public override void OnStart()
    {
        base.OnStart();
        _meshAgent = _agent.TaskInstance.GetComponent<NavMeshAgent>();
        _destination = _agent.TaskInstance.GetComponent<Destination>().GetRandomDestination();
        _meshAgent.SetDestination(_destination);
        _meshAgent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(_agent.TaskInstance.transform.position, _destination) <= 2f)
        {
            _meshAgent.isStopped = true;
            Debug.LogError("是否需要治疗"+_agent.m_worldState.GetValue<bool>("需要治疗"));
            _taskStatus = TaskStatus.TASK_SUCCESS;
        }

        return _taskStatus;
    }
    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        worldState.SetValue("是否到达",false);
        worldState.SetValue("需要治疗", true);
    }
    
    public override string TestName()
    {
        return TaskName;
    }
    
    
    public override string TaskName {
        get
        {
            taskName = "逃跑";
            return taskName;
        }
    }
}
