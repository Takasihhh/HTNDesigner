using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;

public class StartAttackMethod:Method
{
    public StartAttackMethod()
    {
        _subTaskNodes = new List<TaskNode>();
        CDCondition cdCondition = new CDCondition();
        ConditionContainer con1 = new ConditionContainer(cdCondition);
        // _subTaskNodes.Add(new TaskNode((con1,new StrongAttack())));
        // _subTaskNodes.Add(new TaskNode(((null,new NormalAttack()))));
    }
}
