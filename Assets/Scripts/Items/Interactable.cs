using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactText = "Pick Up";

    // Virtual method to be overridden in child classes (WeaponPickup, Chest...)
    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("Interacted with base object");
    }
}