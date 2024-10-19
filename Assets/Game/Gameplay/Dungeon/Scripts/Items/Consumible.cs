using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class Consumible : Item
{
    public override ItemType GetItemType() { return ItemType.Consumable; }

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Consumable";
        return text;
    }
}