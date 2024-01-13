using System;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using UnityEngine;

namespace HTNDesigner.DataStructure
{
    [Serializable]
    public sealed class ConditionContainer
    {
        private List<Condition> _conditions;
        private Condition _condition;

        public enum ConditionType
        {
            PRIMITIVE,
            LIST
            
        }

        [SerializeField]private ConditionType _type;


        public ConditionContainer()
        {
        }

        public ConditionContainer(List<Condition> conditions)
        {
            _conditions = conditions;
            _type = ConditionType.LIST;
        }

        public ConditionContainer(Condition condition)
        {
            _condition = condition;
            _type = ConditionType.PRIMITIVE;
        }

        public bool CheckCondition(WorldStateBlackBoard worldState)
        {
            if (_type == ConditionType.LIST)
            {
                if (_conditions != null && _conditions.Count > 0)
                {
                    foreach (var vaCondition in _conditions)
                    {
                        if (!vaCondition.CheckCondition(worldState))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (_condition != null)
                {
                    return  _condition.CheckCondition(worldState);
                }
            }

            return true;
        }
        

        public List<Condition> m_Conditions
        {
            get
            {
                if (_conditions == null)
                {
                    Debug.LogError("条件列表中没有值");
                    return null;
                }

                return _conditions;
            }
        }

        public Condition m_Condition
        {
            get
            {
                if (_condition == null)
                {
                    Debug.LogError("条件值为空");
                    return null;
                }

                return _condition;
            }
        }

        public ConditionType m_Type
        {
            get => _type;
        }
    }
}