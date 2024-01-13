using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor.GraphicsElements
{
    using Utilies;
    public class HTNNode:Node
    {
        public string ID { get; set; }
        public string Name => nodeName;
        public bool IsRootNode { get; set; }
        protected GraphView graphView;
        protected string nodeName;
        protected Color defaultBackgroundColor;
        protected Port inputPort;
        public HTNNodeType NodeType { get; set; }
        public virtual void Initialize(string nodeName,HTNGraphView htnGraphView,Vector2 position)
        {
            ID = System.Guid.NewGuid().ToString();
            this.nodeName = nodeName;
            name = "节点";
            SetPosition(new Rect(position, Vector2.zero));
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            
            graphView = htnGraphView;
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {

            /* INPUT CONTAINER */
            inputContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            inputPort = this.CreatePort("输入端口", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            
            inputContainer.Add(inputPort);
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());
            base.BuildContextualMenu(evt);
        }
        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }
        
        
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }


        //TODO:根节点和点击更新
        public override void OnSelected()
        {
            base.OnSelected();
            // this.DrawInspector();
            IOUtility.ClearInspector();
        }
    
        
        public void InitRootNode()
        {
            title = "根节点";
            titleContainer.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            IsRootNode = true;
            this.capabilities &= ~Capabilities.Deletable;
            this.SetRootNode();
            inputPort.SetEnabled(false);
        }

        public virtual IEnumerable<Edge> GetOutputConnection()
        {
            Debug.Log("父节点查询");
            return null;
        }
        
        public string m_DialogueName
        {
            get => nodeName;
            set => nodeName = value;
        }
    }
}