using UnityEngine;

public class EnemyAnimatorHook : MonoBehaviour
{
    EnemyWeaponSlotManager weaponSlotManager;

    private void Awake()
    {
        weaponSlotManager = GetComponentInParent<EnemyWeaponSlotManager>();
    }

    public void OpenRightWeaponCollider()
    {
        if (weaponSlotManager != null)
            weaponSlotManager.OpenRightDamageCollider();
    }

    public void CloseRightWeaponCollider()
    {
        if (weaponSlotManager != null)
            weaponSlotManager.CloseRightDamageCollider();
    }

    public void EnableCombo()
    {
        return;
    }

    public void DisableCombo()
    {
        return;
    }
}