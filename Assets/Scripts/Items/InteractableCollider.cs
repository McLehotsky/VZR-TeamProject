using UnityEngine;

public class InteractableCollider : MonoBehaviour
{
    Interactable interactable; // Reference to the Interactable script on the same GameObject

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                // Object is interactable
                player.SetInteractableObject(interactable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                // Clear the interactable object when player exits the trigger
                player.ClearInteractableObject();
            }
        }
    }
}