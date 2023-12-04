using System;
using HTNDesigner.BlackBoard;

namespace HTNDesigner.Domain
{
    [Serializable]
    public class Condition
    {
        public virtual bool CheckCondition(WorldStateBlackBoard worldState) => true;
        
        public virtual string ConditonName { get; }
    }
}