using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private Dialogue dialogue;
    private DialogueManager dialogueManager;

    [SerializeField] DialogueManager.pathType path;

    private void Start()
    {
       //dialogue.trigger = gameObject;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" && !other.GetComponent<Player>().GetAttacked())
        {
            dialogueManager.StartDialogue(path, null);
            this.gameObject.SetActive(false);
        } 
    }

    /*public void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            dialogueManager.EndDialogue();
        }
    }*/
}
