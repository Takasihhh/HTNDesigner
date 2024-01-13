
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;

public class EscapeContainerBuilder:ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        var hcon2 = new HealthCondition(1, 3);
        var ncon2 =new NeedHealingCondition(false);
        var cons3 = new List<Condition>();
        cons3.Add(hcon2);
        cons3.Add(ncon2);
        return new ConditionContainer(cons3);
    }
}