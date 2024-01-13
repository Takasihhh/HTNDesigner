using System.Collections;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using UnityEngine;

public class IdleTask : PrimitiveTask
{
    public override void OnStart()
    {
        base.OnStart();
        var anim = _agent.TaskInstance.gameObject.GetComponent<Animator>();
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsMove",false);
        anim.SetBool("IsAttack",false);

    }
}
