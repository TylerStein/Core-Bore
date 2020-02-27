using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonController<InputManager>
{
    [SerializeField] MasterRotator mr;
    [SerializeField] Transform character;
    float timer = 0.0f;
    float turnDelay = 0.01f;
    Vector3 offset = new Vector3(0.1f, 0.0f, 0.0f);

    public Transform particleSpawnTransform;
    public GameObject particleSystemPrefab;
    public float particleLifetimes = 4f;

    public AudioManager audioManager;

    public bool enableInput = true;

    private Vector3 characterPosition;
    private Vector3 initialCharacterPosition;

    private void Start()
    {
        audioManager = AudioManager.Inst;
        if (audioManager != null) audioManager.PlayGameScreenMusic();

        //set player position when match starts
        initialCharacterPosition = character.transform.position;
        float newCharacterY = PanelManager.Inst.SelectedPanel.transform.position.y - offset.y;
        characterPosition = new Vector3(initialCharacterPosition.x, newCharacterY, initialCharacterPosition.z);
       // characterPosition =  - offset;
    }


    private void Update() {
        character.localPosition = Vector3.Lerp(character.localPosition, characterPosition, Time.deltaTime * 10f);
    }


    private void LateUpdate()
    {
        if (Input.GetButtonDown("Escape")) {
            Application.Quit();
            return;
        }

        if (timer < turnDelay) {
            timer += Time.deltaTime;
        }

        if (!enableInput) return;

        if (Input.GetButtonDown("Up") && timer >= turnDelay) {
            int lastX = PanelManager.Inst.SelectedPanel.XCoordinate;
            Panel newPanel = PanelManager.Inst.MoveSelection(1, 0);

            if (lastX != newPanel.XCoordinate) {
                mr.spinTo(true);
                timer = 0.0f;
            }
        } else if (Input.GetButtonDown("Down") && timer >= turnDelay) {
            int lastX = PanelManager.Inst.SelectedPanel.XCoordinate;
            Panel newPanel = PanelManager.Inst.MoveSelection(-1, 0);

            if (lastX != newPanel.XCoordinate) {
                mr.spinTo(false);
                timer = 0.0f;
            }
        } else if (Input.GetButtonDown("Right")) {
            int lastY = PanelManager.Inst.SelectedPanel.YCoordinate;
            Panel newPanel = PanelManager.Inst.MoveSelection(0, -1);

            if (lastY != newPanel.YCoordinate) {
                float newCharacterY = PanelManager.Inst.SelectedPanel.transform.position.y - offset.y;
                characterPosition = new Vector3(initialCharacterPosition.x, newCharacterY, initialCharacterPosition.z);
            }

            timer = 0.0f;
        } else if (Input.GetButtonDown("Left")) {
            int lastY = PanelManager.Inst.SelectedPanel.YCoordinate;
            Panel newPanel = PanelManager.Inst.MoveSelection(0, 1);

            if (lastY != newPanel.YCoordinate) {
                float newCharacterY = PanelManager.Inst.SelectedPanel.transform.position.y - offset.y;
                characterPosition = new Vector3(initialCharacterPosition.x, newCharacterY, initialCharacterPosition.z);
            }

            timer = 0.0f;
        } else if (Input.GetButtonDown("Interact")) {
            if (PanelManager.Inst.SelectedPanel.GetPanelState() == PanelState.DAMAGED) {
                PanelManager.Inst.SelectedPanel.Repair();
                if (audioManager != null) {
                    audioManager.PlayRepair();
                }

                GameObject particleObject = Instantiate(particleSystemPrefab, particleSpawnTransform);
                particleObject.GetComponent<ParticleSystem>().Play();
                Destroy(particleObject, particleLifetimes);
            }
        }
    }

}
