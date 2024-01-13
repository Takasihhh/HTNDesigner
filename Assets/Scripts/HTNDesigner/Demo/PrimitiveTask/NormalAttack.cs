using System;
using Cysharp.Threading.Tasks;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEngine;

[Serializable]
public class NormalAttack : PrimitiveTask
{
    [SerializeField] private  string taskName = "普通攻击";

    public override void OnStart()
    {
        base.OnStart();
        StartAttack().Forget();
    }


    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        // _agent.TaskInstance.GetComponent<>
        //TODO:敌人的攻击逻辑
    }

    private async UniTaskVoid StartAttack()
    {
        await UniTask.WaitForSeconds(1);
        _taskStatus = TaskStatus.TASK_SUCCESS;
    }
    
    public override string TestName()
    {
        return TaskName;
    }

    
    public override string TaskName {
        get
        {
            taskName = "普通攻击";
            return taskName;
        }
    }
}
