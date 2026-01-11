using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;
    InputHandler inputHandler;
    PlayerManager playerManager; // Reference to PlayerManager for weapon and combat flags
    WeaponSlotManager weaponSlotManager; // For information about current weapon

    public string lastAttack;


    private void Awake()
    {
        animatorManager = GetComponent<PlayerAnimatorManager>();
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HandleCombatInput()
    {
        if (playerManager.currentWeapon == null)
            return;

        if (inputHandler.rb_Input)
        {
            // Light Attack
            HandleLightAttack(playerManager.currentWeapon);
        }

        if (inputHandler.rt_Input)
        {
            // Heavy Attack
            HandleHeavyAttack(playerManager.currentWeapon);
        }
    }

    // Light attack logic
    private void HandleLightAttack(WeaponItem weapon)
    {
        //Pass Physical Damage to the Weapon Collider
        // We do this here to ensure the correct damage is set before the attack animation plays
        if (weaponSlotManager.rightHandDamageCollider != null)
        {
            weaponSlotManager.rightHandDamageCollider.currentDamage = weapon.physicalDamage;
        }

        // Check if we can do a combo
        if (playerManager.canDoCombo)
        {
            // We are in a combo, so we can chain the next attack
            inputHandler.rb_Input = false;

            // Determine which attack to play next based on the last attack
            if (lastAttack == weapon.Light_Attack_1)
            {
                animatorManager.PlayTargetAnimation(weapon.Light_Attack_2, true);
                lastAttack = weapon.Light_Attack_2; // Store the name
            }
            // Possible continuation for more attacks can be added here
        }
        else
        {
            // Not in a combo
            if (playerManager.isInteracting)
                return;

            // Start a new light attack
            inputHandler.rb_Input = false;
            animatorManager.PlayTargetAnimation(weapon.Light_Attack_1, true);
            lastAttack = weapon.Light_Attack_1; // Store the name
        }
    }

    // Heavy attack logic
    private void HandleHeavyAttack(WeaponItem weapon)
    {

        // Pass Heavy Damage to the Weapon Collider
        if (weaponSlotManager.rightHandDamageCollider != null)
        {
            weaponSlotManager.rightHandDamageCollider.currentDamage = weapon.physicalDamageHeavy;
        }

        inputHandler.rt_Input = false;

        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation(weapon.Heavy_Attack_1, true);
    }
}