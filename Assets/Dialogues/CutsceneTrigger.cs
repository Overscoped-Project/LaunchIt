using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] DialogueManager.pathType path;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => dialogueManager.GetReady());
        dialogueManager.StartDialogue(path, null);
    }
}
