using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    PlayerAnimatorManager animatorManager; // Aby sme hned nastavili animacie

    private void Awake()
    {
        animatorManager = GetComponent<PlayerAnimatorManager>();

        // Najdeme sloty v detoch (na kostiach)
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

            // Posleme ID zbrane do Animatora, aby vedel aku "Combat Idle" animaciu ma hrat
            // Sending holdId from weapon to determine what hold layer to use
            if (weaponItem != null)
            {
                animatorManager.SetWeaponType(weaponItem.holdTypeID);
            }
        }
    }
}