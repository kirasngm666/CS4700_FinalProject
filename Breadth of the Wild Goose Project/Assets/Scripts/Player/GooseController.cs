using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseController : MonoBehaviour, IDamageable
{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;

    // Movement
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float jumpSpeed = 3.7f;
    
    // Key Input
    float keyHorizontal;
    bool keyJump;
    bool keyPeck;

    bool isGrounded;
    bool isPecking;
    bool isFacingRight;

    // Combat related
    float peckTime;
    bool keyPeckRelease;

    public Transform meleeAttackOrigin = null;
    public float meleeAttackRadius = 0.6f;
    public float meleeDamage = 2f;
    public float meleeAttackDelay = 1.1f;
    public LayerMask enemyLayer = 10;
    private float timeUntilMeleeReadied = 0;
    public float healthPool = 200;
    public float currentHealth;

    bool IsTakingDamage;
    bool isInvincible;
    bool hitSideRight;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        // sprite defaults to facing right
        isFacingRight = true;

        
    }

    private void FixedUpdate() //Check to see if we are on the ground or not
    {
        isGrounded = false;
        Color raycastColor;
        RaycastHit2D raycastHit;
        float raycastDistance = 0.05f;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        // ground check
        Vector3 box_origin = box2d.bounds.center;
        box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);
        Vector3 box_size = box2d.bounds.size;
        box_size.y = box2d.bounds.size.y / 4f;
        raycastHit = Physics2D.BoxCast(box_origin, box_size, 0f, Vector2.down, raycastDistance, layerMask);
        // player box colliding with ground layer
        if (raycastHit.collider != null)
        {
            isGrounded = true;
        }
        // draw debug lines
        raycastColor = (isGrounded) ? Color.green : Color.red;
        Debug.DrawRay(box_origin + new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, box2d.bounds.extents.y / 4f + raycastDistance), Vector2.right * (box2d.bounds.extents.x * 2), raycastColor);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsTakingDamage)
        {
            animator.Play("Player_Hit");
            return;
        }
        PlayerDirectionInput();
        PlayerPeckInput();
        PlayerMovement();
    }

    void OnDrawGizmosSelected() 
    {
        if (meleeAttackOrigin != null)
        {
            Gizmos.DrawWireSphere(meleeAttackOrigin.position, meleeAttackRadius);
        }
    }
    
    void PlayerDirectionInput()
    {
        // get keyboard input
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        keyJump = Input.GetKeyDown(KeyCode.Space);
        keyPeck = Input.GetKey(KeyCode.C);
    }

    void PlayerPeckInput()
    {
        float peckTimeLength = 0;
        float keyPeckReleaseTimeLength = 0;

        // peck key is being pressed and key release flag true
        if (keyPeck && keyPeckRelease)
        {
            isPecking = true;
            keyPeckRelease = false;
            peckTime = Time.time;
            // peck Bullet
            Debug.Log("The goose is pecking");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(meleeAttackOrigin.position, meleeAttackRadius, enemyLayer);
            for (int i = 0; i < overlappedColliders.Length; i--)
            {
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
                if (enemyAttributes != null)
                {
                    enemyAttributes.ApplyDamage(meleeDamage);
                }
                timeUntilMeleeReadied = meleeAttackDelay;
            }
        }
        // peck key isn't being pressed and key release flag is false
        if (!keyPeck && !keyPeckRelease)
        {
            keyPeckReleaseTimeLength = Time.time - peckTime;
            keyPeckRelease = true;
        }
        // while pecking limit its duration
        if (isPecking)
        {
            peckTimeLength = Time.time - peckTime;
            if (peckTimeLength >= 0.25f || keyPeckReleaseTimeLength >= 0.15f)
            {
                isPecking = false;
            }
        }
    }

    void PlayerMovement()
    {
        // left arrow key - moving left
        if (keyHorizontal < 0)
        {
            // facing right while moving left - flip
            if (isFacingRight)
            {
                Flip();
            }
            // grounded play run animation
            // if (isGrounded)
            // {
            //     // play run peck or run animation
            //     if (isPecking)
            //     {
            //         animator.Play("Player_Runpeck");
            //     }
            //     else
            //     {
            //         animator.Play("Player_Run");
            //     }
            // }
            // negative move speed to go left
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        else if (keyHorizontal > 0) // right arrow key - moving right
        {
            // facing left while moving right - flip
            if (!isFacingRight)
            {
                Flip();
            }
            // grounded play run animation
            // if (isGrounded)
            // {
            //     // play run peck or run animation
            //     if (isPecking)
            //     {
            //         animator.Play("Player_Runpeck");
            //     }
            //     else
            //     {
            //         animator.Play("Player_Run");
            //     }
            // }
            // positive move speed to go right
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        else   // no movement
        {
            // grounded play idle animation
            // if (isGrounded)
            // {
            //     // play peck or idle animation
            //     if (isPecking)
            //     {
            //         animator.Play("Player_peck");
            //     }
            //     else
            //     {
            //         animator.Play("Player_Idle");
            //     }
            // }
            // no movement zero x velocity
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }

        // pressing jump while grounded - can only jump once
        if (keyJump && isGrounded)
        {
            // play jump/jump peck animation and jump speed on y velocity
            // if (isPecking)
            // {
            //     animator.Play("Player_Jumppeck");
            // }
            // else
            // {
            //     animator.Play("Player_Jump");
            // }
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        // while not grounded play jump animation (jumping or falling)
        if (!isGrounded)
        {
            // jump or jump peck animation
            // if (isPecking)
            // {
            //     animator.Play("Player_JumpPeck");
            // }
            // else
            // {
            //     animator.Play("Player_Jump");
            // }
        }
    }

    void Flip()
    {
        // invert facing direction and rotate object 180 degrees on y axis
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
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
            else
            {
                StartDamageAnimation();
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

    public void StartDamageAnimation()
    {
        if (!IsTakingDamage)
        {
            IsTakingDamage = true;
            isInvincible = true;
            float hitForceX = 500f;
            float hitForceY = 500f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX,hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        IsTakingDamage = false;
        isInvincible = false;
        animator.Play("Player_Idle", -1, 0f);

    }
}
