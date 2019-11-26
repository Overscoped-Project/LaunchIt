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
            if (slot.GetItem().Equals(one) && slot.GetItem().GetItemType() == one.GetItemType())
            {
                itemOne = true;
                slot.ClearSlot();
            }
            else if (slot.GetItem() == two && slot.GetItem().GetItemType() == two.GetItemType())
            {
                itemTwo = true;
                slot.ClearSlot();
            }
            else if (slot.GetItem() == three && slot.GetItem().GetItemType() == three.GetItemType())
            {
                itemThree = true;
                slot.ClearSlot();
            }
            else
            {

            }
        }
        if (itemOne && itemTwo && itemThree)
        {
            repaired = true;
        }
        Debug.Log(repaired);

    }
}
