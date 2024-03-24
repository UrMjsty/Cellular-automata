using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public enum CellState
{
    Empty,
    Normal,
    Zombie,
    Hunter
}
public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    public CellState State { get; private set; }
    public CellState nextState;
  //  public bool isCellAlive;
   // private bool markedAlive, markedDead, markedZombie;
    public Color aliveColor, deadColor, zombieColor; // Add color for Zombie cells

    private void Awake()
    {
       // state = CellState.Normal;
       // SetState(CellState.Normal);
        aliveColor.a = 1;
        deadColor.a = 1;
        zombieColor.a = 1; // Initialize alpha for Zombie color
     //   spriteRenderer.color = GetCellColor();
    }
/*
    public void MarkDead() {
        markedDead = true;
    }

    public void MarkAlive() {
        markedAlive = true;
    }

    public void MarkZombie() {
        markedZombie = true;
    }
*/
    public void UpdateCell()
    {
      //  if (nextState != CellState.Normal && nextState !=CellState.Zombie && nextState!= CellState.Hunter)
           // nextState = CellState.Empty;
        SetState(nextState);
     
    }
/*
    public void ActivateCell(CellState cs)
    {
        SetState(cs);
        markedAlive = false;
        markedDead = false;
        isCellAlive = true;

        spriteRenderer.color = GetCellColor();
    }

    public void DeactivateCell() {
        markedAlive = false;
        markedDead = false;
        isCellAlive = false;

        spriteRenderer.color = GetCellColor();
    }

    private Color GetCellColor() {
        if (isCellAlive) {
            return aliveColor;
        } else {
            // Check if it's a Zombie cell
            if (markedZombie) {
                return zombieColor;
            } else {
                return deadColor;
            }
        }
    }
*/
    public void OnPointerEnter(PointerEventData eventData) {
        if (Input.GetMouseButton(0)) {
            SetState(CellState.Zombie);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        SetState(CellState.Hunter);
    }

    public void SetState(CellState cs)
    {
        State = cs;
        switch (cs)
        {
            case CellState.Empty:
                spriteRenderer.sprite = sprites[0];
                break;
            case CellState.Normal:
                spriteRenderer.sprite = sprites[1];
                break;
            case CellState.Zombie:
                var rand = new Random().Next(4);
                spriteRenderer.sprite = sprites[4+rand];
                break;
            case CellState.Hunter: 
                rand = new Random().Next(2);
                spriteRenderer.sprite = sprites[2+rand];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cs), cs, null);
        }
        //spriteRenderer.sprite = sprites[(int)State];
        //if(cs == CellState.Normal)
          //  Debug.Log("yes");
    }
}
