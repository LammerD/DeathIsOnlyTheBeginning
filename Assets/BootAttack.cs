using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BootAttack : MonoBehaviour
{
    [SerializeField] private int damage;

    public void Start()
    {
        transform.GetComponent<SpriteRenderer>().color = PersistencyHandler.Instance.currentColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    public void ScreenShake()
    {
        CineMachineShake.Instance.ShakeCamera(1,0.2f);
    }
}
