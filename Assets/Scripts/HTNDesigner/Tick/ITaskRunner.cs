using HTNDesigner.BlackBoard;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEngine;


public interface ITaskAgent
{
    public MonoBehaviour TaskInstance { get; }
    public TaskGraph TaskGraph { get;}
    public  WorldStateBlackBoard WorldState_BB { get; }
}