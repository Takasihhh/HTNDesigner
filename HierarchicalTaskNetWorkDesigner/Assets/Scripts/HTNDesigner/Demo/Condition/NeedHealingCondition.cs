
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;

public class NeedHealingCondition:Condition
{
    private bool need;

    public NeedHealingCondition(bool val)
    {
        need = val;
    }
    
    public override bool CheckCondition(WorldStateBlackBoard worldState)
    {
      return worldState.GetValue<bool>("需要治疗") == need;
    }
    
    public override string ConditonName { get=>"需要健康值条件"; }

}
