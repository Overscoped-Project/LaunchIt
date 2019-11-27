using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Image icon;
    private Item item;
    private bool empty = true;
    [SerializeField] private int amount = 0;

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
