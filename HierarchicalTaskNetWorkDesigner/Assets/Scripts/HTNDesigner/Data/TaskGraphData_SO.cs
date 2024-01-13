using HTNDesigner.DataStructure;
using UnityEngine;

namespace HTNDesigner.Data
{
    public class TaskGraphData_SO:ScriptableObject
    {
        [SerializeReference] private TaskGraph m_GraphData;


        public void Initialize(TaskGraph graphData)
        {
            m_GraphData = graphData;
        }

        public TaskGraph GraphData
        {
            get => m_GraphData;
        }
    }
}