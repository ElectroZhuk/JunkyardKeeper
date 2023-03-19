using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerVenicleHolder))]
public class PlayerWallet : MonoBehaviour, ICurrencyStorage
{
    [SerializeField] private int _money;

    public int Money => _money;

    public event UnityAction CurrencyCollected;

    private TriggerEventsInvoker _trigger;

    private void Awake()
    {
        PlayerVenicleHolder playerVenicleHolder = GetComponent<PlayerVenicleHolder>();
        playerVenicleHolder.VenicleChanged += OnVenicleChanged;
        _trigger = playerVenicleHolder.CurrentVenicle.VenicleBodyTrigger;
    }

    private void OnEnable()
    {
        _trigger.TriggerEntered += OnMoneyTriggerEnter;
    }

    private void OnDisable()
    {
        _trigger.TriggerEntered -= OnMoneyTriggerEnter;
    }

    public void Spend(int amount)
    {
        if (CanSpend(amount) == false)
        {
            Debug.Log($"You can't spend {amount}$ because you have {_money}$");
            return;
        }

        _money -= amount;
        Debug.Log($"You spend {amount} money");
    }

    public bool CanSpend(int amount)
    {
        return -1 < amount && amount <= _money;
    }

    private void CollectMoney(Money money)
    {
        _money += money.Amount;
        money.Collect();
        CurrencyCollected?.Invoke();
        Debug.Log($"Money in wallet: {Money}");
    }

    private void OnMoneyTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Money>(out Money money) == false)
            return;

        CollectMoney(money);
    }

    private void OnVenicleChanged(Venicle newVenicle)
    {
        _trigger.TriggerEntered -= OnMoneyTriggerEnter;
        _trigger = newVenicle.VenicleBodyTrigger;
        _trigger.TriggerEntered += OnMoneyTriggerEnter;
    }
}
