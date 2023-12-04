
using System.Collections.Generic;
using HTNDesigner.Domain;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace HTNDesigner.Editor
{
    public class HTNSearchWindow : ScriptableObject,ISearchWindowProvider
    {
        public HTNGraphView _graphView;
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>();
            //添加至少一项 否则不显示
            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("HTN Nodes")));
            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("PrimitiveNode"),1));
            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("PrimitiveTask"))
            {
                userData = NodeType.PRIMITIVE_NODE,
                level =  2
            });
            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("CompoundNode"),1));
            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("CompoundTask"))
            {
                userData = NodeType.COMPOUND_NODE,
                level =  2
            });
            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("MethodNode"),1));
            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("Method"))
            {
                userData = NodeType.METHOD_NODE,
                level =  2
            });
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            switch (SearchTreeEntry.userData)
            {
                case NodeType.PRIMITIVE_NODE:
                    PrimitiveNodeGraph pnode = new PrimitiveNodeGraph();
                    pnode.root = _graphView.root; 
                    _graphView.AddElement(pnode);
                    break;
                case NodeType.COMPOUND_NODE:
                    CompoundNodeGraph cnode = new CompoundNodeGraph();
                    _graphView.AddElement(cnode);
                    break;
                case NodeType.METHOD_NODE:
                    MethodGroup group = new MethodGroup();
                    group.root = _graphView.root;
                    _graphView.AddElement(group);
                    break;
            }

            return true;
        }
    }
}