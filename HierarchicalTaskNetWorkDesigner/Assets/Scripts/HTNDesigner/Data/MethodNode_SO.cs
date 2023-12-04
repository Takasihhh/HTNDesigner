using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HTNDesigner.Data
{
    using DataStructure;
    using Domain;
    using Utilies;
    [CreateAssetMenu(menuName = "HTNDesigner/MethodNode",fileName = "New Method")]
    public class MethodNode_SO : ScriptableObject
    {
        private ConditionContainer _container;
        public Method method;
        [SerializeField] private TextAsset _conditionBuilder;
        [SerializeField] private TextAsset _methodBuilder;
        private ConditionContainer ConstructContainer()
        {
            // if (_conditions != null && _conditions.Count > 0)
            // {
            //     List<Condition> list = new List<Condition>();
            //     foreach (var cType in _conditions)
            //     {
            //         string classCode = cType.text;
            //         Assembly assembly = Assembly.GetExecutingAssembly();
            //         Type classCons = assembly.GetType(cType.name);
            //         if (classCons != null)
            //         {
            //             object instance = Activator.CreateInstance(classCons);
            //             list.Add(instance as Condition);
            //         }
            //         else
            //         {
            //             Debug.LogError("类型不存在");
            //         }
            //     }
            //
            //     _container = new ConditionContainer(list);
            //     return _container;
            // }
            // else
            // {
            //         
            //     string classCode = _condition.text;
            //     Assembly assembly = Assembly.GetExecutingAssembly();
            //     Type classCons = assembly.GetType(_condition.name);
            //     if (classCons != null)
            //     {
            //         object instance = Activator.CreateInstance(classCons);
            //         _container = new ConditionContainer(instance as Condition);
            //     }
            //     else
            //     {
            //         Debug.LogError("类型不存在");
            //     }
            if (_conditionBuilder != null &&_conditionBuilder.name != "")
            {
                object res = ReflectionMethodExtension.CreateInstance(_conditionBuilder.name);
                if (res != null)
                    _container = (res as ConditionBuilder).Container;
                else
                    Debug.LogError("转换失败");
            }
            else
            {
                _container = null;
            }

            return _container;
        }

        private CompoundTask.ConditionMethod ConstructMethod()
        {
            CompoundTask.ConditionMethod mth;
            mth.method = method;
            mth.container = Container;
            return mth;
        }
        
        

        public ConditionContainer Container
        {
            get
            {
                return ConstructContainer();
            }
        }

        public CompoundTask.ConditionMethod m_Method
        {
            get
            {
                return ConstructMethod();
            }
        }
    }
}
