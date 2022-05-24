using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public bool playerCanBeDamaged = true;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        if (playerCanBeDamaged)
        {
            StartCoroutine(iFrames());
            //StartCoroutine(HurtColor());
            playerCurrentHealth -= damageAmount;
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
        // yield return new WaitForSeconds(1.5f);
        playerCanBeDamaged = true;
    }
    
    // IEnumerator HurtColor() {
    //     for (int i = 0; i < 3; i++) {
    //         _spriteRenderer.color = new Color (1f, 1f, 1f, 0.3f); //Red, Green, Blue, Alpha/Transparency
    //         yield return new WaitForSeconds (.1f);
    //         _spriteRenderer.color = Color.white; //White is the default "color" for the sprite, if you're curious.
    //         yield return new WaitForSeconds (.1f);
    //     }
    // }
}
