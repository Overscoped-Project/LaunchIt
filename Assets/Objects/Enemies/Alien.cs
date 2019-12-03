using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 30f;
    [SerializeField] private int aggression = 70;
    [SerializeField] private int damage = 20;

    private GameObject enemy;
    [SerializeField] private float attackRate = 1f;
    private bool canAttack = true;
    private float timeSinceAttack = 0;
    [SerializeField] private float hitJumpBack = 3f;
    
    private int ambientTime = 0;
    [SerializeField] private int maxRandomAmbientTime = 100;
    [SerializeField] private int minRandomAmbientTime = 20;
    [SerializeField] private float ambientRange = 3f;
    private Vector2 newPosition;
    void Start()
    {
        newPosition = transform.position;
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
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
        if (ambientTime == 0)
        {
            ambientTime = Random.Range(minRandomAmbientTime, maxRandomAmbientTime);
            newPosition += new Vector2(Random.Range(-ambientRange, ambientRange), Random.Range(-ambientRange, ambientRange));
            Debug.Log(newPosition);
        }
        else if ((GetComponent<Rigidbody2D>().position.x >= newPosition.x + 1 || GetComponent<Rigidbody2D>().position.x <= newPosition.x - 1) && (GetComponent<Rigidbody2D>().position.y >= newPosition.y + 1 || GetComponent<Rigidbody2D>().position.y <= newPosition.y - 1))
        {
            Vector2 moveToPoint = newPosition - GetComponent<Rigidbody2D>().position ;
            moveToPoint = moveToPoint.normalized;
            GetComponent<Rigidbody2D>().position += new Vector2(moveToPoint.x * (speed / 2) * Time.deltaTime, moveToPoint.y * (speed / 2) * Time.deltaTime);
        }
        else
        {
            ambientTime--;
        }


    }

    private void AttackMovement(GameObject player)
    {
        if (canAttack)
        {
            Vector2 moveToPlayer = player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
            moveToPlayer = moveToPlayer.normalized;
            GetComponent<Rigidbody2D>().position += new Vector2(moveToPlayer.x * speed * Time.deltaTime, moveToPlayer.y * speed * Time.deltaTime);
        }
        else
        {
            timeSinceAttack += attackRate * Time.deltaTime;
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
            //Little Knockback
            Vector3 moveToPlayer = collision.transform.position - transform.position;
            moveToPlayer = moveToPlayer.normalized;
            GetComponent<Rigidbody2D>().position -= new Vector2((moveToPlayer.x * speed * Time.deltaTime) * hitJumpBack, (moveToPlayer.y * speed * Time.deltaTime) * hitJumpBack);

            canAttack = false;
        }
    }
}
