using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;

    private void Awake()
    {
        // Nájde skript na rodičovskom objekte (Player)
        animatorManager = GetComponentInParent<PlayerAnimatorManager>();
    }

    // Túto funkciu zavolá Animation Event
    public void EnableCombo()
    {
        if (animatorManager != null)
        {
            animatorManager.EnableCombo();
        }
    }

    // Túto funkciu zavolá Animation Event (ak ju pridáš na koniec animácie)
    public void DisableCombo()
    {
        if (animatorManager != null)
        {
            animatorManager.DisableCombo();
        }
    }
}