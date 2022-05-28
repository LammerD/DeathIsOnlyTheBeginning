using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseEnemy : MonoBehaviour
{
    public int health;
    public GrowthCircle lastGrowthCircleEntered;
    [SerializeField] private GameObject GrowthCirclePrefab;
    private bool _enemyCanBeDamaged = true;
    private SpriteRenderer _ownSR;
    private Color _ownColor;

    private void Start()
    {
        _ownSR = GetComponent<SpriteRenderer>();
        _ownColor = _ownSR.color;
    }

    public void TakeDamage(int damageAmount)
    {
        if (_enemyCanBeDamaged)
        {
            StartCoroutine(iFrames());
            health -= damageAmount; 
            if (health <= 0)
            {
                EnemyDeath();
            }
        }
    }

    private void EnemyDeath()
    {
        if (lastGrowthCircleEntered != null)
        {
            lastGrowthCircleEntered.Grow();
        }
        else
        {
            GameManager.Instance.growthCirclesInCurrentRoom.Add(Instantiate(GrowthCirclePrefab, transform.position, quaternion.identity).GetComponent<GrowthCircle>());
        }
        GameManager.Instance.EnemyDied();
        Destroy(gameObject);
    }

    private IEnumerator iFrames()
    {
        _enemyCanBeDamaged = false;
        _ownSR.color = new Color (1f, 0f, 0f, 1f);
        yield return new WaitForSeconds (.1f);
        _ownSR.color = _ownColor;
        _enemyCanBeDamaged = true;
        yield return new WaitForSeconds (.1f);
        _ownSR.color = new Color (1f, 0f, 0f, 1f);
        yield return new WaitForSeconds (.1f);
        _ownSR.color = _ownColor;
    }
}
