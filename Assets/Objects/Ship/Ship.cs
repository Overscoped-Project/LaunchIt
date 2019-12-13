using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private bool repaired = false;
    [System.Serializable] private struct SpawnQuestObject
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
    [SerializeField] List<SpawnQuestObject> spawnRequiredItems = new List<SpawnQuestObject>();
    private List<QuestObject> requiredItems = new List<QuestObject>();
    private void Start()
    {
        foreach(SpawnQuestObject obj in spawnRequiredItems)
        {
            QuestObject newObj = new QuestObject(obj.GetItem(), obj.GetRequiredAmount());
            requiredItems.Add(newObj);
        }
    }
    public bool GetRepaired()
    {
        return repaired;
    }
    
    public void RepairShip(GameObject inventory)
    {
        foreach (Slot slot in inventory.GetComponentsInChildren<Slot>())
        {
            if (!slot.GetEmpty())
            {
                foreach (QuestObject obj in requiredItems)
                {
                    if ((obj.GetRequiredAmount() != 0 ) && (slot.GetItem().Equals(obj.GetItem())))
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
            }
        }
        if (finished == requiredItems.Count)
        {
            repaired = true;
            Debug.Log(repaired);
            GameObject.Find("GameController").GetComponent<LevelManager>().GoToOutro();
        }
    }
}
