using System.Collections.Generic;
using UnityEngine;

public class StageSettings : MonoBehaviour
{
    [Header("Level configurations")]
    [SerializeField][Min(1)] private int _targetPlayerLevel;
    [SerializeField] private PlayerLevelJunkConfiguration[] _junkCongigurationForLevels;
    [SerializeField] private int[] _levelElementsToLevelUp;

    public static int TargetPlayerLevel { get; private set; }
    public static IReadOnlyList<PlayerLevelJunkConfiguration> JunkConfigurationForLevel;
    public static IReadOnlyList<int> LevelElementsToLevelUpForLevel;

    private int _minElementsToLevelUpAmount = 1;

    private void OnValidate()
    {
        if (_targetPlayerLevel > _junkCongigurationForLevels.Length)
        {
            PlayerLevelJunkConfiguration[] newJunkModels = new PlayerLevelJunkConfiguration[_targetPlayerLevel];
            _junkCongigurationForLevels.CopyTo(newJunkModels, 0);
            _junkCongigurationForLevels = newJunkModels;
        }
        else if (_targetPlayerLevel < _junkCongigurationForLevels.Length)
        {
            PlayerLevelJunkConfiguration[] newJunkModels = new PlayerLevelJunkConfiguration[_targetPlayerLevel];

            for (int i = 0; i < newJunkModels.Length; i++)
            {
                newJunkModels[i] = _junkCongigurationForLevels[i];
            }

            _junkCongigurationForLevels = newJunkModels;
        }

        if (_targetPlayerLevel > _levelElementsToLevelUp.Length)
        {
            int[] newElements = new int[_targetPlayerLevel - 1];
            _levelElementsToLevelUp.CopyTo(newElements, 0);
            _levelElementsToLevelUp = newElements;
        }
        else if (_targetPlayerLevel < _levelElementsToLevelUp.Length)
        {
            int[] newElements = new int[_targetPlayerLevel - 1];

            for (int i = 0; i < newElements.Length; i++)
            {
                newElements[i] = _levelElementsToLevelUp[i];
            }

            _levelElementsToLevelUp = newElements;
        }

        foreach (int elementsToLevelUp in _levelElementsToLevelUp)
            if (elementsToLevelUp < _minElementsToLevelUpAmount)
                Debug.LogError($"Elements to level up can't be less then {_minElementsToLevelUpAmount}");
    }

    private void Awake()
    {
        TargetPlayerLevel = _targetPlayerLevel;
        JunkConfigurationForLevel = _junkCongigurationForLevels;
        LevelElementsToLevelUpForLevel = _levelElementsToLevelUp;
    }
}


[System.Serializable]
public class PlayerLevelJunkConfiguration
{
    [SerializeField][Min(1)] private int _amountInUnit;
    [SerializeField] private List<Junk> _models;

    public int AmountInUnit => _amountInUnit;
    public IReadOnlyList<Junk> Models => _models;
}
