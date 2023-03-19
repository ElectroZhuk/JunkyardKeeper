using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerWallet _wallet;
    [SerializeField] private PlayerLevel _level;
    [SerializeField] private PlayerJunkCollector _junkCollector;

    public PlayerWallet Wallet => _wallet;
    public PlayerLevel Level => _level;
    public PlayerJunkCollector JunkCollector => _junkCollector;
}
