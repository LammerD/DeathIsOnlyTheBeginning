using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistencyHandler : MonoBehaviour
{
    public static PersistencyHandler Instance { get; private set; }
    public List<Color> possibleColors = new List<Color>();
    public List<GameObject> startImage;
    public GameObject currentStartImage;
    public Color currentColor;
    public bool isBluePlayer;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
        }
    }

    public void ChangePlayerColor()
    {
        //Changes from blueplayer to redplayer
        if (isBluePlayer)
        {
            currentColor = possibleColors[0];
            currentStartImage = startImage[0];
            isBluePlayer = false;
        }
        //Changes from Red to blue
        else
        {
            currentColor = possibleColors[1];
            currentStartImage = startImage[1];
            isBluePlayer = true;
        }
    }

    public void GrabReferences()
    {
        startImage = new List<GameObject>();
        foreach (GameObject startScreen in GameObject.FindGameObjectsWithTag("startScreen"))
        {
            startImage.Add(startScreen);
            startScreen.SetActive(false);
        }

        if (!isBluePlayer)
        {
            currentStartImage = startImage[0];
        }
        else
        {
            currentStartImage = startImage[1];
        }
        currentStartImage.SetActive(true);
    }
}
