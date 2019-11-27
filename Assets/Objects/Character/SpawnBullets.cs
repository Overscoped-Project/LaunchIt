using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private Bullet shot;
    [SerializeField] private float fireRate = 0.5f;
    private float timeSinceShoot = 0;
    private bool canShoot = true;


    void Start()
    {
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            Shoot();
            canShoot = false;

        }
        if (!canShoot)
        {
            timeSinceShoot += fireRate *Time.deltaTime;
            if (timeSinceShoot >= 1)
            {
                timeSinceShoot = 0;
                canShoot = true;
            }
        }
    }

    public void Shoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - GetComponent<Rigidbody2D>().position;
        lookDir = lookDir.normalized;
        Quaternion q = Quaternion.Euler(0, 0, 180 - Vector2.SignedAngle(lookDir*(-1), transform.up));
        Bullet bullet = Instantiate(shot, transform.position, q) as Bullet;
        bullet.setDirection(lookDir);
    }   

    public Bullet getShot()
    {
        return shot;
    }
}
