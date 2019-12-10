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

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}
