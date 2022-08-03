using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldGenerationObject : MonoBehaviour{
    public abstract void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials);
    public abstract bool ShouldGenerate(WorldGenerationData data);
}
