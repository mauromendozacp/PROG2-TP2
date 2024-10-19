using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Arms/Projectile")]
public class Projectile : Arms
{
    // Agregar daño a los proyectiles ¿ +1 +2 +3 ?
    public override ArmsType GetArmsType() { return ArmsType.Proyectile; }

    public override string ItemToString()
    {
        string text = base.ItemToString();
        text += "\nType: Proyectile";
        return text;
    }
}