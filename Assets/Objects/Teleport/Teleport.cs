using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject Portal;
    public GameObject Player;
    private IEnumerator coroutine;
    private bool inTeleport = false;

    private void Start()
    {
        coroutine = Teleportation();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && inTeleport == true)
        {
            StartCoroutine(coroutine);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("true");
            inTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inTeleport = false;
        StopCoroutine(coroutine);
        Debug.Log("Stopped");
        Debug.Log("false");
    }

    IEnumerator Teleportation()
    {
        while (true)
        {
            Debug.Log("Started");
            yield return new WaitForSeconds(3);
            Player.transform.position = new Vector3(Portal.transform.position.x, Portal.transform.position.y, 0);
            yield return null;
        }
        
    }
}
