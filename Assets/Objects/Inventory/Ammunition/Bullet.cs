using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 10;
    void Start()
    {
        GetComponent<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float getSpeed()
    {
        return speed;
    }


}


