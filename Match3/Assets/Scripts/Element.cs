using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using static Match3;

public class Element : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    const float acceleration = 2500;
    public ElementType type;
    public bool frozen;
    public int gridY;
    public int gridX;
    public float velocity;
    public delegate void OnDrag(Element element, int deltaX, int deltaY);
    [NonSerialized] public OnDrag onDrag;
    [NonSerialized] public Animator animator;

    public bool isDragging = false;
    Vector2 clickPosition = Vector2.zero;

    public void Start() {
        animator = GetComponentInChildren<Animator>();
        //GetComponent<UnityEngine.UI.Image>().color = ElementTypeToColor(type);
    }

    public void SetRandomType() {
        type = (ElementType)UnityEngine.Random.Range(0, 7);
    }

    public void SetTargetPosition(int x, int y, int initialVelocity = -100) {
        gridX = x;
        gridY = y;
        velocity = initialVelocity;
        frozen = false;
    }

    public void Update() {
        if (isDragging) {
            float threshold = 30.0f;
            float differenceX = clickPosition.x - Input.mousePosition.x;
            float differenceY = clickPosition.y - Input.mousePosition.y;
            if (differenceX > threshold) onDrag(this, -1, 0);  // print("left");
            else if (differenceX < -threshold) onDrag(this, 1, 0); //  print("right");
            else if (differenceY > threshold) onDrag(this, 0, -1); // print("down");
            else if (differenceY < -threshold) onDrag(this, 0, 1); // print("up");
        }

        if (frozen) return;
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