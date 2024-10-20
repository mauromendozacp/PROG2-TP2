using System.Collections.Generic;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Slot> CurrentItems;
    [SerializeField] private int size = 10;
    
    private Equipment equipmentComponent;

    public enum SortType 
    { 
        Type, 
        Name, 
        Level 
    }

    private void Awake()
    {
        equipmentComponent = GetComponent<Equipment>();
        for (int i = 0; i < size; i++)
        {
            Slot newSlot = new Slot();
            CurrentItems.Add(newSlot);
        }
    }

    public void SetNewInventory(List<Slot> newInventory)
    {
        CurrentItems.Clear();
        foreach (Slot slot in newInventory)
        {
            CurrentItems.Add(slot);
        }
    }

    public bool AddNewItem(int ID, int amount, int slotPos)
    {
        if (CurrentItems[slotPos].IsEmpty())
        {
            CurrentItems[slotPos].FillSlot(ID, amount);
            return true;
        }
        else
        {
            if (ID == CurrentItems[slotPos].ID && GameplayManager.GetInstance().GetItemFromID(ID).maxStack >= CurrentItems[slotPos].amount + amount)
            {
                CurrentItems[slotPos].AddAmount(amount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool AddNewItem(int ID, int amount)
    {
        for (int i = 0; i < size; i++)
        {
            if (CurrentItems[i].IsEmpty())
            {
                CurrentItems[i].FillSlot(ID, amount);
                return true;
            }
        }
        return false;
    }

    public void DeleteItem(int slotPos)
    {
        if (!CurrentItems[slotPos].IsEmpty())
        {
            CurrentItems[slotPos].EmptySlot();
        }
    }

    public void SwapItem(int slotPosFrom, int slotPosTo)
    {
        if (slotPosFrom == slotPosTo) return;
        if (!CurrentItems[slotPosFrom].IsEmpty() && !CurrentItems[slotPosTo].IsEmpty())
        {
            Item fromItem = GameplayManager.GetInstance().GetItemFromID(CurrentItems[slotPosFrom].ID);
            Item toItem = GameplayManager.GetInstance().GetItemFromID(CurrentItems[slotPosTo].ID);
            if (fromItem.GetItemType() == toItem.GetItemType() && toItem.maxStack > 1 && fromItem.maxStack > 1)
            {
                CurrentItems[slotPosFrom].amount = CurrentItems[slotPosTo].AddAmount(CurrentItems[slotPosFrom].amount);
                if (CurrentItems[slotPosFrom].amount <= 0)
                {
                    CurrentItems[slotPosFrom].EmptySlot();
                }
                return;
            }
        }
        Slot temp = new Slot(CurrentItems[slotPosFrom].ID, CurrentItems[slotPosFrom].amount);
        CurrentItems[slotPosFrom] = CurrentItems[slotPosTo];
        CurrentItems[slotPosTo] = temp;
    }

    public bool UseItem(int slotPos)    // Doble click o Click Derecho
    {
        if (GameplayManager.GetInstance().GetItemFromID(CurrentItems[slotPos].ID).GetItemType() == ItemType.Consumable)
        {
            CurrentItems[slotPos].AddAmount(-1);
            if (CurrentItems[slotPos].IsEmpty())
                return false;
        }
        else
        {
            CurrentItems[slotPos] = equipmentComponent.SwapEquipment(CurrentItems[slotPos]);
        }
        return true;
    }

    public void Divide(int slotPos)
    {
        if (CurrentItems[slotPos].amount > 1)
        {
            int dividedAmount = (CurrentItems[slotPos].amount / 2);
            if (CurrentItems[slotPos].amount % 2 != 0) dividedAmount++;
            if (AddNewItem(CurrentItems[slotPos].ID, dividedAmount))
            {
                CurrentItems[slotPos].amount /= 2;
            }
        }
    }

    public void Sort(int type)
    {
        switch ((SortType)type)
        {
            case SortType.Type:
                SortByName sortNameByType = new SortByName();
                CurrentItems.Sort(sortNameByType);
                SortByType sortType = new SortByType();
                CurrentItems.Sort(sortType);
                break;
            case SortType.Name:
                SortByName sortName = new SortByName();
                CurrentItems.Sort(sortName);
                break;
            case SortType.Level:
                SortByName sortNameByLevel = new SortByName();
                CurrentItems.Sort(sortNameByLevel);
                SortByLevel sortLevel = new SortByLevel();
                CurrentItems.Sort(sortLevel);
                break;
            default:
                break;
        }
    }

    public int GetSize()
    {
        return size;
    }

    public Slot GetSlot(int index)
    {
        return CurrentItems[index];
    }

    public void SetSlot(int index, Slot slot)
    {
        CurrentItems[index] = slot;
    }

    public int GetID(int index)
    {
        return CurrentItems[index].ID;
    }

    public List<Slot> GetInventoryList()
    {
        return CurrentItems;
    }

    class SortByName : IComparer<Slot>
    {
        public int Compare(Slot x, Slot y)
        {
            if (x.IsEmpty()) return 1;
            else if (y.IsEmpty()) return -1;
            return GameplayManager.GetInstance().GetItemFromID(x.ID).name.CompareTo(GameplayManager.GetInstance().GetItemFromID(y.ID).name);
        }
    }
    class SortByLevel : IComparer<Slot>
    {
        public int Compare(Slot x, Slot y)
        {
            if (x.IsEmpty()) return 1;
            else if (y.IsEmpty()) return -1;
            return GameplayManager.GetInstance().GetItemFromID(x.ID).level.CompareTo(GameplayManager.GetInstance().GetItemFromID(y.ID).level);
        }
    }
    class SortByType : IComparer<Slot>
    {
        public int Compare(Slot x, Slot y)
        {
            if (x.IsEmpty()) return 1;
            else if (y.IsEmpty()) return -1;
            return GameplayManager.GetInstance().GetItemFromID(x.ID).GetItemType().CompareTo(GameplayManager.GetInstance().GetItemFromID(y.ID).GetItemType());
        }
    }
}