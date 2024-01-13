using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTNDesigner.Data;
using HTNDesigner.DataStructure;

public class ContainerConstructor : MonoBehaviour
{
    [SerializeField]private MethodNode_SO so;


    private void Start()
    {
        var co = so.Container;
        if (co.m_Type == ConditionContainer.ConditionType.LIST)
        {
            foreach (var cos in co.m_Conditions)
            {
                Debug.Log(cos.ConditonName);
            }
        }
        else
        {
            Debug.Log(co.m_Condition.ConditonName);
        }
    }
}
