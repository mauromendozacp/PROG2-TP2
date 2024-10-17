using UnityEngine;

public class RoomBehaviour : Room
{
    //0-Up, 1-Down, 2-Right, 3-Left
    [SerializeField] private GameObject[] walls = null;
    [SerializeField] private GameObject[] doors = null;

    public override void Init()
    {
        
    }

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}