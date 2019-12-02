using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 30f;
    [SerializeField] private int aggression = 70;
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float hitJumpBack = 3f;
    private bool canAttack = true;
    private float timeSinceAttack = 0;
    private GameObject enemy;
    void Start()
    {

    }

    void Update()
    {
        if (enemy != null)
        {
            Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), enemy.GetComponent<CircleCollider2D>());
            AttackMovement(enemy);
        }
        else
        {
            AmbientMovement();
        }

        //that the Entitie not has a velocity after a hit
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
    }

    public void Hit(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void AmbientMovement()
    {

    }

    private void AttackMovement(GameObject player)
    {
        if (canAttack)
        {
            Vector3 moveToPlayer = player.transform.position - transform.position;
            moveToPlayer = moveToPlayer.normalized;
            transform.position += new Vector3(moveToPlayer.x * speed * Time.deltaTime, moveToPlayer.y * speed * Time.deltaTime, 0);
        }
        else
        {
            timeSinceAttack += attackRate * Time.deltaTime;
            Debug.Log(timeSinceAttack);
            if (timeSinceAttack >= 1)
            {
                timeSinceAttack = 0;
                canAttack = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && GetComponent<CircleCollider2D>().IsTouching(collision.GetComponent<CapsuleCollider2D>()))
        {
            enemy = collision.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Hit(damage);
            Vector3 moveToPlayer = collision.transform.position - transform.position;
            moveToPlayer = moveToPlayer.normalized;
            transform.position -= new Vector3((moveToPlayer.x * speed * Time.deltaTime) * hitJumpBack, (moveToPlayer.y * speed * Time.deltaTime) * hitJumpBack, 0);
            canAttack = false;
        }
    }
}
