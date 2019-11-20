using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private bool isPickupRange = false;
    private List<GameObject> pickupAbleObjects = new List<GameObject>();
    private bool invEnable = false;
    [SerializeField] private int invSlots = 6;
    private List<Item> stored;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject emptySlot;
    

    void Start()
    {
        stored = new List<Item>();
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
                for (int i = 0; i < pickupAbleObjects.Count; i++)
                {
                    if (stored.Count < invSlots)
                    {
                        Item item = pickupAbleObjects[i].gameObject.GetComponent<Item>();
                        AddItem(item);
                        pickupAbleObjects.Remove(pickupAbleObjects[i]);
                    }
                    

                    if (pickupAbleObjects.Count == 0)
                    {
                        isPickupRange = false;
                    }
                    
                }
            }

        }

    }



    private void OnTriggerEnter(Collider collider)
    {
        isPickupRange = true;
        pickupAbleObjects.Add(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider)
    {
        pickupAbleObjects.Remove(collider.gameObject);
        if (pickupAbleObjects.Count == 0)
        {
            isPickupRange = false;
        }
    }

    public void AddItem(Item item)
    {
        stored.Add(item);      
    }

    public Item GetItem(int i)
    {
        return stored[i];
    }

    public void RemoveItem(Item item)
    {
        stored.Remove(item);
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
