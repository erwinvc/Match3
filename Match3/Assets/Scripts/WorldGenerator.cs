using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour{
    public CityGenerator cityGenerator;
    public AuroraBorealisGenerator auroraBorealisGenerator;

    [Header("Objects")]
    public GameObject sun;
    public GameObject skyFog;

    public List<WorldGenerationObject> generationObjects = new List<WorldGenerationObject>();
    public WorldGenerationData currentGenerationData;

    public void Start() {
        currentGenerationData = WorldGenerationData.GenerateRandom();
    }

    public void GenerateNoMaterials() {
        currentGenerationData = WorldGenerationData.GenerateRandom();
        foreach (var generationObject in generationObjects) {
            generationObject.Generate(currentGenerationData, generationObject.ShouldGenerate(currentGenerationData), false);
        }
    }

    public void Generate() {
        currentGenerationData = WorldGenerationData.GenerateRandom();
        foreach(var generationObject in generationObjects) { 
            generationObject.Generate(currentGenerationData, generationObject.ShouldGenerate(currentGenerationData), true);
        }
    }

    public void Cleanup() {
        foreach (var generationObject in generationObjects) {
            generationObject.Generate(currentGenerationData, false, true);
        }
    }
}
