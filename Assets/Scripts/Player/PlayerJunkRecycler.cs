using UnityEngine;

[RequireComponent(typeof(PlayerVenicleHolder))]
public class PlayerJunkRecycler : MonoBehaviour
{
    private TriggerEventsInvoker _trigger;
    private JunkTank _junkTank;

    private void Awake()
    {
        PlayerVenicleHolder playerVenicleHolder = GetComponent<PlayerVenicleHolder>();
        playerVenicleHolder.VenicleChanged += OnVenicleChanged;
        _trigger = playerVenicleHolder.CurrentVenicle.VenicleBodyTrigger;
        _junkTank = playerVenicleHolder.CurrentVenicle.JunkTank;
    }

    private void OnEnable()
    {
        _trigger.TriggerEntered += OnRecycleTriggerEnter;
    }

    private void OnDisable()
    {
        _trigger.TriggerEntered -= OnRecycleTriggerEnter;
    }

    private void OnRecycleTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Recycler>(out Recycler recycler) == false)
            return;

        Recycle(recycler);
    }

    private void Recycle(Recycler recycler)
    {
        if (recycler.CanAdd(_junkTank.JunkAmount) == false)
            return;

        recycler.AddJunk(_junkTank.JunkAmount);
        _junkTank.Dump();
    }

    private void OnVenicleChanged(Venicle newVenicle)
    {
        _trigger.TriggerEntered -= OnRecycleTriggerEnter;
        _trigger = newVenicle.VenicleBodyTrigger;
        _trigger.TriggerEntered += OnRecycleTriggerEnter;
        _junkTank = newVenicle.JunkTank;
    }
}
