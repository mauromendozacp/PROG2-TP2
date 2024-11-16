using System;
using UnityEngine;

[Serializable]
public class SpawnPosition
{
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
}

[Serializable]
public enum ArmsType 
{ 
    Sword,
    Bow,
    Wand,
    Shield, 
    Proyectile,
    Consumable
}

public abstract class Arms : Item
{
    [Header("Arms General")]
    public ArmsType armsType;
    public SpawnPosition spawnPositionR;
    public SpawnPosition spawnPositionL;

    public override ItemType GetItemType() => ItemType.Arms;

    public ArmsType GetArmsType() => armsType;
}