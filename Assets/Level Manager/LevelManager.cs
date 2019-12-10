using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Scene scene;
    private int previousScene;


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(previousScene);
            Debug.Log("Previous Scene");
        }
    }
   
    private void PreviousScene()
    {
        previousScene = SceneManager.GetActiveScene().buildIndex - 1;
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}
