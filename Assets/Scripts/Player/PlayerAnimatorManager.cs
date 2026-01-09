using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    Animator animator;
    PlayerLocomotion playerLocomotion;
    PlayerManager playerManager;

    int horizontalID;
    int verticalID;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerManager = GetComponent<PlayerManager>();

        horizontalID = Animator.StringToHash("Horizontal");
        verticalID = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        // Snapping logic
        float snappedHorizontal;
        float snappedVertical;

        #region Snapping Horizontal
        // ZMENA: Namiesto > 0 dávame > 0.1f (Deadzone), aby sme ignorovali mikro-hodnoty
        if (horizontalMovement > 0.05f && horizontalMovement < 0.55f) snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f) snappedHorizontal = 1;
        else if (horizontalMovement < -0.1f && horizontalMovement > -0.55f) snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f) snappedHorizontal = -1;
        else snappedHorizontal = 0;
        #endregion

        #region Snapping Vertical
        // ZMENA: Aj tu pridana tolerancia 0.1f
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
            // ZMENA: Tolerancia 0.1f aj tu
            if (moveAmount > 0.1f && moveAmount <= 0.55f) v = 0.5f;
            else if (moveAmount > 0.55f) v = 1;

            if (isSprinting && moveAmount > 0.1f) v = 2;

            // FIX PRE TVOJ PROBLEM S ČÍSLAMI:
            // Ak máme stáť (v=0) a animátor je už skoro na nule, vypneme dampTime
            // aby sme sa zbavili toho "1.573e-13"
            if (v == 0 && animator.GetFloat(verticalID) < 0.05f)
            {
                animator.SetFloat(horizontalID, 0); // Bez dampTime = okamzita nula
                animator.SetFloat(verticalID, 0);   // Bez dampTime = okamzita nula
            }
            else
            {
                // Inak používame plynulý prechod (0.1f)
                animator.SetFloat(horizontalID, 0, 0.1f, Time.deltaTime);
                animator.SetFloat(verticalID, v, 0.1f, Time.deltaTime);
            }
        }
    }
}