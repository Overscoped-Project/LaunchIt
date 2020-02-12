using System.Collections;
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
        SceneManager.LoadScene("Outro", LoadSceneMode.Single);
    }

    public void GoToOutroRepository()
    {
        SceneManager.LoadScene("Outro", LoadSceneMode.Single);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneManager.LoadScene(previousScene);
    //        Debug.Log("Previous Scene");
    //    }
    //}
   
    private void PreviousScene()
    {
        previousScene = SceneManager.GetActiveScene().buildIndex - 1;
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}
