using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3_Controller : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float attackDistance = 2.0f;
    public float attackSpeed = 1.0f;
    public int attackDamage = 10;
    public float attackCooldown = 2.0f;
    public float followDistance = 10.0f;
    public float maxHealth = 100.0f;

    private Animator animator;
    private GameObject player;
    private Rigidbody rb;
    private float currentHealth;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < attackDistance)
        {
            Attack();
        }
        else if (distance < followDistance)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
            player.GetComponent<PlayerHealth>().ApplyDamage(attackDamage);
            lastAttackTime = Time.time;
        }
        else
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        Destroy(gameObject, 2.0f);
    }

}
