using System.Collections.Generic;
using UnityEngine;

public class StageJunkSpawner : MonoBehaviour
{
    [SerializeField] private CameraJunkVisibilityController _cameraJunkVisibilityController;
    [SerializeField] private PlayerLevel _playerLevel;
    [SerializeField] private float _percentageNextLevelJunkOnFirstLayer;
    [SerializeField] private int _junkAmountPerSecond;
    [SerializeField][Min(0)] private float _secondsToSpawnJunkAmount;

    public int MaxJunkAmountOnStage { get; private set; }
    public int CollectedJunkAmountOnStage => GetCollectedJunkAmountOnStage();

    private PileJunkSpawner[] _pileJunkSpawners;

    private void OnValidate()
    {
        _percentageNextLevelJunkOnFirstLayer = Mathf.Clamp(_percentageNextLevelJunkOnFirstLayer, 0, 1);

        if (_junkAmountPerSecond < 0)
            _junkAmountPerSecond = 0;
    }

    private void Start()
    {
        _pileJunkSpawners = FindObjectsOfType<PileJunkSpawner>();

        foreach (PileJunkSpawner junkPileSpawner in _pileJunkSpawners)
        {
            junkPileSpawner.Init(_playerLevel, _percentageNextLevelJunkOnFirstLayer, new Vector2(_junkAmountPerSecond, _secondsToSpawnJunkAmount), _cameraJunkVisibilityController);
            MaxJunkAmountOnStage += junkPileSpawner.GetAllPointsAmount();
        }
    }

    public int GetCollectedJunkAmountOnStage()
    {
        int amount = 0;

        foreach (PileJunkSpawner pileJunkSpawner in _pileJunkSpawners)
            amount += pileJunkSpawner.GetFreePointsAmount();

        return amount;
    }
}
