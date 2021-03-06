﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Scene scene;
    private int previousScene;


    public IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }

    public void GoToDeathScreen()
    {
        SceneManager.LoadScene("DeathScreen", LoadSceneMode.Single);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }
    public void GoToIntro()
    {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    public void GoToOutroSpaceship()
    {
        SceneManager.LoadScene("Outro_Spaceship", LoadSceneMode.Single);
    }

    public void GoToOutroRepository()
    {
        SceneManager.LoadScene("Outro_Repository", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}
