using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
}
