using UnityEngine;

[System.Serializable]
public class SpawnPosition
{
    public Vector3 pos;
    public Vector3 rot;
}

public enum ArmsType { Weapon, Shield, Proyectile }

public abstract class Arms : Item
{
    [Header("Spawn Arms")]
    public SpawnPosition spawnPositionR;
    public SpawnPosition spawnPositionL;

    public override ItemType GetItemType() { return ItemType.Arms; }

    public abstract ArmsType GetArmsType();

    public override string ToString() { return ""; }
}