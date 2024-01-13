using System;


namespace HTNDesigner.DataStructure
{
    [Serializable]
    public class SensorDictionary<Tkey, TValue> : SerializableDictionary<Tkey, TValue>
    {

    }
    
    [Serializable]
    public class PassWordDictionary :SerializableDictionary<string,int>
    {
    
    }
}