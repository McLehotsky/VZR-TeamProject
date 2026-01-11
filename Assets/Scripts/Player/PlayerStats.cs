using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    PlayerManager playerManager;
    PlayerAnimatorManager animatorManager;
    PlayerUIManager playerUIManager;

    [Header("Health Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Stamina Stats")]
    public float maxStamina = 100;
    public float currentStamina;
    public float staminaRegenAmount = 35f;
    public float staminaRegenDelay = 2f;

    private float staminaRegenTimer = 0f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerUIManager = GetComponent<PlayerUIManager>();
    }

    private void Start()
    {
        // Set initial health and stamina
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Initialize UI
        if (playerUIManager != null)
        {
            playerUIManager.InitHealthBar(maxHealth);
            playerUIManager.InitStaminaBar(maxStamina);
        }
    }

    private void Update()
    {
        HandleStaminaRegeneration();
    }


    public void TakeDamage(int damage)
    {
        // Todo dodge mechanic
        // if (playerManager.isInvulnerable) 
        //     return;

        currentHealth -= damage;

        // Update UI
        if (playerUIManager != null)
        {
            playerUIManager.SetCurrentHealth(currentHealth);
        }

        // Play hit animation
        animatorManager.PlayTargetAnimation("Damage_01", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Death_01", true);
            // Disable movement and other actions
            //TODO
        }
    }

    public void TakeStaminaDamage(float cost)
    {
        currentStamina -= cost;

        // Update UI
        if (playerUIManager != null)
        {
            playerUIManager.SetCurrentStamina(currentStamina);
        }

        // Reset regeneration timer
        staminaRegenTimer = 0f;

        if (currentStamina < 0)
            currentStamina = 0;
    }

    private void HandleStaminaRegeneration()
    {
        // When interacting or sprinting, reset timer and do not regen
        if (playerManager.isInteracting || playerManager.isSprinting)
        {
            staminaRegenTimer = 0f;
            return;
        }

        // Increment timer
        staminaRegenTimer += Time.deltaTime;

        // If delay passed, regenerate stamina
        if (staminaRegenTimer >= staminaRegenDelay)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenAmount * Time.deltaTime;


                if (playerUIManager != null)
                {
                    playerUIManager.SetCurrentStamina(currentStamina);
                }
            }
        }
    }

    // Check if enough stamina is available
    public bool HasEnoughStamina(float cost)
    {
        if (currentStamina >= cost)
        {
            return true;
        }
        else
        {
            Debug.Log("NEDOSTATOK STAMINY!");
            return false;
        }
    }
}