using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;

    private void Awake()
    {
        // Get reference to PlayerAnimatorManager in parent
        animatorManager = GetComponentInParent<PlayerAnimatorManager>();
    }

    // Function called by Animation Event to enable combo
    public void EnableCombo()
    {
        if (animatorManager != null)
        {
            animatorManager.EnableCombo();
        }
    }

    // Function called by Animation Event to disable combo
    public void DisableCombo()
    {
        if (animatorManager != null)
        {
            animatorManager.DisableCombo();
        }
    }
}