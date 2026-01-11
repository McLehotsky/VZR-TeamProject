using UnityEngine;

public class WeaponPickup : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        // 1. Get Inventory
        PlayerInventory inventory = playerManager.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            // 2. Add weapon logic
            inventory.weaponsInRightHandSlots.Add(weapon);

            // Notification (optional)
            Debug.Log("Picked up: " + weapon.itemName);

            // 3. Destroy object + Hide UI
            // Important: We must tell the player to clear the interaction reference first!
            playerManager.ClearInteractableObject();

            Destroy(gameObject);
        }
    }
}