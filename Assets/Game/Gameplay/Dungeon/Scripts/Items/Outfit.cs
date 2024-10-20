using UnityEngine;

public enum OutfitSlotPosition 
{ 
    Helmet, 
    Gloves, 
    Boots, 
    Shoulder, 
    Armor 
};

[CreateAssetMenu(fileName = "Outfit", menuName = "Items/Outfit")]
public class Outfit : Item
{
    [Header("Armor Specific")]
    public OutfitSlotPosition type;
    public int defense;

    public override ItemType GetItemType() { return ItemType.Outfit; }

    public override string ItemToString()
    {
        string text = base.ItemToString();
        string thisType;
        switch (type)
        {
            case OutfitSlotPosition.Armor:
                thisType = "Armor";
                break;
            case OutfitSlotPosition.Boots:
                thisType = "Boots";
                break;
            case OutfitSlotPosition.Gloves:
                thisType = "Gloves";
                break;
            case OutfitSlotPosition.Helmet:
                thisType = "Helmet";
                break;
            case OutfitSlotPosition.Shoulder:
                thisType = "Shoulder";
                break;
            default:
                thisType = "------";
                break;
        }

        text += "\nType: " + thisType + "\nDefense: " + defense;

        return text;
    }
}
