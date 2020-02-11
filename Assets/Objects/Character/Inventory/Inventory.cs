using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> questObjects = new List<Item>();
    [SerializeField] private int invSlots = 6;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject emptySlot;
    private int invCount = 0;
    private Ship ship;
    private Transform inventoryTransform;
    private AudioManager audioManager;

    private DialogueManager dialogueManager;

    void Start()
    {
        inventoryTransform = GameObject.FindGameObjectWithTag("Inventory").transform;
        for (int i = 0; i < invSlots; i++)
        {
            Instantiate(emptySlot, inventoryTransform);
        }
        audioManager = FindObjectOfType<AudioManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        inventory.SetActive(true);
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Item"))
        {
            for (int i = 0; i < invSlots; i++)
            {
                if (inventory.GetComponentsInChildren<Slot>()[i].GetEmpty())
                {
                    AddItem(inventory.GetComponentsInChildren<Slot>()[i], collider.gameObject);
                    invCount++;
                    dialogueManager.StartDialogue(DialogueManager.pathType.Inventory, collider.gameObject.GetComponent<ConnectorItem>().GetItem());

                    break;
                }
            }
        }
        if (collider.gameObject.CompareTag("Ship"))
        {
            ship = collider.gameObject.GetComponent<Ship>();
            ship.RepairShip(inventory);
        }

    }


    public void AddItem(Slot slot, GameObject item)
    {
        slot.AddItem(item.GetComponent<ConnectorItem>().GetItem());
        Destroy(item);
        audioManager.Play("ItemCollect");
    }

    public void RemoveItem(Slot slot)
    {
        slot.ClearSlot();
        audioManager.Play("ItemUse");
    }

    public void AddInventorySlot(int newSlots, bool addSlot)
    {
        for (int i = 0; i < newSlots; i++)
        {
            Instantiate(emptySlot, inventoryTransform);
            if (addSlot)
            {
                invSlots++;
            }
        }
    }

    public void RecreateInventorySlot(GameObject slot)
    {
        Destroy(slot);
        AddInventorySlot(1, false);
    }

    public void SetInvCount(int invCount)
    {
        this.invCount = invCount;
    }

    public int GetInvCount()
    {
        return invCount;
    }


    public Dialogue GetDialogue(Item obj)
    {
        string name = obj.GetName();
        int count = FindObjectOfType<Ship>().GetRequiredItems().Count - invCount;
        string[] text;
        if (count == 0)
        {
            text = new string[3];
            text[0] = "You found " + name + ".";
            text[1] = "Now we can repair our ship.";
            text[2] = "Let us return to it!";
        }
        else
        {
            text = new string[2];
            text[0] = "You found " + name + ".";
            text[1] = "You only need " + count + " more Supplies.";
        }

        Dialogue.Names[] names = new Dialogue.Names[2];
        names[0] = Dialogue.Names.Inventory;
        names[1] = Dialogue.Names.Inventory;
        Dialogue invDialogue = new Dialogue(names, text);

       return invDialogue;
    }
}
