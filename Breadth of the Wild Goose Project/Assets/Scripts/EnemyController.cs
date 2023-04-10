﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all enemies will require these components
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyControllerDemo : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    bool isInvincible;

    GameObject explodeEffect;

    RigidbodyConstraints2D rb2dConstraints;

    public bool freezeEnemy;

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;
    public int explosionDamage = 0;

    [SerializeField] GameObject explodeEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // get handles to components
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // start at full health
        currentHealth = maxHealth;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        // take damage if not invincible
        if (!isInvincible)
        {
            // take damage amount from health and call defeat if no health
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Defeat();
            }
        }
    }

    void StartDefeatAnimation()
    {
        // play explosion animation
        //   create copy of prefab, place its spawn location at center of sprite, 
        //   set explosion damage value (if any), destroy after two seconds
        explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.name = explodeEffectPrefab.name;
        explodeEffect.transform.position = sprite.bounds.center;
        explodeEffect.GetComponent<ExplosionScript>().SetDamageValue(this.explosionDamage);
        Destroy(explodeEffect, 2f);
    }

    void StopDefeatAnimation()
    {
        // we have this function in case we want to remove the explosion before it finishes
        Destroy(explodeEffect);
    }

    void Defeat()
    {
        // play explosion animation and remove enemy
        StartDefeatAnimation();
        Destroy(gameObject);
    }

    public void FreezeEnemy(bool freeze)
    {
        // freeze/unfreeze the enemy on screen
        // zero animation speed and freeze XYZ rigidbody constraints
        // NOTE: this will be called from the GameManager but could be used in other scripts
        if (freeze)
        {
            freezeEnemy = true;
            animator.speed = 0;
            rb2dConstraints = rb2d.constraints;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            freezeEnemy = false;
            animator.speed = 1;
            rb2d.constraints = rb2dConstraints;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // check for collision with player
        if (other.gameObject.CompareTag("Player"))
        {
            // colliding with player inflicts damage and takes contact damage away from health
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x);
            player.TakeDamage(this.contactDamage);
        }
    }
}