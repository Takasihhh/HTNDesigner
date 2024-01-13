
using HTNDesigner.DataStructure;

public class DeadConditionContainer:ConditionBuilder
{
    protected override ConditionContainer Build()
    {
        var hcon3 = new HealthCondition(-100, 0);
        return new ConditionContainer(hcon3);
    }
}