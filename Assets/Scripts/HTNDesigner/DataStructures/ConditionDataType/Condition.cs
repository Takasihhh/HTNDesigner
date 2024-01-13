using System;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;

namespace HTNDesigner.Domain
{
    [Serializable]
    public abstract class Condition
    {
        protected List<Func<bool,WorldStateBlackBoard>> _conditions;

        public bool CheckCondition(WorldStateBlackBoard worldState)
        {
            foreach (var iFunc in _conditions)
            {
                if (!iFunc(worldState)) return false;
            }

            return true;
        }

        protected virtual void InitCondtions()
        {
            _conditions = new List<Func<bool, WorldStateBlackBoard>>();
        }
    }
}