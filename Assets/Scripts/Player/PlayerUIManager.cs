using UnityEngine;
using UnityEngine.UI; // Dôležité pre Slider

public class PlayerUIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public Slider healthBar;
    public Slider staminaBar;

    public void InitHealthBar(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        healthBar.value = currentHealth;
    }

    public void InitStaminaBar(float maxStamina)
    {
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void SetCurrentStamina(float currentStamina)
    {
        staminaBar.value = currentStamina;
    }
}