using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float playerSpeed = 1;
    private bool sprintAvailable = true;
    [SerializeField] private float sprintMultiplier = 1;
    private bool walk = false;
    private float directionX = 0;
    private float directionY = 0;
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
            directionY = 1f;
            walk = true;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, playerSpeed * Time.deltaTime, 0);
            directionY = -1f;
            walk = true;
        }
        else
        {
            directionY = 0;
        }


        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
            directionX = 1f;
            walk = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(playerSpeed * Time.deltaTime, 0, 0);
            directionX = -1f;
            walk = true;
        }
        else
        {
            directionX = 0;
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

        GetComponent<Animator>().SetFloat("WalkDirectionX", directionX);
        GetComponent<Animator>().SetFloat("WalkDirectionY", directionY);
        GetComponent<Animator>().SetBool("walk", walk);
        walk = false;
    }

}
