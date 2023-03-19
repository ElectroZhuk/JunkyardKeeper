using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLevelElementsSpawner : MonoBehaviour
{
    [SerializeField] private PlayerLevelElement _levelElementTemplate;
    [SerializeField] private PlayerLevel _playerLevel;

    private int _lastSpawnedLevel = 0;
    private int _targetJunkPointsLayerIndex = 0;
    private PileJunkSpawner[] _allJunkSpawners;

    private void OnEnable()
    {
        _playerLevel.LevelChanged += OnPlayerLevelChanged;
    }

    private void Start()
    {
        _allJunkSpawners = FindObjectsOfType<PileJunkSpawner>();
        PileJunkSpawner[] notMatchingSpawners = _allJunkSpawners.Where(junkSpawner => junkSpawner.JunkPointsLayers.Count <= _targetJunkPointsLayerIndex + 1).Select(junkSpawner => junkSpawner).ToArray();
        
        foreach (PileJunkSpawner pileJunkSpawner in notMatchingSpawners)
            Debug.Log(pileJunkSpawner.JunkPointsLayers.Count);

        _allJunkSpawners = _allJunkSpawners.Except(notMatchingSpawners).ToArray();
        SpawnLevelElements(_playerLevel.Level);
    }

    private void OnDisable()
    {
        _playerLevel.LevelChanged -= OnPlayerLevelChanged;
    }

    private void OnPlayerLevelChanged(int currentLevel)
    {
        if (currentLevel <= StageSettings.LevelElementsToLevelUpForLevel.Count)
            SpawnLevelElements(currentLevel);
    }

    private void SpawnLevelElements(int currentLevel)
    {
        if (currentLevel <= 0)
        {
            Debug.LogError("Current level can't be zero or less!");
            return;
        }

        if (_lastSpawnedLevel >= currentLevel)
        {
            Debug.LogError("You already spawned level elements for this player level!");
            return;
        }

        int elementsToSpawn = StageSettings.LevelElementsToLevelUpForLevel[currentLevel - 1];
        List<PileJunkSpawner> takenJunkSpawners = new List<PileJunkSpawner>();

        for (int i = 0; i < elementsToSpawn; i++)
        {
            PileJunkSpawner[] targetJunkSpawners = _allJunkSpawners.Except(takenJunkSpawners).ToArray();

            if (targetJunkSpawners.Length == 0)
            {
                takenJunkSpawners.Clear();
                _allJunkSpawners.CopyTo(targetJunkSpawners, 0);
            }

            int randomIndex = Random.Range(0, targetJunkSpawners.Length - 1);
            PileJunkSpawner randomSpawner = targetJunkSpawners[randomIndex];
            randomIndex = Random.Range(0, randomSpawner.JunkPointsLayers[_targetJunkPointsLayerIndex].Count - 1);
            JunkPoint randomJunkPoint = randomSpawner.JunkPointsLayers[_targetJunkPointsLayerIndex][randomIndex];

            while (randomSpawner.AreAnyPointsOnTop(randomJunkPoint) == false)
            {
                randomIndex = Random.Range(0, randomSpawner.JunkPointsLayers[_targetJunkPointsLayerIndex].Count - 1);
                randomJunkPoint = randomSpawner.JunkPointsLayers[_targetJunkPointsLayerIndex][randomIndex];
            }

            PlayerLevelElement spawnedPlayerLevelElement = Instantiate(_levelElementTemplate, randomJunkPoint.transform);
            spawnedPlayerLevelElement.transform.position = randomJunkPoint.transform.position;
            takenJunkSpawners.Add(randomSpawner);
        }
    }
}
