// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Enemy_3_Controller : MonoBehaviour, IDamageable
// {
//     public float speed = 5.0f;
//     public float attackDistance = 2.0f;
//     public float attackSpeed = 1.0f;
//     public int attackDamage = 10;
//     public float attackCooldown = 2.0f;
//     public float followDistance = 10.0f;
//     public float maxHealth = 10.0f;

//     private Animator animator;
//     private GameObject player;
//     private Rigidbody rb;
//     private float currentHealth;
//     private float lastAttackTime;
    
//     public Transform playerTransform;
//     public bool isChasing;
//     public float chaseDistance;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         player = GameObject.FindGameObjectWithTag("Player");
//         rb = GetComponent<Rigidbody>();
//         currentHealth = maxHealth;
//     }

//     void Update()
//     {
//         float distance = Vector3.Distance(transform.position, player.transform.position);

//         if (distance < attackDistance)
//         {
//             Attack();
//         }
//         else if (distance < followDistance)
//         {
//             MoveTowardsPlayer();
//         }
//     }

//     void MoveTowardsPlayer()
//     {
//         // Vector3 direction = (player.transform.position - transform.position).normalized;
//         // direction.y = 0;
//         // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
//         // rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
//         // animator.SetBool("isRunning", true);
//         // animator.SetBool("isAttacking", false);
//         if (isChasing)
//         {
//             if(transform.position.x > playerTransform.position.x)
//             {
//                 transform.localScale = new Vector3(20,20,1);
//                 transform.position += Vector3.left * speed * Time.deltaTime;
//             }
//             if(transform.position.x < playerTransform.position.x)
//             {
//                 transform.localScale = new Vector3(-20,20,1);
//                 transform.position += Vector3.right * speed * Time.deltaTime;
//             }
//         } 
//         else
//         {
//             if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
//             {
//                 isChasing = true;
//             }
//         }
//     }

//     void Attack()
//     {
//         if (Time.time - lastAttackTime > attackCooldown)
//         {
//             animator.SetBool("isRunning", false);
//             animator.SetBool("isAttacking", true);
//             player.GetComponent<PlayerHealth>().ApplyDamage(attackDamage);
//             lastAttackTime = Time.time;
//         }
//         else
//         {
//             animator.SetBool("isRunning", true);
//             animator.SetBool("isAttacking", false);
//         }
//     }

//     // public void TakeDamage(float amount)
//     // {
//     //     currentHealth -= amount;
//     //     if (currentHealth <= 0)
//     //     {
//     //         Die();
//     //     }
//     // }

//     // void Die()
//     // {
//     //     animator.SetTrigger("die");
//     //     Destroy(gameObject, 2.0f);
//     // }
//     public virtual void ApplyDamage(float amount)
//     {
//         currentHealth -= amount;
//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//     }

//     private void Die()
//     {
//         gameObject.SetActive(false);
//     }

// }
