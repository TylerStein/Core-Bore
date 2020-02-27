using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEventManager))]
public class GameEventManagerEditor : Editor
{
    public override void OnInspectorGUI() {
        if (GUILayout.Button("Game Over")) {
            ((GameEventManager)target).OnGameOver();
        }


        if (GUILayout.Button("Game Win")) {
            ((GameEventManager)target).OnGameWin();
        }

        if (GUILayout.Button("Break Panel")) {
            ((GameEventManager)target).BreakPanel();
        }

        DrawDefaultInspector();
    }
}
