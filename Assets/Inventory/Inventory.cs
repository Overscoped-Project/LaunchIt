using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Player
{
    private bool inventoryEnabled;
    public GameObject inventory;

    private int allSlots;
    private GameObject[] slot;

    public GameObject slotHolder;



    private void Start()
    {
        allSlots = 6;
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
            if (slot[i].GetComponent<Slot>().item == null)
                slot[i].GetComponent<Slot>().empty = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryEnabled = !inventoryEnabled;

        if (inventoryEnabled == true)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isPickupRange && (pickupAbleObjects != null))
            {
                for (int i = 0; i < pickupAbleObjects.Count; i++)
                {
                    GameObject itemPickedUp = pickupAbleObjects[i];
                    Item item = itemPickedUp.GetComponent<Item>();

                    AddItem(itemPickedUp, item.type, item.description, item.icon);
                    pickupAbleObjects.Remove(pickupAbleObjects[i]);

                    if (pickupAbleObjects.Count == 0)
                    {
                        isPickupRange = false;
                    }
                }
            }

        }
        void AddItem(GameObject itemObject, string itemType, string itemDescription, Sprite itemIcon)
        {
           for (int i = 0; i < allSlots; i++)
            {
                if (slot[i].GetComponent<Slot>().empty)
                {

                    //add item to slot

                    slot[i].GetComponent<Slot>().item = itemObject;
                    slot[i].GetComponent<Slot>().type = itemType;
                    slot[i].GetComponent<Slot>().icon = itemIcon;
                    slot[i].GetComponent<Slot>().setId(i); 
                    slot[i].GetComponent<Slot>().description = itemDescription;

                    itemObject.transform.parent = slot[i].transform;
                    itemObject.SetActive(false);

                    slot[i].GetComponent<Slot>().UpdateSlot();
                    slot[i].GetComponent<Slot>().empty = false;
                    break;
                }
                
            }
        }
    } 
}
