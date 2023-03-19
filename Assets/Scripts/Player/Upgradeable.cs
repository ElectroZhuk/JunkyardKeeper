using UnityEngine;
using UnityEngine.Events;

public abstract class Upgradeable : MonoBehaviour
{
    [SerializeField] private PriceController _priceController;

    protected int _level = 1;

    public int Level => _level;
    public int Price => _priceController.GetPrice(_level);

    public event UnityAction<int> LevelChanged;

    protected void InvokeLevelChangedEvent()
    {
        LevelChanged?.Invoke(_level);
    }

    public abstract void Upgrade();

    protected abstract void LevelUp();
}
