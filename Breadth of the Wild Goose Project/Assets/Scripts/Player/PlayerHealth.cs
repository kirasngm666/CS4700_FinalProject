// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerHealth : MonoBehaviour, IDamageable
// {
//     bool IsTakingDamage;
//     bool isInvincible;
//     bool hitSideRight;

//     GooseController player = GameObject.GetComponent<GooseController>();

//     public float maxHealth = 20;
//     public float health;

//     // Start is called before the first frame update
//     void Start()
//     {
//         health = maxHealth;
//     }

//     // Update is called once per frame
//     public virtual void ApplyDamage(float amount)
//     {
//         if (!isInvincible)
//         {
//             currentHealth -= amount;
//             if (currentHealth <= 0)
//             {
//                 Die();
//             }
//             else
//             {
//                 player.StartDamageAnimation();
//             }
//         }  
//     }

//     private void Die()
//     {
//         gameObject.SetActive(false);
//     }

//     // public void hitSide(bool rightSide)
//     // {
//     //     hitSideRight = rightSide;
//     // }

//     // public void Invincible(bool invincibility)
//     // {
//     //     isInvincible = invincibility;
//     // }

//     // void StartDamageAnimation()
//     // {
//     //     if (!IsTakingDamage)
//     //     {
//     //         IsTakingDamage = true;
//     //         isInvincible = true;
//     //         float hitForceX = 0.50f;
//     //         float hitForceY = 1.5f;
//     //         if (hitSideRight) hitForceX = -hitForceX;
//     //         rb2d.velocity = Vector2.zero;
//     //         rb2d.AddForce(new Vector2(hitForceX,hitForceY), ForceMode2D.Impulse);
//     //     }
//     // }

//     // void StopDamageAnimation()
//     // {
//     //     IsTakingDamage = false;
//     //     isInvincible = false;
//     // }
// }
