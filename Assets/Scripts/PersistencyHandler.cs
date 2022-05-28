using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistencyHandler : MonoBehaviour
{
    public static PersistencyHandler Instance { get; private set; }
    public List<Color> possibleColors = new List<Color>();
    public Color currentColor;
    public int amountOfDeaths;
    
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
