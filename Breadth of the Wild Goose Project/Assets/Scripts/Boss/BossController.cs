using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;

    Transform player;
    Rigidbody2D rb;
    BossLookAtPlayer bossLookAtPlayer;

    // Called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        bossLookAtPlayer = animator.GetComponent<BossLookAtPlayer>();
    }

    // Called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
        bossLookAtPlayer.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }

    }

    // Called when a transition ends and the state machine finished evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
<<<<<<< Updated upstream
        animator.ResetTrigger("Attack");
=======
        if (Time.time - lastAttackTime > attackCooldown)
        {
            //animator.SetBool("isRunning", false);
            //animator.SetBool("isAttacking", true);
            //player.GetComponent<GooseController>().hitSide(transform.position.x > player.transform);
            gooseController.hitSide(transform.position.x > player.transform.position.x);
            //player.GetComponent<GooseController>().ApplyDamage(attackDamage);
            gooseController.ApplyDamage(attackDamage);
            lastAttackTime = Time.time;
            Debug.Log("GOOSE THE BOSS is pecking");
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
            hitCount++;
            currentHealth -= amount;
            BossUIHealthBar.instance.SetValue(currentHealth / (float)healthPool);
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
        GameManagerController.instance.BossDefeated();
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
        animator.Play("Boss_Idle", -1, 0f);

>>>>>>> Stashed changes
    }
}
