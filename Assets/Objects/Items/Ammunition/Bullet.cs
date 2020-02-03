using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 30;
    [SerializeField] private float range = 100f;
    private Vector3 startPosition;
    private GameObject origin;
    private AudioManager audioManager;
    void Start()
    {
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
        GameObject obj = collision.gameObject;
        if (obj.tag != origin.tag)
        {
            if (obj.tag == "Entity")
            {
                obj.GetComponent<Alien>().Hit(damage);
                audioManager.Play("HitEnemy");
                Destroy(this.gameObject);
            }
            else if (obj.tag == "Player")
            {
                obj.GetComponent<Player>().Hit(damage);
                audioManager.PlayIfNot("HitPlayer");
                Destroy(this.gameObject);
            }
            else
            {
                audioManager.Play("HitWall");
                Destroy(this.gameObject);
            }
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), obj.GetComponent<CapsuleCollider2D>());
        }
    }

    public void SetDirection(Vector2 lookDir, GameObject origin, AudioManager audioManager)
    {
        GetComponent<Rigidbody2D>().velocity = lookDir * GetSpeed();
        this.origin = origin;
        this.audioManager = audioManager;

    }

}


