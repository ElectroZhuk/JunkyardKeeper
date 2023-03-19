using UnityEngine;

public class Venicle : MonoBehaviour
{
    [SerializeField] private TriggerEventsInvoker _venicleBodyTrigger;
    [SerializeField] private JunkContactor _junkContactor;
    [SerializeField] private JunkTank _junkTank;
    [SerializeField] private Transform _characterControllerSpot;
    [SerializeField] private Transform _cameraSpot;

    public TriggerEventsInvoker VenicleBodyTrigger => _venicleBodyTrigger;
    public JunkContactor JunkContactor => _junkContactor;
    public JunkTank JunkTank => _junkTank;
    public Transform CharacterControllerSpot => _characterControllerSpot;
    public Transform CameraSpot => _cameraSpot;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
