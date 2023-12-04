using HTNDesigner.BlackBoard;
using HTNDesigner.Domain;
using UnityEngine;


public interface ITaskAgent
{
    public GameObject TaskInstance { get; }
    public TaskNode m_Root { get;}
    public  WorldStateBlackBoard m_worldState { get; }
}