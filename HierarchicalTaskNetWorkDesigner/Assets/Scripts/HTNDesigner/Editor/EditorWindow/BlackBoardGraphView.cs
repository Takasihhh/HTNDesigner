using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    using DataStructure.Variable;
    using Utilies;
    public sealed class BlackBoardGraphView : Blackboard
    {
        public new class UxmlFactory : UxmlFactory<BlackBoardGraphView, UxmlTraits>
        {
            
        }
        
        private readonly ScrollView scrollView;
        public VisualElement RawContainer => scrollView;
        private readonly List<Sensor> sharedVariables;


        public BlackBoardGraphView()
        {
            
        }
        
        public BlackBoardGraphView(GraphView graphView) : base(graphView)
        {
            var header = this.Q("header");
            header.style.height = new StyleLength(50);
            Add(scrollView = new());
            scrollView.Add(new BlackboardSection { title = "Shared Variables" });
            // RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            // RegisterCallback<DetachFromPanelEvent>(OnDispose);
            sharedVariables = IOUtility.Variables;
            if (!Application.isPlaying) InitRequestDelegate();
        }
        
        private void InitRequestDelegate()
        {
            addItemRequested = _blackboard =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Bool"), false, () => AddSharedVariable(new SensorBool()));
                menu.AddItem(new GUIContent("Enum"), false, () => AddSharedVariable(new SensorEnum()));
                menu.ShowAsContext();
            };
            editTextRequested = (_blackboard, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                var index = sharedVariables.FindIndex(x => x.Name == oldPropertyName);
                if (string.IsNullOrEmpty(newValue))
                {
                    RawContainer.RemoveAt(index + 1);
                    sharedVariables.RemoveAt(index);
                    return;
                }
                if (sharedVariables.Any(x => x.Name == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "A variable with the same name already exists !",
                        "OK");
                    return;
                }
                var targetIndex = sharedVariables.FindIndex(x => x.Name == oldPropertyName);
                sharedVariables[targetIndex].Name = newValue;
                ((BlackboardField)element).text = newValue;
                var placeHolder = new VisualElement();
            };

        }

        public void UpdateBlackBoard()
        {
            foreach (var sensor in IOUtility.Variables)
            {
                AddSharedVariable(sensor);
            }
        }
        
        public void AddSharedVariable(Sensor variable)
        {
            if (string.IsNullOrEmpty(variable.Name)) variable.Name = variable.GetType().Name;
            var localPropertyName = variable.Name;
            int index = 1;
            while (sharedVariables.Any(x => x.Name == localPropertyName))
            {
                localPropertyName = $"{variable.Name}{index++}";
            }

            variable.Name = localPropertyName;
            sharedVariables.Add(variable);
            if(!IOUtility.Variables.Contains(variable))
                IOUtility.Variables.Add(variable);
            var container = new VisualElement();
            var field = new HTNBlackBoardField(graphView,RawContainer) { text = localPropertyName, typeText = variable.GetType().Name };
            field.capabilities &= ~Capabilities.Deletable;
            field.capabilities &= ~Capabilities.Movable;
            if (Application.isPlaying)
            {
                field.capabilities &= ~Capabilities.Renamable;
            }

            
            field.AddManipulator(new ContextualMenuManipulator((evt) => BuildBlackboardMenu(evt, field)));
            RawContainer.Add(field);
        }
        
        private void BuildBlackboardMenu(ContextualMenuPopulateEvent evt, VisualElement element)
        {
            evt.menu.MenuItems().Clear();
            if (Application.isPlaying) return;
            evt.menu.AppendAction("Delate Variable", actionEvent =>
            {
                int index = RawContainer.IndexOf(element);
                var variable = sharedVariables[index - 1];
                sharedVariables.RemoveAt(index - 1);
                RawContainer.Remove(element);
                return;
            });
        }

        public void ClearField()
        {
            RawContainer.Clear();
            scrollView.Add(new BlackboardSection { title = "Shared Variables" });
        }
    }
}
