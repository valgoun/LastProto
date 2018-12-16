using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugUI : MonoBehaviour {

    public GameObject GameOverUI;

    public static DebugUI Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ButtonQuit ()
    {
        Application.Quit();
    }

    public void ButtonRestart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver ()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
