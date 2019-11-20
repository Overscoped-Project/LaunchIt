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
           InvokeRepeating("shoot", fireTime, fireRate);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CancelInvoke("shoot");
        }
    }

    public void shoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - GetComponent<Rigidbody2D>().position;
        lookDir = lookDir.normalized;
        Quaternion q = Quaternion.identity;
        q.eulerAngles = new Vector3(0, 180, 0);
        Bullet bullet = Instantiate(shot, transform.position, q) as Bullet;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(lookDir.x * bullet.getSpeed() * Time.deltaTime, lookDir.y * bullet.getSpeed() * Time.deltaTime);
    }
}
