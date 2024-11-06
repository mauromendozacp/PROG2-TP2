using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    GameObject enemy;

    void Start()
    {
        enemy = gameObject;
    }

    public void GenerateNewItem()
    {
        int randomID = ItemManager.Instance.GetRandomItemID();
        int randomAmount = ItemManager.Instance.GetRandomAmmountOfItem(randomID);
        ItemManager.Instance.GenerateItemInWorldSpace(randomID, randomAmount, enemy.transform.position);
    }
}
