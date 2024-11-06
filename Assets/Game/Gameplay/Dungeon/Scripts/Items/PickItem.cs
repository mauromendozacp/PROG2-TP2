using System.Collections.Generic;

using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] private LayerMask itemLayer = default;

    private List<ItemData> overlapItems = null;

    private void Start()
    {
        overlapItems = new List<ItemData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(itemLayer, other.gameObject.layer))
        {
            ItemData item = other.gameObject.GetComponent<ItemData>();
            if (!overlapItems.Contains(item))
            {
                overlapItems.Add(item);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utils.CheckLayerInMask(itemLayer, other.gameObject.layer))
        {
            ItemData item = other.gameObject.GetComponent<ItemData>();
            RemoveDestroyItem(item);
        }
    }

    public ItemData GetClosestItem()
    {
        if (overlapItems.Count > 0)
        {
            Vector3 centerPosition = transform.position;
            float closestDistance = Vector3.Distance(centerPosition, overlapItems[0].transform.position);
            int closestItemIndex = 0;

            for (int i = 1; i < overlapItems.Count; i++)
            {
                float auxDistance = Vector3.Distance(centerPosition, overlapItems[i].transform.position);
                if (closestDistance < auxDistance)
                {
                    closestDistance = auxDistance;
                    closestItemIndex = i;
                }
            }

            return overlapItems[closestItemIndex];
        }

        return null;
    }

    public void RemoveDestroyItem(ItemData item)
    {
        if (overlapItems.Contains(item))
        {
            overlapItems.Remove(item);
        }
    }
}
