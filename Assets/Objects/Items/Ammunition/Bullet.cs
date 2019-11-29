using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 100f;
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>());
    }

    void Update()
    {
        if (transform.position.magnitude > range)
        {
            Destroy(this.gameObject);
        }
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


