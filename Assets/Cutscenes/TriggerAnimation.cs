using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerAnimation : MonoBehaviour
{

    public Animator anim;
    bool started;
    public float neededTime;
    public enum Level
    {
        MainMenu,
        Settings,
        Intro,
        Game,
        Outro_SpaceShip,
        Outro_Repository,
        Credits,
        DeathScreen
    }
    public Level level;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("ScreenOn", false);
        anim.SetTrigger("Play");

        started = true;
    }

    float timer = 0;
    private void Update()
    {
        if(started) {
            if(timer >= neededTime)
            {
                timer = 0;
                started = false;
                SceneManager.LoadScene((int)level);
            } else
            {
                timer += Time.deltaTime;
            }
        }
    }

}
