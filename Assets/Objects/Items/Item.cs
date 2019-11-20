using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected enum itemTypes { Basic, Food, Ammunition, Weapon, QuestObject };
    private int itemType;
    private int amount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public int getAmount()
    {
        return amount;
    }
    public void setAmount(int amount)
    {
        this.amount = amount;
    }
    public int getItenType()
    {
        return itemType;
    }
    public void setItemType(int itemType)
    {
        this.itemType = itemType;
    }
}
