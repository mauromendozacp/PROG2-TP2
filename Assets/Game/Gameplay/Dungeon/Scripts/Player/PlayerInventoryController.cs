using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[Serializable]
public class PlayerMesh
{
    public Mesh hair;
    public Mesh armor;
    public Mesh gloves;
    public Mesh boots;
}

public enum PlayerPart
{
    Helmet,
    Shoulders,
    Armor,
    Gloves,
    Boots,
    Arms
}

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private GameObject[] playerMesh = new GameObject[8];
    [SerializeField] private GameObject[] playerUIMesh = new GameObject[8];
    [SerializeField] private PlayerMesh playerDefaultMesh = null;
    [SerializeField] private UiInventory panelInventory = null;
    [SerializeField] private Transform playerMeshTransform = null;
    [SerializeField] private Transform uiMeshTransform = null;
    [SerializeField] private TextAsset defaultInvetoryJson = null;

    [Header("Debug"), Space]
    [SerializeField] private int ItemID = 0;
    [SerializeField] private int ItemAmount = 0;

    private Inventory inventory = null;
    private Equipment equipment = null;

    private const string savePath = "PlayerInventoryFile.json";

    public Inventory Inventory { get => inventory; }
    public Equipment Equipment { get => equipment; }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();
    }

    private void OnDestroy()
    {
        SaveJson();
    }

    public void Init()
    {
        panelInventory?.Init(inventory, equipment, uiMeshTransform, UpdateMesh, GetDropItemPosition);

        LoadJson();
    }

    public void UpdateMesh()
    {
        SetMesh(0, equipment.GetEquipmentList()[4].ID, PlayerPart.Helmet);
        SetMesh(2, equipment.GetEquipmentList()[7].ID, PlayerPart.Shoulders);
        SetMesh(3, equipment.GetEquipmentList()[8].ID, PlayerPart.Armor);
        SetMesh(4, equipment.GetEquipmentList()[5].ID, PlayerPart.Gloves);
        SetMesh(5, equipment.GetEquipmentList()[6].ID, PlayerPart.Boots);
        SetMesh(6, equipment.GetEquipmentList()[0].ID, PlayerPart.Arms);
        SetMesh(7, equipment.GetEquipmentList()[1].ID, PlayerPart.Arms);

        UpdatePlayerUi();
    }

    public void SetMesh(int index, int id, PlayerPart part)
    {
        if (id != -1)
        {
            Item item = ItemManager.Instance.GetItemFromID(id);
            if (item != null)
            {
                if (part == PlayerPart.Helmet)
                {
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = item.mesh;
                    playerMesh[1].GetComponent<SkinnedMeshRenderer>().sharedMesh = new Mesh();
                }
                if (part == PlayerPart.Arms)
                {
                    playerMesh[index].GetComponent<MeshFilter>().mesh = item.mesh;
                    playerMesh[index].GetComponent<MeshRenderer>().material = item.material;

                    if (item is Arms itemArms)
                    {
                        if (index == 6)
                        {
                            playerMesh[index].transform.localPosition = itemArms.spawnPositionL.pos;
                            playerMesh[index].transform.localEulerAngles = itemArms.spawnPositionL.rot;
                            playerMesh[index].transform.localScale = itemArms.spawnPositionL.scale;
                        }
                        else if (index == 7)
                        {
                            playerMesh[index].transform.localPosition = itemArms.spawnPositionR.pos;
                            playerMesh[index].transform.localEulerAngles = itemArms.spawnPositionR.rot;
                            playerMesh[index].transform.localScale = itemArms.spawnPositionR.scale;
                        }
                    }
                }
                else
                {
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = item.mesh;
                }
            }
        }
        else
        {
            switch (part)
            {
                case PlayerPart.Helmet:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = new Mesh();
                    playerMesh[1].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.hair;

                    break;
                case PlayerPart.Shoulders:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = new Mesh();
                    break;

                case PlayerPart.Armor:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.armor;

                    break;
                case PlayerPart.Gloves:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.gloves;

                    break;
                case PlayerPart.Boots:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.boots;

                    break;
                case PlayerPart.Arms:
                    playerMesh[index].GetComponent<MeshFilter>().sharedMesh = new Mesh();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }

    private void UpdatePlayerUi()
    {
        if (panelInventory.gameObject.activeSelf)
        {
            for (int i = 0; i < playerMesh.Length; i++)
            {
                if (i < 6)
                {
                    playerUIMesh[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerMesh[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                }
                else
                {
                    playerUIMesh[i].GetComponent<MeshFilter>().mesh = playerMesh[i].GetComponent<MeshFilter>().mesh;
                    playerUIMesh[i].GetComponent<MeshRenderer>().material = playerMesh[i].GetComponent<MeshRenderer>().material;
                }

                playerUIMesh[i].transform.localPosition = playerMesh[i].transform.localPosition;
                playerUIMesh[i].transform.localEulerAngles = playerMesh[i].transform.localEulerAngles;
                playerUIMesh[i].transform.localScale = playerMesh[i].transform.localScale;
            }
        }
    }

    private Vector3 GetDropItemPosition()
    {
        return playerMeshTransform.position;
    }

    public List<Slot> GetSaveSlots()
    {
        List<Slot> newList = new List<Slot>();

        for (int i = 0; i < equipment.GetEquipmentList().Count; i++)
        {
            newList.Add(equipment.GetEquipmentList()[i]);
        }

        for (int i = 0; i < inventory.GetInventoryList().Count; i++)
        {
            newList.Add(inventory.GetInventoryList()[i]);
        }
        return newList;
    }

    public void SetSaveSlots(List<Slot> newList)
    {
        int equipmentTotalSlots = equipment.GetEquipmentAmount();

        List<Slot> equipmentList = new List<Slot>();
        for (int i = 0; i < equipmentTotalSlots; i++)
        {
            equipmentList.Add(newList[i]);
        }
        equipment.SetNewEquipment(equipmentList);

        List<Slot> itemsList = new List<Slot>();
        for (int i = equipmentTotalSlots; i < newList.Count; i++)
        {
            itemsList.Add(newList[i]);
        }
        inventory.SetNewInventory(itemsList);
    }

    public void SaveJson()
    {
        List<Slot> playerItems = GetSaveSlots();
        string json = "";
        for (int i = 0; i < playerItems.Count; i++)
        {
            json += JsonUtility.ToJson(playerItems[i]);
        }

        FileStream fs;

        if (!File.Exists(savePath))
            fs = File.Create(savePath);
        else
            fs = File.Open(savePath, FileMode.Truncate);

        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(json);
        fs.Close();
        bw.Close();
    }

    public void LoadJson()
    {
        string savedData = string.Empty;
        if (File.Exists(savePath))
        {
            FileStream fs;
            fs = File.Open(savePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            savedData = br.ReadString();
            fs.Close();
            br.Close();
        }
        else
        {
            savedData = defaultInvetoryJson.text;
        }

        List<Slot> newList = new List<Slot>();
        for (int i = 0; i < savedData.Length; i++)
        {
            if (savedData[i] == '{')
            {
                string slotString = "";
                int aux = 0;
                while (savedData[i + aux] != '}')
                {
                    slotString += savedData[i + aux];
                    aux++;
                }
                slotString += '}';
                Slot newSlot = JsonUtility.FromJson<Slot>(slotString);
                newList.Add(newSlot);
            }
        }
        SetSaveSlots(newList);
        UpdateMesh();
        panelInventory.RefreshAllButtons();
    }

    public void ToggleInventory()
    {
        panelInventory.Toggle(!IsOpenPanelInventory());

        if (IsOpenPanelInventory())
        {
            UpdatePlayerUi();
            panelInventory.RefreshAllButtons();
        }
    }

    public void AddNewItem(ItemData item)
    {
        inventory.AddNewItem(item.itemID, item.itemAmount);
    }

    public void ConsumeEquipmentItem(int id)
    {
        Slot slot = equipment.GetSlot(id);
        slot.amount--;

        if (slot.amount <= 0)
        {
            slot.EmptySlot();
            equipment.SetSlot(slot, id);
            UpdateMesh();
        }
    }

    public bool IsOpenPanelInventory()
    {
        return panelInventory.gameObject.activeSelf;
    }

    public void ChangeWeapons()
    {
        equipment.SwapItem(0, 3);
        equipment.SwapItem(1, 2);
        panelInventory.RefreshAllButtons();
    }

    public void AddItemDebug()
    {
        inventory.AddNewItem(ItemID, ItemAmount);
        panelInventory.RefreshAllButtons();
    }
}
