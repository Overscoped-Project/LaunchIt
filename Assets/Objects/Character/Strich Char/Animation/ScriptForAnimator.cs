using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForAnimator : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("PRessedS", true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("PRessedS", false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("PressedW", true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("PressedW", false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("PressedA", true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("PressedA", false);
        }
    }
}
