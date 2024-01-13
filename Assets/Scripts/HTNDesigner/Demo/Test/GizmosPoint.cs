using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosPoint : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color triggerColor;
    private Color _curColor;

    private void Awake()
    {
        _curColor = normalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _curColor = triggerColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _curColor = normalColor;
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = _curColor;
            Gizmos.DrawWireSphere(transform.position,GetComponent<SphereCollider>().radius);
        }
        catch
        {
            
        }
    }
}
