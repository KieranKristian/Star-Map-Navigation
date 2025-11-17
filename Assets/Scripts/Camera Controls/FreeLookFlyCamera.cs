//SpeedTutor (2024), "GHOST FLYCAM Camera In Unity (New Input System Tutorial)",
//[online], YouTube, Available at: https://www.youtube.com/watch?v=_j7Rh27ccqk&t=630s,
//(Accessed: April 2025)

using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookFlyCamera : MonoBehaviour
{
    [Header("MovementSettings")]
    [SerializeField] float normalSpeed = 40f;
    [SerializeField] float boostedSpeed = 85f;
    [SerializeField] float verticalSpeed = 40f;

    [Header("Mouse Settings")]
    [SerializeField] float lookSensitivity = 0.5f;

    Vector2 moveInput;
    Vector2 lookInput;
    bool isBoosting;
    bool isLooking;
    Vector3 verticalMovement = Vector3.zero;
    float xRotation;
    float yRotation;

    private Inputs inputActions;

    private void Awake() {
        Vector3 initialRotation = transform.localEulerAngles;
        xRotation = initialRotation.x;
        yRotation = initialRotation.y;
    }

    private void OnEnable() {
        inputActions = new Inputs();
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        inputActions.Player.MoveUp.performed += OnMoveUp;
        inputActions.Player.MoveUp.canceled += OnMoveUp;

        inputActions.Player.Boost.performed += OnBoost;
        inputActions.Player.Boost.canceled += OnBoost;

        inputActions.Player.MoveDown.performed += OnMoveDown;
        inputActions.Player.MoveDown.canceled += OnMoveDown;

        inputActions.Player.RightClickHold.performed += OnRightClickHold;
        inputActions.Player.RightClickHold.canceled+= OnRightClickHold;
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context) {
        isBoosting = context.performed;
    }

    public void OnMoveUp(InputAction.CallbackContext context) {
        verticalMovement = context.performed ? Vector3.up : Vector3.zero;
    }

    public void OnMoveDown(InputAction.CallbackContext context) {
        verticalMovement = context.performed ? Vector3.down : Vector3.zero;
    }

    public void OnRightClickHold(InputAction.CallbackContext context) {
        isLooking = context.performed;
    }

    private void Update() {
        if (isLooking) {
            LookAround();
        }
        MoveCamera();
    }

    void MoveCamera() {
        float speed = isBoosting ? boostedSpeed : normalSpeed;

        Vector3 forwardMovement = Vector3.Normalize(transform.forward) * moveInput.y * speed * Time.deltaTime;
        Vector3 rightMovement = Vector3.Normalize(transform.right) * moveInput.x * speed * Time.deltaTime;

        transform.position += forwardMovement + rightMovement + (verticalMovement * verticalSpeed * Time.deltaTime);
    }

    void LookAround() {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
