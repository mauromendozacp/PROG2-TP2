using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Items/Arms/Shield")]
public class Shield : Arms
{
    [Header("Shield Specific")]
    public int resistance;

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Shield\nResistance: " + resistance;
        return text;
    }
}