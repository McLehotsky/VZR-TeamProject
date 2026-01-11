using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    PlayerAnimatorManager animatorManager;

    private void Awake()
    {
        animatorManager = GetComponent<PlayerAnimatorManager>();

        // Get weapon holder slots
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot slot in weaponHolderSlots)
        {
            if (slot.isLeftHandSlot)
                leftHandSlot = slot;
            else if (slot.isRightHandSlot)
                rightHandSlot = slot;
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            // TODO logic for left hand (shield, etc.)
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);

            // Sending holdId from weapon to determine what hold layer to use
            if (weaponItem != null)
            {
                animatorManager.SetWeaponType(weaponItem.holdTypeID);
            }
        }
    }
}