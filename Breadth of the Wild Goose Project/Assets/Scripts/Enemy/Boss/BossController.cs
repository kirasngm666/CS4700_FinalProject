using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Controller : MonoBehaviour, IDamageable
{
    // Initialize component
    Animator animator;
    BoxCollider2D box2d;
    [SerializeField] AudioClip bossLaugh;
    private GameObject player;
    private Rigidbody rb2d;
    private GooseController gooseController;
    public Transform playerTransform;

    // Bool values
    bool IsTakingDamage;
    bool isInvincible;
    bool hitSideRight;
    public bool isChasing = false;
    private bool isAboutToAttack;

    // Boss Info
    public int healthPool = 100;
    [SerializeField] private int currentHealth;
    public float speed = 70f;
    //public float jumpForce = 350f;
    public float attackDistance = 30.0f;
    public float attackSpeed = 1.0f;
    public int attackDamage = 45;
    public float attackCooldown = 4.0f;
    private float lastAttackTime;
    private int hitCount = 0;
    public float chaseDistance;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gooseController = player.GetComponent<GooseController>();
        rb2d = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsTakingDamage)
        {
            animator.Play("Boss_Hit");
            return;
        }
        // If the boss is within range of attack
        if (isAboutToAttack)
        {
            animator.Play("Boss_Slap");
            return;
            //Attack();
        }
        // If the boss is not within range of attack
        else
        {
            BossMovement();
        } 
        
    }

    public void BossMovement()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (isChasing && Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        {
            if(transform.position.x > (playerTransform.position.x - 1.0f))
            {
                animator.Play("Boss_Run");
                transform.localScale = new Vector3(-100,70,1);
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    Attack();
                }
            }
            if(transform.position.x < (playerTransform.position.x - 1.0f))
            {
                animator.Play("Boss_Run");
                transform.localScale = new Vector3(100,70,1);
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (distance < attackDistance)
                {
                    //animator.SetActive("Boss_Run", false);
                    //animator.Play("Boss_Slap");
                    Attack();
                    //isAboutToAttack = true;
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
            //animator.SetBool("isRunning", false);
            //animator.SetBool("isAttacking", true);
            //player.GetComponent<GooseController>().hitSide(transform.position.x > player.transform);
            //player.GetComponent<GooseController>().ApplyDamage(attackDamage);
            
        }
        // else
        // {
            // animator.SetBool("Boss_Run", true);
            // animator.SetBool("isRunning", true);
            // animator.SetBool("isAttacking", false);
        // }
    }

    public virtual void ApplyDamage(int amount)
    {
        if (!isInvincible)
        {
            hitCount++;
            currentHealth -= amount;
            BossUIHealthBar.instance.SetValue((float)currentHealth / (float)healthPool);
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
            float hitForceX = 150000f;
            float hitForceY = 150000f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX,hitForceY), ForceMode.Impulse);
        }
    }

    void StopEnemyDamageAnimation()
    {
        IsTakingDamage = false;
        isInvincible = false;
        animator.Play("Boss_Idle", -1, 0f);
        SoundManager.Instance.Play(bossLaugh);
    }

    public void StartEnemyAttackAnimation()
    {
        if (!isAboutToAttack)
        {
            isAboutToAttack = true;
            gooseController.hitSide(transform.position.x > player.transform.position.x);
            gooseController.ApplyDamage(attackDamage);
            lastAttackTime = Time.time;
            Debug.Log("GOOSE THE BOSS is pecking");
        }
    }

    void StopEnemyAttackAnimation()
    {
        isAboutToAttack = false;
        animator.Play("Boss_Idle", -1, 0f);
        SoundManager.Instance.Play(bossLaugh);
    }

}
