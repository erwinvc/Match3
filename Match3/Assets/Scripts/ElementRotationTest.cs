using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRotationTest : MonoBehaviour {
    private Vector3 rot;
    void Start() {
        rot = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    void FixedUpdate() {
        transform.Rotate(rot);
    }
}
