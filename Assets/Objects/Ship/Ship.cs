using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Util;


public class Ship : MonoBehaviour
{
    private bool repaired = false;
    [SerializeField] private HashMap<Item, int> requiredItems = new HashMap<Item, int>();
   // private List;

    void Start()
    {

       // for (int i = 0; i < requiredItems.Count; i++)
        {
          //  requiredItems[i];
        }
    }

    void Update()
    {

    }

    public bool GetRepaired()
    {
        return repaired;
    }
    
    /*public void RepairShip(GameObject inventory)
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
                else
                {

                }
            }          
        }
        if (itemOne && itemTwo && itemThree)
        {
            repaired = true;
        }

    }*/
}
