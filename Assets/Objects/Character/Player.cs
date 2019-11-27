using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float playerSpeed = 1;
    private bool sprintAvailable = true;
    [SerializeField] private float sprintMultiplier = 1;
     void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {

        }

        //Player Movement

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, playerSpeed * Time.deltaTime, 0);
           
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, playerSpeed * Time.deltaTime, 0);
        }


        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(playerSpeed * Time.deltaTime, 0, 0);
        }

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

    }

}
