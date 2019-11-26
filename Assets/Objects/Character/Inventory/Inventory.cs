using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool isPickupRange = false;
    private List<GameObject> pickupAbleObjects = new List<GameObject>();
    private bool invEnable = false;
    [SerializeField] private int invSlots = 6;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject emptySlot;
    

    void Start()
    {
        for (int i = 0; i < invSlots; i++)
        {
            Instantiate(emptySlot, GameObject.FindGameObjectWithTag("Inventory").transform);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invEnable = !invEnable;
        }
        if (invEnable)
        {
            inventory.SetActive(true);
        }
        else if (!invEnable)
        {
            inventory.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isPickupRange && (pickupAbleObjects != null))
            {
                for (int j = 0; j < pickupAbleObjects.Count; j++)
                {
                    for (int i = 0; i < invSlots; i++)
                    {
                        if (inventory.GetComponentsInChildren<Slot>()[i].getEmpty())
                        {
                            AddItem(inventory.GetComponentsInChildren<Slot>()[i], pickupAbleObjects[j].gameObject);
                        }
                        if (pickupAbleObjects.Count == 0)
                        {
                            isPickupRange = false;
                            break;
                        }                       
                    } 
                }
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        isPickupRange = true;
        pickupAbleObjects.Add(collider.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        pickupAbleObjects.Remove(collider.gameObject);
        if (pickupAbleObjects.Count == 0)
        {
            isPickupRange = false;
        }
    }

    public void AddItem(Slot slot,GameObject item)
    {
        slot.AddItem(item.GetComponent<Item>());
        Destroy(item);
    }

    public void RemoveItem(Slot slot)
    {
        slot.ClearSlot();
    }

    public void AddInventorySlot(int newSlots)
    {
        for (int i = 0; i < newSlots; i++)
        {
            Instantiate(emptySlot, GameObject.FindGameObjectWithTag("Inventory").transform);
            invSlots++;
        }      
    }
}
