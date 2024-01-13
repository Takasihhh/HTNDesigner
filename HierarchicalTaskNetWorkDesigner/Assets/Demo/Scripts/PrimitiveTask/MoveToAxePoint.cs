using Cysharp.Threading.Tasks;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using UnityEngine;
using UnityEngine.AI;

public class MoveToAxePoint :PrimitiveTask
{
    private Vector3 axePoint;
    private TaskStatus _status;
    private bool getAxe;
    public override void OnStart()
    {
        base.OnStart();
        getAxe = false;
        var anim = _agent.TaskInstance.gameObject.GetComponent<Animator>();
        anim.SetBool("IsMove",true);
        anim.SetBool("IsIdle",false);
        anim.SetBool("IsAttack",false);
        axePoint = Vector3.zero;
        
        _status = TaskStatus.TASK_RUNNIGN;

        axePoint = GameObject.FindGameObjectWithTag("AxeBox").transform.position;
        
        var agent = _agent.TaskInstance.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(axePoint);
        agent.speed = 3f;
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(axePoint, _agent.TaskInstance.transform.position) <= 3f&&!getAxe)
        {
            getAxe = true;
            GetAxe().Forget();
        }
        
        return _status;
    }

    private async UniTaskVoid GetAxe()
    {
        var anim = _agent.TaskInstance.gameObject.GetComponent<Animator>();
        anim.SetBool("IsMove",false);
        anim.SetBool("IsIdle",true);
        await UniTask.WaitForSeconds(3f);
        _agent.TaskInstance.GetComponent<AxeEnemy>().CurrentAxeHealth = 10;
        _status = TaskStatus.TASK_SUCCESS;
    }
    
    
    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        worldState.SetValue("HasAxe", true);
    }
}