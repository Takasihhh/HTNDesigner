using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using HTNDesigner.BlackBoard;
using TMPro;
using UnityEngine;

public class AxeEnemy : MonoBehaviour
{
    [SerializeField]private WorldStateBlackBoard _worldStateBlackBoard;

    public int AxeMaxHealth = 5;

    private AsyncReactiveProperty<int> currentAxeHealth;
    [SerializeField] private TextMeshProUGUI axeHealth;
    public GameObject lastTree;
    public GameObject axeObj;
    private string HasAxe = "HasAxe";
    private string OnArriveTree = "OnArriveTree";

    private void Awake()
    {
        TreeTest.OnTreeFinishAct += () =>
        {
            // _worldStateBlackBoard.InputValue(HasAxe,false);
            lastTree = null;
            _worldStateBlackBoard.InputValue(OnArriveTree,false);
        };
        TreeTest.DescreaseAxeAct += () => { CurrentAxeHealth -= 1; };
        currentAxeHealth = new AsyncReactiveProperty<int>(AxeMaxHealth);
        currentAxeHealth.Subscribe((arg) =>
        {
            if (arg <= 0)
            {
                axeHealth.text = "Find Axe";
                axeObj.SetActive(false);
                _worldStateBlackBoard.InputValue(OnArriveTree,false);
                _worldStateBlackBoard.InputValue(HasAxe,false);
                return;
            }
            axeObj.SetActive(true);
            axeHealth.text = CurrentAxeHealth.ToString();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeWorldState();
    }

    private void InitializeWorldState()
    {
        _worldStateBlackBoard.SetValue("OnArriveTree", false);
        _worldStateBlackBoard.SetValue("HasAxe", true);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public int CurrentAxeHealth
    {
        get => currentAxeHealth ??= new AsyncReactiveProperty<int>(AxeMaxHealth);
        set => currentAxeHealth.Value = value;
    }
}
