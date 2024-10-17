using UnityEngine;

public enum ROOM_TYPE
{
    Default,
    Entry,
    Enemy,
    Final
}

public abstract class Room : MonoBehaviour
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private ROOM_TYPE type = default;

    public string Id { get => id; }
    public ROOM_TYPE Type { get => type; }

    public abstract void Init();
}
