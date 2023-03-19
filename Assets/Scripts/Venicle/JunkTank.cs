using UnityEngine;
using UnityEngine.Events;

public class JunkTank : Upgradeable
{
    [SerializeField] [Min(1)] private int _startCapacity;
    [Header("Upgrade settings")]
    [SerializeField] [Min(0.01f)] protected int _maxUpgradeableCapacity;
    [SerializeField] [Min(2)] protected int _maxLevel;
    [SerializeField] protected AnimationCurve _sizeForLevelCurve;

    public int JunkAmount => _junkAmount;
    public int Capacity => _currentCapacity;
    public bool IsFilled => _junkAmount >= _currentCapacity;
    public event UnityAction<int> CapacityChanged;
    public event UnityAction<int> JunkAmountChanged;

    private int _junkAmount = 0;
    private int _currentCapacity;

    private void Awake()
    {
        _currentCapacity = _startCapacity;
    }

    public void AddJunk(Junk junk)
    {
        _junkAmount += junk.Amount;
        junk.Collected();
        JunkAmountChanged?.Invoke(_junkAmount);

        if (IsFilled)
            Debug.Log("Junk container filled");
    }

    public void Dump()
    {
        _junkAmount = 0;
        JunkAmountChanged?.Invoke(_junkAmount);
    }

    public override void Upgrade()
    {
        LevelUp();
        UpdateCapacity();
    }

    protected override void LevelUp()
    {
        _level++;
        InvokeLevelChangedEvent();
    }

    private void UpdateCapacity()
    {
        _currentCapacity = _startCapacity + Mathf.RoundToInt(_sizeForLevelCurve.Evaluate((float)_level / _maxLevel) * _maxUpgradeableCapacity);
        CapacityChanged?.Invoke(_currentCapacity);
    }
}
