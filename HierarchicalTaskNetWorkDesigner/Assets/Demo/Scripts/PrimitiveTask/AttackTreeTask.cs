using System.Collections;
using System.Collections.Generic;
using HTNDesigner.Domain;
using UnityEngine;

public class AttackTreeTask : PrimitiveTask
{
    public override void OnStart()
    {
        base.OnStart();
        var anim = _agent.TaskInstance.gameObject.GetComponent<Animator>();
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMove",false);
        anim.SetBool("IsAttack",true);
    }
    
    
}
