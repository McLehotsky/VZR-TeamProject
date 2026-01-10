using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    Animator animator;
    PlayerLocomotion playerLocomotion;
    PlayerManager playerManager;

    int horizontalID;
    int verticalID;

    // --- NOVÉ: Cache pre indexy vrstiev ---
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

        // --- NOVÉ: Získame indexy vrstiev podľa názvov ---
        // Uisti sa, že sa v Animatore volajú presne takto!
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
        float snappedHorizontal;
        float snappedVertical;

        #region Snapping Horizontal
        if (horizontalMovement > 0.05f && horizontalMovement < 0.55f) snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f) snappedHorizontal = 1;
        else if (horizontalMovement < -0.1f && horizontalMovement > -0.55f) snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f) snappedHorizontal = -1;
        else snappedHorizontal = 0;
        #endregion

        #region Snapping Vertical
        if (verticalMovement > 0.05f && verticalMovement < 0.55f) snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f) snappedVertical = 1;
        else if (verticalMovement < -0.1f && verticalMovement > -0.55f) snappedVertical = -0.5f;
        else if (verticalMovement < -0.55f) snappedVertical = -1;
        else snappedVertical = 0;
        #endregion

        if (isSprinting && snappedVertical > 0)
        {
            snappedVertical = 2;
        }

        // --- Aplikovanie do Animatora ---

        if (playerManager.isStrafing)
        {
            animator.SetFloat(horizontalID, snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(verticalID, snappedVertical, 0.1f, Time.deltaTime);
        }
        else
        {
            // FREE ROAM MODE (Exploration)
            float moveAmount = Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement);

            float v = 0;
            // Tolerancia
            if (moveAmount > 0.1f && moveAmount <= 0.55f) v = 0.5f;
            else if (moveAmount > 0.55f) v = 1;

            if (isSprinting && moveAmount > 0.1f) v = 2;

            // ZMENA: Odstránený "FIX" blok. Používame štandardný prechod.
            animator.SetFloat(horizontalID, 0, 0.1f, Time.deltaTime);
            animator.SetFloat(verticalID, v, 0.1f, Time.deltaTime);
        }
    }

    // --- NOVÉ: Metóda na prepínanie vrstiev zbraní ---
    public void SetWeaponType(int weaponID)
    {
        animator.SetInteger("WeaponType", weaponID);

        // 1. RESET - Všetky override vrstvy nastavíme na váhu 0 (vypneme ich)
        animator.SetLayerWeight(layer2H_ID, 0);
        animator.SetLayerWeight(layerPolearm_ID, 0);
        animator.SetLayerWeight(layerGripRight_ID, 0);
        animator.SetLayerWeight(layerGripLeft_ID, 0);

        // 2. LOGIKA ZAPÍNANIA - Zapneme len tie, ktoré potrebujeme
        switch (weaponID)
        {
            case 1: // 1H (Len Grip pravej ruky)
                animator.SetLayerWeight(layerGripRight_ID, 1);
                break;

            case 2: // 2H (Shoulder Stance + Grip pravej ruky)
                animator.SetLayerWeight(layer2H_ID, 1);
                animator.SetLayerWeight(layerGripRight_ID, 1);
                break;

            case 3: // Polearm (Both Arms Stance + Grip oboch rúk)
                animator.SetLayerWeight(layerPolearm_ID, 1);
                animator.SetLayerWeight(layerGripRight_ID, 1);
                animator.SetLayerWeight(layerGripLeft_ID, 1);
                break;

                // Case 0 (Unarmed) neriešime, lebo sme hore všetko vynulovali
        }
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);

        // Povie animatoru, aby s jemnym prechodom (0.2s) prehral konkretnu animaciu
        animator.CrossFade(targetAnim, 0.2f);

        // Nastavi flag v PlayerManageri (aby sme sa nemohli hybat)
        playerManager.isInteracting = isInteracting;
    }

    public void EnableCombo()
    {
        playerManager.canDoCombo = true;
    }

    public void DisableCombo()
    {
        playerManager.canDoCombo = false;
    }
}