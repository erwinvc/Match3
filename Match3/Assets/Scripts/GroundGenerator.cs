using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : WorldGenerationObject {
    public Material groundMaterial;
    public Material groundFogMaterial;
    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        groundFogMaterial.SetColor("_Color", data.colorPrimary);
        groundMaterial.SetColor("_Color", Color.HSVToRGB(data.hueSecondary, 1.0f, 0.12f));
    }

    public override bool ShouldGenerate(WorldGenerationData data) {
        return true;
    }
}
