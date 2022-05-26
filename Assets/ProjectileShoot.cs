using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private int shotSpeed;
    public bool playerInRange;
    private PlayerHealth _playerHealth;
    private Vector2 _dir;

    private void OnEnable()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            _dir = (Vector2) _playerHealth.transform.position - (Vector2) transform.position;
            _dir.Normalize();
        }
    }
    public void ShootProjectile()
    {
        GameObject currentProjectile = Instantiate(projectile, transform.position,Quaternion.identity);
        Rigidbody2D currentrb= currentProjectile.GetComponent<Rigidbody2D>();
        currentrb.velocity = _dir*shotSpeed;
        Destroy(currentProjectile,5f);
    }
}
