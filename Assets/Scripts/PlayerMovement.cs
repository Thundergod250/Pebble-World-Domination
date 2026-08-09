using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Grab actions directly from the Player map
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];

        jumpAction.performed += OnJump;
    }

    private void OnDestroy() => jumpAction.performed -= OnJump;

    private void Update()
    {
        // Read movement input (WASD → Vector2)
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        controller.Move(move * (moveSpeed * Time.deltaTime));

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}