using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameEventManager : SingletonController<GameEventManager>
{
    public Spinnyboi drillBitSpinner;
    public VisualEffect smokeEffect;

    public float timescale = 1f;

    public float minTimeBetweenPanelDamage = 10f;
    public float maxTimeBetweenPanelDamage = 20f;

    public float tick = 0f;
    public float nextTickTarget = 0f;

    public AnimationCurve timeBetweenEvents;
    public float playTimeSeconds = 0f;

    public float totalPlaySeconds = 240f;
    public float progressPercent = 0f;

    public float gameOverSlowdownTime = 3f;
    public float gameOverPauseTime = 2f;

    public int minHealthyPanels = 6;

    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Inst;
        UpdateTickTarget();
        timescale = 1f;
        playTimeSeconds = 0f;
        progressPercent = 0f;
        GUIManager.Inst.progressBarController.SetProgress(0f);

        if (drillBitSpinner == null) drillBitSpinner = FindObjectOfType<Spinnyboi>();
    }

    void UpdateProgress() {
        progressPercent = playTimeSeconds / totalPlaySeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (tick >= nextTickTarget) {
            BreakPanel();
        } else {
            tick += Time.deltaTime * timescale;
        }

        playTimeSeconds += Time.deltaTime * timescale;
        UpdateProgress();
        GUIManager.Inst.progressBarController.SetProgress(progressPercent);
        if (progressPercent >= 1f) {
            OnGameWin();
        }
    }

    public void BreakPanel() {
        Panel healthyPanel = PanelManager.Inst.GetRandomHealthyPanel();
        if (healthyPanel != null) {
            PanelState oldState = healthyPanel.GetPanelState();
            healthyPanel.SetHealth(50);
            PanelManager.Inst.UpdatePanelCategory(healthyPanel, oldState);
            // healthyPanel.OffsetByRandomRotation(healthyPanel.damagedPanel.transform);
        }

        UpdateHealth();
        UpdateTickTarget();
        tick = 0f;
    }

    public void UpdateHealth() {
        float healthyPanelCount = PanelManager.Inst.HealthyPanelCount - minHealthyPanels;
        if (healthyPanelCount < 0f) healthyPanelCount = 0f;

        float totalPanelCount = PanelManager.Inst.TotalPanelCount;

        float health = healthyPanelCount / totalPanelCount;

       // GUIManager.Inst.damageBarController.SetHealth(healthyPanelCount / totalPanelCount);

        if (health <= 0f) {
            OnGameOver();
        }

    }

    void UpdateTickTarget() {
        float currentDifficulty = timeBetweenEvents.Evaluate(progressPercent);
        nextTickTarget = currentDifficulty;
    }


    public void OnGameOver() {
        timescale = 0f;
        InputManager.Inst.enableInput = false;
        StartCoroutine(GameOverSequence());
    }
    
    public void OnGameWin() {
        timescale = 0f;
        InputManager.Inst.enableInput = false;
        StartCoroutine(GameWinSequence());
    }

    IEnumerator GameOverSequence() {
        GameScene gs = GameScene.Inst;
        float initialBackgroundSpeed = gs.BackgroundSpeed;
        float initialDrillSpeed = drillBitSpinner.rotateSpeed;

        float lerpProgress = 0f;
        while (gs.BackgroundSpeed > 0f) {
            float newBackgroundSpeed = Mathf.Lerp(initialBackgroundSpeed, 0, lerpProgress);
            float newDrillSpeed = Mathf.Lerp(initialDrillSpeed, 0, lerpProgress);


            lerpProgress += Time.deltaTime * (1f / gameOverSlowdownTime);

            if (newBackgroundSpeed < 0.001f) newBackgroundSpeed = 0f;
            if (newDrillSpeed < 0.001f) newDrillSpeed = 0f;

            gs.BackgroundSpeed = newBackgroundSpeed;
            drillBitSpinner.rotateSpeed = newDrillSpeed;

            yield return new WaitForEndOfFrame();
        };

        yield return new WaitForSeconds(gameOverPauseTime);

        GoToGameOverScene();
    }

    IEnumerator GameWinSequence() {
        GameScene gs = GameScene.Inst;
        float initialBackgroundSpeed = gs.BackgroundSpeed;
        float initialDrillSpeed = drillBitSpinner.rotateSpeed;

        float lerpProgress = 0f;
        while (gs.BackgroundSpeed > 0f) {
            float newBackgroundSpeed = Mathf.Lerp(initialBackgroundSpeed, 0, lerpProgress);
            float newDrillSpeed = Mathf.Lerp(initialDrillSpeed, 0, lerpProgress);

            lerpProgress += Time.deltaTime * (1f / gameOverSlowdownTime);

            if (newBackgroundSpeed < 0.001f) newBackgroundSpeed = 0f;
            gs.BackgroundSpeed = newBackgroundSpeed;
            drillBitSpinner.rotateSpeed = newDrillSpeed;

            yield return new WaitForEndOfFrame();
        };

        yield return new WaitForSeconds(gameOverPauseTime);

        GoToGameWinScene();
    }

    public void GoToGameOverScene() {
        if (audioManager != null) audioManager.StopMusic();
        SceneManager.LoadScene("GameOverScene");
    }

    public void GoToGameWinScene() {
        if (audioManager != null) audioManager.StopMusic();
        SceneManager.LoadScene("GameWinScene");
    }
}
