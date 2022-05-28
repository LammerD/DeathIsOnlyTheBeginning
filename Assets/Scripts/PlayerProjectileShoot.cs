using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectileShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private int shotSpeed;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void ShootProjectile()
    {
        GameObject currentProjectile = Instantiate(projectile, transform.position,Quaternion.identity);
        Rigidbody2D currentrb= currentProjectile.GetComponent<Rigidbody2D>();
        Vector2 dir;
        dir = (Vector2) _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - (Vector2) transform.position;
        dir.Normalize();
        currentrb.velocity = dir*shotSpeed;
        Destroy(currentProjectile,5f);
    }
}
