using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSword : MonoBehaviour
{
    public float rotationRadiusAroundPlayer = 1f;
    public Transform _Sword;

    [Serializable]
    public class Sword
    {
        public String swordAnimationString;
        public String swordAnimationIdleString;
    }

    public List<Sword> potentialSwords = new List<Sword>();
    public int playerDamage;
    public float cooldownTime;

    private Transform _playerChar;
    private Animator _swordAnimator;
    private Camera _camera;
    private int _activeQuadrant;
    public int _currentSwordIndex;
    private bool _isCooldown;
    private bool _isFiring;

    void Start()
    {
        _camera = Camera.main;
        _playerChar = transform.transform;
        _swordAnimator = transform.GetChild(0).GetComponentInChildren<Animator>();
    }
    void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isFiring = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isFiring = false;
        }

        if (_isFiring)
        {
            Attack();
        }
        Vector3 position = _playerChar.position;
        
        Vector3 charToMouseDir = 
            _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - position;
        charToMouseDir.z = 0; 
        
        _Sword.position = position + (rotationRadiusAroundPlayer * charToMouseDir.normalized);
        Vector3 playerScreenPoint = _camera.WorldToScreenPoint(_playerChar.transform.position);

        int currentQuadrant = 0;
        if (Mouse.current.position.ReadValue().x < playerScreenPoint.x &&
            Mouse.current.position.ReadValue().y < playerScreenPoint.y)
        {
            currentQuadrant = 1;
        }
        else if (Mouse.current.position.ReadValue().x < playerScreenPoint.x &&
                 Mouse.current.position.ReadValue().y >= playerScreenPoint.y)
        {
            currentQuadrant = 2;
        }
        else if (Mouse.current.position.ReadValue().x >= playerScreenPoint.x &&
                 Mouse.current.position.ReadValue().y < playerScreenPoint.y)
        {
            currentQuadrant = 3;
        } else if (Mouse.current.position.ReadValue().x >= playerScreenPoint.x &&
                   Mouse.current.position.ReadValue().y >= playerScreenPoint.y)
        {
            currentQuadrant = 4;
        }
        if (currentQuadrant != _activeQuadrant)
        {
            _activeQuadrant = currentQuadrant;
            Vector3 newScale = _Sword.localScale;
            switch (_activeQuadrant)
            {
                case 1:
                    newScale.x = -1;
                    newScale.y = -1;
                    break;
                case 2:
                    newScale.x = -1;
                    newScale.y = 1;
                    break;
                case 3:
                    newScale.x = 1;
                    newScale.y = -1;
                    break;
                case 4:
                    newScale.x = 1;
                    newScale.y = 1;
                    break;
            }
            _Sword.localScale = newScale;
        }
    }
    public void LevelUpSword()
    {
        _currentSwordIndex++;
        _swordAnimator.SetTrigger(potentialSwords[_currentSwordIndex].swordAnimationIdleString);
    }
    // private void OnFire()
    // {
    //     if (!_isCooldown)
    //     {
    //         StartCoroutine(Cooldown());
    //         _swordAnimator.SetTrigger(potentialSwords[_currentSwordIndex].swordAnimationString);  
    //     }
    // }
    private void Attack()
    {
        if (!_isCooldown)
        {
            StartCoroutine(Cooldown());
            _swordAnimator.SetTrigger(potentialSwords[_currentSwordIndex].swordAnimationString);  
        }
    }
    private IEnumerator Cooldown()
    {
        _isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        _isCooldown = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().TakeDamage(playerDamage);
        }
    }
}
