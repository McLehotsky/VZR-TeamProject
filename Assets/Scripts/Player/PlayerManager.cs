using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager animatorManager;
    PlayerCamera playerCamera;
    WeaponSlotManager weaponSlotManager;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isStrafing;
    public bool isSprinting;
    public bool isLockedOn;

    public WeaponItem startingWeapon;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerCamera = GetComponent<PlayerCamera>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    private void Start()
    {
        if (startingWeapon != null)
        {
            // Na zaciatku nacitame zbran do pravej ruky
            weaponSlotManager.LoadWeaponOnSlot(startingWeapon, false);
        }
    }

    private void Update()
    {
        // Každý frame čítame inputy
        inputHandler.TickInput(Time.deltaTime);

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
}