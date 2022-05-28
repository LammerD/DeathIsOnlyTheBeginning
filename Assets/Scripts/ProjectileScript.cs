using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]private int damage;
    [SerializeField] private bool playerProjectile;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerProjectile)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
        
    }
}
