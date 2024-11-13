using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class Consumible : Item
{
    [Header("Consumible Specific")]
    public int amount = 0;

    public override ItemType GetItemType() => ItemType.Consumable;

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Consumable\nAmount: " + amount;
        return text;
    }
}