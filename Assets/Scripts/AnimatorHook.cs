using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;
    WeaponSlotManager weaponSlotManager;

    private void Awake()
    {
        // Get reference to PlayerAnimatorManager in parent
        animatorManager = GetComponentInParent<PlayerAnimatorManager>();
        weaponSlotManager = GetComponentInParent<WeaponSlotManager>();
    }

    // Function called by Animation Event to enable combo
    public void EnableCombo()
    {
        if (animatorManager != null)
        {
            animatorManager.EnableCombo();
        }
    }

    // Function called by Animation Event to disable combo
    public void DisableCombo()
    {
        if (animatorManager != null)
        {
            animatorManager.DisableCombo();
        }
    }

    public void OpenRightWeaponCollider()
    {
        if (weaponSlotManager != null)
        {
            weaponSlotManager.OpenRightDamageCollider();
        }
    }

    public void CloseRightWeaponCollider()
    {
        if (weaponSlotManager != null)
        {
            weaponSlotManager.CloseRightDamageCollider();
        }
    }
}