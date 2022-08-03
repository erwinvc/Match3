using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(AuroraBorealisGenerator), true)]
public class AuroraBorealisGeneratorEditor : Editor {
    BoxBoundsHandle boxBounds;

    void OnEnable() {
        boxBounds = new BoxBoundsHandle();
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnDisable() {
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    private void OnUndoRedo() {
        AuroraBorealisGenerator cSpawner = (AuroraBorealisGenerator)target;
        cSpawner.Generate();
    }

    public void OnSceneGUI() {
        AuroraBorealisGenerator cSpawner = (AuroraBorealisGenerator)target;
        boxBounds.center = cSpawner.spawnArea.center + cSpawner.transform.position;
        boxBounds.size = cSpawner.spawnArea.size;

        EditorGUI.BeginChangeCheck();
        boxBounds.DrawHandle();
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(cSpawner, "cAuroraBorealis Bounds");
            cSpawner.spawnArea.center = boxBounds.center - cSpawner.transform.position;
            cSpawner.spawnArea.size = boxBounds.size;
            cSpawner.Generate();
            Debug.Log("AuroraBorealisSpawnerSetDirty");
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnInspectorGUI() {
        AuroraBorealisGenerator cSpawner = (AuroraBorealisGenerator)target;

        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck()) {
            cSpawner.Generate();
        }
        if (GUILayout.Button("Generate")) {
            cSpawner.Generate();
        }
        if (GUILayout.Button("Cleanup")) {
            cSpawner.Cleanup();
        }

    }
}


