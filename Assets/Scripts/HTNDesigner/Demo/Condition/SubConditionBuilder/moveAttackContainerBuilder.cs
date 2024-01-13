
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;

public class moveAttackContainerBuilder:ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        var hcon1 = new HealthCondition(5);
        var dcon2 = new DistanceCondition(false);
        var ncon2 =new NeedHealingCondition(false);
        var cons2 = new List<Condition>();
        cons2.Add(hcon1);
        cons2.Add(dcon2);
        cons2.Add(ncon2);
        return new ConditionContainer(cons2);
    }
}

