using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected Upgradeable _item;
    [SerializeField] private MonoBehaviour _iCurrencyStorage;
    [Header("View")]
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;

    public int Price => _currentPrice;
    public bool CanBuy => _canBuy;

    public event UnityAction<int, Upgradeable, ICurrencyStorage, ShopItem> ButtonClicked;

    protected ICurrencyStorage _currencyStorage;
    protected int _currentPrice;
    protected bool _canBuy;

    private void OnValidate()
    {
        if (_iCurrencyStorage == null)
            return;

        if (_iCurrencyStorage is ICurrencyStorage == false)
            throw new UnityException("Currency storage must be ICurrencyStorage");

        _currencyStorage = (ICurrencyStorage)_iCurrencyStorage;
    }

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
        _currencyStorage.CurrencyCollected += UpdateData;
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _currencyStorage.CurrencyCollected -= UpdateData;
    }
    
    public void UpdateData()
    {
        _currentPrice = _item.Price;
        _canBuy = _currencyStorage.CanSpend(_currentPrice);

        if (_item is PlayerLevel playerLevel)
            _canBuy = _canBuy && StageSettings.TargetPlayerLevel != playerLevel.Level;

        UpdateView();
    }

    protected void ChangeItem(Upgradeable newItem)
    {
        _item = newItem;
        UpdateData();
    }

    private void UpdateView()
    {
        _text.text = Price.ToString();
        _button.interactable = _canBuy;
    }

    private void OnUpgradeableChanged(Upgradeable newUpgradeable)
    {
        _item = newUpgradeable;
        UpdateData();
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(_currentPrice, _item, _currencyStorage, this);
    }
}