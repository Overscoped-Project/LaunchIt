using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private int damage = 10;
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>());
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    public void SetDirection(Vector2 lookDir)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(lookDir.x * GetSpeed() * Time.deltaTime, lookDir.y * GetSpeed() * Time.deltaTime);
    }
}


