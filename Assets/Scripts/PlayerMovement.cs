using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float runningMultiplier = 1.5f; // public multiplier for running
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float lookSensitivity = 2f;

    [SerializeField] private Transform cameraTransform; // assign your Camera in Inspector

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction runAction; // new Run action

    private Vector3 velocity;
    private float xRotation = 0f; // pitch

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        lookAction = playerInput.actions["Look"];
        runAction = playerInput.actions["Run"]; // Shift binding

        jumpAction.performed += OnJump;
    }

    private void OnDestroy() => jumpAction.performed -= OnJump;

    private void Update()
    {
        // Movement (WASD)
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        // Speed calculation
        float currentSpeed = baseSpeed;

        // Running Speed
        if (runAction != null && runAction.ReadValue<float>() > 0)
            currentSpeed += baseSpeed * runningMultiplier;

        // Other Bonus Speed (future content)
        float otherBonusSpeed = 0f;
        currentSpeed += otherBonusSpeed;

        controller.Move(move * (currentSpeed * Time.deltaTime));

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Look (Mouse Delta → Vector2)
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * (lookSensitivity * Time.deltaTime);

        // Horizontal rotation (yaw)
        transform.Rotate(Vector3.up * lookInput.x);

        // Vertical rotation (pitch)
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // prevent flipping
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
