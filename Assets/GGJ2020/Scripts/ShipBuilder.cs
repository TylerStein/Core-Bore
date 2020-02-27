using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShipBuilder : MonoBehaviour
{
    public GameObject panelPrefab;

    public int ringCount = 12;
    public int stackCount = 5;

    public float panelHeight = 0.39f;
    public float panelWidth = 0.557f;

    public float radius = 2f;

    public bool rebuildOnStart = true;

    public Vector3 postBuildRotation = new Vector3(0, 0, 0);

    private void Start() {
        if (rebuildOnStart) {
            DestroyShip();
            BuildShip();
        }
    }

    public void BuildShip() {
        DestroyShip();

        float baseHeight = panelHeight * 0.5f;

        Panel[,] panelGrid = new Panel[ringCount, stackCount];
        Vector3 localUp = transform.up;
        Vector3 localRight = transform.right;
        Vector3 localForward = transform.forward;

        transform.rotation = Quaternion.identity;
        
        for (int y = 0; y < stackCount; y++) {
            for (int i = 0; i < ringCount; i++) {
                float angle = i * Mathf.PI * 2 / ringCount;
                float x = Mathf.Cos(angle) * -(radius * 0.5f);
                float z = Mathf.Sin(angle) * -(radius * 0.5f);
                
                Vector3 position = transform.position + new Vector3(x, baseHeight + (y * panelHeight), z);

                float angleDeg = -angle * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, angleDeg, 0);

                GameObject newPanel = Instantiate(panelPrefab, position, rotation, transform);
                newPanel.transform.parent = transform;

                Panel panelComponent = newPanel.GetComponent<Panel>();
                panelComponent.SetCoordinates(i, y);
                panelGrid[i, y] = panelComponent;
            }
        }

       // transform.rotation = Quaternion.Euler(postBuildRotation);

       // transform.Rotate(postBuildRotation, Space.World);
        // transform.rot(transform.position, Quaternion.Euler(postBuildRotation));
        PanelManager.Inst.SetPanelGrid(panelGrid, ringCount, stackCount, Vector2Int.zero);
    }

    public void DestroyShip() {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}


