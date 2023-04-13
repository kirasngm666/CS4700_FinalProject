using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Controller : MonoBehaviour, IDamageable
{
    
    public float healthPool = 10f;

    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;

    private float currentHealth = 10f;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            if(transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1,1,1);
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if(transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-11,1,1);
                transform.position += Vector3.right * speed * Time.deltaTime;
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

    public virtual void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
