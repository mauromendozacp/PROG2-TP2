using UnityEngine;

public class ItemManager : MonoBehaviourSingleton<ItemManager>
{
    [SerializeField] private ItemList allItems = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private LayerMask floorLayer = default;

    private const float itemArmScale = 0.018f;

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
        return (id >= 0 && id < allItems.List.Count) ? allItems.List[id] : null;
    }

    public void GenerateItemInWorldSpace(int itemID, int randomAmount, Vector3 spawnPosition)
    {
        GameObject item = Instantiate(itemPrefab, GetDropItemPosition(spawnPosition), Quaternion.identity);
        item.GetComponent<MeshFilter>().mesh = GetItemFromID(itemID).mesh;
        item.GetComponent<MeshCollider>().sharedMesh = GetItemFromID(itemID).mesh;
        item.GetComponent<ItemData>().itemID = itemID;
        item.GetComponent<ItemData>().itemAmount = randomAmount;
        item.GetComponent<MeshRenderer>().material = GetItemFromID(itemID).material;
        Instantiate(GetItemFromID(itemID).particle, item.transform);

        if (GetItemFromID(itemID).GetItemType() == ItemType.Arms)
        {
            item.transform.localScale *= itemArmScale;
        }
    }

    private Vector3 GetDropItemPosition(Vector3 spawnPosition)
    {
        if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            return hit.point;
        }

        return spawnPosition;
    }
}
