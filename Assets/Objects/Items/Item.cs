using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] private string name;
    public enum ItemTypes { Basic, Food, Ammunition, Weapon, QuestObject };
    [SerializeField] private ItemTypes itemType;
    [SerializeField] private int amount = 1;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private Dialogue dialogue;

    public int GetAmount()
    {
        return amount;
    }
    public ItemTypes GetItemType()
    {
        return itemType;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public string getDescription()
    {
        return description;
    }
    public Dialogue GetDialogue()
    {
        return dialogue;
    }

    public string GetName()
    {
        return name;
    }
}
