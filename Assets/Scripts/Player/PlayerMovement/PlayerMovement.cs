using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _character;
    [Header("Configuration")]
    [SerializeField] [Min(0.01f)] private float _movingSpeed;
    [SerializeField] private Vector3 _gravityForce;

    private PlayerInput _input;
    private Vector3 _verticalInputDirection;
    private Vector3 _horisontalInputDirection ;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
        MovementDirection.Updated += UpdateMovementDirection;
    }

    private void Start()
    {
        MovementDirection.Init();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }

    private void OnDisable()
    {
        MovementDirection.Updated -= UpdateMovementDirection;
        _input.Disable();
    }

    private void Move()
    {
        Vector2 inputVector = _input.Player.Move.ReadValue<Vector2>();
        Vector3 verticalMotion = inputVector.y * _movingSpeed * _verticalInputDirection;
        Vector3 horisontalMotion = inputVector.x * _movingSpeed * _horisontalInputDirection;
        Vector3 resultMotion = (verticalMotion + horisontalMotion) * Time.fixedDeltaTime;

        _character.Move(resultMotion);
    }

    private void ApplyGravity()
    {
        _character.Move(_gravityForce * Time.fixedDeltaTime);
    }

    private void UpdateMovementDirection()
    {
        _verticalInputDirection = MovementDirection.VerticalInputDirection;
        _horisontalInputDirection = MovementDirection.HorisontalInputDirection;
    }
}
