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
        int randomID = GameplayManager.GetInstance().GetRandomItemID();
        int randomAmount = GameplayManager.GetInstance().GetRandomAmmountOfItem(randomID);
        GameplayManager.GetInstance().GenerateItemInWorldSpace(randomID, randomAmount, enemy.transform.position);
    }
}
