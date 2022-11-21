using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NotEditable))]
public class NotEditableDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndDisabledGroup();
    }
}

