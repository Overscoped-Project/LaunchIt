using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Image icon;
    private Item item;
    private Inventory inventory;
    private bool empty = true;
    [SerializeField] private int amount = 0;

    public void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }
    public void AddItem(Item newItem)
    {
        amount = amount + newItem.GetAmount();
        icon = gameObject.GetComponent<Image>();
        item = newItem;
        icon.sprite = newItem.GetIcon();
        icon.enabled = true;
        empty = false;



    }

    public int RemoveItem(int arg)
    {
        if (amount - arg <= 0)
        {
            int temp = amount;
            ClearSlot();
            return temp;
        }
        else
        {
            amount = amount - arg;
            return arg;
        }
    }

    public void ClearSlot()
    {
        inventory.SetInvCount(inventory.GetInvCount() - 1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().RecreateInventorySlot(gameObject);
    }

    public Item GetItem()
    {
        return item;
    }

    public bool GetEmpty()
    {
        return empty;
    }
}
