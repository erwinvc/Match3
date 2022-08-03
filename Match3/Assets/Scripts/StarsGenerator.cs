using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsGenerator : WorldGenerationObject {
    public Material starsMaterial;
    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        float redFactor = (data.colorPrimary.r + data.colorSecondary.r) / 2.0f;
        float blueFactor = (data.colorPrimary.b + data.colorSecondary.b) / 2.0f;
        float starColor = (redFactor - blueFactor) * 4.0f;
        starColor = Mathf.Clamp(starColor * 1.25f, -4.0f, 4.0f);

        bool hasSun = data.generationType == WorldGenerationType.SUN;
        starsMaterial.SetFloat("_StarsTemperature", starColor);
        starsMaterial.SetFloat("_Size", Random.Range(40000f, 130000f));
        starsMaterial.SetFloat("_StarIntensity", Random.Range(hasSun ? 50f : 100f, hasSun ? 100f : 200f));
    }

    public override bool ShouldGenerate(WorldGenerationData data) {
        return true;
    }
}
