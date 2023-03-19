using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CapsuleJunkContactor : JunkContactor
{
    private CapsuleCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _collider.isTrigger = true;
        _collider.radius = _startSize;
    }

    public override void UpdateSize()
    {
        _collider.radius = _startSize + _sizeForLevelCurve.Evaluate((float)_level / _maxLevel) * _maxUpgradeableSize;
        InvokeSizeChangedEvent(this);
    }

    public override void Upgrade()
    {
        LevelUp();
        UpdateSize();
    }

    protected override void LevelUp()
    {
        _level++;
        InvokeLevelChangedEvent();
    }
}
