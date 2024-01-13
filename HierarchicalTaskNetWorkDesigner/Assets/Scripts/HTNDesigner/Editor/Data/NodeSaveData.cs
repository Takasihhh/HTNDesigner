using System;
using System.Collections.Generic;
using HTNDesigner.Editor.Utilies;
using UnityEngine;

namespace HTNDesigner.Editor.Data
{
    using Domain;
    [Serializable]
    public class NodeSaveData
    {
        [field:SerializeField]public string ID { get; set; }
        [field:SerializeField]public string Name { get; set; }
        [field:SerializeField]public HTNNodeType NodeType { get; set; }
        [field:SerializeField] public List<string> ConnectionID { get; set; }
        [field:SerializeField] public Vector2 Position { get; set; }
        
        [field:SerializeField]public List<ConditionStruct> Conditions { get; set; }
        [field:SerializeField] public string Description { get; set; }
        [field:SerializeField] public bool isRootNode { get; set; }
    }
    
    

}