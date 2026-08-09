using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactLayer; // assign layer for items

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

    private void OnDestroy()
    {
        interactAction.performed -= OnInteract;
    }

    private void Update()
    {
        // Raycast forward to detect item
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<Item>(out Item item))
            {
                // Show item name (for now just Debug.Log, later UI)
                Debug.Log($"Looking at: {item.itemName}");
            }
        }

    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                item.Interact();
            }
        }
    }
}