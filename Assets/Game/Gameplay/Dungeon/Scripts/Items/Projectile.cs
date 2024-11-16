using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Arms/Projectile")]
public class Projectile : Arms
{
    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Proyectile";
        return text;
    }
}