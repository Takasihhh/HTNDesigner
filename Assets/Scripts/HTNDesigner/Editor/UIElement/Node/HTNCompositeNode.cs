using System.Collections.Generic;
using HTNDesigner.Editor.Utilies;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor.GraphicsElements
{
    public class HTNCompositeNode:HTNNode
    {
        private Port outputPort;
        public override void Initialize(string nodeName, HTNGraphView htnGraphView, Vector2 position)
        {
            base.Initialize(nodeName, htnGraphView, position);
            NodeType = HTNNodeType.Composite;
            defaultBackgroundColor = Color.green;
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }

        public override void Draw()
        {
            base.Draw();
            Label title = new Label("CompoundTask");
            title.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            titleContainer.Insert(0,title); 
            /* INPUT CONTAINER */
            outputContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            outputPort = this.CreatePort("输出端口", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            
            outputContainer.Add(outputPort);
        }
        
        public override IEnumerable<Edge> GetOutputConnection()
        {
            Debug.Log("查询接口");
            return outputPort.connections;
        }
    }
}