using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

public class CityGenerator : WorldGenerationObject {
    [Serializable]
    public struct BuildingTexture {
        public Texture albedo;
        public Texture normal;
    }

    public List<BuildingTexture> buildingMaterials;
    public GameObject buildingPrefab;
    public int width;
    public int minBuildingCount, maxBuildingCount;
    public float centerDistance, minSkylineCenterDistance, maxSkylineCenterDistance;
    public float perlinSize, perlinEdge;
    public Material buildingMaterial;
    public Material ground;
    public Material cityFog;
    public Material airFog;
    public AnimationCurve randomSizeCurve;
    [NonSerialized]
    private Vector3 cityCenterPosition = new Vector3(0.0f, 0.0f, 50.0f);


    public override bool ShouldGenerate(WorldGenerationData data) {
        return true;
    }

    private Vector3 GetRandomPosition() {
        float halfWidth = width / 2.0f;
        float x = UnityEngine.Random.Range(-halfWidth, halfWidth);
        float y = UnityEngine.Random.Range(-10, -4) + UnityEngine.Random.Range(-0.01f, 0.01f);
        float z = UnityEngine.Random.Range(0.0f, 50.0f);
        return new Vector3(x, y, z);
    }

    Material GetRandomMaterial(bool generateNewMaterials, float emissionIntensityOffset, Color color1, Color color2) {
        Material material = buildingMaterial;
        if (generateNewMaterials) {
            material = new Material(buildingMaterial);
        }
        BuildingTexture buildingTexture = buildingMaterials[UnityEngine.Random.Range(0, buildingMaterials.Count)];
        material.SetTexture("_Albedo", buildingTexture.albedo);
        material.SetTexture("_Normal", buildingTexture.normal);
        material.SetFloat("_ColorLerp", UnityEngine.Random.Range(9.0f, 11.0f));
        material.SetFloat("_EmissionAmount", UnityEngine.Random.Range(10f, 40f) + emissionIntensityOffset);

        material.SetColor("_ColorPrimary", color1);
        material.SetColor("_ColorSecondary", color2);
        //material.SetFloat("_Edge", UnityEngine.Random.Range(0.94f, 0.98f));
        material.SetFloat("_Lights", UnityEngine.Random.Range(-0.15f, 0.0f));

        return material;
    }


    public void Generate() {
        WorldGenerationData data = WorldGenerationData.GenerateRandom();
        Generate(data, true, true);
    }

    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        Cleanup();
        if (!isActive) return;
        float emissionIntensityOffset = UnityEngine.Random.Range(0f, 50f);
        int maxIterations = 10000;
        int iterations = 0;
        int count = 0;
        float skylineCenterDistance = UnityEngine.Random.Range(minSkylineCenterDistance, maxSkylineCenterDistance);
        int buildingCount = UnityEngine.Random.Range(minBuildingCount, maxBuildingCount);
        while (count < buildingCount && iterations < maxIterations) {
            Vector3 randomScale = new Vector3(randomSizeCurve.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f)), 1.0f, randomSizeCurve.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f)));
            float fadeInDuration = UnityEngine.Random.Range(1.8f, 2.2f);
            Material material = GetRandomMaterial(generateNewMaterials, emissionIntensityOffset, data.colorPrimary, data.colorSecondary);
            GameObject building = CreateBuilding(transform, GetRandomPosition(), randomScale, material, fadeInDuration, true);
            bool perlinCheck, distanceCheck;
            float distance;
            do {
                building.transform.position = GetRandomPosition();
                perlinCheck = Mathf.PerlinNoise(building.transform.position.x * perlinSize + data.seed, building.transform.position.z * perlinSize + data.seed) > perlinEdge;
                distance = Vector3.Distance(building.transform.position, cityCenterPosition);
                distanceCheck = distance < UnityEngine.Random.Range(0, centerDistance);
                iterations++;
            } while ((!distanceCheck || !perlinCheck) && iterations < maxIterations);

            int storyCount = 0;
            if (distance < UnityEngine.Random.Range(0, skylineCenterDistance)) {
                if (Utils.RandomBool()) {
                    GameObject secondBuilding = CreateBuilding(building.transform, building.transform.position + new Vector3(0.0f, 12.0f, 0.0f), randomScale, material, 0f, false);
                    storyCount++;
                    if (Utils.RandomBool()) {
                        Vector3 randomScale2 = new Vector3(randomScale.x * UnityEngine.Random.Range(0.6f, 0.9f), 1.0f, randomScale.y);
                        CreateBuilding(secondBuilding.transform, secondBuilding.transform.position + new Vector3(0.0f, 12.0f, 0.0f), randomScale2, material, 0f, false);
                        storyCount++;
                    }
                }
            }
            building.GetComponent<Building>().SetStoryCount(storyCount);

            count++;
            iterations++;
        }
    }

    private GameObject CreateBuilding(Transform parent, Vector3 position, Vector3 scale, Material material, float fadeInDuration, bool shouldFade) {
        GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
        building.transform.localScale = scale;
        building.transform.parent = parent;
        building.GetComponent<MeshRenderer>().material = material;
        building.GetComponent<Building>().SetFadeInDuration(fadeInDuration, shouldFade);
        return building;
    }

    public void Cleanup() {
        while (transform.childCount > 0) {
            GameObject child = transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }
    }

    public void OnDrawGizmos() {
        Color color = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(cityCenterPosition, centerDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cityCenterPosition, minSkylineCenterDistance);
        Gizmos.DrawWireSphere(cityCenterPosition, maxSkylineCenterDistance);
        Gizmos.color = color;

    }
}
