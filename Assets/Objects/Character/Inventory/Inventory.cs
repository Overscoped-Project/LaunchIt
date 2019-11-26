using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool isPickupRange = false;
    private bool isRepairRange = false;  
    private bool invEnable = false;
    private List<GameObject> pickupAbleObjects = new List<GameObject>();
    private List<Item> questObjects = new List<Item>();
    [SerializeField] private int invSlots = 6;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject emptySlot;
    private Ship ship;
    

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
        if (Input.GetKeyDown(KeyCode.F) && isPickupRange && (pickupAbleObjects != null))
        {
            for (int j = 0; j < pickupAbleObjects.Count; j++)
            {
                for (int i = 0; i < invSlots; i++)
                {
                    if (inventory.GetComponentsInChildren<Slot>()[i].GetEmpty())
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
        Debug.Log(inventory.GetComponentsInChildren<Slot>()[0].GetItem());
        if (Input.GetKeyDown(KeyCode.E) && isRepairRange && ship != null)
        {
            ship.RepairShip(inventory);
        }

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Item"))
        {
            isPickupRange = true;
            pickupAbleObjects.Add(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Ship"))
        {
            ship = collider.gameObject.GetComponent<Ship>();
            isRepairRange = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Item"))
        {
            pickupAbleObjects.Remove(collider.gameObject);
            if (pickupAbleObjects.Count == 0)
            {
                isPickupRange = false;
            }
        }
        if (collider.gameObject.CompareTag("Ship"))
        {
            ship = null;
            isRepairRange = false;
        }
    }

    public void AddItem(Slot slot,GameObject item)
    {
        slot.AddItem(item.GetComponent<Item>());        
        //TODO Destroy(item); löscht nach zeit das Item im inventar
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
