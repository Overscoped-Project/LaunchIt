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
        if (other.tag == "Player" && !other.GetComponent<Player>().GetAttacked() && dialogueManager.running == false)
        {
            this.gameObject.SetActive(false);
            dialogueManager.StartDialogue(path, null);
            Destroy(gameObject);
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
