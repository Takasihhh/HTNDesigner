using System.Collections;
using System.Collections.Generic;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using UnityEngine;

public class ConditionBuilder
{
    protected virtual ConditionContainer Build() => null;
    public ConditionContainer Container
    {
        get => Build();
    }
}
