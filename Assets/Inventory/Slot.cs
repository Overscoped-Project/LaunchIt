  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    public GameObject item;
    [SerializeField] private int id;
    public string type;
    public string description;
    public bool empty;

    public Transform slotIconG0;
    public Sprite icon;

    private void Start()
    {
        slotIconG0 = transform.GetChild(0);
    }
    public void UpdateSlot()
    {
        slotIconG0.GetComponent<Image>().sprite = icon;
    }

    public void UseItems()
    {

    }
    public int getId()
    {
        return id;
    }
    public void setId(int id)
    {
        this.id = id;
    }

}
