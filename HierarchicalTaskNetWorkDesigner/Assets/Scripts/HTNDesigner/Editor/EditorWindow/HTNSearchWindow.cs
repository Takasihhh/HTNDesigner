using System.Collections.Generic;
using System.Linq;
using HTNDesigner.Editor.GraphicsElements;
using HTNDesigner.Editor.Utilies;
using HTNDesigner.Utilies;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    public class HTNSearchWindow:ScriptableObject, ISearchWindowProvider
    {
        private HTNGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(HTNGraphView HTNgraphView)
        {
            graphView = HTNgraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Tasks")),
                new SearchTreeGroupEntry(new GUIContent("Custom Primitive Tasks"), 1),
            };

            var taskList = IOUtility.PrimitiveTaskName;
            foreach (var taskName in taskList)
            {
                searchTreeEntries.Add(
                    new SearchTreeEntry(new GUIContent(taskName, indentationIcon))
                    {
                        userData = taskName,
                        level = 2
                    }
                    );
            }
            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("Composite Tasks"), 1));
            
            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("Create Hub",indentationIcon))
            {
                userData = HTNNodeType.Hub,
                level = 2          
            });
            
            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("Create CompositeNode",indentationIcon))
            {
                userData = HTNNodeType.Composite,
                level = 2          
            });

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            HTNNode node;
            
            if(!graphView.graphElements.Any())
            {
                node = graphView.CreateNode("RootNode", HTNNodeType.Composite,
                    localMousePosition);
                node.InitRootNode();
                graphView.AddElement(node);
                return true;
            }
            
            if (SearchTreeEntry.userData is string className)
            {
                node = graphView.CreateNode(className, HTNNodeType.Primitive, localMousePosition);
                graphView.AddElement(node);
                
                return true;
            }
            
            switch (SearchTreeEntry.userData)
            {
                case HTNNodeType.Hub:
                    node = graphView.CreateNode("Hub", HTNNodeType.Hub, localMousePosition);
                    graphView.AddElement(node);
                    break;
                case HTNNodeType.Composite:
                    node = graphView.CreateNode("CompoundNode", HTNNodeType.Composite, localMousePosition);
                    graphView.AddElement(node);
                    break;
                
                default:
                {
                    return false;
                }
            }

            return true;
        }
    }
}