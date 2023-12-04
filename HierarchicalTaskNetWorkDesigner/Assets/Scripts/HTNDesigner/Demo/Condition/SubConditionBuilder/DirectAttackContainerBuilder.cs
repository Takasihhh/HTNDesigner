using System.Collections;
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEngine;

public class DirectAttackContainerBuilder : ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        var hcon1 = new HealthCondition(5);
        var dcon1 = new DistanceCondition(true);
        var ncon2 =new NeedHealingCondition(false);
        List<Condition> cons1 = new List<Condition>();
        cons1.Add(hcon1);
        cons1.Add(dcon1);
        cons1.Add(ncon2);
        return new ConditionContainer(cons1);
    }
    
}
