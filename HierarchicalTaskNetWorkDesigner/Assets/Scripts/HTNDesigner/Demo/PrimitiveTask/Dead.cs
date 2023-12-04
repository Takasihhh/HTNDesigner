using System;
using System.Collections;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using UnityEngine;

[Serializable]
public class Dead : PrimitiveTask
{
    [SerializeField] private  string taskName = "死亡";

    public override void OnStart()
    {
        base.OnStart();
        Debug.Log("角色死亡");
        GameObject.Destroy(_agent.TaskInstance.gameObject);
    }

    public override string TestName()
    {
        return TaskName;
    }

    public override string TaskName {
        get
        {
            taskName = "死亡";
           return taskName;
        }
    }
}
