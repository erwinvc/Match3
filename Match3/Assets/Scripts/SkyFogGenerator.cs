using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFogGenerator : WorldGenerationObject {
    public Material skyFogMaterial;
    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        gameObject.SetActive(isActive);
        if(!isActive) return;
        skyFogMaterial.SetColor("_Color", Utils.ColorWithAlpha(data.colorPrimary, Random.Range(0.2f, 0.4f)));
    }

    public override bool ShouldGenerate(WorldGenerationData data) {
        return data.generationType == WorldGenerationType.SKYFOG;
    }
}
