using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Arms/Projectile")]
public class Projectile : Arms
{
    [Header("Projectile Specific")]
    public int damage = 0;

    public override ArmsType GetArmsType() => ArmsType.Proyectile;

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Proyectile\nDamage: " + damage;
        return text;
    }
}