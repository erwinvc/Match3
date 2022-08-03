using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
[CanEditMultipleObjects]
public class WorldGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        WorldGenerator generator = target as WorldGenerator;

        if (GUILayout.Button("Generate World")) {
            generator.Generate();
        }
        if (GUILayout.Button("Generate World Default Materials")) {
            generator.GenerateNoMaterials();
        }
        if (GUILayout.Button("Cleanup")) {
            generator.Cleanup();
        }

        base.OnInspectorGUI();
    }
}
