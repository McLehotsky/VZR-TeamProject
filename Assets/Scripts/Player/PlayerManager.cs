using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager animatorManager;
    PlayerCamera playerCamera;
    WeaponSlotManager weaponSlotManager;
    PlayerAttacker playerAttacker;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isStrafing;
    public bool isSprinting;
    public bool isLockedOn;

    [Header("Combat Flags")]
    public bool canDoCombo;

    public WeaponItem startingWeapon;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerCamera = GetComponent<PlayerCamera>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    private void Start()
    {
        if (startingWeapon != null)
        {
            // Na zaciatku nacitame zbran do pravej ruky
            weaponSlotManager.LoadWeaponOnSlot(startingWeapon, false);
            playerAttacker.currentWeapon = startingWeapon;
        }
    }

    private void Update()
    {
        // Každý frame čítame inputy
        inputHandler.TickInput(Time.deltaTime);

        playerAttacker.HandleCombatInput();

        if (playerCamera != null)
        {
            playerCamera.HandleAllCameraMovement();
        }

        if (isLockedOn)
        {
            isStrafing = true;
        }

        // Sprint logic
        if (inputHandler.sprint_Input && inputHandler.moveAmount > 0.5f)
            isSprinting = true;
        else
            isSprinting = false;

        animatorManager.UpdateAnimatorValues(inputHandler.horizontal, inputHandler.vertical, isSprinting);
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate() // Alebo Update
    {
        // Ak sme prestali útočiť (animácia skončila), resetneme combo flag
        if (!isInteracting)
        {
            canDoCombo = false;
        }
    }
}