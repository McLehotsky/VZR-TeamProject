using Unity.VisualScripting;
using UnityEngine;

public class GateHandle : MonoBehaviour
{
    public GameObject player;
    public UIManager uiManager;
    public WorldGameManager worldGameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (worldGameManager == null)
        {
            worldGameManager = WorldGameManager.instance;
        }
        player = GameObject.Find("Player");
        uiManager = player.GetComponent<UIManager>();
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (worldGameManager.towersLit == 3)
        {
            this.gameObject.SetActive(false);
            uiManager.DisplayGateNotification();
        }
    }
}
