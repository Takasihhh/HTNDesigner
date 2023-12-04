using System;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using UnityEngine;

namespace HTNDesigner.Domain
{
    [Serializable]
    public sealed class CompoundTask
    {
        [Serializable]
        public struct ConditionMethod
        {
            public ConditionContainer container;
            [SerializeField]public Method method;
        }
        
        
        [SerializeField]private List<ConditionMethod> _methods;
        [SerializeField]private ConditionMethod _method;
        public CompoundTask(){}

        public CompoundTask(List<ConditionMethod> methods)
        {
            _methods = new List<ConditionMethod>();
            _methods = methods;
        }

        public CompoundTask(ConditionMethod method)
        {
            _method = method;
        }
        
        /// <summary>
        /// 寻找适合条件的method
        /// </summary>
        /// <returns></returns>
        public Method SearchForSatisfaiedMethod(WorldStateBlackBoard worldState)
        {
            if (_methods != null && _methods.Count>0)
            {
                foreach (var method in _methods)
                {
                    if (method.container == null)
                    {
                        return method.method;
                    }
                    if (method.container.CheckCondition(worldState))
                    {
                        return method.method;
                    }
                }
            }
            else
            {
                if (_method.container == null ||_method.container.CheckCondition(worldState))
                {
                    return _method.method;
                }
            }
            return null;
        }


        public Method m_Method
        {
            get
            {
                 return _method.method;
            }
        }

        public List<Method> m_Methods
        {
            get
            {
                List<Method> mss = new List<Method>();
                if(_methods !=null && _methods.Count>0)
                {
                    foreach (var method in _methods)
                    {
                        mss.Add(method.method);
                    }
                }
                else
                {
                    return null;
                }

                return mss;
            }
        }
    }
    
}