using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using Random = UnityEngine.Random;

public class BossWalking : StateMachineBehaviour
{
    private Transform _player;
    private Rigidbody2D _rb;
    private BaseEnemy _baseBoss;
    [SerializeField] private List<string> triggerStringList = new List<string>();
    public float speed;
    public float timeBetweenAttacks;
    private float _currentTime = 0;
    private string _lastTriggerString;
    private bool _canAttack = true;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = FindObjectOfType<PlayerHealth>().transform;
        _rb = animator.GetComponent<Rigidbody2D>();
        _baseBoss = animator.GetComponent<BaseEnemy>();
        _currentTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = _player.position;
        Vector2 dir = (Vector2) target - (Vector2)_rb.position;
        dir.Normalize();
        _rb.MovePosition(_rb.position + dir * (speed * Time.fixedDeltaTime));
        _currentTime += Time.deltaTime;
        if (_currentTime >= timeBetweenAttacks)
        {
            if (_canAttack)
            {
                _canAttack = false;
                if (_baseBoss.canSpecialAttack)
                {
                    _baseBoss.canSpecialAttack = false;
                    _lastTriggerString = "isHeartAttacking";
                    animator.SetTrigger(_lastTriggerString);
                    timeBetweenAttacks = 2f;
                }else if (_baseBoss.canFistAttack)
                {
                    _baseBoss.canFistAttack = false;
                    _lastTriggerString = "isFistAttacking";
                    animator.SetTrigger(_lastTriggerString);
                }
                else
                {
                    _lastTriggerString = triggerStringList[Random.Range(0, triggerStringList.Count)];
                    animator.SetTrigger(_lastTriggerString);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(_lastTriggerString);
        _canAttack = true;
        _currentTime = 0;
    }
}
