using System;
using System.Collections.Generic;
using HTNDesigner.BlackBoard;
using UnityEngine;

namespace HTNDesigner.Domain
{
    [Serializable]
    public class Method
    {
        [SerializeReference]protected List<TaskNode> _subTaskNodes = new List<TaskNode>();
        
        public Method(){}

        public Method(List<TaskNode> nodeList)
        {
            _subTaskNodes = new List<TaskNode>();
            _subTaskNodes = nodeList;
        }
        
        

        public List<TaskNode> SubTasks
        {
            get => _subTaskNodes??= new List<TaskNode>();
        }
    }
}