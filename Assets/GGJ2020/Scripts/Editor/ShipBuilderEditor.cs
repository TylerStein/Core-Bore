using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipBuilder))]
public class ShipBuilderEditor : Editor
{
    public override void OnInspectorGUI() {
        if (GUILayout.Button("Build")) {
            ((ShipBuilder)target).BuildShip();
        }

        if (GUILayout.Button("Destroy")) {
            ((ShipBuilder)target).DestroyShip();
        }

        DrawDefaultInspector();
    }
}
