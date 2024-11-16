using UnityEngine;

public enum POTION_TYPE
{
    LIFE,
    MANA
}

[CreateAssetMenu(fileName = "Potion", menuName = "Items/Consumable/Potion")]
public class Potion : Consumible
{
    [Header("Potion Specific")]
    public POTION_TYPE type;

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nPotion Type: " + type;
        return text;
    }
}
