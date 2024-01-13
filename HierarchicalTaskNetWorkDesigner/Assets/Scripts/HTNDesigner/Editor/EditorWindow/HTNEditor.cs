using System;
using System.IO;
using HTNDesigner.Utilies;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HTNDesigner.Editor
{
    using Utilies;
    using Data;
    public class HTNEditor : EditorWindow
    {
        private HTNGraphView _graphView;
        private static TextField fileNameTextField;
        private bool onInit;

        [MenuItem("Window/AI/HTNDesigner")]
        private static void OpenWnd()
        {
            var wnd = GetWindow<HTNEditor>();
            wnd.titleContent = new GUIContent("HTN编辑器");
            wnd.maxSize = new Vector2(1920, 1080);
            wnd.minSize = new Vector2(1280, 720);
        }

        private void OnEnable()
        {
                IOUtility.DrawInspectorAct += DrawInspector;
                IOUtility.ClearInspectorAct += ClearInspector;
                LoadWnd();
                Clear();
                IOUtility.InitializeData(_graphView, "");
        }

        private void OnDisable()
        {
            Clear();
        }

        private void LoadWnd()
        {
            DrawMainContainer();    
            DrawGraphView();
            BindItems();
        }

        private void DrawMainContainer()
        {
            //加载主界面
            VisualTreeAsset editorViewTree = (VisualTreeAsset)EditorGUIUtility.Load("MainWnd.uxml");
            TemplateContainer editorInstance = editorViewTree.CloneTree();
            editorInstance.StretchToParentSize();
            rootVisualElement.Add(editorInstance);
        }

        private void DrawGraphView()
        {
            _graphView = new HTNGraphView(this);
            _graphView.style.minHeight = 1000;
            _graphView.style.minWidth = 500;
            _graphView.StretchToParentSize();
            rootVisualElement.Q<VisualElement>("RightPanel").Add(_graphView);
        }


        private void DrawInspector(VisualElement element)
        {
            var container = rootVisualElement.Q<VisualElement>("ContextContainer");
            container.Clear();
            container.Add(element);
        }
        
        
        private void BindItems()
        {
            // rootVisualElement.Q<Button>("MiniMapBtn").clicked += ToggleMiniMap;
            rootVisualElement.Q<Button>("SaveBtn").clicked += SaveFile;
            rootVisualElement.Q<Button>("LoadBtn").clicked += LoadFile;
            rootVisualElement.Q<Button>("ClearBtn").clicked += Clear;
            fileNameTextField = rootVisualElement.Q<TextField>("FileNameText");
           
            fileNameTextField.RegisterValueChangedCallback( callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });
        }

        // private void ToggleMiniMap()
        // {
        //     _graphView.ToggleMiniMap();
        // }


        #region 功能

        private void CreateNewFile()
        {
            
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            IOUtility.InitializeData(_graphView, fileNameTextField.value);
            IOUtility.Save();
        }

        private void LoadFile()
        {
            string filePath = EditorUtility.OpenFilePanel("HTN Graphs", "Assets/Editor/HTNDesigner/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();
            IOUtility.InitializeData(_graphView, Path.GetFileNameWithoutExtension(filePath));
            IOUtility.Load();
        }

        private void Clear()
        {
            _graphView.ClearGraph();
            var container = rootVisualElement.Q<VisualElement>("ContextContainer");
            container.Clear();
            IOUtility.ClearData();
        }

        public void ClearInspector()
        {
            var container = rootVisualElement.Q<VisualElement>("ContextContainer");
            container.Clear();
        }
        
        public static void UpdateFileName(string fileName)
        {
            fileNameTextField.value = fileName;
        }

        #endregion
    }
}