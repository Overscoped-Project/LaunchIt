using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private Bullet shot;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float fireTime = 0.0000001f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           InvokeRepeating("Shoot", fireTime, fireRate);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CancelInvoke("Shoot");
        }
    }

    public void Shoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - GetComponent<Rigidbody2D>().position;
        lookDir = lookDir.normalized;
        Quaternion q = Quaternion.Euler(0, 0, 180 - Vector2.SignedAngle(lookDir*(-1), transform.up));
        Debug.Log(Vector2.SignedAngle(lookDir, transform.up));
        Bullet bullet = Instantiate(shot, transform.position, q) as Bullet;
        bullet.setDirection(lookDir);       
    }   

    public Bullet getShot()
    {
        return shot;
    }
}
