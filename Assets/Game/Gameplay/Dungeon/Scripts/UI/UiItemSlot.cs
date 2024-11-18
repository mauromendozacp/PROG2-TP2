using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UiItemSlot : MonoBehaviour
{
    public enum PlayerList 
    { 
        Inventory,
        Outfit,
        Arms, 
        None 
    }

    [SerializeField] private PlayerList playerList = PlayerList.Inventory;
    [SerializeField] private int indexList;
    [SerializeField] private int id;
    [SerializeField] private int idDefaultSprite;
    [SerializeField] private ArmsType[] blockArmItems = null;

    private UiInventory inv = null;
    private Action onRefreshMeshAsStatic = null;
    private Func<Vector3> onGetDropItemPosition = null;

    public int GetID() => id;
    public int GetIndex() => indexList;
    public PlayerList GetPlayerList() => playerList;
    public ArmsType[] BlockArmItems => blockArmItems;

    public void Init(UiInventory inv, Action onRefreshMeshAsStatic, Func<Vector3> onGetDropItemPosition)
    {
        this.inv = inv;
        this.onRefreshMeshAsStatic = onRefreshMeshAsStatic;
        this.onGetDropItemPosition = onGetDropItemPosition;

        inv.RefreshAllButtonsEvent += RefreshButton;
    }

    public void SetButton(int indexList, int id)
    {
        if (playerList == PlayerList.None)
        {
            id = -1;
            return;
        }
        this.indexList = indexList;
        this.id = id;

        if (id < 0)
        {
            if (playerList == PlayerList.Inventory)
            {
                transform.GetChild(0).GetComponent<Image>().sprite = inv.defaultSprites[0];
            }
            else
            {
                transform.GetChild(0).GetComponent<Image>().sprite = inv.defaultSprites[idDefaultSprite];
            }
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Sprite sprite = ItemManager.Instance.GetItemFromID(id).icon;
            transform.GetChild(0).GetComponent<Image>().sprite = sprite;

            if (ItemManager.Instance.GetItemFromID(id).maxStack > 1)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                switch (playerList)
                {
                    case PlayerList.Arms:
                    case PlayerList.Outfit:
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.Equipment.GetSlot(indexList).amount.ToString();
                        break;
                    case PlayerList.Inventory:
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.Inventory.GetSlot(indexList).amount.ToString();
                        break;
                }
            }
            else
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        onRefreshMeshAsStatic?.Invoke();
    }

    private void Refresh(PlayerList playerlist)
    {
        switch (playerlist)
        {
            case PlayerList.Arms:
            case PlayerList.Outfit:
                id = inv.Equipment.GetID(indexList);
                break;
            case PlayerList.Inventory:
                id = inv.Inventory.GetID(indexList);
                break;
        }
        SetButton(indexList, id);
    }

    public void MouseDown(RectTransform btn)
    {
        if (id < 0)
            return;

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            if (playerList == PlayerList.Inventory)
            {
                inv.Inventory.Divide(indexList);
                inv.RefreshAllButtons();
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
        {
            if (playerList == PlayerList.Inventory)
            {
                Vector3 temporalItemPosition = onGetDropItemPosition.Invoke();
                ItemManager.Instance.GenerateItemInWorldSpace(inv.Inventory.GetID(indexList), inv.Inventory.GetSlot(indexList).amount, temporalItemPosition);
                inv.Inventory.DeleteItem(indexList);
                Refresh(playerList);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            inv.MouseDown(btn);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            switch (playerList)
            {
                case PlayerList.Inventory:
                    break;
                case PlayerList.Outfit:
                case PlayerList.Arms:
                    if (inv.Equipment.RemoveEquipment(indexList))
                    {
                        inv.RefreshAllButtons();
                        inv.RefreshToolTip(btn);
                    }

                    break;
                case PlayerList.None:
                default:
                    break;
            }
        }
    }

    public bool CanSwapSlot(ArmsType type)
    {
        return !blockArmItems.Contains(type);
    }

    public void RefreshButton()
    {
        Refresh(playerList);
    }

    public void MouseDrag()
    {
        inv.MouseDrag();
    }

    public void MouseUp(RectTransform btn)
    {
        inv.MouseUp(btn);
    }

    public void MouseEnterOver(RectTransform btn)
    {
        if (inv.secondParameter)
        {
            Vector2 mousePos = Input.mousePosition;
            if (inv.mousePos == mousePos)
            {
                inv.slotDrop = btn.GetComponent<UiItemSlot>();
                inv.SwapButtonsIDs();
            }

            inv.secondParameter = false;
        }

        if (id < 0)
            return;

        if (playerList != PlayerList.None)
        {
            inv.toolTip.gameObject.SetActive(true);
            inv.MouseEnterOver(btn);
        }
    }

    public void MouseExitOver()
    {
        inv.toolTip.gameObject.SetActive(false);
    }
}