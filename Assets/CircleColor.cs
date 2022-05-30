using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleColor : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = PersistencyHandler.Instance.currentColor;
    }
}
