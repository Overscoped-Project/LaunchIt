using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 30;
    [SerializeField] private float range = 100f;
    private Vector3 startPosition;
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>());
        startPosition = transform.position;
    }

    void Update()
    {
        if ((transform.position.magnitude - startPosition.magnitude) > range)
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Hit(damage);
        }
        if (collision.gameObject.tag == "Entity")
        {
            collision.gameObject.GetComponent<Alien>().Hit(damage);
        }
        Destroy(this.gameObject);
    }

    public void SetDirection(Vector2 lookDir)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(lookDir.x * GetSpeed() * Time.deltaTime, lookDir.y * GetSpeed() * Time.deltaTime);
    }
}


