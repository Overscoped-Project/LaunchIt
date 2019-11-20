using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    [SerializeField] private float speed = 10;
    [SerializeField] private int damage = 10;
    void Start()
    {
        SetItemType((int)itemTypes.Ammunition);       
    }

    void Update()
    {
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetDamage()
    {
        return damage;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }

}


