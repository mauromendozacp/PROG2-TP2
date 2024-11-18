using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private int weaponSlotsAmount = 4;
    [SerializeField] private List<Slot> currentEquipment = null;
    
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        for (int i = 0; i < weaponSlotsAmount; i++)
        {
            Slot newSlot = new Slot();
            currentEquipment.Add(newSlot);
        }

        for (int i = 0; i <= (int)OutfitSlotPosition.Armor; i++)
        {
            Slot newSlot = new Slot();
            currentEquipment.Add(newSlot);
        }
    }

    public void SetNewEquipment(List<Slot> newEquipment)
    {
        currentEquipment.Clear();
        foreach (Slot slot in newEquipment)
        {
            currentEquipment.Add(slot);
        }
    }

    public int GetEquipmentAmount()
    {
        return weaponSlotsAmount + (int)OutfitSlotPosition.Armor + 1;
    }

    public bool RemoveEquipment(int slotPos)
    {
        if (inventory.AddNewItem(currentEquipment[slotPos].ID, currentEquipment[slotPos].amount))
        {
            currentEquipment[slotPos].EmptySlot();
            return true;
        }
        return false;
    }

    public Slot SwapEquipment(Slot newItemSlot)
    {
        Item itemToSwap = ItemManager.Instance.GetItemFromID(newItemSlot.ID);
        if (itemToSwap.GetItemType() == ItemType.Outfit)
        {
            int index = weaponSlotsAmount;
            switch (((Outfit)itemToSwap).type)
            {
                case OutfitSlotPosition.Helmet:
                    index += (int)OutfitSlotPosition.Helmet;
                    break;
                case OutfitSlotPosition.Gloves:
                    index += (int)OutfitSlotPosition.Gloves;
                    break;
                case OutfitSlotPosition.Boots:
                    index += (int)OutfitSlotPosition.Boots;
                    break;
                case OutfitSlotPosition.Shoulder:
                    index += (int)OutfitSlotPosition.Shoulder;
                    break;
                case OutfitSlotPosition.Armor:
                    index += (int)OutfitSlotPosition.Armor;
                    break;
            }

            Slot temp = currentEquipment[index];
            currentEquipment[index] = newItemSlot;
            return temp;
        }
        else 
        {
            for (int i = 0; i < weaponSlotsAmount; i++)
            {
                if (currentEquipment[i].IsEmpty())
                {
                    Slot newSlot = currentEquipment[i];
                    currentEquipment[i] = newItemSlot;
                    return newSlot;
                }
            }
            Slot returnSlot = currentEquipment[0];
            currentEquipment[0] = newItemSlot;
            return returnSlot;
        }
    }
    public bool SwapItem(int index1, int index2)
    {
        if (index1 < weaponSlotsAmount && index2 < weaponSlotsAmount)   // Ambas son armas
        {
            if (!currentEquipment[index1].IsEmpty() && !currentEquipment[index2].IsEmpty())
            {
                Item fromItem = ItemManager.Instance.GetItemFromID(currentEquipment[index1].ID);
                Item toItem = ItemManager.Instance.GetItemFromID(currentEquipment[index2].ID);
                if (fromItem.GetItemType() == toItem.GetItemType() && fromItem.maxStack > 1 && toItem.maxStack > 1)
                {
                    currentEquipment[index1].amount = currentEquipment[index2].AddAmount(currentEquipment[index2].amount);
                    if (currentEquipment[index1].amount <= 0)
                    {
                        currentEquipment[index1].EmptySlot();
                    }
                    return true;
                }
            }
            Slot temp = currentEquipment[index1];
            currentEquipment[index1] = currentEquipment[index2];
            currentEquipment[index2] = temp;
            return true;
        }
        return false;
    }

    public bool TrySwapCross(int index1, int index2, bool InvententoryToEquipment, ArmsType[] blockArmItems) // Cambiar
    {
        Item itemToSwap;
        int indexInventory = 0;
        int indexOutfit = 0;

        if (InvententoryToEquipment)
        {
            indexInventory = index1;
            indexOutfit = index2;
            itemToSwap = ItemManager.Instance.GetItemFromID(inventory.GetSlot(indexInventory).ID);
            if (!currentEquipment[indexOutfit].IsEmpty())
            {
                if (currentEquipment[indexOutfit].ID == inventory.GetSlot(indexInventory).ID && itemToSwap.maxStack > 1)
                {
                    inventory.GetSlot(indexInventory).amount = currentEquipment[indexOutfit].AddAmount(inventory.GetSlot(indexInventory).amount);
                    if (inventory.GetSlot(indexInventory).amount <= 0)
                    {
                        inventory.GetSlot(indexInventory).EmptySlot();
                    }

                    return true;
                }
            }
        }
        else
        {
            indexInventory = index2;
            indexOutfit = index1;
            itemToSwap = ItemManager.Instance.GetItemFromID(GetSlot(indexOutfit).ID);
            if (!inventory.GetSlot(indexInventory).IsEmpty())
            {
                Item itemSwaped = ItemManager.Instance.GetItemFromID(inventory.GetSlot(indexInventory).ID);
                if (inventory.GetSlot(indexInventory).ID == currentEquipment[indexOutfit].ID && itemToSwap.maxStack > 1)
                {
                    currentEquipment[indexOutfit].amount = inventory.GetSlot(indexInventory).AddAmount(currentEquipment[indexOutfit].amount);
                    if (currentEquipment[indexOutfit].amount <= 0)
                    {
                        currentEquipment[indexOutfit].EmptySlot();
                    }
                    return true;
                }
                if (itemToSwap.GetItemType() != itemSwaped.GetItemType())
                {
                    return false;
                }
                else if (itemToSwap.GetItemType() == ItemType.Outfit)
                {
                    if (((Outfit)itemToSwap).type != ((Outfit)itemSwaped).type)
                    {
                        return false;
                    }
                }
            }
        }

        if (indexOutfit < weaponSlotsAmount) // Se tiró en un arma
        {
            if (itemToSwap.GetItemType() == ItemType.Arms)
            {
                Arms armItem = itemToSwap as Arms;
                if (!blockArmItems.Contains(armItem.armsType))
                {
                    Slot temp = inventory.GetSlot(indexInventory);
                    inventory.SetSlot(indexInventory, currentEquipment[indexOutfit]);
                    currentEquipment[indexOutfit] = temp;
                    return true;
                }
            }
        }
        else if (itemToSwap.GetItemType() == ItemType.Outfit) // Se tiró en un Outfit
        {
            OutfitSlotPosition slotsOutfit = (OutfitSlotPosition)(indexOutfit - weaponSlotsAmount);
            if (((Outfit)itemToSwap).type == slotsOutfit)
            {
                Slot temp = inventory.GetSlot(indexInventory);
                inventory.SetSlot(indexInventory, currentEquipment[indexOutfit]);
                currentEquipment[indexOutfit] = temp;
                return true;
            }
        }

        return false;
    }

    public Slot GetSlot(int index)
    {
        return currentEquipment[index];
    }

    public List<Slot> GetEquipmentList()
    {
        return currentEquipment;
    }

    public int GetID(int index)
    {
        return currentEquipment[index].ID;
    }

    public void SetSlot(Slot slot, int index)
    {
        currentEquipment[index] = slot;
    }
}