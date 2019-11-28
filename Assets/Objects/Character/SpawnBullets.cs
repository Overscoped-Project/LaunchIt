using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private Bullet shot;
    [SerializeField] private float fireRate = 0.5f;
    private float timeSinceShoot = 0;
    private bool canShoot = true;
    private Vector2 lookDir;
    private float angle = 0;


    void Start()
    {
    }


    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mousePos - GetComponent<Rigidbody2D>().position;
        lookDir = lookDir.normalized;
        angle = 180 - Vector2.SignedAngle(lookDir * (-1), transform.up);
        GetComponent<Animator>().SetFloat("Rotation", angle);
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
        Bullet bullet = Instantiate(shot, transform.position, q);
        bullet.SetDirection(lookDir);
    }   

    public Bullet getShot()
    {
        return shot;
    }
}
