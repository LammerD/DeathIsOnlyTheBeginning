using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public bool playerCanBeDamaged = true;

    [SerializeField] private List<Image> hearts = new List<Image>();
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        for(int i =0 ; i < hearts.Count;i++)
        {
            if (i < playerMaxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (playerCanBeDamaged)
        {
            StartCoroutine(iFrames());
            playerCurrentHealth -= damageAmount;
            UpdateHealthImage();
        }
    }
    public void Heal()
    {
        playerCurrentHealth += 1;
        UpdateHealthImage();
    }

    private void UpdateHealthImage()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < playerCurrentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    private IEnumerator iFrames()
    {
        playerCanBeDamaged = false;
        //Makes sprite flash, need solution to do this without magic number but don't know how at the moment.
        for(int i = 0; i < 5;i++)
        {
            _spriteRenderer.color = new Color (1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds (.1f);
            _spriteRenderer.color = Color.white; 
            yield return new WaitForSeconds (.1f);
        }
        playerCanBeDamaged = true;
    }
}
