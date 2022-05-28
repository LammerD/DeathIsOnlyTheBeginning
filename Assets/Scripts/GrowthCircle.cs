using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthCircle : MonoBehaviour
{
    public int countEnemiesDiedInCircle;

    private Vector3 _currentScaleTarget;

    private void Update()
    {
        var localScale = transform.localScale;
        localScale = Vector3.Lerp (localScale, _currentScaleTarget ,  countEnemiesDiedInCircle * Time.deltaTime);
        transform.localScale = localScale;
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
        var localScale = transform.localScale;
        _currentScaleTarget = new Vector3(localScale.x + countEnemiesDiedInCircle * .1f,
            localScale.y + countEnemiesDiedInCircle * .1f, 1);
    }
}
