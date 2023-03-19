using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TriggerEventsInvoker _trigger;
    [SerializeField] private ShopItem[] _items;

    public IReadOnlyCollection<ShopItem> ShopItems => _items;

    private void OnEnable()
    {
        foreach (ShopItem item in _items)
        {
            item.ButtonClicked += OnBuyButtonClicked;
            item.UpdateData();
        }
    }

    private void Start()
    {
        _trigger.TriggerEntered += OnShopTriggerEnter;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (ShopItem item in _items)
            item.ButtonClicked -= OnBuyButtonClicked;
    }

    private void OnBuyButtonClicked(int price, Upgradeable item, ICurrencyStorage currencyStorage, ShopItem shopItem)
    {
        if (currencyStorage.CanSpend(price) == false)
        {
            Debug.LogError("You can't buy this item");
            return;
        }
        
        item.Upgrade();
        currencyStorage.Spend(price);
        shopItem.UpdateData();
    }

    private void OnShopTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player) == false)
            return;

        Activate();
        _trigger.TriggerEntered -= OnShopTriggerEnter;
        _trigger.TriggerExited += OnShopTriggerExit;
    }

    private void OnShopTriggerExit(Collider other)
    {
        Deactivate();
        _trigger.TriggerEntered += OnShopTriggerEnter;
        _trigger.TriggerExited -= OnShopTriggerExit;
    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
