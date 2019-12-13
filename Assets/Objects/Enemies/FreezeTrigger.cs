using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrigger : MonoBehaviour
{
    private bool freezed = true;
    private Vector3 enteredPosition;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.tag == "Player" && GetComponent<BoxCollider2D>().IsTouching(collision.GetComponent<CapsuleCollider2D>()))
        {
            enteredPosition = collision.transform.position;
        }
        */
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (collision.gameObject.tag == "Player")
        {
            Bounds bounds = GetComponent<BoxCollider2D>().bounds;
            Vector2 direction = bounds.center - enteredPosition;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    if ((collision.transform.position - enteredPosition).x > (bounds.center - enteredPosition).x + bounds.extents.x)
                    {
                        freezed = !freezed;
                    }
                }
                else
                {
                    if ((enteredPosition - collision.transform.position).x > (enteredPosition - bounds.center).x - bounds.extents.x)
                    {
                        freezed = !freezed;
                    }
                }
            }   
            else 
            {
                if (direction.y > 0)
                {
                    if ((collision.transform.position - enteredPosition).y > (bounds.center - enteredPosition).y + bounds.extents.y)
                    {
                        freezed = !freezed;
                    }
                }
                else
                {
                    if ((enteredPosition - collision.transform.position).y > (enteredPosition - bounds.center).y - bounds.extents.y)
                    {
                        freezed = !freezed;
                    }
                }   
            }
        }*/
    }

    public bool GetFreezed()
    {
        return freezed;
    }

    public void SetFreezed(bool freez)
    {
        freezed = freez;
    }
}
