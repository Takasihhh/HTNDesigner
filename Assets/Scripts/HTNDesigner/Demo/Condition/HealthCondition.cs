using System;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;

public class HealthCondition:Condition
{
    private int minHealth;
    private int maxHealth;

    public HealthCondition(int arg1 = 0,int arg2 = Int32.MaxValue)
    {
        minHealth = arg1;
        maxHealth = arg2;
    }
    
    public override bool CheckCondition(WorldStateBlackBoard worldState)
    {
        var val = worldState.GetValue<int>("健康值");
        return val >= minHealth && val <= maxHealth;
    }
    
    public override string ConditonName { get=>"健康值条件"; }

}
