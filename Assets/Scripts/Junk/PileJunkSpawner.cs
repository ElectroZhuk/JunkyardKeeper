using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class PileJunkSpawner : MonoBehaviour
{
    public IReadOnlyList<IReadOnlyList<JunkPoint>> JunkPointsLayers => _allPointsLayers;

    private PlayerLevel _playerLevel;
    private float _percentageNextLevelJunkOnFirstLayer;
    private Vector2 _junkSpawnPerSeconds;
    private CameraJunkVisibilityController _cameraJunkVisibilityController;
    private List<List<JunkPoint>> _allPointsLayers = new List<List<JunkPoint>>();
    private List<JunkPoint> _pointsForNextLevelJunk = new List<JunkPoint>();
    private List<Junk> _availableJunk = new List<Junk>();
    private int _layerToSpawnNextLevelJunk = 0;
    private IEnumerator _spawningCoroutine;

    private void Awake()
    {
        JunkPointsLayer[] junkPointsLayers = GetComponentsInChildren<JunkPointsLayer>();

        for (int i = 0; i < junkPointsLayers.Length; i++)
        {
            _allPointsLayers.Add(new List<JunkPoint>());
            List<JunkPoint> layerPoints = junkPointsLayers[i].GetComponentsInChildren<JunkPoint>().ToList();
            layerPoints.ForEach(point => _allPointsLayers[i].Add(point));
        }
    }

    public void Init(PlayerLevel playerLevel, float percentageNextLevelJunkOnFirstLayer, Vector2 junkSpawnPerSeconds, CameraJunkVisibilityController cameraJunkVisibilityController)
    {
        if (playerLevel == null)
        {
            Debug.LogError("Player level component can't be null!");
            return;
        }

        if (percentageNextLevelJunkOnFirstLayer < 0 || 1 < percentageNextLevelJunkOnFirstLayer)
        {
            Debug.LogError($"Percentage of next level junk on firsrt layer must be more then zero and less or equal then one!");
            return;
        }

        if (junkSpawnPerSeconds.x <= 0 || junkSpawnPerSeconds.y <= 0)
        {
            Debug.LogError($"Junk spawn per seconds can't be zero or less!");
            return;
        }

        _playerLevel = playerLevel;
        _percentageNextLevelJunkOnFirstLayer = percentageNextLevelJunkOnFirstLayer;
        _junkSpawnPerSeconds = junkSpawnPerSeconds;
        _cameraJunkVisibilityController = cameraJunkVisibilityController;

        foreach (Junk junkTemplate in StageSettings.JunkConfigurationForLevel[_playerLevel.Level - 1].Models)
            _availableJunk.Add(junkTemplate);

        InitScheduleJunkPoints();
        ForceSheduleNextLevelJunk();

        foreach (List<JunkPoint> layer in GetScheduledJunkPointsLayers())
            foreach (JunkPoint junkPoint in layer)
                SpawnScheduledJunk(junkPoint);

        _playerLevel.LevelChanged += OnPlayerLevelChanged;
        _spawningCoroutine = RequestSpawnJunk();
        StartCoroutine(_spawningCoroutine);
    }

    public int GetAllPointsAmount()
    {
        int amount = 0;

        foreach (List<JunkPoint> layer in _allPointsLayers)
            amount += layer.Count;

        return amount;
    }

    public int GetFreePointsAmount()
    {
        int amount = 0;

        foreach (List<JunkPoint> layer in _allPointsLayers)
            foreach (JunkPoint junkPoint in layer)
                if (junkPoint.IsFilled == false)
                    amount++;

        return amount;
    }

    public bool AreAnyPointsOnTop(JunkPoint pointToCheck)
    {
        int checkingPointIndex = 0;
        int checkingPointLayerIndex = 0;

        for (int i = 0; i < _allPointsLayers.Count; i++)
        {
            foreach (JunkPoint junkPoint in _allPointsLayers[i])
            {
                if (junkPoint == pointToCheck)
                {
                    checkingPointLayerIndex = i;
                    checkingPointIndex = _allPointsLayers[i].IndexOf(junkPoint);
                }
            }
        }

        if (checkingPointLayerIndex == _allPointsLayers.Count - 1)
            return false;

        return _allPointsLayers[checkingPointLayerIndex + 1].Count > checkingPointIndex;
    }

    private IEnumerator RequestSpawnJunk()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_junkSpawnPerSeconds.y);

        while (true)
        {
            int targetLayer = 0;
            List<List<JunkPoint>> scheduledPointsLayers = GetScheduledJunkPointsLayers();

            for (int i = scheduledPointsLayers.Count - 1; i >= 0; i--)
            {
                if (scheduledPointsLayers[i].Count > 0)
                    targetLayer = i;
            }

            List<JunkPoint> notVisibleJunk = _cameraJunkVisibilityController.GetJunkPointsWithNotVisibleJunk(scheduledPointsLayers[targetLayer]);

            for (int i = 0; i < _junkSpawnPerSeconds.x; i++)
            {
                if (notVisibleJunk.Count > 0)
                {
                    int randomIndex = Random.Range(0, notVisibleJunk.Count);
                    JunkPoint targetJunkPoint = notVisibleJunk[randomIndex];
                    SpawnScheduledJunk(targetJunkPoint);
                    notVisibleJunk.RemoveAt(randomIndex);
                }
            }

            yield return waitForSeconds;
        }
    }

    private void SpawnScheduledJunk(JunkPoint junkPoint)
    {
        if (junkPoint.IsScheduled == false)
        {
            Debug.LogError("Point is't scheduled!");
            return;
        }

        if (junkPoint.IsFilled)
        {
            Debug.LogError("Point is filled!");
            return;
        }

        junkPoint.SpawnScheduledJunk();
        junkPoint.Released += OnJunkPointReleased;
    }

    private void InitScheduleJunkPoints()
    {
        PlayerLevelJunkConfiguration configurationForLevel = StageSettings.JunkConfigurationForLevel[_playerLevel.Level - 1];

        for (int i = 0; i < _allPointsLayers.Count; i++)
        {
            foreach (JunkPoint junkPoint in _allPointsLayers[i])
            {
                int randomIndex = Random.Range(0, _availableJunk.Count);
                Junk toSpawn = _availableJunk[randomIndex];
                Junk spawnedJunk = junkPoint.ScheduleSpawn(toSpawn);
                spawnedJunk.Init(_playerLevel.Level, configurationForLevel.AmountInUnit, _playerLevel);
            }
        }
    }

    private void ForceSheduleNextLevelJunk()
    {
        if (_playerLevel.Level >= StageSettings.TargetPlayerLevel)
        {
            Debug.LogError("Player's level is already maximum");
            return;
        }

        List<JunkPoint> targetLayer = _allPointsLayers[_layerToSpawnNextLevelJunk];
        int junkToSpawnAmount = Mathf.RoundToInt(targetLayer.Count * _percentageNextLevelJunkOnFirstLayer);
        List<JunkPoint> alreadyScheduled = new List<JunkPoint>();

        for (int i = 0; i < junkToSpawnAmount; i++)
        {
            int randomPointIndex = Random.Range(0, targetLayer.Count - 1);
            JunkPoint targetPoint = targetLayer[randomPointIndex];

            while (alreadyScheduled.Contains(targetPoint))
            {
                randomPointIndex = Random.Range(0, targetLayer.Count - 1);
                targetPoint = targetLayer[randomPointIndex];
            }

            if (targetPoint.IsFilled)
            {
                _pointsForNextLevelJunk.Add(targetPoint);
                alreadyScheduled.Add(targetPoint);

                continue;
            }

            if (targetPoint.IsScheduled)
                targetPoint.RemoveScheduleSpawn();

            PlayerLevelJunkConfiguration configurationForLevel = StageSettings.JunkConfigurationForLevel[_playerLevel.Level];
            IReadOnlyList<Junk> levelModels = configurationForLevel.Models;
            int randomJunkIndex = Random.Range(0, levelModels.Count);
            Junk toSchedule = levelModels[randomJunkIndex];
            Junk spawnedJunk = targetPoint.ScheduleSpawn(toSchedule);
            spawnedJunk.Init(_playerLevel.Level + 1, configurationForLevel.AmountInUnit, _playerLevel);
            alreadyScheduled.Add(targetPoint);
        }
    }

    private int GetPointLayer(JunkPoint junkPoint)
    {
        int layerIndex = 0;

        for (int i = 0; i < _allPointsLayers.Count; i++)
        {
            foreach (JunkPoint checkingJunkPoint in _allPointsLayers[i])
            {
                if (checkingJunkPoint == junkPoint)
                    layerIndex = i;
            }
        }

        return layerIndex;
    }

    private void OnJunkPointReleased(JunkPoint releasedJunkPoint)
    {
        releasedJunkPoint.Released -= OnJunkPointReleased;

        if (_pointsForNextLevelJunk.Contains(releasedJunkPoint))
        {
            ScheduleNextLevelJunkPoint(releasedJunkPoint);
            _pointsForNextLevelJunk.Remove(releasedJunkPoint);

            return;
        }

        SchedulePoint(releasedJunkPoint);
    }

    private void SchedulePoint(JunkPoint junkPoint)
    {
        if (junkPoint.IsScheduled)
        {
            Debug.LogError("Junk point is already scheduled!");
            return;
        }


        int randomIndex = Random.Range(0, _availableJunk.Count);
        Junk junkToSpawn = _availableJunk[randomIndex];
        Junk spawnedJunk = junkPoint.ScheduleSpawn(junkToSpawn);
        spawnedJunk.Init(_playerLevel.Level, StageSettings.JunkConfigurationForLevel[_playerLevel.Level - 1].AmountInUnit, _playerLevel);
    }

    private void ScheduleNextLevelJunkPoint(JunkPoint junkPoint)
    {
        if (junkPoint.IsScheduled)
        {
            Debug.LogError("Junk point is already scheduled!");
            return;
        }

        PlayerLevelJunkConfiguration configurationForLevel = StageSettings.JunkConfigurationForLevel[_playerLevel.Level];
        IReadOnlyList<Junk> levelModels = configurationForLevel.Models;
        int randomIndex = Random.Range(0, levelModels.Count);
        Junk junkToSpawn = levelModels[randomIndex];
        Junk spawnedJunk = junkPoint.ScheduleSpawn(junkToSpawn);
        spawnedJunk.Init(_playerLevel.Level + 1, configurationForLevel.AmountInUnit, _playerLevel);
    }

    private void OnPlayerLevelChanged(int currentLevel)
    {
        if (currentLevel > StageSettings.TargetPlayerLevel)
        {
            Debug.LogError("Current player level greater then max level!");
            return;
        }

        if (currentLevel == StageSettings.TargetPlayerLevel)
        {
            StopCoroutine(_spawningCoroutine);

            foreach (List<JunkPoint> layer in _allPointsLayers)
            {
                foreach (JunkPoint junkPoint in layer)
                {
                    junkPoint.Released -= OnJunkPointReleased;
                }
            }

            return;
        }

        foreach (Junk junkTemplate in StageSettings.JunkConfigurationForLevel[_playerLevel.Level - 1].Models)
                _availableJunk.Add(junkTemplate);

        foreach (List<JunkPoint> layer in GetScheduledJunkPointsLayers())
        {
            foreach (JunkPoint junkPoint in layer)
            {
                if (junkPoint.IsScheduled)
                    junkPoint.RemoveScheduleSpawn();
                
                SchedulePoint(junkPoint);
            }
        }

        ForceSheduleNextLevelJunk();
    }

    private List<List<JunkPoint>> GetFilledJunkPointsLayers()
    {
        List<List<JunkPoint>> filledJunkPointsLayers = new List<List<JunkPoint>>();

        foreach (List<JunkPoint> checkingLayer in _allPointsLayers)
        {
            List<JunkPoint> currentLayer = new List<JunkPoint>();

            foreach (JunkPoint junkPoint in checkingLayer)
            {
                if (junkPoint.IsFilled)
                    currentLayer.Add(junkPoint);
            }

            filledJunkPointsLayers.Add(currentLayer);
        }

        return filledJunkPointsLayers;
    }

    private List<List<JunkPoint>> GetScheduledJunkPointsLayers()
    {
        List<List<JunkPoint>> scheduledJunkPointsLayers = new List<List<JunkPoint>>();

        foreach (List<JunkPoint> checkingLayer in _allPointsLayers)
        {
            List<JunkPoint> currentLayer = new List<JunkPoint>();

            foreach (JunkPoint junkPoint in checkingLayer)
            {
                if (junkPoint.IsScheduled)
                    currentLayer.Add(junkPoint);
            }

            scheduledJunkPointsLayers.Add(currentLayer);
        }

        return scheduledJunkPointsLayers;
    }
}
