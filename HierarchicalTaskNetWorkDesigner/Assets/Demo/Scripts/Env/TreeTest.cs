using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;
using UnityEngine;

public class TreeTest : MonoBehaviour
{
    public int maxHealth = 100;
    private AsyncReactiveProperty<int> currentHealth;
   [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    private CancellationTokenSource cancelDecrease;

    public static event Action OnTreeFinishAct;
    public static event Action DescreaseAxeAct;

    private void Awake()
    {
        currentHealth = new AsyncReactiveProperty<int>(maxHealth);
        _textMeshProUGUI = transform.GetComponentInChildren<TextMeshProUGUI>();
        cancelDecrease = new CancellationTokenSource();
    }

    private void Start()
    {
        currentHealth.Subscribe(BindTree);
    }


    private void BindTree(int arg)
    {
        if (arg <= 0)
        {
            cancelDecrease.Cancel();
            cancelDecrease.Dispose();
            Destroy(this.gameObject);
            gameObject.tag = "Default";
            OnTreeFinishAct?.Invoke();
            return;
        }
        _textMeshProUGUI.text = arg.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DecreaseHealth(cancelDecrease.Token).Forget();
        }
    }

    private async UniTaskVoid DecreaseHealth(CancellationToken token)
    {
        while (!token.IsCancellationRequested && currentHealth>0)
        {
            await UniTask.WaitForSeconds(1f);
            DescreaseAxeAct?.Invoke();
            CurrentHealth -= 30;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            cancelDecrease.Cancel();
            cancelDecrease.Dispose();
            cancelDecrease = new CancellationTokenSource();
        }
    }

    public int CurrentHealth
    {
        get => currentHealth ??= new AsyncReactiveProperty<int>(maxHealth);
        set => currentHealth.Value = value;
    }
}
