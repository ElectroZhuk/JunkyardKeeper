using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerVenicleHolder))]
public class PlayerVenicleDetailCollector : MonoBehaviour
{
    private TriggerEventsInvoker _trigger;
    private int _detailsAmount = 0;

    public event UnityAction<int> DetailCollected;

    private void Awake()
    {
        PlayerVenicleHolder playerVenicleHolder = GetComponent<PlayerVenicleHolder>();
        playerVenicleHolder.VenicleChanged += OnVenicleChanged;
        _trigger = playerVenicleHolder.CurrentVenicle.VenicleBodyTrigger;
    }

    private void OnEnable()
    {
        _trigger.TriggerEntered += OnTriggerEntered;
    }

    private void OnDisable()
    {
        _trigger.TriggerEntered -= OnTriggerEntered;
    }

    private void OnTriggerEntered(Collider collider)
    {
        if (collider.TryGetComponent<VenicleDetail>(out VenicleDetail venicleDetail) == false)
            return;

        venicleDetail.Collect();
        _detailsAmount++;
        DetailCollected?.Invoke(_detailsAmount);
    }

    private void OnVenicleChanged(Venicle newVenicle)
    {
        _trigger.TriggerEntered -= OnTriggerEntered;
        _trigger = newVenicle.VenicleBodyTrigger;
        _trigger.TriggerEntered += OnTriggerEntered;
    }
}
