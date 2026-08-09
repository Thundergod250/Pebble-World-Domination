using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction jumpAction;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Grab the Jump action directly from the Player map
        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += OnJump;
    }

    private void OnDestroy() => jumpAction.performed -= OnJump;

    private void Update()
    {
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