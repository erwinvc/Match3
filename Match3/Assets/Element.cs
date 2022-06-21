using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using static Match3;

public class Element : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [NonSerialized] public ElementType type;
    public bool frozen;
    [NonSerialized] public int gridY;
    [NonSerialized] public int gridX;
    public float velocity;
    
    public delegate void OnDrag(Element element, int deltaX, int deltaY);
    [NonSerialized] public OnDrag onDrag;

    public bool isDragging = false;
    Vector2 clickPosition = Vector2.zero;

    public void Start() {
        type = (ElementType)UnityEngine.Random.Range(0, 7);
        GetComponent<UnityEngine.UI.Image>().color = ElementTypeToColor(type);
    }

    public void SetTargetPosition(int x, int y, int acceleration = -100) {
        gridX = x;
        gridY = y;
        this.velocity = acceleration;
        frozen = false;
    }

    public void CustomUpdate() {
        if (isDragging) {
            float threshold = 30.0f;
            if (clickPosition.x - Input.mousePosition.x > threshold) onDrag(this, -1, 0);  // print("left");
            else if (clickPosition.x - Input.mousePosition.x < -threshold) onDrag(this, 1, 0); //  print("right");
            else if (clickPosition.y - Input.mousePosition.y > threshold) onDrag(this, 0, -1); // print("down");
            else if (clickPosition.y - Input.mousePosition.y < -threshold) onDrag(this, 0, 1); // print("up");
        }

        if (frozen) return;
        float acceleration = 2500;
        velocity -= acceleration * Time.deltaTime;
        transform.localPosition += new Vector3(0, velocity * Time.deltaTime, 0.0f);
        if (transform.localPosition.y <= gridY * Match3.elementSize) {
            transform.localPosition = new Vector3(transform.localPosition.x, gridY * Match3.elementSize, 0);
            frozen = true;
        }
    }

    public void OnPointerDown(PointerEventData data) {
        isDragging = true;
        clickPosition = data.position;
    }

    public void OnPointerUp(PointerEventData data) {
        isDragging = false;
    }
}