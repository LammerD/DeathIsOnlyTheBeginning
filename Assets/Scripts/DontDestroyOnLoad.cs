using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < FindObjectsOfType<DontDestroyOnLoad>().Length; i++)
        {
            if (FindObjectsOfType<DontDestroyOnLoad>()[i] != this)
            {
                if (FindObjectsOfType<DontDestroyOnLoad>()[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
            
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
