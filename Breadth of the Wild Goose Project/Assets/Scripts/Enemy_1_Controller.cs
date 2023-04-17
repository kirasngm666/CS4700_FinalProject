using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Controller : MonoBehaviour, IDamageable
{
    bool isInvincible;
    
    public float healthPool = 10f;
    public float speed = 5f;
    public float jumpForce = 6f;
    //public float groundedLeeway = 0.1f;

    public float attackDistance = 2.0f;
    public float attackSpeed = 1.0f;
    public int attackDamage = 10;
    public float attackCooldown = 2.0f;

    private float currentHealth = 10f;
    private float lastAttackTime;

    private GameObject player;
    private Rigidbody rb;
    private GooseController gooseController;
    public Transform playerTransform;

    public bool isChasing;
    public float chaseDistance;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        player = GameObject.FindGameObjectWithTag("Player");
        gooseController = player.GetComponent<GooseController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (isChasing)
        {
            if(transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(45,20,1);
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    Attack();
                }
            }
            if(transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-45,20,1);
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    Attack();
                }
            }
        } 
        else
        {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
            }
        }
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            //animator.SetBool("isRunning", false);
            //animator.SetBool("isAttacking", true);
            //player.GetComponent<GooseController>().hitSide(transform.position.x > player.transform);
            gooseController.hitSide(transform.position.x > player.transform.position.x);
            //player.GetComponent<GooseController>().ApplyDamage(attackDamage);
            gooseController.ApplyDamage(attackDamage);
            lastAttackTime = Time.time;
            Debug.Log("The ENEMY GOOSE is pecking");
        }
        else
        {
            //animator.SetBool("isRunning", true);
            //animator.SetBool("isAttacking", false);
        }
    }

    public virtual void ApplyDamage(float amount)
    {
        if (!isInvincible)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }  
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
