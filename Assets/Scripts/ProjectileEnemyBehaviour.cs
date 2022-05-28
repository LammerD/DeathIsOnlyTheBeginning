using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Animator ownAnimator;
    [SerializeField] private ProjectileShoot projectileShoot;
    [SerializeField] private float attackSpeed;

    private void OnEnable()
    {
        ownAnimator.SetFloat("AttackSpeed", attackSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            projectileShoot.playerInRange = true;
            ownAnimator.SetBool("isAttacking",true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            projectileShoot.playerInRange = false;
            ownAnimator.SetBool("isAttacking",false);
        }
    }
}
