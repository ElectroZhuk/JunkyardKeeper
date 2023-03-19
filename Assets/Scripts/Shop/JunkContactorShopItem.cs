using UnityEngine;

public class JunkContactorShopItem : ShopItem
{
    [SerializeField] private PlayerVenicleHolder _playerVenicleHolder;

    private void Awake()
    {
        ChangeItem(_playerVenicleHolder.CurrentVenicle.JunkContactor);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerVenicleHolder.VenicleChanged += OnVenicleChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerVenicleHolder.VenicleChanged -= OnVenicleChanged;
    }

    private void OnVenicleChanged(Venicle currentVenicle)
    {
        ChangeItem(currentVenicle.JunkContactor);
    }
}
