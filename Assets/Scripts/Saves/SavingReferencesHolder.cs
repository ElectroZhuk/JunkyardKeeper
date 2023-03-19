using UnityEngine;

public class SavingReferencesHolder : MonoBehaviour
{
    [Header("Saving data")]
    [SerializeField] private PlayerWallet _playerWallet;
    [SerializeField] private Shop _shop;
    [SerializeField] private StageGoals _stageGoals;
    [SerializeField] private Venicle[] _venicles;
}
