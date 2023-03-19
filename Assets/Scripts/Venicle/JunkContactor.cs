using UnityEngine;
using UnityEngine.Events;

public abstract class JunkContactor : Upgradeable
{
    [SerializeField] [Min(0.001f)] protected float _startSize;
    [Header("Upgrade settings")]
    [SerializeField] [Min(0.01f)] protected float _maxUpgradeableSize;
    [SerializeField] [Min(2)] protected int _maxLevel;
    [SerializeField] protected AnimationCurve _sizeForLevelCurve;

    public event UnityAction<Junk> JunkContacted;
    public event UnityAction<JunkContactor> SizeChanged;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Junk>(out Junk junk) == false)
            return;

        JunkContacted?.Invoke(junk);
    }

    public abstract void UpdateSize();

    protected void InvokeSizeChangedEvent(JunkContactor junkContactor)
    {
        SizeChanged?.Invoke(junkContactor);
    }
}
