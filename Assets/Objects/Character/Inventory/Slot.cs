using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Image icon;
    private Item item;
    private bool empty = true;

    public void AddItem(Item newItem)
    {

        icon = gameObject.GetComponent<Image>();
        item = newItem;
        icon.sprite = newItem.GetIcon();
        icon.enabled = true;
        empty = false;

        

    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        empty = true;
    }

    public Item getItem()
    {
        return item;
    }

    public bool getEmpty()
    {
        return empty;
    }
}
