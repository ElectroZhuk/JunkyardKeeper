using UnityEngine;

public class ReachingMaxPlayerLevelGoal : Goal
{
    [SerializeField] private PlayerLevel _playerLevel;

    private void OnEnable()
    {
        _playerLevel.LevelChanged += OnPlayerLevelChanged;
    }

    private void OnDisable()
    {
        _playerLevel.LevelChanged -= OnPlayerLevelChanged;
    }

    private void OnPlayerLevelChanged(int currentLevel)
    {
        if (currentLevel >= StageSettings.TargetPlayerLevel)
            Achieve();
    }
}
