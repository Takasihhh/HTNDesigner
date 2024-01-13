using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;

public class DistanceCondition : Condition
{
    private bool onArrive;

    public DistanceCondition(bool val)
    {
        onArrive = val;
    }
    
    public override bool CheckCondition(WorldStateBlackBoard worldState)
    {
        return worldState.GetValue<bool>("是否到达") == onArrive;
    }
    
    public override string ConditonName { get=>"距离条件"; }

}
