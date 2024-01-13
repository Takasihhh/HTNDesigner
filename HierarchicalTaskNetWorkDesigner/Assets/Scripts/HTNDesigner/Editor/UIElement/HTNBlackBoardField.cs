using System;
using System.Linq;
using HTNDesigner.DataStructure.Variable;
using HTNDesigner.Editor.Utilies;
using HTNDesigner.Utilies;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    public class HTNBlackBoardField:BlackboardField
    {

        public VisualElement RawContainer;
        public HTNBlackBoardField(GraphView view,VisualElement rawContainer) : base()
        {
            RawContainer = rawContainer;
        }


        public override void OnSelected()
        {
            base.OnSelected();
            int index = RawContainer.IndexOf(this);
            var sensor = IOUtility.Variables[index-1];

            if (sensor.m_SensorType.Name == "object")
            {
                VisualElement FieldInfo = new();
                DropdownField enumField = new DropdownField();
                enumField.label = "EnumType";
                var names = Enum.GetNames(typeof(EnumTypeEnumration));
                enumField.choices = names.ToList();
                enumField.value = this.text;
                enumField.RegisterValueChangedCallback((val) =>
                {
                    IOUtility.Variables[index-1].Name = val.newValue;
                    this.text = val.newValue;
                });
                FieldInfo.Add(enumField);
                IOUtility.CallForUpdateInspector(FieldInfo);
            }
        }
    }
}