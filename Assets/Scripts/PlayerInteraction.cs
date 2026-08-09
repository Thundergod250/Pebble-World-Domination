using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactLayer; // assign layer for items
    [SerializeField] private UI_InteractionUI interactionUI; // link your UI script in Inspector

    private PlayerInput playerInput;
    private InputAction interactAction;
    private Camera cam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interaction"]; // E binding
        cam = Camera.main;

        interactAction.performed += OnInteract;
    }

    private void OnDestroy() => interactAction.performed -= OnInteract;

    private void Update()
    {
        // Raycast forward to detect item
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<Item>(out Item item)) 
                interactionUI.ShowText($"Press E to interact with {item.itemName}");
        }
        else
            interactionUI.HideText();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent(out Item item))
            {
                item.Interact();
                interactionUI.HideText();
            }
        }
    }

    // Draw the ray in Scene view
    private void OnDrawGizmos()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Gizmos.color = Color.green;
        Vector3 start = cam.transform.position;
        Vector3 end = start + cam.transform.forward * interactRange;
        Gizmos.DrawLine(start, end);
    }
}
