using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementRigidbody : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseSpeed = 5f;
    public float runningMultiplier = 1.5f;
    public float jumpHeight = 2f;
    public float gravityMultiplier = 2f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform; // assign Main Camera (with Cinemachine Brain)

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // prevent physics spin

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Run"];

        jumpAction.performed += OnJump;
    }

    private void OnDestroy() => jumpAction.performed -= OnJump;

    private void FixedUpdate()
    {
        // --- Ground check ---
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // --- Movement relative to camera ---
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 move = camForward * input.y + camRight * input.x;

        float currentSpeed = baseSpeed;
        if (runAction != null && runAction.ReadValue<float>() > 0)
            currentSpeed *= runningMultiplier;

        Vector3 targetVelocity = move * currentSpeed;
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;
        velocityChange.y = 0; // don’t affect vertical
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // --- Character rotation ---
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }

        // --- Extra gravity ---
        if (!isGrounded)
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }
    }
}
