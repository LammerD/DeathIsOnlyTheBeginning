using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseEnemy : MonoBehaviour
{
    public bool isAlive;
    public int health;
    private bool _enemyCanBeDamaged = true;
    
    public void TakeDamage(int damageAmount)
    {
        if (_enemyCanBeDamaged)
        {
            StartCoroutine(iFrames());
            health -= damageAmount; 
            if (health <= 0) 
            { 
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator iFrames()
    {
        _enemyCanBeDamaged = false;
        yield return new WaitForSeconds(.1f);
        _enemyCanBeDamaged = true;
    }
}
