using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class Sensor
{
    public abstract Type m_SensorValue { get; }
    public UnityAction ActionOnValueChange;
}


[Serializable]
public class SensorBase<T> :Sensor,ISensorValue<T>,ISerializationCallbackReceiver
{
    [SerializeField]protected T _value;

    public override Type m_SensorValue
    {
        get => typeof(T);
    }
    
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

    public virtual void OnBeforeSerialize(){}

    public virtual void OnAfterDeserialize(){}
}
