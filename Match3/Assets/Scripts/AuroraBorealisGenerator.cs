using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AuroraBorealisGenerator : WorldGenerationObject {
    public int minCount;
    public int maxCount;
    public float depthMultiplier;
    public float rotation;
    public GameObject auroraBorealisPrefab;
    public Bounds spawnArea = new Bounds(Vector3.zero, Vector3.one);
    public Material auroraBorealisMaterial;
    public float zIncrement = 5.0f;
    public float perlinSize, perlinEdge;

    public override bool ShouldGenerate(WorldGenerationData data) {
        return data.generationType == WorldGenerationType.AURORABOREALIS;
    }
    
    private Vector3 GetRandomPosition() {
        float x = Random.Range(spawnArea.center.x - spawnArea.size.x / 2, spawnArea.center.x + spawnArea.size.x / 2);
        float z = Random.Range(spawnArea.center.z - spawnArea.size.z / 2, spawnArea.center.z + spawnArea.size.z / 2);
        float y = Random.Range(spawnArea.center.y - spawnArea.size.y / 2, spawnArea.center.y + spawnArea.size.y / 2) + z * zIncrement;
        return new Vector3(x, y, z);
    }

    //Editor usage only
    public void Generate() {
        WorldGenerationData data = WorldGenerationData.GenerateRandom();
        Generate(data, true, true);
    }

    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        Cleanup();
        if(!isActive) return;
        int maxIterations = 10000;
        int iterations = 0;
        int count = 0;
        int goalCount = Random.Range(minCount, maxCount);
        while (count < goalCount && iterations < maxIterations) {
            GameObject obj = Instantiate(auroraBorealisPrefab, GetRandomPosition(), Quaternion.Euler(0.0f, Random.Range(-rotation, rotation), 180.0f), transform);
            bool perlinCheck;

            do {
                obj.transform.position = GetRandomPosition();
                perlinCheck = (Mathf.PerlinNoise(obj.transform.position.x * perlinSize, obj.transform.position.z * perlinSize)) > perlinEdge;
                //physicsOverlap = Physics.CheckBox(obj.transform.position + bc.center, bc.size / 2);
                iterations++;
            } while (!perlinCheck && iterations < maxIterations);

            obj.transform.localScale = new Vector3(70.0f, Random.Range(7.0f, 10.0f) * Mathf.Clamp(obj.transform.localPosition.z * depthMultiplier, 1.0f, 100.0f), 1.0f);

            if (generateNewMaterials) {
                Material material = new Material(auroraBorealisMaterial);
                material.SetFloat("_WobbleFrequency", Random.Range(0.45f, 0.85f));
                material.SetFloat("_WobbleScaleX", Random.Range(0.1f, 0.2f));
                float intensity = Mathf.Pow(2, 9);
                float hueRandom = Random.Range(-0.025f, 0.025f);
                material.SetColor("_ColorPrimary", Color.HSVToRGB(data.huePrimary + hueRandom, 1.0f, 1.0f) * intensity);
                material.SetColor("_ColorSecondary", Color.HSVToRGB(data.hueSecondary + hueRandom, 1.0f, 1.0f) * intensity);
                obj.GetComponent<MeshRenderer>().material = material;
            }
            iterations++;
            count++;
        }
    }

    public void Cleanup() {
        while (transform.childCount > 0) {
            GameObject child = transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }
    }
}
