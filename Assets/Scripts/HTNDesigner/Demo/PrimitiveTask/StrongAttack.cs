using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEngine;


[Serializable]
public class StrongAttack : PrimitiveTask
{
    [SerializeField] private new string taskName = "重攻击";
    public override void OnStart()
    {
        base.OnStart();
        Attack().Forget();
    }

    public override void ApplyEffect(ref WorldStateBlackBoard worldState)
    {
        UseSkill(worldState).Forget();
    }
    
    private async UniTaskVoid Attack()
    {
        await UniTask.WaitForSeconds(3f);
        _taskStatus = TaskStatus.TASK_SUCCESS;
    }

    private async UniTaskVoid UseSkill(WorldStateBlackBoard worldState)
    {
        worldState.SetValue("大招CD", false);
        await UniTask.WaitForSeconds(10f);
        worldState.SetValue("大招CD", true);
    }
    
    public override string TestName()
    {
        return TaskName;
    }

    
    public override string TaskName {
        get
        {
            taskName = "重攻击";
            return taskName;
        }
    }}
