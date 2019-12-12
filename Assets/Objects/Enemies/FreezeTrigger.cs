using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrigger : MonoBehaviour
{
    private bool freezed = true;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            freezed = !freezed;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }

    public bool GetFreezed()
    {
        return freezed;
    }
}
