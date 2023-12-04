using System;
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


namespace HTNDesigner.BlackBoard
{
    [CreateAssetMenu(fileName = "new BlackBoard",menuName = "HTNDesigner/BlackBoard")]
    public class WorldStateBlackBoard:ScriptableObject
    {
        public event Action WorldStateChageEvent;
        private Dictionary<string, Sensor> m_BlackBoradDic;


        [SerializeField] private SensorBase<int> health;
        [SerializeField]private SensorBase<bool> onArrive;
        [SerializeField] private SensorBase<bool> cd;
        [SerializeField] private SensorBase<bool> needHealing;

        public WorldStateBlackBoard()
        {
            InitBlackBoard();
        }

        public void DeepCopy(WorldStateBlackBoard value)
        {
            health.SetValue(value.GetValue<int>("健康值"));
            onArrive.SetValue(value.GetValue<bool>("是否到达"));
            cd.SetValue(value.GetValue<bool>("大招CD"));
            needHealing.SetValue(value.GetValue<bool>("需要治疗"));
        }
        
        private void InitBlackBoard()
        {
            m_BlackBoradDic = new Dictionary<string, Sensor>();
            health = new SensorBase<int>(10);
            onArrive = new SensorBase<bool>(false);
            cd = new SensorBase<bool>(true);
            needHealing = new SensorBase<bool>(false);
            //Debug.Log(nameof(health));
            m_BlackBoradDic.Add("健康值",health);
            m_BlackBoradDic.Add("是否到达",onArrive);
            m_BlackBoradDic.Add("大招CD",cd);
            m_BlackBoradDic.Add("需要治疗",needHealing);
            foreach (var kp in m_BlackBoradDic)
            {
                kp.Value.ActionOnValueChange += () =>
                {
                    WorldStateChageEvent?.Invoke();
                };
            }
        }

        public void ResetWorld()
        {
            m_BlackBoradDic.Clear();
            m_BlackBoradDic = new Dictionary<string, Sensor>();
            health = new SensorBase<int>(10);
            onArrive = new SensorBase<bool>(false);
            cd = new SensorBase<bool>(true);
            needHealing = new SensorBase<bool>(false);
            //Debug.Log(nameof(health));
            m_BlackBoradDic.Add("健康值",health);
            m_BlackBoradDic.Add("是否到达",onArrive);
            m_BlackBoradDic.Add("大招CD",cd);
            m_BlackBoradDic.Add("需要治疗",needHealing);
            foreach (var kp in m_BlackBoradDic)
            {
                kp.Value.ActionOnValueChange += () =>
                {
                    WorldStateChageEvent?.Invoke();
                };
            }
        }

        public T GetValue<T>(string key)
        {
            m_BlackBoradDic.TryGetValue(key,out Sensor result);
            if (result == null)
            {
                return default;
            }

            return (result as SensorBase<T>).GetValue();
        }

        public Sensor GetValue(string key)
        {
            m_BlackBoradDic.TryGetValue(key,out Sensor result);
            if (result == null)
            {
                return default;
            }
            return result;
        }
        
        internal bool SetValue<T>(string key,T value)
        {
            if (m_BlackBoradDic != null)
            {
                if (m_BlackBoradDic.ContainsKey(key))
                {
                    SensorBase<T> res = new SensorBase<T>();
                    res.SetValue(value);
                    m_BlackBoradDic[key] = res;
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