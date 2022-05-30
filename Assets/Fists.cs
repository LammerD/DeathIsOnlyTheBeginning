using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fists : MonoBehaviour
{
    [SerializeField] private List<Sprite> fistSprites = new List<Sprite>();
    [SerializeField] private int damage;
    void Start()
    {
        if (PersistencyHandler.Instance.isBluePlayer)
        {
            GetComponent<SpriteRenderer>().sprite = fistSprites[1];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = fistSprites[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
