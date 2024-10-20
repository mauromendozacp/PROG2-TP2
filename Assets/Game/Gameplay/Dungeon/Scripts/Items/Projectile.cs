using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Arms/Projectile")]
public class Projectile : Arms
{
    public override ArmsType GetArmsType() => ArmsType.Proyectile;

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Proyectile";
        return text;
    }
}