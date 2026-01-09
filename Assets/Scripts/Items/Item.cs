using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public Sprite itemIcon;
}