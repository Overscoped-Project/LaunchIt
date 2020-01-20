using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    
    public SpriteRenderer portal;
    public GameObject player;
    private IEnumerator coroutine;
    private bool inTeleport = false;
    public float timeToTeleport;


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
            inTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTeleport = false;
            StopCoroutine(coroutine);
        }
    }

    IEnumerator Teleportation()
    {
            yield return new WaitForSeconds(timeToTeleport);
            player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y - portal.bounds.extents.magnitude);
          

    }
}
