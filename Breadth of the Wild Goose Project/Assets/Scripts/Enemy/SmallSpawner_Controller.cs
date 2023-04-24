using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSpawner_Controller : MonoBehaviour, IDamageable
{
    Animator animator;
    BoxCollider2D box2d;

    bool IsTakingDamage;
    bool isInvincible;
    bool hitSideRight;
    

    public int healthPool = 12;
    public float speed = 100f;
    public float jumpForce = 350f;
    //public float groundedLeeway = 0.1f;

    public float attackDistance = 25.0f;
    public float attackSpeed = 2.0f;
    public int attackDamage = 2;
    public float attackCooldown = 1.0f;

   [SerializeField] private int currentHealth;
    private float lastAttackTime;

    private GameObject player;
    private Rigidbody rb2d;
    private GooseController gooseController;
    public Transform playerTransform;

    public bool isChasing = false;
    //public bool isMoving = false;
    public float chaseDistance = 300f;

    // public Transform[] patrolPoints;
    // public int patrolDestination;
    // private GameObject[] GameObjPatrolPoint;

    private GameManagerController gameManagerController;
    private GameObject gameManager;

    public EnemyHealthBar healthBar;

    //private int hitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        healthBar.SetMaxHealth(healthPool);
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        gooseController = player.GetComponent<GooseController>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        gameManagerController = gameManager.GetComponent<GameManagerController>();
        rb2d = GetComponent<Rigidbody>();

        // GameObjPatrolPoint[0] = GameObject.FindGameObjectWithTag("Fix Patrol A");
        // GameObjPatrolPoint[1] = GameObject.FindGameObjectWithTag("Fix Patrol B");
        // patrolPoints[0] = GameObjPatrolPoint[0].GetComponent<Transform>();
        // patrolPoints[1] = GameObjPatrolPoint[1].GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsTakingDamage)
        {
            animator.Play("Enemy_Hit");
            return;
        }
        EnemyMovement();
        
    }

    void EnemyMovement()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (isChasing && Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        {
            if(transform.position.x > (playerTransform.position.x - 1.0f))
            {
                animator.Play("Enemy_Run");
                transform.localScale = new Vector3(-45,20,1);
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    Attack();
                }
            }
            if(transform.position.x < (playerTransform.position.x - 1.0f))
            {
                animator.Play("Enemy_Run");
                transform.localScale = new Vector3(45,20,1);
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
            // else
            // {
            //     isChasing = false;
            //     if (patrolDestination == 0)
            //     {
            //         animator.Play("Enemy_Run");
            //         transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, speed * Time.deltaTime);
            //         if(Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
            //         {
            //             transform.localScale = new Vector3(45, 20 ,1);
            //             patrolDestination = 1;
            //         }
            //     }
            //     if (patrolDestination == 1)
            //     {
            //         animator.Play("Enemy_Run");
            //         transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, speed * Time.deltaTime);
            //         if(Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
            //         {
            //             transform.localScale = new Vector3(-45, 20 ,1);
            //             patrolDestination = 0;
            //         }
            //     }
            // }
        }
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
            animator.Play("Enemy_Peck");
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

    public virtual void ApplyDamage(int amount)
    {
        //hitCount++;
        if (!isInvincible)
        {
            currentHealth -= amount;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                //if (hitCount == 3)
                //{
                    StartEnemyDamageAnimation();
                //}           
            }
        }  
    }

    private void Die()
    {
        gameObject.SetActive(false);
        gameManagerController.EnemyDefeated();
    }

    public void hitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void StartEnemyDamageAnimation()
    {
        if (!IsTakingDamage)
        {
            IsTakingDamage = true;
            isInvincible = true;
            //hitCount = 0;
            float hitForceX = 750f;
            float hitForceY = 750f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX,hitForceY), ForceMode.Impulse);
        }
    }

    void StopEnemyDamageAnimation()
    {
        
        IsTakingDamage = false;
        isInvincible = false;
        animator.Play("Enemy_Run", -1, 0f);

    }
}