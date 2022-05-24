using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseEnemy : MonoBehaviour
{
    public bool isAlive;
    public int health;
    public int damage;
    public float speed;
    private Rigidbody2D _rb2D;
    private PlayerHealth _playerHealth;
    private bool _enemyCanBeDamaged = true;
    private bool _canMove = true;
    private Vector2 _dir;
    
    private void OnEnable()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        _dir = (Vector2) _playerHealth.transform.position - (Vector2)transform.position;
        _dir.Normalize();
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            _rb2D.MovePosition(_rb2D.position + _dir * (speed * Time.fixedDeltaTime));
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(KnockBackFrames());
            _playerHealth.TakeDamage(damage);
            Vector2 difference = (transform.position - col.transform.position).normalized;
            Vector2 force = difference * 5;
            //_rb2D.AddForce(force,Forcemode.VelocityChange);
            _rb2D.velocity = force;
        }
    }

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
    private IEnumerator KnockBackFrames()
    {
        _canMove = false;
        yield return new WaitForSeconds(.1f);
        _canMove = true;
    }
    private IEnumerator iFrames()
    {
        _enemyCanBeDamaged = false;
        yield return new WaitForSeconds(.1f);
        _enemyCanBeDamaged = true;
    }
}
