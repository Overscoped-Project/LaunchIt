using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public Text descriptionText;
    public Text nameItem;

    public Animator animator;

    Queue<Dialogue> dialogueQueue;
    Dialogue currentDialogue;

    private Inventory inventory;

    [SerializeField] private List<Dialogue> story = new List<Dialogue>();
    private int storyCount = 0;

    public enum pathType { Story, Inventory };

    private void Start()
    {
        sentences = new Queue<string>();
        inventory = FindObjectOfType<Inventory>();
    }


    public void StartDialogue(pathType path, Item obj)
    {
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

        animator.SetBool("IsOpen", true);
        nameItem.text = currentDialogue.name;
        sentences.Clear();

        foreach (string sentence in currentDialogue.GetSentences())
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if (dialogueQueue.Count != 0)
            {
                SetCurrentDialogue();
            }
            else
            {
                EndDialogue();
                return;
            }
        }

        string sentence = sentences.Dequeue();
        descriptionText.text = sentence;
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        currentDialogue = null;
    }
}
