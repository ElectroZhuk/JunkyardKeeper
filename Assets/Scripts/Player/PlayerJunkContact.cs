using UnityEngine;
using UnityEngine.Events;

public class PlayerJunkContact : Upgradeable
{
    [SerializeField] private JunkContactor _junkContactor;
    [SerializeField][Min(0.001f)] private float _startSize;
    [Header("Upgrade settings")]
    [SerializeField] [Min(0.01f)] private float _maxUpgradeableSize;
    [SerializeField] [Min(2)] private int _maxLevel;
    [SerializeField] private AnimationCurve _sizeForLevelCurve;

    public event UnityAction<Junk> JunkContacted;
    public event UnityAction<JunkContactor> SizeChanged;

    private void Start()
    {
        //_junkContactor.UpgradeSize(_startSize);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Junk>(out Junk junk) == false)
            return;

        JunkContacted?.Invoke(junk);
    }

    public override void Upgrade()
    {
        LevelUp();
        IncreaseSize();
    }

    private void IncreaseSize()
    {
        if (Level > _maxLevel)
            return;

        //_junkContactor.UpgradeSize(_startSize + _sizeForLevelCurve.Evaluate((float)Level / _maxLevel) * _maxUpgradeableSize);
        SizeChanged?.Invoke(_junkContactor);
    }

    protected override void LevelUp()
    {
        throw new System.NotImplementedException();
    }
}
