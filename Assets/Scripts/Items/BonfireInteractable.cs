using UnityEngine;

public class BonfireInteractable : Interactable
{
    private void Start()
    {
        interactText = "Rest at Bonfire";
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PlayerStats playerStats = playerManager.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.Rest();

            playerManager.ClearInteractableObject();
        }
    }
}