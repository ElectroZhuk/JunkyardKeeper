using UnityEngine;

public class ClearPercentageOfJunkOnLastLevelGoal : Goal
{
    [SerializeField] private PlayerLevel _playerLevel;
    [SerializeField] private PlayerJunkCollector _playerJunkCollector;
    [SerializeField] private StageJunkSpawner _stageJunkSpawner;
    [SerializeField] private float _targetPercentage;

    private void OnValidate()
    {
        _targetPercentage = Mathf.Clamp(_targetPercentage, 0, 1);
    }

    private void Start()
    {
        _playerLevel.LevelChanged += OnPlayerLevelChanged;
    }

    private void OnPlayerLevelChanged(int currentLevel)
    {
        if (currentLevel < StageSettings.TargetPlayerLevel)
            return;

        _playerLevel.LevelChanged -= OnPlayerLevelChanged;
        _playerJunkCollector.JunkCollected += OnJunkCollected;
        CheckGoalState();
    }

    private void OnJunkCollected()
    {
        CheckGoalState();
    }

    private void CheckGoalState()
    {
        float currentClearJunkPercentage = (float)System.Math.Round((double)_stageJunkSpawner.CollectedJunkAmountOnStage / _stageJunkSpawner.MaxJunkAmountOnStage, 2);

        if (currentClearJunkPercentage < _targetPercentage)
            return;

        _playerJunkCollector.JunkCollected -= OnJunkCollected;
        Achieve();
    }
}
