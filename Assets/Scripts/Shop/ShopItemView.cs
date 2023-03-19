using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopItemView : MonoBehaviour
{
    private int _price;

    public void UpdateData(int newPrice)
    {
        _price = newPrice;
    }
}
