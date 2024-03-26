using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GOLController : MonoBehaviour
{
    public float updateTime; //call update cells to update cell. We can use coroutines or just create timer system to make repreated calls to it

    public int cellCount_X, cellCount_Y; //number of cells in row and column
    public Cell[][] cells; //holder to store cells. we will store them in row and columt
    public GameObject cellPrefab; //cell prefab that will be initialized
    public Transform parent; //parent to hold all cells in game
    public int time = 0;
    public int frames = 100;
    public bool IsPandemic => ZombieCount > 100;
    public int ZombieCount;
    [Range(0, 100)][SerializeField] private int baseZombieChance;
    private int HunterChance = 0;// * ZombieCount;
    [SerializeField] private Slider zombieChanceSlider;
    [SerializeField] private Slider hunterChanceSlider;
    [SerializeField] private Slider gameSpeedSlider;
    void Initiate()
    {
//      cells[0][0] = gameObject.AddComponent<Cell>();
        /*cells[0][1] = gameObject.AddComponent<Cell>();
        cells[1][0] = gameObject.AddComponent<Cell>();
        cells[1][1] = gameObject.AddComponent<Cell>();*/
    }

    public void CreateCells()
    {
        cells = new Cell[cellCount_X][];
        for (int i = 0; i < cellCount_X; i++)
        {
            cells[i] = new Cell[cellCount_Y];
            for (int j = 0; j < cellCount_Y; j++)
            {
                // Instantiate cells and store them
              //  var rand = new Random().Next(2); // Random number to determine cell type: 0 for dead, 1 for alive, 2 for zombie
                Cell cell = Instantiate(cellPrefab, new Vector3(i - 10, j - 5), Quaternion.identity, parent)
                    .GetComponent<Cell>();
              //  cell.SetState(rand == 0 ? CellState.Empty : CellState.Normal);
                cell.SetState(CellState.Empty);
              //  cell.nextState = (rand == 0 ? CellState.Empty : CellState.Normal);
                cell.nextState = (CellState.Empty);
                
                cells[i][j] = cell;
            }
        }
    }

    public void SetNormal()
    {
        for (int i = 0; i < cellCount_X; i++)
        {
            for (int j = 0; j < cellCount_Y; j++)
            {
                var cell = cells[i][j];
                if(cell.State == CellState.Empty)
                    continue;
                else
                {
                    cell.SetState(CellState.Normal);// = CellState.Normal;
                }
                
            }
        }
    } 
    public void SetRandom()
    {
        for (int i = 0; i < cellCount_X; i++)
        {
            for (int j = 0; j < cellCount_Y; j++)
            {
                var cell = cells[i][j];
                var rand = new Random().Next(2);
                cell.SetState(rand == 1 ? CellState.Normal : CellState.Empty);
            }
        }
    }

    public void StartSimulation()
    {
        if (gameSpeedSlider.value == 0)
        {
            gameSpeedSlider.value = 1;
            Time.timeScale = 1f;
        }
    }

    public void SetDefaultSettings()
    {
        zombieChanceSlider.value = 4;
        baseZombieChance = 4;
        hunterChanceSlider .value = 25;
        HunterChance = 15;
        gameSpeedSlider.value = 1;
        Time.timeScale = 1f;
    }

    public void SetZombieChance()
    {
        baseZombieChance = (int)zombieChanceSlider.value;
    } 
    public void SetHunterChance()
    {
        HunterChance = (int)hunterChanceSlider.value;
    }

    public void SetGameSpeed()
    {
        Time.timeScale = gameSpeedSlider.value;
    }
    private CellState GetRandomState()
    {
        CellState state;
        var rand = new Random().Next(100);
        state = CellState.Normal;
        if (IsPandemic)
        {
            if (rand < HunterChance)
                state = CellState.Hunter;
        }
        else 
        {
            if (rand < baseZombieChance)
                state = CellState.Zombie;
        }
        return state;
    }

    private CellState GetNewPeopleState()
    {
        if (!IsPandemic)
            return CellState.Normal;
        var rand = new Random().Next(100);
        {
            if (rand < HunterChance)
                return CellState.Hunter;
        }

        return CellState.Normal;
    }
    private void UpdateCells()
    {
        //start checking and marking cells
        for (int i = 1; i < cells.Length - 1; i++)
        {
            for (int j = 1; j < cells[i].Length - 1; j++)
            {
                Cell currentCell = cells[i][j];
                var normalNeighbors = 0;
                var zombieNeighbours = 0;
                var hunterNeighbors = 0;
                (normalNeighbors, zombieNeighbours, hunterNeighbors) = CountLiveNeighbors(i, j);
                int peopleNeighbors = normalNeighbors + hunterNeighbors;
                int neighbors = peopleNeighbors + zombieNeighbours;
                //if(liveNeighbours <0 )
                /*  // Rule 4: Mark cells as zombie if they have exactly 4 live neighbors
                  if (cells[i][j].State == CellState.Empty  && liveNeighbours >= 4) {
                      cells[i][j].nextState = CellState.Zombie;
                  }
  */
                currentCell.nextState = CellState.Empty;
                switch (currentCell.State)
                {
                    case CellState.Empty:
                        if (peopleNeighbors == 3)
                            currentCell.nextState = GetRandomState();
                        if (zombieNeighbours > 2 && zombieNeighbours < 7)
                            currentCell.nextState = CellState.Zombie;
                        break;
                    case CellState.Normal:
                        if (peopleNeighbors == 2 || peopleNeighbors == 3)
                            currentCell.nextState = GetNewPeopleState();
                        if (zombieNeighbours > 1)
                            currentCell.nextState = CellState.Zombie;
                        break;
                    case CellState.Zombie:
                        if (zombieNeighbours == 2 || zombieNeighbours == 3)
                            currentCell.nextState = CellState.Zombie;
                        if (hunterNeighbors > 1)
                            currentCell.nextState = CellState.Hunter;
                        break;
                    case CellState.Hunter:
                        if (ZombieCount == 0)
                            currentCell.nextState = CellState.Normal;
                        if(peopleNeighbors > 1 && peopleNeighbors < 5)
                            currentCell.nextState = CellState.Hunter;
                        
                        if (zombieNeighbours > 3)
                               currentCell.nextState = CellState.Empty;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                } 
                /*
                if (currentCell.State == CellState.Normal && liveNeighbours == -1)
                {
                    Debug.Log("zombie");
                    currentCell.nextState = CellState.Zombie;
                }
                  //now after finding the neighbour, we can check rule to mark them dead of alive for next update
                  //Rule 1: A live cell with 2 or 3 alive neighbouring cells survives
                  if (currentCell.State != CellState.Empty && (liveNeighbours == 2 || liveNeighbours == 3))
                  {
                      continue;
                  }

                  //Rule 2:A dead cell with 3 neighbouring cells will get alive
                  if (currentCell.State == CellState.Empty && liveNeighbours == 3)
                  {
                      var rand = new Random().Next(100);
                      currentCell.nextState = CellState.Normal;
                    /*  if (IsZombie)
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
                  currentCell.nextState = CellState.Empty;
              }
          }
          */
                //All cells are marked so update all cells.
            }
        }

        for (int i = 0; i < cells.Length; i++)
        {
            for (int j = 0; j < cells[i].Length; j++)
            {
                cells[i][j].UpdateCell();
            }
        }
    }
    
    (int normalNeighbours, int zombieNeighbours, int hunterNeighbours) CountLiveNeighbors(int x, int y) {
        int normalNeighbours = 0;
        int zombieNeighbours = 0;
        int hunterNeighbours = 0;

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
                            normalNeighbours++;
                            break;
                        case CellState.Zombie:
                           zombieNeighbours++;
                            break;
                        case CellState.Hunter:
                            hunterNeighbours++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        return (normalNeighbours, zombieNeighbours, hunterNeighbours);
    }

    private void Awake()
    {
        CreateCells();
        Initiate();
        SetGameSpeed();
    }

    private void FixedUpdate()
    {
        time++;

        if (time % frames == 0)
        {
            UpdateCells();
           // Debug.Log(ZombieCount);
        }
        if (time % 200 == 0)
        {
            ZombieCount = CalculateZombieCount();
            if (ZombieCount == 0)
            {
                SetNormal();
            }
        }
        if (time > 1000)
            time = 0;
      
    }

  /*  private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            Debug.Log(Camera.main == null);
            RaycastHit hit;
		
            if( Physics.Raycast( ray, out hit, 10000 ) )
            {
                Debug.Log( hit.transform.gameObject.name );
            }
        }
    }
    public void WriteZombieCount()
    {
        Debug.Log(ZombieCount);
    }*/
  
    public int CalculateZombieCount()
    {
        // Flatten the 2D array into a single sequence of cells
        var allCells = cells.SelectMany(row => row);

        // Count the number of cells whose CellState is equal to Zombie
        int zombieCount = allCells.Count(cell => cell.State == CellState.Zombie);

        return zombieCount;
    }
}