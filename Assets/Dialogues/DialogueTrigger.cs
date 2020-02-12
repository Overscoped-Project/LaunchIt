using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] DialogueManager.pathType path;

    private void Start()
    {

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
}
