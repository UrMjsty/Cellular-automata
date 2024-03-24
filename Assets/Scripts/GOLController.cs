using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GOLController : MonoBehaviour
{
    public float
        updateTime; //call update cells to update cell. We can use coroutines or just create timer system to make repreated calls to it

    public int cellCount_X, cellCount_Y; //number of cells in row and column
    public Cell[][] cells; //holder to store cells. we will store them in row and columt
    public GameObject cellPrefab; //cell prefab that will be initialized
    public Transform parent; //parent to hold all cells in game
    public int time = 0;
    public int frames = 100;
    public bool IsZombie => ZombieCount > 0;
    public int ZombieCount => CalculateZombieCount();
    [Range(0, 100)][SerializeField] private int baseZombieChance;
    private int HunterChance => baseZombieChance;// * ZombieCount;
    void Initiate()
    {
//      cells[0][0] = gameObject.AddComponent<Cell>();
        /*cells[0][1] = gameObject.AddComponent<Cell>();
        cells[1][0] = gameObject.AddComponent<Cell>();
        cells[1][1] = gameObject.AddComponent<Cell>();*/
    }

    void CreateCells()
    {
        cells = new Cell[cellCount_X][];
        for (int i = 0; i < cellCount_X; i++)
        {
            cells[i] = new Cell[cellCount_Y];
            for (int j = 0; j < cellCount_Y; j++)
            {
                // Instantiate cells and store them
                var rand = new Random().Next(0,
                    2); // Random number to determine cell type: 0 for dead, 1 for alive, 2 for zombie
                Cell cell = Instantiate(cellPrefab, new Vector3(i - 10, j - 5), Quaternion.identity, parent)
                    .GetComponent<Cell>();
                cell.SetState(rand == 0 ? CellState.Empty : CellState.Normal);

                cells[i][j] = cell;
            }
        }
    }

    void UpdateCells()
    {
        //start checking and marking cells
        for (int i = 1; i < cells.Length - 1; i++)
        {
            for (int j = 1; j < cells[i].Length - 1; j++)
            {
                int liveNeighbours = CountLiveNeighbors(i, j);
                liveNeighbours -= 8;
                //if(liveNeighbours >0 )
                    Debug.Log(liveNeighbours);
              /*  // Rule 4: Mark cells as zombie if they have exactly 4 live neighbors
                if (cells[i][j].State == CellState.Empty  && liveNeighbours >= 4) {
                    cells[i][j].nextState = CellState.Zombie;
                }
*/
                //now after finding the neighbour, we can check rule to mark them dead of alive for next update
                //Rule 1: A live cell with 2 or 3 alive neighbouring cells survives
                if (cells[i][j].State != CellState.Empty && (liveNeighbours == 2 || liveNeighbours == 3))
                {
                    continue;
                }

                //Rule 2:A dead cell with 3 neighbouring cells will get alive
                if (cells[i][j].State == CellState.Empty && liveNeighbours == 3)
                {
                    var rand = new Random().Next(100);
                    cells[i][j].nextState = CellState.Normal;
                    if (IsZombie)
                    {
                        if (rand < HunterChance)
                            cells[i][j].nextState = CellState.Hunter;
                    }
                    else 
                    {
                        if (rand < baseZombieChance)
                            cells[i][j].nextState = CellState.Zombie;
                    }
                    continue;
                }

                //Rule 3: All other cells dies
                cells[i][j].nextState = CellState.Empty;
            }
        }

        //All cells are marked so update all cells.
        for (int i = 0; i < cells.Length; i++)
        {
            for (int j = 0; j < cells[i].Length; j++)
            {
                cells[i][j].UpdateCell();
            }
        }
    }
    
    int CountLiveNeighbors(int x, int y) {
        int liveNeighbours = 0;

        // Check neighboring cells
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0) continue; // Skip the current cell
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < cells.Length && ny >= 0 && ny < cells[x].Length) {
                    switch (cells[nx][ny].State)
                    {
                        case CellState.Empty:
                            break;
                        case CellState.Normal:
                            liveNeighbours++;
                            break;
                        case CellState.Zombie:
                           // return -1;
                           liveNeighbours++;
                            break;
                        case CellState.Hunter:
                            liveNeighbours++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    liveNeighbours++;
                }
            }
        }

        return liveNeighbours;
    }

    private void Awake()
    {
        CreateCells();
        Initiate();
    }

    private void Update()
    {
        time++;

        if (time % frames == 0)
        {
            UpdateCells();
           // Debug.Log(ZombieCount);
        }

        if (time > 100)
            time = 0;
    }

    public int CalculateZombieCount()
    {
        // Flatten the 2D array into a single sequence of cells
        var allCells = cells.SelectMany(row => row);

        // Count the number of cells whose CellState is equal to Zombie
        int zombieCount = allCells.Count(cell => cell.State == CellState.Zombie);

        return zombieCount;
    }
}