using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int maxHealth;
    [SerializeField] private int health = 100;
    [SerializeField] private float playerSpeed = 1;
    [SerializeField] private int aggression = 30;
    [SerializeField] private int healthReg = 30;
    private bool sprintAvailable = true;
    private bool walk = false;
    private float directionX = 0;
    private float directionY = 0;
    private bool isAttacked = false;

    private DialogueManager dialogueManager;
    private LevelManager levelManager;
    private AudioManager audioManager;
    private Animator animator;
    private Rigidbody2D body;

    [SerializeField] Texture2D cursor;
    private Slider healthBar;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        levelManager = GameObject.Find("GameController").GetComponent<LevelManager>();
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        healthBar.value = 100;
        maxHealth = health;

        Cursor.SetCursor(cursor, new Vector2(cursor.width/2, cursor.height/2), CursorMode.ForceSoftware);
    }


    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }
    void Update()
    {
        if(!isAttacked)
        {
            audioManager.Stop("EnemyShoot");
            audioManager.Stop("EnemyAttack");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            levelManager.GoToMainMenu();
        }

        Vector2 direction = Vector2.zero;

        //Player Movement
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            directionY = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
            directionY = -1f;
            walk = true;
        }
        else
        {
            directionY = 0;
        }


        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            directionX = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            directionX = -1f;
            walk = true;
        }
        else
        {
            directionX = 0;
        }
       
        direction.Normalize();
        direction *= playerSpeed;
        body.velocity = direction;

        animator.SetFloat("WalkDirectionX", directionX);       
        animator.SetFloat("WalkDirectionY", directionY);
        animator.SetBool("walk", walk);
        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
        {
            walk = false;
            //that the Entitie not has a velocity after a hit
            body.velocity = new Vector2(0, 0);
        }
        
        if (walk)
        {
            audioManager.PlayIfNot("PlayerWalk");
        } else
        {
            audioManager.Stop("PlayerWalk");
        }
    }

    public void Hit(int damage)
    {
        health -= damage;
        healthBar.value -= (100 / maxHealth) * damage;
        if (health <= 0)
        {
            levelManager.GoToDeathScreen();
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

    public void SetAttacked(bool isAttacked)
    {
        this.isAttacked = isAttacked;
    }

    public void AddHealth()
    {
        health = health + healthReg;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.value = health;
    }
}
