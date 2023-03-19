using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : Upgradeable
{
    private int _targetLevel;

    private void Awake()
    {
        _targetLevel = StageSettings.TargetPlayerLevel;
    }

    public override void Upgrade()
    {
        LevelUp();
    }

    protected override void LevelUp()
    {
        if (_level == _targetLevel)
        {
            Debug.Log("Player reached maximum level!");
            return;
        }

        _level++;
        InvokeLevelChangedEvent();
    }
}
