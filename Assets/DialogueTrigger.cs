using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject uiObject;

    private void Start()
    {
        uiObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            uiObject.SetActive(true);
        }

        else
        {
            if (uiObject.SetActive = true && Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(uiObject);
            }
        }
    }


}
