using UnityEngine;
using UnityEngine.Events;

public class PlayerVenicleHolder : MonoBehaviour
{
    [SerializeField] private Venicle _currentVenicle;
    [SerializeField] private CharacterController _characterController;
    [Header("Testing")]
    [SerializeField] private Venicle _firstVenicle;
    [SerializeField] private Venicle _secondVenicle;

    public Venicle CurrentVenicle => _currentVenicle;

    public event UnityAction<Venicle> VenicleChanged;

    public void SwitchVenicle(Venicle switchTo)
    {
        _currentVenicle.Deactivate();
        _currentVenicle = switchTo;
        _characterController.center = _characterController.transform.worldToLocalMatrix.MultiplyPoint(_currentVenicle.CharacterControllerSpot.position);
        _currentVenicle.Activate();
        VenicleChanged?.Invoke(_currentVenicle);
    }

    [ContextMenu("Switch to first venicle")]
    private void SwitchToFirstVenicle()
    {
        SwitchVenicle(_firstVenicle);
    }

    [ContextMenu("Switch to second venicle")]
    private void SwitchToSecondVenicle()
    {
        SwitchVenicle(_secondVenicle);
    }
}
