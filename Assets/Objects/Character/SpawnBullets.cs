using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private Bullet shot;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float offsetRateX = 1.5f;
    [SerializeField] private float offsetRateY = 3.5f;
    private Vector2 offset = new Vector2(0, 0);
    private float timeSinceShoot = 0;
    private bool canShoot = true;
    private Vector2 lookDir;
    private float angle = 0;
    private Rigidbody2D body;
    private Animator animator;
    private AudioManager audioManager;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();

    }
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mousePos - body.position;
        lookDir = lookDir.normalized;

        if (Mathf.Abs(lookDir.x) > Mathf.Abs(lookDir.y))
        {
            if (lookDir.x > 0)
            {
                offset.x = offsetRateX;
            }
            else
            {
                offset.x = -offsetRateX;
            }
            offset.y = 0;
        }
        else
        {
            if (lookDir.y > 0)
            {
                offset.y = offsetRateY;
            }
            else
            {
                offset.y = -offsetRateY;
            }
            offset.x = 0;
        }
        angle = 180 - Vector2.SignedAngle(lookDir * (-1), transform.up);
        animator.SetFloat("Rotation", angle);
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            Shoot();
            canShoot = false;

        }
        if (!canShoot)
        {
            timeSinceShoot += fireRate * Time.deltaTime;
            if (timeSinceShoot >= 1)
            {
                timeSinceShoot = 0;
                canShoot = true;
            }
        }
    }

    public void Shoot()
    {
        Quaternion q = Quaternion.Euler(0, 0, angle);
        Bullet bullet = Instantiate(shot, body.position + offset, q);
        bullet.SetDirection(lookDir, this.gameObject, audioManager);
        audioManager.Play("PlayerFire");
    }   

    public Bullet getShot()
    {
        return shot;
    }
}
