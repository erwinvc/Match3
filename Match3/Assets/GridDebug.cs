using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GridDebug : MonoBehaviour {
    public GameObject debug;
    private  Element debugElement;
    private Color randomColor;

    public void Start() {
        randomColor = Random.ColorHSV();
    }

    public void SetDebug(bool toggle, Element element) {
        debug.SetActive(toggle);
        debugElement = element;
    }

    private void OnDrawGizmos() {
        if(debugElement != null) {
            Gizmos.color = randomColor;
            Gizmos.DrawLine(transform.position, debugElement.transform.position + new Vector3(0, 0, 10));
        }
    }
}
