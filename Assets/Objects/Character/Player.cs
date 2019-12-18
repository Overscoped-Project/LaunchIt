using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //DEBUG
    [SerializeField] private float sprintMultiplier = 1;
    [SerializeField] private Alien alien;
    //DEBUG
    [SerializeField] private int health = 100;
    [SerializeField] private float playerSpeed = 1;
    [SerializeField] private int aggression = 30;
    private bool collisionUp = false;
    private bool collisionDown = false;
    private bool collisionLeft = false;
    private bool collisionRight = false;
    private bool sprintAvailable = true;
    private bool walk = false;
    private float directionX = 0;
    private float directionY = 0;
    private bool isAttacked = false;
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {

        }

        Vector2 direction = Vector2.zero;

        //Player Movement
        if (Input.GetKey(KeyCode.W) && !collisionUp)
        {
            direction += Vector2.up;
            directionY = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.S) && !collisionDown)
        {
            direction += Vector2.down;
            directionY = -1f;
            walk = true;
        }
        else
        {
            directionY = 0;
        }


        if (Input.GetKey(KeyCode.D) && !collisionRight)
        {
            direction += Vector2.right;
            directionX = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.A) && !collisionLeft)
        {
            direction += Vector2.left;
            directionX = -1f;
            walk = true;
        }
        else
        {
            directionX = 0;
        }

        

        //DEBUG Controls
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
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            GetComponent<Rigidbody2D>().position = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Instantiate(alien, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }

        if (Input.GetKey(KeyCode.End))
        {
            Camera.main.orthographicSize = 40;
        }
        if (Input.GetKey(KeyCode.PageUp))
        {
            Camera.main.orthographicSize += 10;
        }
        if (Input.GetKey(KeyCode.PageDown))
        {
            if (Camera.main.orthographicSize-10 > 0)
            {
                Camera.main.orthographicSize -= 10;
            }
        }

        direction.Normalize();
        direction *= playerSpeed * Time.deltaTime;
        GetComponent<Rigidbody2D>().position += direction;
        GetComponent<Animator>().SetFloat("WalkDirectionX", directionX);
        GetComponent<Animator>().SetFloat("WalkDirectionY", directionY);
        GetComponent<Animator>().SetBool("walk", walk);
        walk = false;

        //that the Entitie not has a velocity after a hit
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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

    public int GetAggression()
    {
        return aggression;
    }
    public bool GetAttacked()
    {
        return isAttacked;
    }
}
