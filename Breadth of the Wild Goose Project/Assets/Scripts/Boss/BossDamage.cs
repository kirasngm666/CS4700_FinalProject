using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    // BoxCollider2D box2d;
    // Rigidbody2D rb2d;

    // public int damage;
    // public PlayerHealth playerHealth;

    // bool isCausingDamage;

    // // Combat related
    // float peckTime;
    // bool keyPeckRelease;

    // public Transform meleeAttackOrigin = null;
    // public float meleeAttackRadius = 0.6f;
    // public float meleeDamage = 2f;
    // public float meleeAttackDelay = 1.1f;
    // public LayerMask enemyLayer = 10;
    // private float timeUntilMeleeReadied = 0;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     //animator = GetComponent<Animator>();
    //     box2d = GetComponent<BoxCollider2D>();
    //     rb2d = GetComponent<Rigidbody2D>();
    // }

    // void OnDrawGizmosSelected() 
    // {
    //     if (meleeAttackOrigin != null)
    //     {
    //         Gizmos.DrawWireSphere(meleeAttackOrigin.position, meleeAttackRadius);
    //     }
    // }

    // void PlayerPeckInput()
    // {
    //     float peckTimeLength = 0;
    //     float keyPeckReleaseTimeLength = 0;
        

    //     // peck key is being pressed and key release flag true
    //     if (keyPeck && keyPeckRelease)
    //     {
    //         isCausingDamage = true;
    //         keyPeckRelease = false;
    //         peckTime = Time.time;
    //         // peck Bullet
    //         Debug.Log("The goose is pecking");
    //         Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(meleeAttackOrigin.position, meleeAttackRadius, enemyLayer);
    //         for (int i = 0; i < overlappedColliders.Length; i--)
    //         {
    //             IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
    //             if (enemyAttributes != null)
    //             {
    //                 enemyAttributes.ApplyDamage(meleeDamage);
    //             }
    //             timeUntilMeleeReadied = meleeAttackDelay;
    //         }
    //     }
    //     // peck key isn't being pressed and key release flag is false
    //     if (!keyPeck && !keyPeckRelease)
    //     {
    //         keyPeckReleaseTimeLength = Time.time - peckTime;
    //         keyPeckRelease = true;
    //     }
    //     // while pecking limit its duration
    //     if (isCausingDamage)
    //     {
    //         peckTimeLength = Time.time - peckTime;
    //         if (peckTimeLength >= 0.25f || keyPeckReleaseTimeLength >= 0.15f)
    //         {
    //             isCausingDamage = false;
    //         }
    //     }
    // }
}
