using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Item List", menuName = "Items/Item List")]
public class ItemList : ScriptableObject
{
    public List<Item> List;
}