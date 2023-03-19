using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerVenicleHolder))]
public class PlayerLevelElementsCollector : MonoBehaviour, ICurrencyStorage
{
    public event UnityAction CurrencyCollected;

    private TriggerEventsInvoker _trigger;
    private int _currentLevelElements;

    private void Awake()
    {
        PlayerVenicleHolder playerVenicleHolder = GetComponent<PlayerVenicleHolder>();
        playerVenicleHolder.VenicleChanged += OnVenicleChanged;
        _trigger = playerVenicleHolder.CurrentVenicle.VenicleBodyTrigger;
    }

    private void OnEnable()
    {
        _trigger.TriggerEntered += OnLevelElementsTriggerEnter;
    }

    private void OnDisable()
    {
        _trigger.TriggerEntered -= OnLevelElementsTriggerEnter;
    }

    public void Spend(int amount)
    {
        if (CanSpend(amount) == false)
        {
            Debug.Log($"You can't spend {amount} element because you have {_currentLevelElements} elements");
            return;
        }

        _currentLevelElements -= amount;
    }

    public bool CanSpend(int amount)
    {
        return -1 < amount && amount <= _currentLevelElements;
    }

    private void CollectPlayerLevelElement(PlayerLevelElement playerLevelElement)
    {
        if (playerLevelElement.Amount < 1)
        {
            Debug.LogError("Player level element amount can't be less then one!");
            return;
        }

        _currentLevelElements += playerLevelElement.Amount;
        playerLevelElement.Collect();
    }

    private void OnLevelElementsTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerLevelElement>(out PlayerLevelElement levelElement) == false)
            return;

        CollectPlayerLevelElement(levelElement);
    }

    private void OnVenicleChanged(Venicle newVenicle)
    {
        _trigger.TriggerEntered -= OnLevelElementsTriggerEnter;
        _trigger = newVenicle.VenicleBodyTrigger;
        _trigger.TriggerEntered += OnLevelElementsTriggerEnter;
    }
}
