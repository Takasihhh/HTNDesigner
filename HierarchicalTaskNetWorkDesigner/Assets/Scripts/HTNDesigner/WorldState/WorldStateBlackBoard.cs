using System;
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using HTNDesigner.Utilies;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


namespace HTNDesigner.BlackBoard
{
    using DataStructure.Variable;
    [CreateAssetMenu(fileName = "new BlackBoard",menuName = "HTNDesigner/BlackBoard")]
    public class WorldStateBlackBoard:ScriptableObject
    {
        public event Action WorldStateChageEvent;
        [SerializeReference] private PassWordDictionary m_BlackBoardDic;
        [SerializeReference]private List<Sensor> m_BB_List;
        public WorldStateBlackBoard()
        {
            InitBlackBoard();
        }


        public void AddItem(Sensor value)
        {
            m_BB_List ??= new List<Sensor>();
            m_BlackBoardDic ??= new PassWordDictionary();
            m_BB_List.Add(value);
            m_BlackBoardDic.Add(value.Name,m_BB_List.Count-1);
        }
        
        public void DeepCopy(WorldStateBlackBoard value)
        {
            m_BB_List = ReflectionMethodExtension.DeepCopy(value.m_BB_List);
            // m_BlackBoardDic = ReflectionMethodExtension.DeepCopy(value.m_BlackBoardDic);
            m_BlackBoardDic = value.m_BlackBoardDic;
        }
        
        public void InitBlackBoard()
        {
            m_BlackBoardDic = new PassWordDictionary();
            m_BB_List = new List<Sensor>();
        }

        public void ResetWorld()
        {
        }

        public T GetValue<T>(string key)
        {
            m_BlackBoardDic.TryGetValue(key,out int index);
            var result = m_BB_List[index];

            return (result as SensorBase<T>).GetValue();
        }
        
        internal bool SetValue<T>(string key,T value)
        {
            if (m_BlackBoardDic != null)
            {
                if (m_BlackBoardDic.ContainsKey(key))
                {
                    // SensorBase<T> res = new SensorBase<T>();
                    // res.SetValue(value);
                    // m_BB_List ??= new List<Sensor>();
                    // m_BB_List.Add(res);
                    // m_BlackBoardDic[key] = m_BB_List.Count-1;
                    int index = m_BlackBoardDic[key];
                    (m_BB_List[index] as SensorBase<T>)?.SetValue(value);
                    return true;
                }
                else
                {
                    Debug.LogError("世界状态根本不存在这个属性");  
#if UNITY_EDITOR
                    EditorApplication.isPaused = true;
#endif
                    
                }
            }
            else
            {
                Debug.LogError("世界状态为空");  
#if UNITY_EDITOR
                EditorApplication.isPaused = true;
#endif
            }

            return false;
        }
        
        public void InputValue<T>(string key, T value)
        {
            if (SetValue(key,value))
                WorldStateChageEvent?.Invoke();
        }
        
    }
}