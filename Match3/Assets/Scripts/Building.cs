using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Building : MonoBehaviour {
    Material material;
    float offset = 0.0f;
    public float fadeInDuration = 10.0f;
    public int storyCount = 0;
    public bool shouldFade = true;

    void Start() {
        offset = UnityEngine.Random.Range(-10f, 10f);
        material = GetComponent<MeshRenderer>().material;
    }

    void Update() {
        float phase = (Mathf.Sin(Time.time / 200f + offset) + 1.0f) / 2.0f;
        material.SetFloat("_LightsRandomization", phase);
    }

    public void SetFadeInDuration(float fadeInDuration, bool shouldFade) {
        this.fadeInDuration = fadeInDuration;
        this.shouldFade = shouldFade;
    }

    public void SetStoryCount(int storyCount) {
        this.storyCount = storyCount;
    }

    public void FadeIn() {
        if(!shouldFade) return;
        float originalY = transform.position.y;
        transform.position = new Vector3(transform.position.x, -15.0f - (storyCount * 12.0f), transform.position.z);
        transform.DOLocalMoveY(originalY, fadeInDuration).SetEase(Ease.OutCirc);
    }
}
