using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("AnimationNumber", Random.Range(0, 4));
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
