using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Player player;
    private bool repaired = false;
    private int lastFinished = 0;
    private int lastI = 0;
    [SerializeField] private GameObject[] damageVFX;
    [System.Serializable]
    private struct SpawnQuestObject
    {
        [SerializeField] private Item item;
        [SerializeField] private int requiredAmount;

        public Item GetItem()
        {
            return item;
        }
        public int GetRequiredAmount()
        {
            return requiredAmount;
        }
    }
    [SerializeField] private List<SpawnQuestObject> spawnRequiredItems = new List<SpawnQuestObject>();
    private List<QuestObject> requiredItems = new List<QuestObject>();

    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        player = GetComponent<Player>();
        foreach (SpawnQuestObject obj in spawnRequiredItems)
        {
            QuestObject newObj = new QuestObject(obj.GetItem(), obj.GetRequiredAmount());
            requiredItems.Add(newObj);
        }
    }
    public bool GetRepaired()
    {
        return repaired;
    }

    public List<QuestObject> GetRequiredItems()
    {
        return requiredItems;
    }


    public void RepairShip(GameObject inventory)
    {
        foreach (Slot slot in inventory.GetComponentsInChildren<Slot>())
        {
            if (!slot.GetEmpty())
            {
                foreach (QuestObject obj in requiredItems)
                {
                    if ((obj.GetRequiredAmount() != 0) && (slot.GetItem().Equals(obj.GetItem())))
                    {
                        obj.SetRequiredAmount(obj.GetRequiredAmount() - slot.RemoveItem(obj.GetRequiredAmount()));
                    }
                    
                }
            }
        }
        int finished = 0;
        foreach (QuestObject obj in requiredItems)
        {
            if (obj.GetRequiredAmount() == 0)
            {
                finished++;
                player.AddHealth();
            }
        }
        if (lastFinished != finished)
        {
            for (int i = 0 + lastI; i < (Mathf.FloorToInt(damageVFX.Length / 100f * ((100f / requiredItems.Count) * finished))); i++)
            {
                Destroy(damageVFX[i]);
            }
            lastI = Mathf.FloorToInt(damageVFX.Length / 100f * ((100f / requiredItems.Count) * finished));


        }
        lastFinished = finished;

        if (finished == requiredItems.Count)
        {
            repaired = true;
            dialogueManager.StartDialogue(DialogueManager.pathType.Story, null);

        }
    }

    public int GetLastFinished()
    {
        return lastFinished;
    }
}
