using UnityEngine;
using TMPro; // Nutné pre Text Mesh Pro
using System.Collections; // Nutné pre Coroutines (IEnumerator)

public class UIManager : MonoBehaviour
{
    [Header("Interaction UI")]
    public GameObject interactPopup; // Okno "Press F"
    public TextMeshProUGUI interactMessage; // Text "Press F"

    [Header("Item Notification UI")]
    public GameObject itemNotificationPopup; // Okno "Picked up..."
    public TextMeshProUGUI itemNotificationText; // Text "Picked up..."

    Coroutine notificationCoroutine; // Premenná na sledovanie časovača

    private void Awake()
    {
        // Na začiatku skryjeme obe okná
        if (interactPopup != null)
            interactPopup.SetActive(false);

        if (itemNotificationPopup != null)
            itemNotificationPopup.SetActive(false);
    }

    // --- INTERACTION LOGIC ---
    public void ShowInteractPopup(bool isVisible, string message = "")
    {
        if (interactPopup != null)
        {
            interactPopup.SetActive(isVisible);
        }

        if (isVisible && interactMessage != null)
        {
            interactMessage.text = message;
        }
    }

    // --- ITEM NOTIFICATION LOGIC (NOVÉ) ---
    public void DisplayItemNotification(string itemName)
    {
        // 1. Nastavíme text
        if (itemNotificationText != null)
            itemNotificationText.text = "Picked up: " + itemName;

        // 2. Zapneme popup
        if (itemNotificationPopup != null)
            itemNotificationPopup.SetActive(true);

        // 3. Ak už beží odpočítavanie (zobral si iný item pred chvíľou), resetujeme ho
        if (notificationCoroutine != null)
            StopCoroutine(notificationCoroutine);

        // 4. Spustíme nové odpočítavanie
        notificationCoroutine = StartCoroutine(HideNotificationAfterTime());
    }

    private IEnumerator HideNotificationAfterTime()
    {
        // Čakáme 2 sekundy
        yield return new WaitForSeconds(2f);

        // Vypneme popup
        if (itemNotificationPopup != null)
            itemNotificationPopup.SetActive(false);
    }
}