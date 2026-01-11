using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;
    InputHandler inputHandler;
    PlayerManager playerManager; // Reference to PlayerManager for weapon and combat flags
    WeaponSlotManager weaponSlotManager; // For information about current weapon


    // TODO reference to current weapon from PlayerManager
    public WeaponItem currentWeapon;

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
        if (inputHandler.rb_Input)
        {
            // Light Attack
            HandleLightAttack(currentWeapon);
        }

        if (inputHandler.rt_Input)
        {
            // Heavy Attack
            HandleHeavyAttack(currentWeapon);
        }
    }

    // Light attack logic
    private void HandleLightAttack(WeaponItem weapon)
    {
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
        inputHandler.rt_Input = false;

        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation(weapon.Heavy_Attack_1, true);
    }
}