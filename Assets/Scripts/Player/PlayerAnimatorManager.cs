using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    Animator animator;
    PlayerLocomotion playerLocomotion;
    PlayerManager playerManager;

    int horizontalID;
    int verticalID;

    private float stickDeadzone = 0.05f;

    // Index cache for animation layers
    int layer2H_ID;
    int layerPolearm_ID;
    int layerGripRight_ID;
    int layerGripLeft_ID;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerManager = GetComponent<PlayerManager>();

        horizontalID = Animator.StringToHash("Horizontal");
        verticalID = Animator.StringToHash("Vertical");

        // Setting indexes by name from Animator
        layer2H_ID = animator.GetLayerIndex("Layer_Stance_2H");
        layerPolearm_ID = animator.GetLayerIndex("Layer_Stance_Polearm");
        layerGripRight_ID = animator.GetLayerIndex("Layer_Grip_Right");
        layerGripLeft_ID = animator.GetLayerIndex("Layer_Grip_Left");
    }

    private void LateUpdate()
    {
        playerManager.isInteracting = animator.GetBool("isInteracting");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        // Snapping logic
        // Cause of use = For animation blend tree states (0, 0.5, 1 etc)
        float snappedHorizontal;
        float snappedVertical;

        #region Snapping Horizontal
        if (horizontalMovement > stickDeadzone && horizontalMovement < 0.55f) snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f) snappedHorizontal = 1;
        else if (horizontalMovement < -0.1f && horizontalMovement > -0.55f) snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f) snappedHorizontal = -1;
        else snappedHorizontal = 0;
        #endregion

        #region Snapping Vertical
        if (verticalMovement > stickDeadzone && verticalMovement < 0.55f) snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f) snappedVertical = 1;
        else if (verticalMovement < -0.1f && verticalMovement > -0.55f) snappedVertical = -0.5f;
        else if (verticalMovement < -0.55f) snappedVertical = -1;
        else snappedVertical = 0;
        #endregion

        // Hadling sprinting animaiton 
        if (isSprinting && snappedVertical > 0)
        {
            snappedVertical = 2;
        }

        // Linking values with Animator

        if (playerManager.isStrafing)
        {
            // Handling animations for strafing, All direction movement
            animator.SetFloat(horizontalID, snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(verticalID, snappedVertical, 0.1f, Time.deltaTime);
        }
        else
        {
            // Basic movement for exploration, character only runs straight
            float moveAmount = Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement);

            // Zeroing vertical value that we send to Animator, use case = only run forward
            float v = 0;

            if (moveAmount > stickDeadzone && moveAmount <= 0.55f) v = 0.5f;
            else if (moveAmount > 0.55f) v = 1;

            if (isSprinting && moveAmount > 0.1f) v = 2;

            // Handling animations for exploration state
            animator.SetFloat(horizontalID, 0, 0.1f, Time.deltaTime);
            animator.SetFloat(verticalID, v, 0.1f, Time.deltaTime);
        }
    }

    // Handlling animation layers for holding items/weapons
    public void SetWeaponType(int weaponID)
    {
        animator.SetInteger("WeaponType", weaponID);

        // RESET of animation layers (Weight = 0)
        animator.SetLayerWeight(layer2H_ID, 0);
        animator.SetLayerWeight(layerPolearm_ID, 0);
        animator.SetLayerWeight(layerGripRight_ID, 0);
        animator.SetLayerWeight(layerGripLeft_ID, 0);

        // SWITCH LOGIC
        switch (weaponID)
        {
            case 1: // 1H Weapon = Hand Grip Only
                animator.SetLayerWeight(layerGripRight_ID, 1);
                break;

            case 2: // 2H Weapon = (Shoulder Stance + Hand Grip)
                animator.SetLayerWeight(layer2H_ID, 1);
                animator.SetLayerWeight(layerGripRight_ID, 1);
                break;

            case 3: // Polearm (Both Arms Stance + Both Hands Grip)
                animator.SetLayerWeight(layerPolearm_ID, 1);
                animator.SetLayerWeight(layerGripRight_ID, 1);
                animator.SetLayerWeight(layerGripLeft_ID, 1);
                break;

                // Case 0 (Unarmed) No need to implement here because its already implemented when all layers are RESET
        }
    }

    // Function finds specific animation to play based on targetAnim
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);

        // Plays target anim with soft transition (0.2s)
        animator.CrossFade(targetAnim, 0.2f);

        // Sets flag in PlayerManager for Player to not be able to move when attack animation is playing
        playerManager.isInteracting = isInteracting;
    }

    // Help functions for combo attack system
    public void EnableCombo()
    {
        playerManager.canDoCombo = true;
    }

    public void DisableCombo()
    {
        playerManager.canDoCombo = false;
    }
}