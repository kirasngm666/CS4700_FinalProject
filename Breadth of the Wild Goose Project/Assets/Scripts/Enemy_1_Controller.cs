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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(gameObject);
    }
}
