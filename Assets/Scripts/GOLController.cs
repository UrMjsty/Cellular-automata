using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GOLController : MonoBehaviour
{
    public float updateTime; //call update cells to update cell. We can use coroutines or just create timer system to make repreated calls to it
    public int cellCount_X, cellCount_Y; //number of cells in row and column
    public Cell[][] cells; //holder to store cells. we will store them in row and columt
    public GameObject cellPrefab; //cell prefab that will be initialized
    public Transform parent; //parent to hold all cells in game
    public int time = 0;
    void Initiate()
    {
//      cells[0][0] = gameObject.AddComponent<Cell>();
      /*cells[0][1] = gameObject.AddComponent<Cell>();
      cells[1][0] = gameObject.AddComponent<Cell>();
      cells[1][1] = gameObject.AddComponent<Cell>();*/
    }
    void CreateCells() {
      cells = new Cell[cellCount_X][];
      for (int i = 0; i < cellCount_X; i++) {
        cells[i] = new Cell[cellCount_Y];
        for (int j = 0; j < cellCount_Y; j++) {
          //instantiate cells and store them
          var rand = new Random().Next(0, 2);
            Cell cell = Instantiate(cellPrefab, new Vector3(i - 10, j - 5), Quaternion.identity, parent).GetComponent<Cell>();
            if(rand > 0)
            {
              cell.isCellAlive = true;
            }
            cells[i][j] = cell;

          //  cells[i][j].name = $ "cell{i}{j}";
        }
      }
    }
  void UpdateCells() {
    //start checking and marking cells
    for (int i = 1; i < cells.Length - 1; i++) {
      for (int j = 1; j < cells[i].Length - 1; j++) {
        //find numbers of live neighbours. Also don't forget to skip corner and edge cells. they will have fewer number of neighbours.
        int liveNeighbours = 0;

        //check bottom, left
        if (i > 0 && j > 0 && cells[i - 1][j - 1].isCellAlive) {
          liveNeighbours++;
        }

        //check bottom
        if (j > 0 && cells[i][j - 1].isCellAlive) {
          liveNeighbours++;
        }

        //check bottom right
        if (i < cells.Length - 1 & j > 0 && cells[i + 1][j - 1].isCellAlive) {
          liveNeighbours++;
        }

        //check for right
        if (i > 0 && cells[i - 1][j].isCellAlive) {
          liveNeighbours++;
        }

        //check for left
        if (i < cells.Length - 1 && cells[i + 1][j].isCellAlive) {
          liveNeighbours++;
        }

        //check for top left
        if (i > 0 && j < cells[i].Length - 1 && cells[i - 1][j + 1].isCellAlive) {
          liveNeighbours++;
        }

        //check for top
        if (j < cells[i].Length - 1 && cells[i][j + 1].isCellAlive) {
          liveNeighbours++;
        }

        //check for top right
      if (i < cells.Length - 1 && j < cells[i].Length - 1 && cells[i + 1][j + 1].isCellAlive) {
        liveNeighbours++;
      }

      //now after finding the neighbour, we can check rule to mark them dead of alive for next update
      //Rule 1: A live cell with 2 or 3 alive neighbouring cells survives
      if (cells[i][j].isCellAlive && (liveNeighbours == 2 || liveNeighbours == 3)) {
        continue;
      }
      //Rule 2:A dead cell with 3 neighbouring cells will get alive
      if (!cells[i][j].isCellAlive && liveNeighbours == 3) {
        cells[i][j].MarkAlive();
        continue;
      }

      //Rule 3: All other cells dies
      cells[i][j].MarkDead();
      }
    }

    //All cells are marked so update all cells.
      for (int i = 0; i < cells.Length; i++) {
        for (int j = 0; j < cells[i].Length; j++) {
          cells[i][j].UpdateCell();
        }
      } 

}

  private void Awake()
  {
    CreateCells();
    Initiate();
  }

  private void Update()
  {
    time++;
    if (time % 6 == 0)
    {
      UpdateCells();
    }

    if (time > 100)
      time = 0;
  }
}
