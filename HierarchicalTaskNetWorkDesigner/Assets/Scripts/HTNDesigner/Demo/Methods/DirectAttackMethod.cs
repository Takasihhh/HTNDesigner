using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;

public class DirectAttackMethod : Method
{
    public DirectAttackMethod()
    {
        _subTaskNodes = new List<TaskNode>();
        
       // CompoundTask attackTask = new CompoundTask((null,new StartAttackMethod()));
       // _subTaskNodes.Add(new TaskNode(attackTask));
    }

    
}
