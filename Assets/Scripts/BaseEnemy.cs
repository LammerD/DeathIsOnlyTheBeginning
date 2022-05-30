using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public GrowthCircle lastGrowthCircleEntered;
    [SerializeField] private bool isBoss;
    [SerializeField] private List<Image> hearts = new List<Image>();
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private bool isBossMinion;
    [SerializeField] private GameObject heartPickup;
    public bool canSpecialAttack;
    public bool canFistAttack;
    [SerializeField] private GameObject GrowthCirclePrefab;
    private bool _enemyCanBeDamaged = true;
    private SpriteRenderer _ownSR;
    private Color _ownColor;
    private bool _hasReachedHalfway;
    private bool _hasUsedFirstFist;
    private bool _hasUsedSecondFist;
    private int _numberOfHitsTaken;

    private void Start()
    {
        _ownSR = GetComponent<SpriteRenderer>();
        _ownColor = _ownSR.color;
    }

    public void TakeDamage(int damageAmount)
    {
        if (_enemyCanBeDamaged)
        {
            if (isBoss)
            {
                StartCoroutine(iFramesBoss());
            }
            else
            {
                StartCoroutine(iFrames());
            }
            health -= damageAmount;
            //Should not be so hard coded but oh well..
            if (isBoss)
            {
                _numberOfHitsTaken++;
                UpdateHealthImage();
                if (_numberOfHitsTaken >= 10 && !_hasReachedHalfway)
                {
                    _hasReachedHalfway = true;
                    canSpecialAttack = true;
                }else if (_numberOfHitsTaken >= 15 && !_hasUsedFirstFist)
                {
                    _hasUsedFirstFist = true;
                    canFistAttack = true;
                }else if (_numberOfHitsTaken >= 5 && !_hasUsedSecondFist)
                {
                    _hasUsedSecondFist = true;
                    canFistAttack = true;
                }
            }

            if (health <= 0)
            {
                if (isBoss)
                {
                    GameManager.Instance.BossDefeated();
                    GetComponent<Animator>().SetTrigger("isDead");
                }
                else
                { 
                    EnemyDeath();
                }
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

        if (isBossMinion)
        {
            Instantiate(heartPickup, transform.position, quaternion.identity);
        }
        else
        {
            GameManager.Instance.EnemyDied();
        }
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
    private IEnumerator iFramesBoss()
    {
        _enemyCanBeDamaged = false;
        for(int i = 0; i < 5;i++)
        {
            _ownSR.color = new Color (1f, 0f, 0f, 1f);
            yield return new WaitForSeconds (.1f);
            _ownSR.color = _ownColor;
            yield return new WaitForSeconds (.1f);
        }
        _enemyCanBeDamaged = true;
    }
    public IEnumerator UpdateMaxHealth()
    {
        float duration = (float)2 / maxHealth;
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

            yield return new WaitForSeconds(duration);
        }
    }
    public void UpdateHealthImage()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
