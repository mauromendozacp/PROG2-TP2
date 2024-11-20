using UnityEngine;

public class ItemManager : MonoBehaviourSingleton<ItemManager>
{
    [SerializeField] private ItemList allItems = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private LayerMask floorLayer = default;

    private const float itemArmScale = 0.012f;
    private const float offsetY = 0.15f;

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
        GameObject itemGO = Instantiate(itemPrefab, GetDropItemPosition(spawnPosition), Quaternion.identity);
        Item item = GetItemFromID(itemID);

        itemGO.GetComponent<MeshFilter>().mesh = item.mesh;
        itemGO.GetComponent<MeshRenderer>().material = item.material;
        ItemData itemData = itemGO.GetComponent<ItemData>();
        itemData.itemID = itemID;
        itemData.itemAmount = randomAmount;

        if (item is Arms armItem)
        {
            itemGO.transform.localScale = armItem.spawnPositionL.scale;
        }
        itemGO.transform.position = itemGO.transform.position + new Vector3(0f, offsetY, 0f);

        Instantiate(GetItemFromID(itemID).particle, itemGO.transform);
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
