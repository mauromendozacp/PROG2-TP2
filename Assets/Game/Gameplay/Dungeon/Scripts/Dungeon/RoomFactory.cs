using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [SerializeField] private Room[] rooms = null;
    [SerializeField] private Transform holder = null;

    private List<Room> roomsList = null;

    public void Init()
    {
        roomsList = new List<Room>();
    }

    public Room GetRoomByType(ROOM_TYPE type)
    {
        List<Room> roomList = rooms.ToList().Where(r => r.Type == type).ToList();

        if (roomList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, roomList.Count);
            return CreateRoom(roomList[randomIndex].Id);
        }

        return null;
    }

    public void ClearRooms()
    {
        foreach (var room in roomsList)
        {
            Destroy(room.gameObject);
        }

        roomsList.Clear();
    }

    private Room CreateRoom(string id)
    {
        Room newRoom = rooms.ToList().Find(r => r.Id == id);
        if (newRoom != null)
        {
            Room room = Instantiate(newRoom, holder);
            room.Init();

            roomsList.Add(room);

            return room;
        }

        return null;
    }
}
