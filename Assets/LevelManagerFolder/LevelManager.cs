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
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void GoToIntro()
    {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }


    public void GoToOutro()
    {
        SceneManager.LoadScene("Outro", LoadSceneMode.Single);
    }
    public void GoToDeathScreen()
    {
        SceneManager.LoadScene("DeathScreen", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}

