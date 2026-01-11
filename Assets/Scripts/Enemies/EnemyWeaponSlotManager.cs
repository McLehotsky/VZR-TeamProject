using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

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
                // Toto je dôležité! Voláme priamo Animator, lebo EnemyAnimatorManager 
                // zatiaľ nemá funkciu SetWeaponType, ale môžeme ju tam pridať, 
                // alebo to nastaviť priamo tu:
                animator.SetInteger("WeaponType", weaponItem.holdTypeID);

                // TU BY SME MALI ZAVOLAŤ PREPÍNANIE VRSTIEV (Layers)
                // Ideálne je pridať metódu SetWeaponType aj do EnemyAnimatorManagera
                // a zavolať ju odtiaľto.
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