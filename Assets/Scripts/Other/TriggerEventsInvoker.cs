using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEventsInvoker : MonoBehaviour
{
    public event UnityAction<Collider> TriggerEntered;
    public event UnityAction<Collider> TriggerStaying;
    public event UnityAction<Collider> TriggerExited;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_collider.isTrigger == false)
        {
            Debug.Log($"{name}'s collider is not a trigger!");
            _collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerEntered?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerStaying?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExited?.Invoke(other);
    }
}
