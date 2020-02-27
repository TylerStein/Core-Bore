using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : SingletonController<PanelManager>
{
    [SerializeField] private Panel[,] panelGrid;

    [SerializeField] private Panel selectedPanel;
    public Panel SelectedPanel { get => selectedPanel; }

    [SerializeField] private List<Panel> healthyPanels;
    [SerializeField] private List<Panel> damagedPanels;
    [SerializeField] private List<Panel> destroyedPanels;

    [SerializeField] private int panelGridWidth;
    [SerializeField] private int panelGridHeight;

    public int TotalPanelCount { get => panelGridWidth * panelGridHeight; }
    public int HealthyPanelCount { get => healthyPanels.Count + (selectedPanel?.GetPanelState() == PanelState.HEALTHY ? 1 : 0); }
    public int DamagedPanelCount { get => damagedPanels.Count + (selectedPanel?.GetPanelState() == PanelState.DAMAGED ? 1 : 0); }
    public int DestroyedPanelCount { get => destroyedPanels.Count + (selectedPanel?.GetPanelState() == PanelState.DESTROYED ? 1 : 0); }

    public bool isDebugHighlightEnabled = false;

    /// <summary>
    /// Initialize the panel grid
    /// </summary>
    /// <param name="panels"></param>
    /// <param name="gridWidth"></param>
    /// <param name="gridHeight"></param>
    /// <param name="initialSelection"></param>
    public void SetPanelGrid(Panel[,] panels, int gridWidth, int gridHeight, Vector2Int initialSelection) {
        panelGrid = panels;
        panelGridWidth = gridWidth;
        panelGridHeight = gridHeight;

        healthyPanels = new List<Panel>();
        damagedPanels = new List<Panel>();
        destroyedPanels = new List<Panel>();
        for (int i = 0; i < gridWidth; i++) {
            for (int j = 0; j < gridHeight; j++) {
                if (i == initialSelection.x && j == initialSelection.y) {
                    SetSelectedPanel(i, j);
                } else {
                    AddPanelToCategory(panelGrid[i, j]);
                }
            }
        }
    }

    /// <summary>
    /// Change the current selected panel to the panel at the given coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>True if selected panel was set successfully, false if the coordinates were invalid</returns>
    public bool SetSelectedPanel(int x, int y) {
        if (x >= panelGridWidth || y >= panelGridHeight || x < 0 || y < 0) {
            // invalid position
            Debug.LogWarning($"invalid attempt to set selected panel to ({x}, {y})");
            return false;
        } else if (selectedPanel != null && (x != selectedPanel.XCoordinate && y != selectedPanel.YCoordinate)) {
            // cannot select already selected panel
            Debug.LogWarning($"invalid attempt to set selected panel to existing coords at ({x}, {y})");
            return false;
        }

        Panel potentialSelection = panelGrid[x, y];

        if (selectedPanel != null) {
            // check that this panel can be selected in its current state
            if (potentialSelection.GetPanelState() == PanelState.DESTROYED) {
                return false;
            }

            // move current selection to category list
            AddPanelToCategory(selectedPanel);
            selectedPanel.IsSelected = false;
        }

        // add new selection to category 
        RemovePanelFromCategory(potentialSelection);
        selectedPanel = potentialSelection;
        selectedPanel.IsSelected = true;

        return true;
    }

    /// <summary>
    /// Move selection by some amount, wraps x and clamps y
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>The new selected panel</returns>
    public Panel MoveSelection(int xOffset, int yOffset) {
        int projectedX = selectedPanel.XCoordinate + xOffset;
        int projectedY = selectedPanel.YCoordinate + yOffset;

        if (projectedX < 0) projectedX = panelGridWidth - 1;
        if (projectedX >= panelGridWidth) projectedX = 0;

        if (projectedY < 0) projectedY = 0;
        else if (projectedY > panelGridHeight - 1) projectedY = panelGridHeight - 1;

        SetSelectedPanel(projectedX, projectedY);
        return selectedPanel;
    }

    public Panel GetRandomHealthyPanel() {
        if (healthyPanels.Count == 0) return null;
        return healthyPanels[Mathf.RoundToInt(Random.Range(0, healthyPanels.Count - 1))];
    }

    public Panel GetRandomDamagedPanel() {
        if (damagedPanels.Count == 0) return null;
        return damagedPanels[Mathf.RoundToInt(Random.Range(0, damagedPanels.Count - 1))];
    }

    public Panel GetRandomDestroyedPanel() {
        if (destroyedPanels.Count == 0) return null;
        return destroyedPanels[Mathf.RoundToInt(Random.Range(0, destroyedPanels.Count - 1))];
    }

    public void UpdatePanelCategory(Panel panel, PanelState oldState) {
        PanelState newState = panel.GetPanelState();
        if (oldState == newState) return;
        RemovePanelFromCategory(panel, oldState);
        AddPanelToCategory(panel);
    }

    public void RemovePanelFromCategory(Panel panel) {
        PanelState panelState = panel.GetPanelState();
        if (panelState == PanelState.NONE || panel.listIndex == -1) return;

        RemovePanelFromCategory(panel, panelState);
    }

    public void RemovePanelFromCategory(Panel panel, PanelState state) {
        switch (state) {
            case PanelState.HEALTHY:
                try {
                    healthyPanels.Remove(panel);
                    panel.listIndex = -1;
                } catch (System.Exception e) {
                    Debug.LogError(e);
                }
                break;
            case PanelState.DAMAGED:
                try {
                    damagedPanels.Remove(panel);
                    panel.listIndex = -1;
                } catch (System.Exception e) {
                    Debug.LogError(e);
                }
                break;
            case PanelState.DESTROYED:
                try {
                    destroyedPanels.Remove(panel);
                    panel.listIndex = -1;
                } catch (System.Exception e) {
                    Debug.LogError(e);
                }
                break;
        }
    }

    public void AddPanelToCategory(Panel panel, PanelState state = PanelState.NONE) {
        if (state == PanelState.NONE) state = panel.GetPanelState();
        switch (state) {
            case PanelState.HEALTHY:
                panel.listIndex = healthyPanels.Count;
                healthyPanels.Add(panel);
                break;
            case PanelState.DAMAGED:
                panel.listIndex = damagedPanels.Count;
                damagedPanels.Add(panel);
                break;
            case PanelState.DESTROYED:
                panel.listIndex = destroyedPanels.Count;
                destroyedPanels.Add(panel);
                break;
        }
    }

    public Panel FindAdjacentSafePanel(Panel origin) {
        int ox = origin.XCoordinate;
        int oy = origin.YCoordinate;

        int tx = ox;
        int ty = oy;

        // right
        tx = ox;
        ty = oy - 1;
        if (ty >= 0) {
            if (panelGrid[tx, ty].GetPanelState() != PanelState.DESTROYED) return panelGrid[tx, ty];
        }

        // left
        tx = ox;
        ty = oy + 1;
        if (ty < panelGridHeight - 1) {
            if (panelGrid[tx, ty].GetPanelState() != PanelState.DESTROYED) return panelGrid[tx, ty];
        }

        // up
        tx = ox + 1;
        ty = oy;
        if (tx > panelGridWidth - 1) {
            tx = 0;
            if (panelGrid[tx, ty].GetPanelState() != PanelState.DESTROYED) return panelGrid[tx, ty];
        }

        // down
        tx = ox - 1;
        ty = oy;
        if (tx < 0) {
            tx = panelGridWidth - 1;
            if (panelGrid[tx, ty].GetPanelState() != PanelState.DESTROYED) return panelGrid[tx, ty];
        }

        return null;
    }
}
