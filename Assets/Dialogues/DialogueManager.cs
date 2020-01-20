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

    Dialogue currentDialogue;

    private void Start ()
    {
        sentences = new Queue<string>();
    }

    
    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        animator.SetBool("IsOpen", true);
        nameItem.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        descriptionText.text = sentence;
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

        if(currentDialogue != null && currentDialogue.destroyWhenDone)
        {
            Destroy(currentDialogue.trigger);
        }

        currentDialogue = null;
    }
}
