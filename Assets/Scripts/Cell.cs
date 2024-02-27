using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; //an image for showing state of cell
    public bool isCellAlive; //store status of cell
    private bool markedAlive, markedDead; //we will update status of cell in two steps.
//first mark all cells dead or alive acording to it's neighbours
//then update state of cells after all cells are marked
    public Color aliveColor, deadColor; //colours we set from editor to indicate dead and alive cell

//set colour of cell at the start according to starting state
    private void Awake() {
        //image = GetComponent<Image>();
        aliveColor.a = 1;
        deadColor.a = 1;
       spriteRenderer.color = isCellAlive ? aliveColor : deadColor;
    }

//mark a cell as dead or alive. This will not change the state, just mark for later
    public void MarkDead() {
        markedDead = true;
    }

    public void MarkAlive() {
        markedAlive = true;
    }

//update the state of cell. The cell will be dead or alive if it was marked previously
    public void UpdateCell() {
        if (markedAlive) {
            ActivateCell();
        }
        if (markedDead) {
            DeactivateCell();
        }
    }

//theese methods will kill and revive cells
    public void ActivateCell() {
        markedAlive = false;
        markedDead = false;
        isCellAlive = true;

        spriteRenderer.color = aliveColor; //update graphics
    }

    public void DeactivateCell() {
        markedAlive = false;
        markedDead = false;
        isCellAlive = false;
        
        spriteRenderer.color = deadColor; //update graphics
    }

//mouse pointers input handlere. cell will switch state if clicked
    public void OnPointerEnter(PointerEventData eventData) {
        if (Input.GetMouseButton(0)) {
            if (isCellAlive) {
                DeactivateCell();
            } else {
                ActivateCell();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (isCellAlive) {
            DeactivateCell();
        } else {
            ActivateCell();
        }
    }
}
