﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" && !other.GetComponent<Player>().GetAttacked())
        {
            Debug.Log("Bugged");
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        } 
    }

    public void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("left");
            FindObjectOfType<DialogueManager>().EndDialogue();
        }
    }
}