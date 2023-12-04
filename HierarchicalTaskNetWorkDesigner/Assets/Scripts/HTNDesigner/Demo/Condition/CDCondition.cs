 using HTNDesigner.BlackBoard;
 using HTNDesigner.Domain;

 public class CDCondition:Condition
 {
     public override bool CheckCondition(WorldStateBlackBoard worldState)
     {
         return worldState.GetValue<bool>("大招CD") == true;
     }

     public override string ConditonName { get=>"CD条件"; }
 }
