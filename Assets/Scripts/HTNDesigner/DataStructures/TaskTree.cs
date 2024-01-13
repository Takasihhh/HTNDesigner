using System;
using System.Collections.Generic;
using HTNDesigner.Domain;
using UnityEngine;
namespace HTNDesigner.DataStructure
{
    [Serializable]
    public class TaskTree
    {
        [SerializeField]private TaskNode rootNode;


        // public TaskTree()
        // {
        //     List<(ConditionContainer,Method)> rtMethods = new List<(ConditionContainer,Method)>();
        //     HealthCondition hcon1, hcon2, hcon3;
        //     hcon1 = new HealthCondition(5);
        //     hcon2 = new HealthCondition(1, 3);
        //     hcon3 = new HealthCondition(-100, 0);
        //     DistanceCondition dcon1, dcon2;
        //     dcon1 = new DistanceCondition(true);
        //     dcon2 = new DistanceCondition(false);
        //     NeedHealingCondition ncon1 = new NeedHealingCondition(true), ncon2 =new NeedHealingCondition(false);
        //     
        //     List<Condition> cons1 = new List<Condition>(), cons2 = new List<Condition>(),cons3 = new List<Condition>();
        //     cons1.Add(hcon1);
        //     cons1.Add(dcon1);
        //     cons1.Add(ncon2);
        //     cons2.Add(hcon1);
        //     cons2.Add(dcon2);
        //     cons2.Add(ncon2);
        //     cons3.Add(hcon2);
        //     cons3.Add(ncon2);
        //     
        //     ConditionContainer directAtkContainer = new ConditionContainer(cons1);
        //     ConditionContainer moveAtkContainer = new ConditionContainer(cons2);
        //     ConditionContainer healingContainer = new ConditionContainer(ncon1);
        //     ConditionContainer escapContainer = new ConditionContainer(cons3);
        //     ConditionContainer deathContainer = new ConditionContainer(hcon3);
        //
        //     rtMethods.Add((deathContainer, new DeathMethod()));
        //     rtMethods.Add((escapContainer, new EscapMethod()));
        //     rtMethods.Add((moveAtkContainer,new MoveAttackMethod()));
        //     rtMethods.Add((directAtkContainer,new DirectAttackMethod()));
        //     rtMethods.Add((healingContainer,new HealingMethod()));
        //     CompoundTask rootTask = new CompoundTask(rtMethods);
        //     rootNode = new TaskNode(rootTask);
        // }
        
            
        public TaskNode m_RootNode
        {
            get => rootNode;
            set => rootNode = value;
        }
    }
}