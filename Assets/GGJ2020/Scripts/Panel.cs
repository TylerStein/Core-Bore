using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] public Light warningLight;
    [SerializeField] public float warningLightOnTime = 0.66f;
    [SerializeField] public float warningLightOffTime = 0.33f;

    [SerializeField] public float healthyThreshold = 98f;

    [SerializeField] private int xCoordinate;
    public int XCoordinate { get => xCoordinate; }

    [SerializeField] private int yCoordinate;
    public int YCoordinate { get => yCoordinate; }

    [SerializeField] private bool isSelected = false;
    public bool IsSelected {
        get => isSelected;
        set => SetSelected(value);
    }

    [SerializeField] public int listIndex = -1;

    [SerializeField] private float health = 100f;
    public float Health {
        get => health;
        set => SetHealth(value);
    }

    public float destructionTimer = 0f;
    public float secondsToFullDestruction = 16f;
    public float secondsLightBlinkThreshold = 8f;
    public bool isLightBlinking = false;

    public GameObject fullPanel;

    public GameObject[] damagedPanels;

    public GameObject destroyedPanel;
    public GameObject debugSelect;

    private Coroutine activeBlinkRoutine;

    public void Update() {
        if (health < 98f && health > 0f) {
            // damages state control
            if (destructionTimer >= secondsLightBlinkThreshold && !isLightBlinking) {
                startLightBlinking();
            }

            if (destructionTimer >= secondsToFullDestruction) {
                PanelManager.Inst.RemovePanelFromCategory(this);
                SetHealth(0);
                PanelManager.Inst.AddPanelToCategory(this);
            }

            // pause countdown while selected
            if (!isSelected) destructionTimer += Time.deltaTime * GameEventManager.Inst.timescale;
        }
    }

    public PanelState GetPanelState() {
        if (health > 98f) {
            return PanelState.HEALTHY;
        } else if (health > 0f) {
            return PanelState.DAMAGED;
        } else {
            return PanelState.DESTROYED;
        }
    }

    public void SetSelected(bool selected) {
        if (PanelManager.Inst.isDebugHighlightEnabled) {
            debugSelect.SetActive(selected);
        }
        isSelected = selected;
    }

    public void SetCoordinates(int x, int y) {
        xCoordinate = x;
        yCoordinate = y;
    }
        
    public void SetHealth(float health) {
        if (health < 0f) health = 0f;
        if (health > 100f) health = 100f;

        this.health = health;
        
        if (health > 98f) {
            fullPanel.SetActive(true);
            disableAllDamagePanels();
            destroyedPanel.SetActive(false);
            disableLight();
            destructionTimer = 0f;
        } else if (health > 0f) {
            fullPanel.SetActive(false);
            enableRandomDamagePanel();
            destroyedPanel.SetActive(false);
            enableLight();
        } else {
            fullPanel.SetActive(false);
            disableAllDamagePanels();
            destroyedPanel.SetActive(true);
            disableLight();
        }
    }

    public void enableLight() {
        if (!warningLight.enabled) warningLight.enabled = true;
    }

    public void startLightBlinking() {
        if (!isLightBlinking) {
            isLightBlinking = true;
            if (activeBlinkRoutine == null) {
                activeBlinkRoutine = StartCoroutine(BlinkLight());
            }
        }
    }
    
    public void stopLightBlinking() {
        if (isLightBlinking) {
            if (activeBlinkRoutine != null) {
                StopCoroutine(activeBlinkRoutine);
                activeBlinkRoutine = null;
            }
            isLightBlinking = false;
        }
    }

    public void disableLight() {
        stopLightBlinking();
        if (warningLight.enabled) {
            warningLight.enabled = false;
        }
    }
    
    public void disableAllDamagePanels() {
        foreach (GameObject panel in damagedPanels) {
            panel.SetActive(false);
        }
    }

    public void enableRandomDamagePanel() {
        damagedPanels[Mathf.RoundToInt(Mathf.Floor(Random.Range(0, damagedPanels.Length)))].SetActive(true);
    }

    public void Repair() {
        if (GetPanelState() == PanelState.DAMAGED) {
            SetHealth(100f);
            GameEventManager.Inst.UpdateHealth();
        }
    }

    public IEnumerator BlinkLight() {
        while (isLightBlinking) {
            warningLight.enabled = true;
            yield return new WaitForSeconds(warningLightOnTime);
            warningLight.enabled = false;
            yield return new WaitForSeconds(warningLightOffTime);
        }
    }
}
