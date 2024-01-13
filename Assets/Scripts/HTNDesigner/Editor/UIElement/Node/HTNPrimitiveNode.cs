using System;
using System.Collections.Generic;
using System.Linq;
using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure.Variable;
using HTNDesigner.Domain;
using HTNDesigner.Editor.Data;
using HTNDesigner.Editor.Utilies;
using HTNDesigner.Utilies;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor.GraphicsElements
{
    public class HTNPrimitiveNode:HTNNode
    {
        private Port outputPort;
        private VisualElement conditionsInfo;
        private VisualElement titleInfo;
        private VisualElement propertyInfo;
        public string Text { get; set; }
        public List<ConditionStruct> BBConditionConstructList { get; set; }

        public override void Initialize(string nodeName, HTNGraphView htnGraphView, Vector2 position)
        {
            base.Initialize(nodeName, htnGraphView, position);
            NodeType = HTNNodeType.Primitive;
            BBConditionConstructList ??= new List<ConditionStruct>();
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }


        
        public override void Draw()
        {
            base.Draw();
            Label title = new Label(nodeName);
            title.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            titleContainer.Insert(0,title);
            
            /* INPUT CONTAINER */
            outputContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            outputPort = this.CreatePort("输出端口", Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            
            outputContainer.Add(outputPort);
            
            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = ElementUtility.CreateFoldout("Description");

            TextField textTextField = ElementUtility.CreateTextArea(Text, null, callback => Text = callback.newValue);

            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
            
            
            /*Init Inspector*/
            DrawInspectorElement();
        }

        private void DrawInspectorElement()
        {
            propertyInfo = new();
            titleInfo = new();
            conditionsInfo = new();
            conditionsInfo.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            Label title = new Label(Name);
            Button addItem = new Button();
            addItem.text = "+";
            addItem.clicked += () =>
            {
                var menu = new GenericMenu();
                foreach (var sensor in IOUtility.Variables)
                {
                    menu.AddItem(new GUIContent(sensor.Name), false, () => AddBlackBoardCondition(conditionsInfo,sensor));
                }
                menu.ShowAsContext();
            };    
            titleInfo.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            titleInfo.Add(title);
            titleInfo.Add(addItem);
            propertyInfo.Add(titleInfo);
            propertyInfo.Add(conditionsInfo);
            propertyInfo.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            IOUtility.CallForUpdateInspector(propertyInfo);
        }
        
        /// <summary>
        /// 添加黑板的条件
        /// </summary>
        /// <param name="contentElement"></param>
        /// <param name="conditionValue"></param>
        public void AddBlackBoardCondition(VisualElement contentElement,Sensor conditionValue,object value = null)
        {
            VisualElement PropertyInfo = new();
            ConditionStruct conditionStruct = new ConditionStruct
            {
                PropertyType = conditionValue.m_SensorType.Name,
                PropertyName = conditionValue.Name,
                PropertyValue = value?.ToString()
            };
            
            BBConditionConstructList.Add(conditionStruct);
            int index = BBConditionConstructList.Count - 1;
            if (conditionValue.m_SensorType.Name == "Boolean")
            {
                Label title = new Label(conditionValue.Name);
                Toggle checkBox = new Toggle();
                if (value != null)
                    checkBox.value = (bool)value;
                checkBox.RegisterValueChangedCallback((val) =>
                {
                    var vStruct = BBConditionConstructList[index];
                    vStruct.PropertyValue = checkBox.value.ToString();
                    if (vStruct.PropertyValue == "")
                    {
                        vStruct.PropertyValue = "False";
                    }
                    BBConditionConstructList[index] = vStruct;
                });
                PropertyInfo.Add(title);
                PropertyInfo.Add(checkBox);
                PropertyInfo.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                contentElement.Add(PropertyInfo);
            }

            if (conditionValue.m_SensorType.Name == "String")
            {
                DropdownField dropMenu = new DropdownField();
                dropMenu.label = conditionValue.Name;
                Debug.LogError(conditionValue.Name);
                // var res= Type.GetType(conditionValue.Name);
                var enumTypes = ReflectionMethodExtension.FindEnumTypes();
                Type typeRes = null;
                foreach (var enumType in enumTypes)
                {
                    if (enumType.Name == conditionValue.Name)
                    {
                        typeRes = enumType;
                        break;
                    }
                }

                if (typeRes != null)
                {
                    var res = Enum.GetNames(typeRes);
                    dropMenu.choices = res.ToList();
                    contentElement.Add(dropMenu);
                    if (value != null)
                        dropMenu.value = (string)value;
                    dropMenu.RegisterValueChangedCallback((val) =>
                    {
                        var vStruct = BBConditionConstructList[index];
                        vStruct.PropertyValue = dropMenu.value;
                        BBConditionConstructList[index] = vStruct;
                    });
                }
                else
                {
                    Debug.LogError("找不到枚举类型");
                }
            }
        }

        /// <summary>
        /// 初始化条件列表
        /// </summary>
        /// <param name="conditionList"></param>
        public void InitConditions(List<ConditionStruct> conditionList)
        {
            foreach (var condition in conditionList)
            {
                if (condition.PropertyType == "Boolean")
                {
                    SensorBool vbool = new SensorBool();
                    vbool.Name = condition.PropertyName;
                    AddBlackBoardCondition(conditionsInfo,vbool,Boolean.Parse(condition.PropertyValue));
                }

                if (condition.PropertyType == "Object")
                {
                    SensorEnum vEnum = new SensorEnum();
                    vEnum.Name = condition.PropertyName;
                    AddBlackBoardCondition(conditionsInfo,vEnum,condition.PropertyValue);
                }
            }
        }
        
        public override IEnumerable<Edge> GetOutputConnection()
        {
            Debug.Log("查询接口");
            return outputPort.connections;
        }
    }
}