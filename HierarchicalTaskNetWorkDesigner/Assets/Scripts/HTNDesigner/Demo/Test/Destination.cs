using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private List<Transform> destinations;
    public Transform playerPos;
    public Vector3 GetRandomDestination()
    {
        return destinations[Random.Range(0, destinations.Count)].position;
    }
}
