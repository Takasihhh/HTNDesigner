using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using UnityEngine;

namespace HTNDesigner.Domain
{
    [Serializable]
    public sealed class CompoundTask
    {
        [SerializeReference] private List<Method> _methods;
        private List<Method> _satisfaiedMethodList;
        private List<Method>.Enumerator _satisfaiedMethodIterator;
        public CompoundTask(){}

        public CompoundTask(List<Method> methods)
        {
            _methods = methods;
        }

        public void Initialize()
        {
            _satisfaiedMethodList = new List<Method>();
        }
        
        /// <summary>
        /// 寻找适合条件的method
        /// </summary>
        /// <returns></returns>
        public Method SearchForSatisfaiedMethod(WorldStateBlackBoard worldState)
        {
            if (_satisfaiedMethodList == null || _satisfaiedMethodList.Count<=0)
                SearchForSatisfaiedMethods(worldState);
            Method result = _satisfaiedMethodIterator.Current;

            if (result != null)
            {
                _satisfaiedMethodIterator.MoveNext();
                return result;
            }
            return null;
        }

        private List<Method> SearchForSatisfaiedMethods(WorldStateBlackBoard worldState)
        {
            _satisfaiedMethodList = new List<Method>();
            foreach (var method in _methods)
            {
                if (method.CheckConditions(worldState))
                {
                    _satisfaiedMethodList.Add(method);
                }
            }

            _satisfaiedMethodIterator = _satisfaiedMethodList.GetEnumerator();
            _satisfaiedMethodIterator.MoveNext();
            return _satisfaiedMethodList;
        }

    }
    
}