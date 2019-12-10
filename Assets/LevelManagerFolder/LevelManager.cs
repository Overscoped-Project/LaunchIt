using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int previousScene;

    public void GoToGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void PreviousScene()
    {
        previousScene = SceneManager.GetActiveScene().buildIndex - 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("loaded");
            SceneManager.LoadScene(previousScene);
        }
    }
}
