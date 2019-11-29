

using UnityEngine;

public class QuestObject : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int requiredAmount;

    public QuestObject(Item item, int requiredAmount)
    {
        this.item = item;
        this.requiredAmount = requiredAmount;
    }

    public Item GetItem()
    {
        return item;
    }
    public void SetItem(Item item)
    {
        this.item = item;
    }
    public int GetRequiredAmount()
    {
        return requiredAmount;
    }
    public void SetRequiredAmount(int requiredAmount)
    {
        this.requiredAmount = requiredAmount;
    }
}
