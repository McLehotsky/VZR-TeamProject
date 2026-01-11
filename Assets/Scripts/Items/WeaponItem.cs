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
    public int physicalDamageHeavy = 40;
    public int staminaCost = 20;
    public int staminaCostHeavy = 45;

    [Header("Attack Animations")]
    public string Light_Attack_1;
    public string Light_Attack_2;
    public string Heavy_Attack_1;
}