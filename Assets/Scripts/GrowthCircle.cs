using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthCircle : MonoBehaviour
{
    public int countEnemiesDiedInCircle;
    public bool isBossCircle;

    private Vector3 _currentScaleTarget;
    private Vector3 _ownBaseScale;
    private bool _canGrow;
    private float _growthDuration = 2f;
    private float _elapsedTime;

    private void Start()
    {
        _ownBaseScale = transform.localScale;
        transform.localScale = new Vector3(0.01f,0.01f,1);
        _currentScaleTarget = _ownBaseScale;
    }

    private void Update()
    {
        if (!isBossCircle && _elapsedTime < _growthDuration)
        {
            _elapsedTime += Time.deltaTime;
            float percentageComplete = _elapsedTime / _growthDuration;
            transform.localScale = Vector3.Lerp (transform.localScale, _currentScaleTarget , Mathf.SmoothStep(0,1,percentageComplete));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().lastGrowthCircleEntered = this;
        }
        //Well this is just beautiful code right here...
        if (other.CompareTag("GrowthCircle"))
        {
            if (other.GetComponent<GrowthCircle>().isBossCircle)
            {
                return;
            }
            if (isBossCircle)
            {
                Destroy(other.gameObject);
            }
            if (other.GetComponent<GrowthCircle>().countEnemiesDiedInCircle > this.countEnemiesDiedInCircle)
            {
                return;
            }
            else if(other.GetComponent<GrowthCircle>().countEnemiesDiedInCircle < this.countEnemiesDiedInCircle)
            {
                this.countEnemiesDiedInCircle += other.GetComponent<GrowthCircle>().countEnemiesDiedInCircle+1;
                UpdateTargetScale();
                Destroy(other.gameObject);
            }
            else
            {
                if (other.GetInstanceID() > this.GetInstanceID())
                {
                    return;
                }
                else
                {
                    this.countEnemiesDiedInCircle += other.GetComponent<GrowthCircle>().countEnemiesDiedInCircle+1;
                    UpdateTargetScale();
                    Destroy(other.gameObject);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().lastGrowthCircleEntered = null;
        }
    }

    public void Grow()
    {
        countEnemiesDiedInCircle++;
        UpdateTargetScale();
    }

    private void UpdateTargetScale()
    {
        _currentScaleTarget = new Vector3(_ownBaseScale.x + countEnemiesDiedInCircle * .1f,
            _ownBaseScale.y + countEnemiesDiedInCircle * .1f, 1);
        _elapsedTime = 0;
    }
}
