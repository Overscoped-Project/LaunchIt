using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float playerSpeed = 1;
    [SerializeField] private float sprintMultiplier = 1;
    [SerializeField] private int aggression = 30;
    private bool collisionUp = false;
    private bool collisionDown = false;
    private bool collisionLeft = false;
    private bool collisionRight = false;
    private bool sprintAvailable = true;
    private bool walk = false;
    private float directionX = 0;
    private float directionY = 0;
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {

        }

        //Player Movement
        if (Input.GetKey(KeyCode.W) && !collisionUp)
        {
            transform.position += new Vector3(0, playerSpeed * Time.deltaTime, 0);
            directionY = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.S) && !collisionDown)
        {
            transform.position -= new Vector3(0, playerSpeed * Time.deltaTime, 0);
            directionY = -1f;
            walk = true;
        }
        else
        {
            directionY = 0;
        }


        if (Input.GetKey(KeyCode.D) && !collisionRight)
        {
            transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
            directionX = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.A) && !collisionLeft)
        {
            transform.position -= new Vector3(playerSpeed * Time.deltaTime, 0, 0);
            directionX = -1f;
            walk = true;
        }
        else
        {
            directionX = 0;
        }


        if (Input.GetKey(KeyCode.LeftShift) && sprintAvailable)
        {
            sprintAvailable = false;
            playerSpeed *= sprintMultiplier;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && !sprintAvailable)
        {
            playerSpeed /= sprintMultiplier;
            sprintAvailable = true;
        }

        GetComponent<Animator>().SetFloat("WalkDirectionX", directionX);
        GetComponent<Animator>().SetFloat("WalkDirectionY", directionY);
        GetComponent<Animator>().SetBool("walk", walk);
        walk = false;

        //that the Entitie not has a velocity after a hit
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag != "Bullet" && collider.gameObject.tag != "Item")
        {
            Vector3 direction = collider.transform.position - collider.otherCollider.transform.position;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    collisionRight = true;
                }
                else
                {
                    collisionLeft = true;
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    collisionUp = true;
                }
                else
                {
                    collisionDown = true;
                }
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.gameObject.tag != "Bullet" && collider.gameObject.tag != "Item")
        {
            Vector3 direction = collider.transform.position - collider.otherCollider.transform.position;

            if (direction.x > 0)
            {
                collisionRight = false;
            }
            else
            {
                collisionLeft = false;
            }

            if (direction.y > 0)
            {
                collisionUp = false;
            }
            else
            {
                collisionDown = false;
            }

        }
    }

    public void Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
