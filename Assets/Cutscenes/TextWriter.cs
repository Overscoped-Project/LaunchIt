using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextWriter : MonoBehaviour
{

    public string[] text;
    public float[] timeTillNextTextPart;
    public TextMeshProUGUI tGUI;
    public enum Level
    {
        MainMenu,
        Settings,
        Intro,
        Game,
        Outro,
        Credits,
        None
    }
    public Level level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float timer = 0;
    public int count = 0;
    // Update is called once per frame
    void Update()
    {
        if (count < text.Length && timer >= timeTillNextTextPart[count])
        {
            timer = 0;
            tGUI.text = text[count];
            count++;
        }
        else if (count < text.Length && timer != timeTillNextTextPart[count])
        {
            timer += Time.deltaTime;
        }
        else
        {
            if(level != Level.None)
            {
                SceneManager.LoadScene((int)level, LoadSceneMode.Single);
            }
        }
    }
}
