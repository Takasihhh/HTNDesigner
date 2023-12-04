
using HTNDesigner.DataStructure;

public class PStartAttackContainerBuilder:ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        CDCondition cdCondition = new CDCondition();
        ConditionContainer con1 = new ConditionContainer(cdCondition);
        return con1;
    }
}
