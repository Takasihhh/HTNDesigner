using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace HTNDesigner.DataStructure.Variable
{

    [Serializable]
    public abstract class Sensor
    {
        public abstract Type m_SensorType { get; }
        public abstract string Name { get; set; }
        public UnityAction ActionOnValueChange;
    }
    

    [Serializable]
    public class SensorBase<T> : Sensor, ISensorValue<T>,ISerializationCallbackReceiver
    {
        [SerializeField] protected T _value;

        public override Type m_SensorType
        {
            get => typeof(T);
        }

        [field:SerializeField]public override string Name { get; set; }

        public SensorBase()
        {
            _value = default;
        }

        public SensorBase(T value)
        {
            _value = value;
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(ISensorValue<T> value)
        {
            _value = value.GetValue();

        }

        public void SetValue(T value)
        {
            _value = value;
            ActionOnValueChange?.Invoke();
        }

        public void OnBeforeSerialize(){}

        public void OnAfterDeserialize(){ Debug.LogWarning("反序列化");}
    }
}