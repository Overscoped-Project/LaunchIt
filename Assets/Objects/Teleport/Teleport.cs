using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject portal;
    public GameObject player;
    private IEnumerator coroutine;
    private bool inTeleport = false;


  
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && inTeleport == true)
        {
            coroutine = Teleportation();
            StartCoroutine(coroutine);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("true trigger enter");
            inTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inTeleport = false;
        StopCoroutine(coroutine);
        Debug.Log("false trigger exit");
    }

    IEnumerator Teleportation()
    {
            Debug.Log("started teleport");
            yield return new WaitForSeconds(3);
            player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y - 15);

    }
}
