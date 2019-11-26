using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected enum itemTypes { Basic, Food, Ammunition, Weapon, QuestObject };
    [SerializeField] private int itemType;
    [SerializeField] private int amount;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
    public int GetItemType()
    {
        return itemType;
    }
    public void SetItemType(int itemType)
    {
        this.itemType = itemType;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public string getDescription()
    {
        return description;
    }

}
