using System;
using UnityEngine;

public class ItemPickup : Interactable
{
    public WeaponItem weapon;
    public Item keyItem; // Optional: If the weapon is also a key item

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        // 1. Get Inventory
        PlayerInventory inventory = playerManager.GetComponent<PlayerInventory>();
        UIManager uiManager = playerManager.GetComponent<UIManager>();

        if (inventory != null)
        {
            if (weapon != null)
            {

                // 2. Add weapon logic
                inventory.weaponsInRightHandSlots.Add(weapon);

                // Notification (optional)
                if (uiManager != null)
                    uiManager.DisplayItemNotification(weapon.itemName);
                else
                    Debug.Log("Picked up: " + weapon.itemName);

                // 3. Destroy object + Hide UI
                // Important: We must tell the player to clear the interaction reference first!
                playerManager.ClearInteractableObject();

                Destroy(gameObject);
            }
            else if (keyItem != null)
            {
                // 2. Add key item logic
                inventory.keyItems.Add(keyItem);

                inventory.UpdateEmberCount();

                // Notification (optional)
                if (uiManager != null)
                    uiManager.DisplayItemNotification(keyItem.itemName);
                else
                    Debug.Log("Picked up key item: " + keyItem.itemName);

                // 3. Destroy object + Hide UI
                // Important: We must tell the player to clear the interaction reference first!
                playerManager.ClearInteractableObject();

                Destroy(gameObject);
            }
        }
    }

}