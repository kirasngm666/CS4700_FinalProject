using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_controller : MonoBehaviour
{
    public float moveSpeed = 3.0f; // set the move speed of the enemy
    public float attackDistance = 2.0f; // set the attack distance

    public GameObject player; // reference to the player object

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < attackDistance)
        {
            Attack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        animator.SetTrigger("attack"); // trigger the attack animation
        // play attack sound effect
    }
}
