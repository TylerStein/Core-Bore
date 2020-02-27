using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Inst;
        string activeSceneName = SceneManager.GetActiveScene().name;

        if(activeSceneName == "MainMenuScene")
        {
            if (audioManager) audioManager.PlayMenuScreenMusic();
        }
        else if (activeSceneName == "GameOverScene" || activeSceneName == "GameWinScene")
        {
            if (audioManager) audioManager.PlayGameOverScreenMusic();
        }
    }
    public void LoadGameScene()
    {
        if (audioManager) audioManager.StopMusic();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainScene()
    {
        if (audioManager) audioManager.StopMusic();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame() {
        Application.Quit();
    }
}