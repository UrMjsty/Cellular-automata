using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public SpriteRenderer spriteRenderer;
    public bool isCellAlive;
    private bool markedAlive, markedDead, markedZombie;
    public Color aliveColor, deadColor, zombieColor; // Add color for Zombie cells

    private void Awake() {
        aliveColor.a = 1;
        deadColor.a = 1;
        zombieColor.a = 1; // Initialize alpha for Zombie color
        spriteRenderer.color = GetCellColor();
    }

    public void MarkDead() {
        markedDead = true;
    }

    public void MarkAlive() {
        markedAlive = true;
    }

    public void MarkZombie() {
        markedZombie = true;
    }

    public void UpdateCell() {
        if (markedAlive) {
            ActivateCell();
        }
        if (markedDead) {
            DeactivateCell();
        }
        // Reset Zombie mark after updating cell
        markedZombie = false;
    }

    public void ActivateCell() {
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
