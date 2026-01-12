using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager animatorManager;
    PlayerCamera playerCamera;
    WeaponSlotManager weaponSlotManager;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerStats playerStats;

    UIController uiController;

    UIManager uiManager;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isStrafing;
    public bool isSprinting;
    public bool isLockedOn;

    [Header("Combat Flags")]
    public bool canDoCombo;

    [Header("Current Equipment")]
    public WeaponItem currentWeapon;
    public WeaponItem startingWeapon;

    [Header("Interaction")]
    public Interactable currentInteractableObject;
    public AudioClip pickupSound;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerCamera = GetComponent<PlayerCamera>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        uiManager = GetComponent<UIManager>();
        uiController = GetComponent<UIController>();
    }

    private void Update()
    {
        // Reads inputs per frame
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
        if (inputHandler.sprint_Input && inputHandler.moveAmount > 0.5f && playerStats.currentStamina > 0)
            isSprinting = true;
        else
            isSprinting = false;

        // Switch Weapon logic
        // We only allow swapping if we are not interacting (attacking/rolling)
        if (inputHandler.d_Pad_Right && !isInteracting)
        {
            inputHandler.d_Pad_Right = false; // Reset input immediately
            playerInventory.ChangeRightWeapon();
        }

        CheckForInteraction();
        CheckForPausedGame();

        animatorManager.UpdateAnimatorValues(inputHandler.horizontal, inputHandler.vertical, isSprinting);
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        // Reset combo flag when attack animation ends
        if (!isInteracting)
        {
            canDoCombo = false;
        }
    }

    private void CheckForInteraction()
    {
        // Ak stlacime tlacidlo A (F) a mame nejaky objekt v dosahu
        if (inputHandler.a_Input)
        {
            inputHandler.a_Input = false; // Reset input

            if (currentInteractableObject != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.5f);
                currentInteractableObject.Interact(this);
            }
        }
    }

    public void SetInteractableObject(Interactable interactable)
    {
        currentInteractableObject = interactable;

        interactable.OnPlayerEnterInteraction(this);

        // Show UI
        if (uiManager != null)
            uiManager.ShowInteractPopup(true, interactable.interactText);
    }

    public void ClearInteractableObject()
    {
        currentInteractableObject = null;

        // Hide UI
        if (uiManager != null)
            uiManager.ShowInteractPopup(false);
    }

    public void CheckForPausedGame()
    {
        // Game is not paused
        if (uiController != null && !uiController.pauseMenuCanvas.activeSelf)
        {
            if (inputHandler.escape_Input)
            {
                inputHandler.escape_Input = false; // Reset input
                uiController.PauseGame();
            }
        }
        else // Game is paused
        {
            if (inputHandler.escape_Input)
            {
                inputHandler.escape_Input = false; // Reset input
                uiController.ResumeGame();
            }
        }
    }
}