using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FadeInManager : MonoBehaviour {
    //public Volume ppfxVolume;

    void Start() {
        foreach (Building building in FindObjectsOfType<Building>()) {
            building.FadeIn();
        }

        //UnityEngine.Rendering.Universal.Bloom bloom;
        //ppfxVolume.profile.TryGet(out bloom);
        //float backup = bloom.intensity.value;
        //bloom.intensity.value = 0.0f;
        //
        //Utils.DelayedAction(10.0f, () => {
        //    DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x, backup, 1.0f);
        //});
    }
}
