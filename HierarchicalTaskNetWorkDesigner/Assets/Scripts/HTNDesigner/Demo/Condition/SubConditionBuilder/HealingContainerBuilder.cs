
using HTNDesigner.DataStructure;

public class HealingContainerBuilder:ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        NeedHealingCondition ncon1 = new NeedHealingCondition(true);
        return new ConditionContainer(ncon1);
    }
}