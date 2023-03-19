using UnityEngine;
using UnityEngine.Events;

public static class MovementDirection 
{
    public static event UnityAction Updated;

    public static Vector3 VerticalInputDirection { get; private set; }
    public static Vector3 HorisontalInputDirection { get; private set; }

    public enum MovementDirectionStates
    {
        CameraView,
        WorldVectorBasis
    }

    private static MovementDirectionStates _currentMovementDirectionState = MovementDirectionStates.CameraView;

    public static void Init()
    {
        SwitchToState(_currentMovementDirectionState);
    }

    public static void SwitchToState(MovementDirectionStates newState)
    {
        _currentMovementDirectionState = newState;
        CalculateDirection();
        Updated?.Invoke();
    }

    private static void CalculateDirection()
    {
        if (_currentMovementDirectionState == MovementDirectionStates.CameraView)
        {
            CalculateCameraViewDirection();
            return;
        }

        if (_currentMovementDirectionState == MovementDirectionStates.WorldVectorBasis)
        {
            CalculateWorldVectorBasisDirection();
            return;
        }
    }

    private static void CalculateCameraViewDirection()
    {
        Vector3 forwardVectorCamera = Camera.main.transform.forward;
        forwardVectorCamera.y = 0;
        VerticalInputDirection = forwardVectorCamera.normalized;
        HorisontalInputDirection = (Quaternion.Euler(new Vector3(0, 90, 0)) * VerticalInputDirection).normalized;
    }

    private static void CalculateWorldVectorBasisDirection()
    {
        VerticalInputDirection = new Vector3(1, 0, 0);
        HorisontalInputDirection = new Vector3(0, 0, -1);
    }
}
