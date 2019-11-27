using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private bool repaired = false;
    [SerializeField] private bool itemOne = false;
    [SerializeField] private bool itemTwo = false;
    [SerializeField] private bool itemThree = false;

    [SerializeField] private Item one;
    [SerializeField] private Item two;
    [SerializeField] private Item three;

    [SerializeField] private int requiredAmountOne = 1;
    [SerializeField] private int requiredAmountTwo = 1;
    [SerializeField] private int requiredAmountThree = 1;
    void Start()
    {

    }

    void Update()
    {

    }

    public bool GetRepaired()
    {
        return repaired;
    }
    
    public void RepairShip(GameObject inventory)
    {
        for (int i = 0; i < inventory.GetComponentsInChildren<Slot>().Length; i++)
        {
            Slot slot = inventory.GetComponentsInChildren<Slot>()[i];
            if (!slot.GetEmpty())
            {
                if (!itemOne && (slot.GetItem().Equals(one) && slot.GetItem().GetItemType() == one.GetItemType()))
                {                  
                    requiredAmountOne -= slot.RemoveItem(requiredAmountOne);
                    if (requiredAmountOne == 0)
                    {
                        itemOne = true;
                    }
                }
                else if (!itemTwo && (slot.GetItem() == two && slot.GetItem().GetItemType() == two.GetItemType()))
                {
                    requiredAmountTwo -= slot.RemoveItem(requiredAmountTwo);
                    if (requiredAmountTwo == 0)
                    {
                        itemTwo = true;
                    }
                }
                else if (!itemThree && (slot.GetItem() == three && slot.GetItem().GetItemType() == three.GetItemType()))
                {
                    requiredAmountThree -= slot.RemoveItem(requiredAmountThree);
                    if (requiredAmountThree == 0)
                    {
                        itemThree = true;
                    }

                }
                else
                {

                }
            }          
        }
        if (itemOne && itemTwo && itemThree)
        {
            repaired = true;
        }

    }
}
