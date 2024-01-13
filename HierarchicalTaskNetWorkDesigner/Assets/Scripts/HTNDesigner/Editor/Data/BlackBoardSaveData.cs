using System;
using System.Collections.Generic;
using HTNDesigner.DataStructure.Variable;
using UnityEngine;

namespace HTNDesigner.Editor.Data
{
    [Serializable]
    public class BlackBoardSaveData
    {
        [field: SerializeField] public List<BBValueSaveStruct> BBValueList { get; set; }
    }

    [Serializable]
    public struct BBValueSaveStruct
    {
        public string typeName;
        public string Name;
    }
}