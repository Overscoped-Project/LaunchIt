using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private bool ready = false;

    public bool running = false;
    public Queue<string> sentences;
    public Queue<Dialogue.Names> names;

    public Text[] descriptionText;
    public Text[] nameItem;

    public Animator[] dialogBoxAnimator;
    public Animator inventoryPanelAnimator;

    Queue<Dialogue> dialogueQueue;
    Dialogue currentDialogue;

    private Inventory inventory;

    [SerializeField] private List<Dialogue> story = new List<Dialogue>();
    private int storyCount = 0;

    public enum pathType { Story, Inventory };

    Dialogue.EventCode eventCode;

    private void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<Dialogue.Names>();
        dialogueQueue = new Queue<Dialogue>();
        inventory = FindObjectOfType<Inventory>();
        ready = true;
    }

    float timer = 0;
    private void Update()
    {
        if (running)
        {
            if (timer >= 1)
            {
                DisplayNextSentence();
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }


    public void StartDialogue(pathType path, Item obj)
    {
        running = true;
        if (inventoryPanelAnimator != null)
        {
            inventoryPanelAnimator.SetBool("IsOpen", false);
        }

        switch (path)
        {

            case pathType.Story:
                dialogueQueue.Enqueue(story[storyCount]);
                if (storyCount < story.Count)
                {
                    storyCount++;
                }
                break;

            case pathType.Inventory:

                dialogueQueue.Enqueue(obj.GetDialogue());
                dialogueQueue.Enqueue(inventory.GetDialogue(obj));

                break;
        }
        SetCurrentDialogue();

    }

    public void SetCurrentDialogue()
    {
        currentDialogue = dialogueQueue.Dequeue();
        sentences.Clear();
        names.Clear();
        eventCode = Dialogue.EventCode.None;


        if (currentDialogue.getEventCode() != Dialogue.EventCode.None)
        {
            eventCode = currentDialogue.getEventCode();

            if (currentDialogue.playBefore)
            {
                ExecuteDialogEvent(eventCode);
            }
        }

        foreach (string sentence in currentDialogue.GetSentences())
        {
            sentences.Enqueue(sentence);
        }

        foreach (Dialogue.Names name in currentDialogue.GetNames())
        {
            names.Enqueue(name);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if (dialogueQueue.Count > 0)
            {
                SetCurrentDialogue();
                return;
            }
            else
            {
                EndDialogue();
                return;
            }
        }

        string sentence = sentences.Dequeue();
        Dialogue.Names name = names.Dequeue();

        if (name == Dialogue.Names.Pilot)
        {
            dialogBoxAnimator[0].SetBool("IsOpen", true);
            dialogBoxAnimator[1].SetBool("IsOpen", false);

            nameItem[0].text = name.ToString();
            descriptionText[0].text = sentence;
        }
        else
        {
            dialogBoxAnimator[0].SetBool("IsOpen", false);
            dialogBoxAnimator[1].SetBool("IsOpen", true);

            nameItem[1].text = name.ToString();
            descriptionText[1].text = sentence;
        }
    }

    public void EndDialogue()
    {
        dialogBoxAnimator[0].SetBool("IsOpen", false);
        dialogBoxAnimator[1].SetBool("IsOpen", false);
        if (inventoryPanelAnimator != null)
        {
            inventoryPanelAnimator.SetBool("IsOpen", true);
        }

        bool temp = false;
        try
        {
            if (!currentDialogue.playBefore) temp = true;

            currentDialogue = null;
            running = false;

        }
        catch (NullReferenceException e)
        {

        }


        if (temp) ExecuteDialogEvent(eventCode);
    }

    public void ExecuteDialogEvent(Dialogue.EventCode eventCode)
    {
        if (eventCode == Dialogue.EventCode.GameEnd_Spaceship)
        {
            GameObject.Find("GameController").GetComponent<LevelManager>().GoToOutroSpaceship();
        }
        else if (eventCode == Dialogue.EventCode.Access_Repository)
        {

        }
        else if (eventCode == Dialogue.EventCode.GameEnd_Repository)
        {
            GameObject.Find("GameController").GetComponent<LevelManager>().GoToOutroRepository();
        }
        else if (eventCode == Dialogue.EventCode.Credits)
        {
            GetComponent<LevelManager>().GoToCredits();
        }
        else if (eventCode == Dialogue.EventCode.Intro)
        {
            if (storyCount == story.Count)
            {
                Animator anim = GameObject.Find("AnimatedImage").GetComponent<Animator>();
                anim.SetTrigger("Play");
                anim.SetBool("ScreenOn", true);
                StartCoroutine(GetComponent<LevelManager>().GoToGame());
            }
            else
            {
                StartDialogue(pathType.Story, null);
            }
        }
        else if (eventCode == Dialogue.EventCode.Death)
        {
            Animator anim = GameObject.Find("AnimatedImage").GetComponent<Animator>();
            anim.SetTrigger("Play");
            anim.SetBool("ScreenOn", false);
        }
        else
        {

        }
    }

    public bool GetReady()
    {
        return ready;
    }


}
