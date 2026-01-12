using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    EnemyAnimatorManager animatorManager;

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animatorManager = GetComponent<EnemyAnimatorManager>();

        // Nájdenie slotov na kostiach
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot slot in weaponHolderSlots)
        {
            if (slot.isLeftHandSlot) leftHandSlot = slot;
            else if (slot.isRightHandSlot) rightHandSlot = slot;
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            // Logika pre ľavú ruku...
        }
        else
        {
            // 1. Spawn modelu
            rightHandSlot.LoadWeaponModel(weaponItem);

            // 2. Nájdenie collidera a nastavenie damage
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.currentDamage = weaponItem.physicalDamage;
                rightHandDamageCollider.targetTag = "Player"; // Nastavenie cieľového tagu na "Player"
            }

            // 3. Nastavenie Animácií (Stance/Idle)
            if (weaponItem != null)
            {
                animatorManager.SetWeaponType(weaponItem.holdTypeID);
            }
        }
    }

    public void OpenRightDamageCollider()
    {
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.DisableDamageCollider();
    }
}