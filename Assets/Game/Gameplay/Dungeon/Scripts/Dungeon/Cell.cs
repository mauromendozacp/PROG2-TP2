public class Cell
{
    public string id = string.Empty;
    public ROOM_TYPE type = default;
    public bool visited = false;
    public bool[] status = new bool[4];
}