using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistencyHandler : MonoBehaviour
{
    public static PersistencyHandler Instance { get; private set; }
    public List<Color> possibleColors = new List<Color>();
    public Color currentColor;
    public bool isBluePlayer;
    
    void Awake()
    {
        Instance = this;
    }

    public void ChangePlayerColor()
    {
        //Changes from blueplayer to redplayer
        if (isBluePlayer)
        {
            currentColor = possibleColors[0];
            isBluePlayer = false;
        }
        //Changes from Red to blue
        else
        {
            currentColor = possibleColors[1];
            isBluePlayer = true;
        }
    }
}
