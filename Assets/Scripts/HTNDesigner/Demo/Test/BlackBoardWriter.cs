using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HTNDesigner.BlackBoard;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackBoardWriter : MonoBehaviour
{
    public bool test1;
    public bool test2;
    [SerializeField] private WorldStateBlackBoard _blackBoard;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!test1)
                WriteDamage().Forget();
        }

        if (other.CompareTag("HealingPoint"))
        {
            if(!test2)
                WriteHealing().Forget();
        }
    }

    public async UniTaskVoid WriteHealing()
    {
        test2 = true;
        await UniTask.WaitForSeconds(2f);
        var val =_blackBoard.GetValue<int>("健康值")+1;
        _blackBoard.InputValue("健康值",val);
        Debug.LogError("当前血量"+val);
        test2 = false;  
    }
    
    public async UniTaskVoid WriteDamage()
    {
        test1 = true;
        var val = _blackBoard.GetValue<int>("健康值") - Random.Range(0, 3);
        Debug.LogError("当前血量"+val);
        _blackBoard.InputValue("健康值",val);
        await UniTask.WaitForSeconds(5f);
        test1 = false;  
    }
}
