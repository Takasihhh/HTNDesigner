using System;
using Cysharp.Threading.Tasks;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using HTNDesigner.DataStructure;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class MoveTo : PrimitiveTask
{
    [SerializeField] private  string taskName = "移动到玩家身边";

    private NavMeshAgent _meshAgent;
    private Vector3 _destination;
    public override void OnStart()
    {
        base.OnStart();
        _meshAgent = _agent.TaskInstance.GetComponent<NavMeshAgent>();
        _destination = _agent.TaskInstance.GetComponent<Destination>().playerPos.position;
        _meshAgent.SetDestination(_destination);
        _meshAgent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log(Vector3.Distance(_agent.TaskInstance.transform.position, _destination));
        Debug.Log(_agent.m_worldState.GetValue<bool>("是否到达"));
        if (Vector3.Distance(_agent.TaskInstance.transform.position, _destination) <= 3f)
        {
            _meshAgent.isStopped = true;
            _taskStatus = TaskStatus.TASK_SUCCESS;
        }
        return _taskStatus;
    }

    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        worldState.SetValue("是否到达",true);
        
    }

    public override string TestName()
    {
        return TaskName;
    }

    public override string TaskName {
        get
        {
            taskName = "移动到玩家身边";
            return taskName;
        }
    }
}
