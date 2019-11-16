using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullets : Player
{

    [SerializeField] private Bullet shot;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            InvokeRepeating("shoot", 0.0000001f, 0.2f);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CancelInvoke("shoot");
        }
    }

    public void shoot()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;
        mouse.z = 0;
        Quaternion q = Quaternion.identity;
        q.eulerAngles = new Vector3(0,180,0);
        Bullet bullet = Instantiate(shot, transform.position, q) as Bullet;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(mouse.x * bullet.getSpeed() * Time.deltaTime, mouse.y * bullet.getSpeed() * Time.deltaTime, 0);
    }
}
