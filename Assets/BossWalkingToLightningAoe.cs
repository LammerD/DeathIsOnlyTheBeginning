using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkingToLightningAoe : StateMachineBehaviour
{
    public float speed;
    public float attackRange;
    private Transform _player;
    private Rigidbody2D _rb;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player =  GameObject.FindObjectOfType<PlayerHealth>().transform;
        _rb = animator.GetComponent<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = _player.position;
        Vector2 dir = (Vector2) target - (Vector2)_rb.position;
        dir.Normalize();
        _rb.MovePosition(_rb.position + dir * (speed * Time.fixedDeltaTime));

        if (Vector2.Distance(_player.position, _rb.position) <= attackRange)
        {
            animator.SetTrigger("isLightningAttacking");
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("isLightningAttacking");
    }
}
