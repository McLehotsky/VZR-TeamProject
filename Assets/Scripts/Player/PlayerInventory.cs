using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;
    PlayerManager playerManager;

    [Header("Inventory Lists")]
    // List of weapons found/picked up by the player
    public List<WeaponItem> weaponsInRightHandSlots = new List<WeaponItem>();

    [Header("Current Indices")]
    public int currentRightWeaponIndex = 0;

    // Default weapon (Unarmed) to fallback if list is empty
    public WeaponItem unarmedWeapon;

    private void Awake()
    {
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        // Load the first weapon in the list at start, or unarmed if empty
        if (weaponsInRightHandSlots.Count == 0)
        {
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            playerManager.currentWeapon = unarmedWeapon;
        }
        else
        {
            // Load the weapon at the current index (usually 0 at start)
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            playerManager.currentWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        }
    }

    public void ChangeRightWeapon()
    {
        // 1. If we have no weapons, do nothing (or load unarmed)
        if (weaponsInRightHandSlots.Count == 0)
            return;

        // 2. Increment index
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        // 3. Reset index if we go out of bounds (Loop back to start)
        if (currentRightWeaponIndex > weaponsInRightHandSlots.Count - 1)
        {
            currentRightWeaponIndex = 0;
            // Note: In real Souls games, index -1 often means "Unarmed". 
            // For simplicity, we just loop the list.
        }

        // 4. Get the weapon from the list
        WeaponItem selectedWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];

        // 5. Load it visually via Slot Manager
        weaponSlotManager.LoadWeaponOnSlot(selectedWeapon, false);

        // 6. Update PlayerManager logic reference
        playerManager.currentWeapon = selectedWeapon;

        // Debug
        Debug.Log("Swapped to: " + selectedWeapon.itemName);
    }
}