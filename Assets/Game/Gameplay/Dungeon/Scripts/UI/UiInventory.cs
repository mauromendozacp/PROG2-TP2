using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UiInventory : MonoBehaviour
{
    public Action RefreshAllButtonsEvent;
    public Sprite[] defaultSprites;
    Inventory.SortType sortBy = Inventory.SortType.Type;
    private string[] nameSortBy = { "By Type", "By Name", "By Level" };
    public RectTransform sortBRect;
    public TMP_Dropdown sortBDrop;
    public Button prefaButtonSlot;
    public Image toolTip;
    public Inventory inventory;
    public Equipment equipment;
    public GameObject player;

    public Image slotAux;
    public RectTransform content;
    private GridLayoutGroup gridLayout;

    public bool secondParameter;
    private float mouseCurrentPosX;
    private float playerCurrentRotY;

    private void Awake()
    {
        gridLayout = content.GetComponent<GridLayoutGroup>();
        sortBDrop = sortBRect.GetComponent<TMP_Dropdown>();
    }

    void Start()    //   Carrera de start con Inventory
    {
        Invoke(nameof(IniciarInventarioUI), 0);       // ver como iniciar despues de la lógica de inventario.

        for (int i = 0; i <= (int)Inventory.SortType.Level; i++)
        {
            sortBDrop.options[i].text = nameSortBy[i];
        }
    }

    void IniciarInventarioUI()
    {
        CreateButtonsSlots();
        ResizeContent();
        RefreshAllButtons();
    }

    public void RefreshAllButtons()
    {
        RefreshAllButtonsEvent?.Invoke();
    }

    void CreateButtonsSlots()
    {
        int invSize = inventory.GetSize();
        for (int i = 0; i < invSize; i++)
        {
            Slot slot = inventory.GetSlot(i);
            Button newButton = Instantiate(prefaButtonSlot, content.transform);
            newButton.name = ("Slot" + i);

            newButton.GetComponent<UiItemSlot>().SetButton(i, slot.ID);
        }
    }

    void ResizeContent()
    {
        int cantChild = content.transform.childCount;

        float cellSize = gridLayout.cellSize.y;
        cellSize += gridLayout.spacing.y;
        int columns = gridLayout.constraintCount;

        int currentColumn = 0;
        while (cantChild % columns != 0)
        {
            cantChild++;
            currentColumn++;
            if (currentColumn > columns)
            {
                Debug.LogError("Supera el Maximo de Columnas ", gameObject);
                break; // Salida de de emergencia de While
            }
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, cantChild * cellSize / columns);
    }

    public void MouseDown(RectTransform btn)
    {
        slotAux.transform.position = Input.mousePosition;
        slotAux.GetComponent<Image>().sprite = btn.GetComponent<Image>().sprite;
        slotAux.gameObject.SetActive(true);
        slotAux.transform.GetChild(0).GetComponent<Image>().sprite = btn.transform.GetChild(0).GetComponent<Image>().sprite;
        toolTip.gameObject.SetActive(false);
    }

    public string RefreshToolTip(RectTransform btn)
    {
        UiItemSlot uiItem = btn.GetComponent<UiItemSlot>();

        toolTip.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y, btn.transform.position.z);
        int id = uiItem.GetID();

        string text = TextFormatter(uiItem, id, uiItem.GetPlayerList());
        TextMeshProUGUI textMesh = toolTip.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
        return text;
    }

    public void MouseEnterOver(RectTransform btn)
    {
        string text = RefreshToolTip(btn);

        int lines = 0;
        int chars = 0;
        int maxChar = 0;
        float offset = 58;
        float margin = 30;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                lines++;
                if (maxChar < chars)
                    maxChar = chars;
                chars = 0;
            }
            else
            {
                chars++;
            }
        }
        toolTip.rectTransform.sizeDelta = new Vector2(toolTip.rectTransform.sizeDelta.x, lines * offset + margin);
    }

    string TextFormatter(UiItemSlot UiSlot, int idItem, UiItemSlot.PlayerList playerList)
    {
        int index = UiSlot.GetIndex();
        Slot slot;
        switch (playerList)
        {
            case UiItemSlot.PlayerList.Arms:
            case UiItemSlot.PlayerList.Outfit:
                slot = equipment.GetSlot(index);
                break;
            case UiItemSlot.PlayerList.Inventory:
                slot = inventory.GetSlot(index);
                break;
            default:
                slot = inventory.GetSlot(index);
                break;
        }

        if (idItem < 0)
        {
            toolTip.gameObject.SetActive(false);
            return "";
        }
        Item myItem = GameplayManager.GetInstance().GetItemFromID(idItem);

        string text = myItem.ItemToString();
        if (myItem.maxStack > 1)
        {
            text += "\nAmount: " + slot.amount;
        }
        return text;
    }

    private UiItemSlot slotPick;
    public UiItemSlot slotDrop;
    public Vector2 mousePos;

    public void MouseUp(RectTransform btn)
    {
        slotAux.transform.position = Input.mousePosition;
        slotAux.gameObject.SetActive(false);

        mousePos = Input.mousePosition;
        secondParameter = true;
        slotPick = btn.GetComponent<UiItemSlot>();
    }

    public void SwapButtonsIDs()
    {
        if (slotDrop.GetPlayerList() == UiItemSlot.PlayerList.None)
        {
            if (slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory)
            {
                inventory.DeleteItem(slotPick.GetIndex());
                slotPick.RefreshButton();
            }
            return;
        }

        if ((slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory) && (slotDrop.GetPlayerList() == UiItemSlot.PlayerList.Inventory))
        {
            inventory.SwapItem(slotPick.GetIndex(), slotDrop.GetIndex());

            int slotid1 = slotPick.GetID();
            slotPick.SetButton(slotPick.GetIndex(), slotDrop.GetID());
            slotDrop.SetButton(slotDrop.GetIndex(), slotid1);
            slotPick.RefreshButton();
        }
        else if ((slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory && slotDrop.GetPlayerList() != UiItemSlot.PlayerList.Inventory) ||
                 slotPick.GetPlayerList() != UiItemSlot.PlayerList.Inventory && slotDrop.GetPlayerList() == UiItemSlot.PlayerList.Inventory)
        {
            if (equipment.TrySwapCross(slotPick.GetIndex(), slotDrop.GetIndex(), slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory))
            {
                int slotid1 = slotPick.GetID();
                slotPick.SetButton(slotPick.GetIndex(), slotDrop.GetID());
                slotDrop.SetButton(slotDrop.GetIndex(), slotid1);
                slotPick.RefreshButton();
                slotDrop.RefreshButton();


            }
        }
        else if (equipment.SwapItem(slotPick.GetIndex(), slotDrop.GetIndex()))
        {
            int slotid1 = slotPick.GetID();
            slotPick.SetButton(slotPick.GetIndex(), slotDrop.GetID());
            slotDrop.SetButton(slotDrop.GetIndex(), slotid1);
            slotPick.RefreshButton();
            slotDrop.RefreshButton();
        }
    }

    public void MouseDrag()
    {
        slotAux.transform.position = Input.mousePosition;
    }

    public void SortInventory()
    {
        sortBy = (Inventory.SortType)sortBDrop.value;

        inventory.Sort(sortBDrop.value);
        RefreshAllButtonsEvent();
    }

    public void SetPositionX()
    {
        mouseCurrentPosX = Input.mousePosition.x;
        playerCurrentRotY = player.transform.eulerAngles.y;
    }

    public void Rotate()
    {
        float auxPosX = mouseCurrentPosX - Input.mousePosition.x;

        Vector3 auxEuler = player.transform.eulerAngles;
        auxEuler.y = playerCurrentRotY + auxPosX;

        player.transform.eulerAngles = auxEuler;
    }
}