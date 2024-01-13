using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using UnityEngine;
using UnityEngine.AI;

public class MoveToNearestTree : PrimitiveTask
{
    private Vector3 nearestPoint;
    private TaskStatus _status;
    public override void OnStart()
    {
        base.OnStart();
        var anim = _agent.TaskInstance.gameObject.GetComponent<Animator>();
        anim.SetBool("IsMove",true);
        anim.SetBool("IsIdle",false);
        anim.SetBool("IsAttack",false);
        var gos = GameObject.FindGameObjectsWithTag("TreeObj");
        float dist = float.MaxValue;
        nearestPoint = Vector3.zero;
        if (_agent.TaskInstance.GetComponent<AxeEnemy>().lastTree != null)
            nearestPoint = _agent.TaskInstance.GetComponent<AxeEnemy>().lastTree.transform.position;
        if (gos == null || gos.Length <= 0)
        {
            _status = TaskStatus.TASK_FALIURE;
            return;
        }

        _status = TaskStatus.TASK_RUNNIGN;

        if (nearestPoint == Vector3.zero)
        {
            foreach (var tree in gos)
            {
                if (Vector3.Distance(_agent.TaskInstance.transform.position, tree.transform.position) < dist)
                {
                    dist = Vector3.Distance(_agent.TaskInstance.transform.position, tree.transform.position);
                    _agent.TaskInstance.GetComponent<AxeEnemy>().lastTree = tree;
                    nearestPoint = tree.transform.position;
                }
            }
        }

        var agent = _agent.TaskInstance.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(nearestPoint);
        agent.speed = 3f;
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(nearestPoint, _agent.TaskInstance.transform.position) <= 1f)
        {
            Debug.Log("到达大树旁边");
            _status = TaskStatus.TASK_SUCCESS;
        }
        // if(_agent.TaskInstance.GetComponent<coll>)
        
        return _status;
    }

    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        worldState.SetValue("OnArriveTree", true);
    }
}
