using System.Collections.Generic;
using UnityEngine;

namespace HTNDesigner.Editor.Data
{
    public class GraphSaveData_SO:ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
 
        [field: SerializeField] public List<NodeSaveData> Nodes { get; set; }
        [field: SerializeField] public BlackBoardSaveData BBValue { get; set; }
        public void Initialize(string fileName)
        {
            FileName = fileName;

            Nodes = new List<NodeSaveData>();
        }
    }
}