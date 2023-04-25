using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Controller : MonoBehaviour, IDamageable
{
    // Initialize component
    Animator animator;
    BoxCollider2D box2d;
    private GameObject player;
    private Rigidbody rb2d;
    private GooseController gooseController;
    public Transform playerTransform;
    private GameManagerController gameManagerController;
    private GameObject gameManager;
    public EnemyHealthBar healthBar;

    // Bool values
    bool IsTakingDamage;
    bool isInvincible;
    bool hitSideRight;
    private bool isAboutToAttack;

    // Enemy Info
    public int healthPool = 10;
    [SerializeField] private int currentHealth;
    public float speed = 100f;
    public float jumpForce = 350f;
    //public float groundedLeeway = 0.1f;
    public float attackDistance = 25.0f;
    public float attackSpeed = 1.0f;
    public int attackDamage = 2;
    public float attackCooldown = 2.0f;
    private float lastAttackTime;
    public bool isChasing = false;
    //public bool isMoving = false;
    public float chaseDistance;
    public Transform[] patrolPoints;
    public int patrolDestination;
    //private int hitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        healthBar.SetMaxHealth(healthPool);
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gooseController = player.GetComponent<GooseController>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        gameManagerController = gameManager.GetComponent<GameManagerController>();
        rb2d = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsTakingDamage)
        {
            animator.Play("Enemy_Hit");
            return;
        }
        
        if (isAboutToAttack)
        {
            animator.Play("Enemy_Peck");
            return;
            //Attack();
        }
        else
        {
            EnemyMovement();
        } 
        
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
            else
            {
                isChasing = false;
                if (patrolDestination == 0)
                {
                    animator.Play("Enemy_Run");
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, speed * Time.deltaTime);
                    if(Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
                    {
                        transform.localScale = new Vector3(45, 20 ,1);
                        patrolDestination = 1;
                    }
                }
                if (patrolDestination == 1)
                {
                    animator.Play("Enemy_Run");
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, speed * Time.deltaTime);
                    if(Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
                    {
                        transform.localScale = new Vector3(-45, 20 ,1);
                        patrolDestination = 0;
                    }
                }
            }
        }
    }
    public void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            StartEnemyAttackAnimation();
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

    public void StartEnemyAttackAnimation()
    {
        if (!isAboutToAttack)
        {
            isAboutToAttack = true;
            gooseController.hitSide(transform.position.x > player.transform.position.x);
            gooseController.ApplyDamage(attackDamage);
            lastAttackTime = Time.time;
            Debug.Log("The ENEMY GOOSE is pecking");
        }
    }

    void StopEnemyAttackAnimation()
    {
        isAboutToAttack = false;
        animator.Play("Enemy_Idle", -1, 0f);
        //SoundManager.Instance.Play(bossLaugh);
    }
}
