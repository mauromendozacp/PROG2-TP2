using UnityEngine;
using UnityEngine.UI;

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

    private UiInventory inv = null;

    public int GetID() => id;
    public int GetIndex() => indexList;
    public PlayerList GetPlayerList() => playerList;

    public void Init(UiInventory inv)
    {
        this.inv = inv;

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
            /*Sprite sprite = GameplayManager.GetInstance().GetItemFromID(id).icon;
            transform.GetChild(0).GetComponent<Image>().sprite = sprite;

            if (GameplayManager.GetInstance().GetItemFromID(id).maxStack > 1)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                switch (playerList)
                {
                    case UiItemSlot.PlayerList.Arms:
                    case UiItemSlot.PlayerList.Outfit:
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.equipment.GetSlot(indexList).amount.ToString();
                        break;
                    case UiItemSlot.PlayerList.Inventory:
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.inventory.GetSlot(indexList).amount.ToString();
                        break;
                }
            }
            else
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }*/
        }

        //Player.OnRefreshMeshAsStatic?.Invoke();
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

    private void Awake()
    {
        inv = FindObjectOfType<UiInventory>();
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
                /*Vector3 temporalItemPosition = playerReference.transform.position + playerReference.transform.forward * 2.5f;
                GameplayManager.GetInstance().GenerateItemInWorldSpace(inv.inventory.GetID(indexList), inv.inventory.GetSlot(indexList).amount, temporalItemPosition);
                inv.inventory.DeleteItem(indexList);
                Refresh(playerList);*/
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
                    if (inv.Inventory.UseItem(indexList))
                    {
                        inv.RefreshAllButtons();
                        inv.RefreshToolTip(btn);
                    }
                    else
                    {
                        Refresh(playerList);
                        if (id < 0)
                        {
                            inv.toolTip.gameObject.SetActive(false);
                        }
                    }
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

    public void RefreshButton()
    {
        Refresh(playerList);
    }

    private void RefreshTooltipText()
    {

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