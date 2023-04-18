using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy_Controller : MonoBehaviour, IDamageable
{
    bool isInvincible;
    
    public float healthPool = 100f;
    public float speed = 100f;
    public float jumpForce = 350f;
    //public float groundedLeeway = 0.1f;

    public float attackDistance = 10.0f;
    public float attackSpeed = 1.0f;
    public int attackDamage = 10;
    public float attackCooldown = 1.0f;

    [SerializeField] private float currentHealth;
    private float lastAttackTime;

    private GameObject player;
    private Rigidbody rb;
    private GooseController gooseController;
    public Transform playerTransform;

    public bool isChasing = false;
    public float chaseDistance;

    public Transform[] patrolPoints;
    public int patrolDestination;

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

        if (isChasing && Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        {
            if(transform.position.x > (playerTransform.position.x - 1.0f))
            {
                transform.localScale = new Vector3(-55,30,1);
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    Attack();
                }
            }
            if(transform.position.x < (playerTransform.position.x - 1.0f))
            {
                transform.localScale = new Vector3(55,30,1);
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
            else
            {
                isChasing = false;
                if (patrolDestination == 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, speed * Time.deltaTime);
                    if(Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
                    {
                        transform.localScale = new Vector3(55, 30 ,1);
                        patrolDestination = 1;
                    }
                }
                if (patrolDestination == 1)
                {
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, speed * Time.deltaTime);
                    if(Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
                    {
                        transform.localScale = new Vector3(-55, 30 ,1);
                        patrolDestination = 0;
                    }
                }
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
