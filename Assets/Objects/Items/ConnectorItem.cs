using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorItem : MonoBehaviour
{
    [SerializeField]private Item item;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public Item GetItem()
    {
        return item;
    }
}
