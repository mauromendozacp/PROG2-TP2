using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] ItemList allItems;

    [SerializeField] GameObject itemPrefab;
    private const float itemArmScale = 0.018f;

    static private GameplayManager instance;

    static public GameplayManager GetInstance() { return instance; }

    string savePath = "SaveFile.json";

    public bool Paused { get; set; }

    public int GetRandomItemID()
    {
        return Random.Range(0, allItems.List.Count);
    }

    public int GetRandomAmmountOfItem(int id)
    {
        return Random.Range(1, allItems.List[id].maxStack);
    }

    public Item GetItemFromID(int id)
    {
        return allItems.List[id];
    }

    public void GenerateItemInWorldSpace(int itemID, int randomAmount, Vector3 SpawnPosition)
    {
        GameObject item = Instantiate(itemPrefab, SpawnPosition, Quaternion.identity);
        item.GetComponent<MeshFilter>().mesh = GetInstance().GetItemFromID(itemID).mesh;
        item.GetComponent<MeshCollider>().sharedMesh = GetInstance().GetItemFromID(itemID).mesh;
        item.GetComponent<ItemData>().itemID = itemID;
        item.GetComponent<ItemData>().itemAmount = randomAmount;
        item.GetComponent<MeshRenderer>().material = GetItemFromID(itemID).material;
        Instantiate(GetItemFromID(itemID).particle, item.transform);

        if (GetItemFromID(itemID).GetItemType() == ItemType.Arms)
        {
            item.transform.localScale *= itemArmScale;
        }
    }
}