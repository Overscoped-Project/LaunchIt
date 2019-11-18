using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float playerSpeed = 1;
    private bool isPickupRange = false;
    private bool sprintAvailable = true;
    [SerializeField] private float sprintMultiplier = 1;
    private List<GameObject> pickupAbleObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            player.transform.position += new Vector3(0, playerSpeed * Time.deltaTime, 0);
           
        }
        else if(Input.GetKey(KeyCode.S))
        {
            player.transform.position -= new Vector3(0, playerSpeed * Time.deltaTime, 0);
        }


        if (Input.GetKey(KeyCode.D))
        {
            player.transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            player.transform.position -= new Vector3(playerSpeed * Time.deltaTime, 0, 0);
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



    private void OnTriggerEnter(Collider collider)
    {
        isPickupRange = true;
        pickupAbleObjects.Add(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider)
    {   
        pickupAbleObjects.Remove(collider.gameObject);
        if (pickupAbleObjects.Count == 0)
        {
            isPickupRange = false;
        }
    }
    
    
}
