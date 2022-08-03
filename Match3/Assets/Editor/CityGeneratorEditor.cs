using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CityGenerator))]
[CanEditMultipleObjects]
public class CityGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        CityGenerator generator = target as CityGenerator;
        
        if (GUILayout.Button("Generate City")) {
            generator.Generate();
        }
        if (GUILayout.Button("Cleanup")) {
            generator.Cleanup();
        }

        base.OnInspectorGUI();
    }
}