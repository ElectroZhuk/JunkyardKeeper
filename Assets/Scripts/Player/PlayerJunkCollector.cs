using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerLevel), typeof(PlayerVenicleHolder))]
public class PlayerJunkCollector : MonoBehaviour
{
    public JunkContactor JunkContact => _junkContact;

    public event UnityAction JunkCollected;
    public event UnityAction<float> JunkContainerChanged;

    private JunkContactor _junkContact;
    private JunkTank _junkTank;
    private PlayerLevel _playerLevel;

    private void Awake()
    {
        _playerLevel = GetComponent<PlayerLevel>();
        PlayerVenicleHolder playerVenicleHolder = GetComponent<PlayerVenicleHolder>();
        playerVenicleHolder.VenicleChanged += OnVenicleChanged;
        _junkContact = playerVenicleHolder.CurrentVenicle.JunkContactor;
        _junkTank = playerVenicleHolder.CurrentVenicle.JunkTank;
    }

    private void OnEnable()
    {
        _junkContact.JunkContacted += OnJunkContacted;
    }

    private void OnDisable()
    {
        _junkContact.JunkContacted -= OnJunkContacted;
    }

    private void OnJunkContacted(Junk junk)
    {
        if (_playerLevel.Level < junk.Level)
        {
            junk.NotCollected();
            return;
        }

        if (_junkTank.IsFilled)
        {
            junk.NotCollected();
            return;
        }

        _junkTank.AddJunk(junk);
        JunkCollected?.Invoke();
    }

    private void OnVenicleChanged(Venicle newVenicle)
    {
        _junkContact.JunkContacted -= OnJunkContacted;
        _junkContact = newVenicle.JunkContactor;
        _junkContact.JunkContacted += OnJunkContacted;
        _junkTank = newVenicle.JunkTank;
    }
}
