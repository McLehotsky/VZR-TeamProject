using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;
    InputHandler inputHandler;
    PlayerManager playerManager; // Potrebujeme vediet aku zbran drzime (neskor)
    WeaponSlotManager weaponSlotManager; // Ak by sme potrebovali info o zbrani


    // Dočasne sem dáme referenciu na zbraň, neskôr to budeme brať z PlayerManagera
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

    private void HandleLightAttack(WeaponItem weapon)
    {
        // COMBO LOGIKA
        if (playerManager.canDoCombo)
        {
            // Sme v okne pre kombo, takže ak stlačíme tlačidlo, ideme ďalej
            inputHandler.rb_Input = false;

            // Ak bol posledný útok "Attack 1", prehráme "Attack 2"
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                lastAttack = weapon.OH_Light_Attack_2; // Uložíme si, že teraz sme v 2. útoku
            }
            // Tu by sme mohli pokračovať (Ak last == Attack 2 -> Play Attack 3...)
        }
        else
        {
            // Nie sme v combe (buď stojíme, alebo sme combo nestihli)
            if (playerManager.isInteracting)
                return; // Ak práve útočíme a nie je otvorené combo okno, ignorujeme kliknutie

            // ŠTART COMB - Prvý útok
            inputHandler.rb_Input = false;
            animatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            lastAttack = weapon.OH_Light_Attack_1; // Uložíme si názov
        }
    }

    private void HandleHeavyAttack(WeaponItem weapon)
    {
        inputHandler.rt_Input = false;

        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
    }
}