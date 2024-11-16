using UnityEngine;

public enum ItemType 
{ 
    Arms,
    Outfit
};

public abstract class Item : ScriptableObject
{
    [Header("Item General")]
    public string itemName;
    public int level = 1;
    public float weight;
    public int price;
    public int maxStack = 1;
    public Sprite icon;
    public Mesh mesh;
    public Material material;
    public GameObject particle;

    public abstract ItemType GetItemType();

    public virtual string ItemToString() 
    { 
        return "Name: " + itemName + "\nLevel: " + level + "\nWeight: " + weight + "\nPrice: " + price;
    }
}