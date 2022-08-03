using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGenerator : WorldGenerationObject {
    public float minMaxX = 50f;
    public float minY = 2f;
    public float maxY = 18f;
    public Material sunMaterial;

    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        gameObject.SetActive(isActive);
        if(!isActive) return;
        transform.localPosition = new Vector3(Random.Range(-minMaxX, minMaxX), Random.Range(minY, maxY), 80f);
        sunMaterial.SetFloat("_LineFadeHeight", Utils.Remap(transform.localPosition.y, minY, maxY, 3f, 5f));
        sunMaterial.SetFloat("_ColorFade", Utils.Remap(transform.localPosition.y, minY, maxY, -0.3f, 0.0f));
      //  if (Utils.RandomBool()) {
            sunMaterial.SetColor("_ColorPrimary", data.colorPrimary);
            sunMaterial.SetColor("_ColorSecondary", data.colorSecondary);
       // } else {
           // sunMaterial.SetColor("_ColorPrimary", new Color(0f, 255f/26f, 255f/51f));
           // sunMaterial.SetColor("_ColorSecondary", new Color(1.0f, 255f/102f, 255f/26f));
      //  }
    }

    public override bool ShouldGenerate(WorldGenerationData data) {
        return data.generationType == WorldGenerationType.SUN;
    }
}
