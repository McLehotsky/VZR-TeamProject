using UnityEngine;
using System.Collections.Generic;

public class WorldGameManager : MonoBehaviour
{
    public static WorldGameManager instance;

    [Header("Game State")]
    public int totalTowers;
    public int towersLit;

    [Header("UI")]
    public GameObject winScreen; // Dragni sem UI Panel "You Win"
    public GameObject loseScreen; // Dragni sem UI Panel "Game Over"

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Na začiatku schováme obrazovky
        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);
    }

    public void RegisterTower()
    {
        totalTowers++;
    }

    public void TowerLit()
    {
        towersLit++;
        CheckWinCondition();
    }

    public void PlayerDied()
    {
        Debug.Log("GAME OVER");
        if (loseScreen) loseScreen.SetActive(true);
        // Tu môžeme zastaviť čas alebo reštartovať scénu
    }

    private void CheckWinCondition()
    {
        if (towersLit >= totalTowers)
        {
            Debug.Log("VICTORY! All towers lit.");
            if (winScreen) winScreen.SetActive(true);
        }
    }
}