using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpawner_Controller : MonoBehaviour, IDamageable
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
    public bool isChasing = false;
    
    // Enemy info
    public int healthPool = 25;
    [SerializeField] private int currentHealth;
    public float speed = 100f;
    public float jumpForce = 350f;
    //public float groundedLeeway = 0.1f;
    public float attackDistance = 20.0f;
    public float attackSpeed = 1.0f;
    public int attackDamage = 10;
    public float attackCooldown = 2.0f;
    private float lastAttackTime;
    private int hitCount = 0;
    public float chaseDistance = 500f;
    // public Transform[] patrolPoints;
    // public int patrolDestination;
    // private GameObject[] patrolPoint;
    

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
        
        // patrolPoint[0] = GameObject.FindGameObjectWithTag("Fix Patrol A");
        // patrolPoint[1] = GameObject.FindGameObjectWithTag("Fix Patrol B");
        // patrolPoints[0] = patrolPoint[0].GetComponent<Transform>();
        // patrolPoints[1] = patrolPoint[1].GetComponent<Transform>();
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
                transform.localScale = new Vector3(-55,30,1);
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    Attack();
                }
            }
            if(transform.position.x < (playerTransform.position.x - 1.0f))
            {
                animator.Play("Enemy_Run");
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
        if (!isInvincible)
        {
            hitCount++;
            currentHealth -= amount;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
               if (hitCount == 3)
               {
                    StartEnemyDamageAnimation(); 
               } 
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
            hitCount = 0;
            float hitForceX = 500f;
            float hitForceY = 500f;
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
            //animator.Play("Enemy_Peck");
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
