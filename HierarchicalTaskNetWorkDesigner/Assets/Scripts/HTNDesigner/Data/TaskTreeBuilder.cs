using System.Collections;
using System.Collections.Generic;
using HTNDesigner.Domain;
using UnityEngine;

namespace HTNDesigner.Data
{   
    using DataStructure;
    public class TaskTreeBuilder : ScriptableObject
    {
        private TaskTree _tree;
        [SerializeReference]private TaskNode _rootNode;
        
        

        public TaskTree m_Tree
        {
            get
            {
                _tree ??= new TaskTree();
                _tree.m_RootNode = m_RootNode;
                return _tree;
            }
        }
        
        public TaskNode m_RootNode
        {
            get => _rootNode ??=new TaskNode();
            set => _rootNode = value;
        }
        
    }
}
