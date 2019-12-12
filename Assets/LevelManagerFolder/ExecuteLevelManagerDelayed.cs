using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecuteLevelManagerDelayed : MonoBehaviour
{
    public float delaySeconds;
    float timer;
    public enum Level {
        MainMenu,
        Settings,
        Intro,
        Game,
        Outro,
        Credits
    }
    public Level level;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > delaySeconds)
        {
            SceneManager.LoadSceneAsync((int)level, LoadSceneMode.Single);
        }
    }
}
