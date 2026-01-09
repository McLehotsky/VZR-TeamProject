using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animation Settings")]
    // 1H (1), 2H (2), Polearm (3)
    public int holdTypeID;

    [Header("Combat Stats")]
    public int physicalDamage = 25;
    public int staminaCost = 20;

    [Header("One Handed / Two Handed")]
    public bool isTwoHanded; // Ak true, pouzijeme UpperBody Masku
}