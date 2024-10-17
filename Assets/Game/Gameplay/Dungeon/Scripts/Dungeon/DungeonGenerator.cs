using System.Collections.Generic;

using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Configuration")]
    [SerializeField] private Vector2 dungeonSize;
    [SerializeField] private int startPos = 0;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int maxIterator = 0;
    
    [Header("Seed Configuration")]
    [SerializeField] private int seed = 0;
    [SerializeField] private bool useRandomSeed = false;

    [Header("Enemy Configuration")]
    [SerializeField] private float enemyProbability = 0f;

    private RoomFactory roomFactory = null;

    private List<Cell> board = null;
    private int lastCell = 0;

    private void Awake()
    {
        roomFactory = GetComponent<RoomFactory>();
    }

    public void Init()
    {
        roomFactory.Init();

        board = new List<Cell>();
    }

    public void MazeGenerator()
    {
        InitRandomSeed();
        InitializeBoard();
        GenerateDungeon();
    }

    public void RegenerateDungeon()
    {
        roomFactory.ClearRooms();
        board.Clear();

        MazeGenerator();
    }

    private void InitializeBoard()
    {
        float boardLenght = dungeonSize.x * dungeonSize.y;

        for (int i = 0; i < boardLenght; i++)
        {
            Cell cell = new Cell();

            board.Add(cell);
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();

        for (int i = 0; i < maxIterator; i++)
        {
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            board[currentCell].type = Random.Range(0, 100) > enemyProbability ? ROOM_TYPE.Enemy : ROOM_TYPE.Default;

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        lastCell = currentCell;

        board[0].type = ROOM_TYPE.Entry;
        board[lastCell].type = ROOM_TYPE.Final;
    }

    private void GenerateDungeon()
    {
        for (int i = 0; i < dungeonSize.x; i++)
        {
            for (int j = 0; j < dungeonSize.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * dungeonSize.x)];

                if (currentCell.visited)
                {
                    Room newRoom = roomFactory.GetRoomByType(currentCell.type);
                    newRoom.transform.position = new Vector3(i * offset.x, 0f, -j * offset.y);
                    newRoom.transform.rotation = Quaternion.identity;

                    if (newRoom is RoomBehaviour rb)
                    {
                        rb.UpdateRoom(currentCell.status);
                    }

                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        
        //check Up
        if(cell - dungeonSize.x >= 0 && !board[Mathf.FloorToInt(cell - dungeonSize.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - dungeonSize.x));
        }
        
        //check Down
        if(cell + dungeonSize.x < board.Count && !board[Mathf.FloorToInt(cell + dungeonSize.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + dungeonSize.x));
        }

        //check Right
        if((cell + 1) % dungeonSize.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        //check Left
        if(cell % dungeonSize.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }

    private void InitRandomSeed()
    {
        if (useRandomSeed)
        {
            int randomSeed = System.DateTime.Now.GetHashCode();
            Random.InitState(randomSeed);

            Debug.Log("Dungeon Seed Generated: " + randomSeed);
        }
        else
        {
            Random.InitState(seed);
        }
    }
}
