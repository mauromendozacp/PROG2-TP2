using UnityEngine;

public enum WeaponType 
{ 
    Sword, 
    Wand,
    Bow 
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Arms/Weapon")]
public class Weapon : Arms
{
    [Header("Weapon Specific")]
    public WeaponType type;
    public bool twoHanded;
    public int damage;
    public int speed;

    public WeaponType GetWeaponType() => type;

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: " + type + "\n";
        text += twoHanded ?  "(Two-Handed)" : "(One-Handed)";
        text += "\nDamage: " + damage + "\nSpeed: " + speed;

        return text;
    }
}