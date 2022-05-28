using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        UpdateMaxHealth();
    }



    public void TakeDamage(int damageAmount)
    {
        if (playerCanBeDamaged)
        {
            CineMachineShake.Instance.ShakeCamera(.2f,.25f);
            playerCanBeDamaged = false;
            playerCurrentHealth -= damageAmount;
            UpdateHealthImage();
            if (playerCurrentHealth <= 0)
            {
                GetComponent<Animator>().SetTrigger("isDead");
                GetComponent<PlayerController>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                StartCoroutine(WaitForDeathAnimation());
                return;
            }
            StartCoroutine(iFrames());
        }
    }
    public void Heal()
    {
        playerCurrentHealth += 1;
        UpdateHealthImage();
    }
    public void GainMaxHealth()
    {
        playerMaxHealth += 1;
        if (playerMaxHealth > playerCurrentHealth)
        {
            playerCurrentHealth += 1; 
        }
        UpdateMaxHealth();
        UpdateHealthImage();
    }
    
    private void UpdateMaxHealth()
    {
        for (int i = 0; i < hearts.Count; i++)
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

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        MenuHandler.Instance.OpenDeathMenu();
    }
}
