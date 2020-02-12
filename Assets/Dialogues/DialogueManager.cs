using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
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
    }

    float timer = 0;
    private void Update()
    {
        if(running)
        {
            if(timer >= 1)
            {
                DisplayNextSentence();
                timer = 0;
            } else
            {
                timer += Time.deltaTime;
            }
        }
    }


    public void StartDialogue(pathType path, Item obj)
    {
        running = true;
        inventoryPanelAnimator.SetBool("IsOpen", false);
        //inventoryPanel.SetActive(false);

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

        if(name == Dialogue.Names.Pilot)
        {
            dialogBoxAnimator[0].SetBool("IsOpen", true);
            dialogBoxAnimator[1].SetBool("IsOpen", false);

            nameItem[0].text = name.ToString();
            descriptionText[0].text = sentence;
        }
        else if (name == Dialogue.Names.AI)
        {
            dialogBoxAnimator[0].SetBool("IsOpen", false);
            dialogBoxAnimator[1].SetBool("IsOpen", true);

            nameItem[1].text = name.ToString();
            descriptionText[1].text = sentence;
        } else if(name == Dialogue.Names.Inventory)
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
        inventoryPanelAnimator.SetBool("IsOpen", true);

        ExecuteDialogEvent(eventCode);

        currentDialogue = null;
        running = false;
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
        else
        {

        }
    }
}
