using System;
using System.Collections;
using System.Collections.Generic;
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
                    3); // Random number to determine cell type: 0 for dead, 1 for alive, 2 for zombie
                Cell cell = Instantiate(cellPrefab, new Vector3(i - 10, j - 5), Quaternion.identity, parent)
                    .GetComponent<Cell>();
                switch (rand)
                {
                    case 0: // Dead cell
                        cell.isCellAlive = false;
                        break;
                    case 1: // Alive cell
                        cell.isCellAlive = true;
                        break;
                    case 2: // Zombie cell
                        cell.isCellAlive = true; // Zombie cells are initially alive
                        cell.MarkZombie(); // Mark the cell as a zombie
                        break;
                }

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
                
                // Rule 4: Mark cells as zombie if they have exactly 4 live neighbors
                if (!cells[i][j].isCellAlive && liveNeighbours >= 4) {
                    cells[i][j].MarkZombie();
                }

                //now after finding the neighbour, we can check rule to mark them dead of alive for next update
                //Rule 1: A live cell with 2 or 3 alive neighbouring cells survives
                if (cells[i][j].isCellAlive && (liveNeighbours == 2 || liveNeighbours == 3))
                {
                    continue;
                }

                //Rule 2:A dead cell with 3 neighbouring cells will get alive
                if (!cells[i][j].isCellAlive && liveNeighbours == 3)
                {
                    cells[i][j].MarkAlive();
                    continue;
                }

                //Rule 3: All other cells dies
                cells[i][j].MarkDead();
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
                if (nx >= 0 && nx < cells.Length && ny >= 0 && ny < cells[x].Length && cells[nx][ny].isCellAlive) {
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
        }

        if (time > 100)
            time = 0;
    }
}