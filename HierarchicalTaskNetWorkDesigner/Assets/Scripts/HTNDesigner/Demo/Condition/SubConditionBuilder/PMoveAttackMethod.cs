
using HTNDesigner.DataStructure;

public class PMoveAttackMethod:ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        DistanceCondition dcon = new DistanceCondition(false);
        return new ConditionContainer(dcon);
    }
}
