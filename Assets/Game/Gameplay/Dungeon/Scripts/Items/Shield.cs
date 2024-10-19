using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Items/Arms/Shield")]
public class Shield : Arms
{
    [Header("Shield Specific")]
    public int resistance;
    public int size;
    public int speedRating;

    public override ArmsType GetArmsType() { return ArmsType.Shield; }

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Shield\nResistance: " + resistance + "\nSize: " + size + "\nSpeed Rating: " + speedRating;
        return text;
    }
}