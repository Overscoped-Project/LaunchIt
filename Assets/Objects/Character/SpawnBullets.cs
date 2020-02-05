using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private Bullet shot;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private Vector2 offset = new Vector2(0, 2);
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
        lookDir = mousePos - body.position + offset;
        lookDir = lookDir.normalized;     

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
