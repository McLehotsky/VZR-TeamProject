using System.Collections.Generic;
using UnityEngine;

public class LightableTower : Interactable
{
    [Header("Requirement")]
    public Item requiredItem; // Sem dragni EmberCore (Scriptable Object)

    [Header("Visuals")]
    public GameObject fireParticle; // Oheň, ktorý sa zapne

    [Header("Environment Changes")]
    public List<GameObject> fogObjectsToDisable;

    bool isLit = false;

    private void Start()
    {
        // Zaregistrujeme sa v GameManageri
        if (WorldGameManager.instance != null)
            WorldGameManager.instance.RegisterTower();

        // Vypneme oheň na začiatku
        if (fireParticle) fireParticle.SetActive(false);
    }

    // Táto funkcia sa zavolá automaticky keď prídeš k veži
    public override void OnPlayerEnterInteraction(PlayerManager playerManager)
    {
        if (isLit)
        {
            interactText = "Tower is already lit";
            return;
        }

        PlayerInventory inventory = playerManager.GetComponent<PlayerInventory>();

        // Máme v inventári Ember Core?
        if (inventory.keyItems.Contains(requiredItem))
        {
            interactText = "Light the Tower";
        }
        else
        {
            interactText = "Need Ember Core";
        }
    }

    public override void Interact(PlayerManager playerManager)
    {
        if (isLit) return;

        PlayerInventory inventory = playerManager.GetComponent<PlayerInventory>();

        if (inventory.keyItems.Contains(requiredItem))
        {
            // 1. Zapáliť
            IgniteTower();

            // 2. Odobrať item z inventára
            inventory.keyItems.Remove(requiredItem);
            inventory.UpdateEmberCount();

            // 3. Zavrieť popup (lebo sme interagovali)
            playerManager.ClearInteractableObject();
        }
        else
        {
            // Nemáme item -> len vypíšeme správu do konzoly alebo cez UI Manager
            Debug.Log("You don't have the required item!");
        }
    }

    void IgniteTower()
    {
        isLit = true;

        // Vizuály
        if (fireParticle) fireParticle.SetActive(true);

        if (fogObjectsToDisable != null)
        {
            foreach (GameObject fogObject in fogObjectsToDisable)
            {
                if (fogObject != null)
                {
                    fogObject.SetActive(false);
                }
            }
        }

        // Oznámiť Managerovi
        if (WorldGameManager.instance != null)
            WorldGameManager.instance.TowerLit();
    }
}