using System;
using UnityEngine;

public enum WorldGenerationType {
    SUN,
    AURORABOREALIS,
    SKYFOG
}

[Serializable]
public class WorldGenerationData {
    public int seed;
    public float huePrimary;
    public float hueSecondary;
    public Color colorPrimary;
    public Color colorSecondary;
    public WorldGenerationType generationType;

    public static WorldGenerationData GenerateRandom() {
        WorldGenerationData data = new WorldGenerationData();
        float hue = UnityEngine.Random.Range(0.0f, 1.0f);
        float offset = UnityEngine.Random.Range(0.1f, 0.25f);
        bool addition = Utils.RandomBool();
        float complimentaryHue = (hue + (addition ? offset : -offset)) % 1.0f;
        data.seed = UnityEngine.Random.Range(-5000, 5000);
        data.huePrimary = hue;
        data.hueSecondary = complimentaryHue;
        data.colorPrimary = Color.HSVToRGB(hue, 1.0f, 1.0f);
        data.colorSecondary = Color.HSVToRGB(complimentaryHue, 1.0f, 1.0f);
        data.generationType = Utils.RandomEnumValue<WorldGenerationType>();
       UnityEngine.Random.InitState(data.seed);
        return data;
    }
}