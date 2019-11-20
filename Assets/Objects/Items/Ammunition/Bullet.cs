using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    [SerializeField] private float speed = 10;
    [SerializeField] private int damage = 10;
    void Start()
    {
        setItemType((int)itemTypes.Ammunition);       
    }

    void Update()
    {
    }

    public float getSpeed()
    {
        return speed;
    }

    public int getDamage()
    {
        return damage;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }

}


