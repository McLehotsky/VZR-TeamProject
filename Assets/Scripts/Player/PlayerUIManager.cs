using UnityEngine;
using UnityEngine.UI; // Dôležité pre Slider
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public Slider healthBar;
    public Slider staminaBar;

    [Header("Weapon & Item UI")]
    public TextMeshProUGUI currentWeaponText;
    public TextMeshProUGUI emberCoreCountText;

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

    public void UpdateCurrentWeaponUI(WeaponItem weapon)
    {
        if (currentWeaponText != null)
        {
            if (weapon != null)
            {
                currentWeaponText.text = weapon.itemName;
            }
            else
            {
                currentWeaponText.text = "Unarmed";
            }
        }
    }

    public void UpdateEmberCoreCountUI(int count)
    {
        if (emberCoreCountText != null)
        {
            emberCoreCountText.text = count.ToString();
        }
    }
}