using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject Portal;
    public GameObject Player;
    private IEnumerator coroutine;

    private void Start()
    {
        coroutine = Teleportation();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(Input.GetKeyDown(KeyCode.Q) && other.gameObject.tag == "Player")
        {
            StartCoroutine (coroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(coroutine);
        Debug.Log("Stopped");
    }

    IEnumerator Teleportation()
    {
        Debug.Log("Started");
        yield return new WaitForSeconds(3);
        Player.transform.position = new Vector3(Portal.transform.position.x, Portal.transform.position.y, 0);
    }
}
