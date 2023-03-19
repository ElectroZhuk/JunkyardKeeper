using UnityEngine;

public class CollectAllVenicleDetailsGoal : Goal
{
    [SerializeField] private PlayerVenicleDetailCollector _playerVenicleDetailCollector;

    private int _allDetailsAmount;

    private void Awake()
    {
        _allDetailsAmount = FindObjectsOfType<VenicleDetail>().Length;
    }

    private void OnEnable()
    {
        _playerVenicleDetailCollector.DetailCollected += OnDetailCollected;
    }

    private void Start()
    {
        if (_allDetailsAmount == 0)
            Achieve();
    }

    private void OnDisable()
    {
        _playerVenicleDetailCollector.DetailCollected -= OnDetailCollected;
    }

    private void OnDetailCollected(int detailsAmount)
    {
        if (detailsAmount == _allDetailsAmount)
            Achieve();
    }
}
