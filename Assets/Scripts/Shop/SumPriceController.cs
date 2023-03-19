using UnityEngine;

public class SumPriceController : PriceController
{
    [SerializeField][Min(0)] private int _startPrice;
    [SerializeField][Min(0)] private int _priceForLevel;

    private int _startItemLevel = 1;

    public override int GetPrice(int itemLevel)
    {
        return _priceForLevel * (itemLevel - _startItemLevel) + _startPrice;
    }
}
