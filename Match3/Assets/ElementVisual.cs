using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementVisual : MonoBehaviour {
    public ParticleSystem[] pSystems;
    void Start() {

    }

    void Update() {

    }

    public void AnimationVFXEvent(int index) {
        if (Random.Range(0, 5) < 1)
            pSystems[index].Play();
    }
}
