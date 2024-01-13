using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;

public class MoveAttackMethod:Method
{
    public MoveAttackMethod()
    {
        _subTaskNodes = new List<TaskNode>();
        DistanceCondition dcon = new DistanceCondition(false);
        // _subTaskNodes.Add(new TaskNode((new ConditionContainer(dcon) ,new MoveTo())));
    }
    
}