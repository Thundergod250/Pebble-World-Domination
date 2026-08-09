using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "Default Item";

    // Called when player interacts
    public void Interact()
    {
        Debug.Log($"Interacted with {itemName}");
        Destroy(gameObject); // remove item from scene
    }
}