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
        public Sprite swordSprite;
        public int swordDamage;
        public String swordAnimationString;
    }

    public List<Sword> potentialSwords = new List<Sword>();
    
    private Transform _playerChar;
    private Animator _swordAnimator;
    private Camera _camera;
    private int _activeQuadrant;
    public int _currentSwordIndex;

    void Start()
    {
        _camera = Camera.main;
        _playerChar = transform.transform;
        _swordAnimator = transform.GetChild(0).GetComponentInChildren<Animator>();
    }
    void Update() {
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
    public void ChangeSword(int swordIndex)
    {
        _currentSwordIndex = swordIndex;
    }
    private void OnFire()
    {
        _swordAnimator.SetTrigger(potentialSwords[_currentSwordIndex].swordAnimationString);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("EnemyTriggEnter");
            other.GetComponent<BaseEnemy>().TakeDamage(potentialSwords[_currentSwordIndex].swordDamage);
            CineMachineShake.Instance.ShakeCamera(.2f,.25f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("EnemyTriggExit");
    }
}
