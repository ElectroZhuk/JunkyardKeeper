using System.Collections;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    [Header("Recycling settings")]
    [SerializeField] [Min(1)] private int _recyclingStepAmount;
    [SerializeField] [Min(0.01f)] private float _timeForRecycle;
    [Header("Money spawning settings")]
    [SerializeField] [Min(0.01f)] private float _waitingTimeForNextMoneyPortion;
    [SerializeField] [Min(1)] private int _minMoneyAmountInUnit;
    [SerializeField] [Min(1)] private int _maxMoneyAmountInUnit;
    [SerializeField] private Money[] _moneyPrefabsProportionalToAmount;
    [Header("Other")]
    [SerializeField] [Min(1)] private int _junkPrice;
    [SerializeField] private Money _moneyPrefab;
    [SerializeField] private Transform _moneyContainer;

    private int _junkQueue;
    private int _moneyToSpawn;

    public bool CanAdd(int junkAmount)
    {
        return junkAmount > 0;
    }

    public void AddJunk(int junkAmount)
    {
        if (CanAdd(junkAmount) == false)
        {
            Debug.LogError($"Added wrong junk amount: {junkAmount}");
            return;
        }

        _junkQueue += junkAmount;

        if (_junkQueue == junkAmount)
            StartCoroutine(StartRecycle());
    }

    private IEnumerator StartRecycle()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_timeForRecycle);

        while (_junkQueue > 0)
        {
            yield return waitingTime;

            int stepRecycledJunk = Mathf.Clamp(_recyclingStepAmount, 0, _junkQueue);
            _junkQueue -= stepRecycledJunk;
            _moneyToSpawn += stepRecycledJunk * _junkPrice;

            if (_moneyToSpawn == stepRecycledJunk * _junkPrice)
                StartCoroutine(StartMoneySpawn());
        }
    }

    private IEnumerator StartMoneySpawn()
    {
        while (_moneyToSpawn > 0)
        {
            float elapseTime = 0;

            while (_moneyToSpawn < _maxMoneyAmountInUnit && elapseTime < _waitingTimeForNextMoneyPortion)
            {
                int moneyBefore = _moneyToSpawn;

                yield return null;

                elapseTime += Time.deltaTime;

                if (moneyBefore != _moneyToSpawn)
                    elapseTime = 0;
            }

            int moneyAmountToSpawn = Mathf.Clamp(_moneyToSpawn, _minMoneyAmountInUnit, _maxMoneyAmountInUnit);
            float prefabToSpawnStepSize = (float)System.Math.Round((double)1 / _moneyPrefabsProportionalToAmount.Length, 2);
            float moneyAmountToSpawnRelativelyToMax = (float)System.Math.Round((double)moneyAmountToSpawn / _maxMoneyAmountInUnit, 2);
            Money prefabToSpawn = _moneyPrefabsProportionalToAmount[Mathf.Clamp(Mathf.RoundToInt(moneyAmountToSpawnRelativelyToMax / prefabToSpawnStepSize), 0, _moneyPrefabsProportionalToAmount.Length - 1)];
            Money spawnedMoney = Instantiate(prefabToSpawn, _moneyContainer);
            spawnedMoney.Init(moneyAmountToSpawn);
            spawnedMoney.transform.position = _moneyContainer.transform.position;
            _moneyToSpawn -= moneyAmountToSpawn;

            yield return null;
        }
    }

    private void GenerateMoney()
    {
        Money money = Instantiate(_moneyPrefab, _moneyContainer);
        money.Init(_junkPrice);
        money.transform.localPosition = Vector3.zero;
    }
}
