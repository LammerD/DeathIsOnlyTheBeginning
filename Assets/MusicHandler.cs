using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public static MusicHandler instance;
    [SerializeField] List<AudioClip> normalTracks = new List<AudioClip>();
    [SerializeField] AudioClip bossTrack;
    [SerializeField] AudioClip gameOverTrack;
    [SerializeField] AudioClip victoryTrack;
    [SerializeField] AudioClip postVictoryTrack;
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] bool normalMusic = true;
    [SerializeField] int trackCounter;
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioPlayer.isPlaying)
        {
            if (normalMusic)
            {
                audioPlayer.clip = normalTracks[trackCounter];
                audioPlayer.Play();
                trackCounter++;
                if (trackCounter > normalTracks.Count)
                {
                    trackCounter = 0;
                }
            }
        }
    }
    public void PlayBossTrack()
    {
        normalMusic = false;
        audioPlayer.clip = bossTrack;
        audioPlayer.loop = true;
        audioPlayer.Play();
    }

    public void PlayVictoryTrack()
    {
        normalMusic = false;
        audioPlayer.clip = victoryTrack;
        audioPlayer.loop = false;
        audioPlayer.Play();
    }
    public void PlayGameOverTrack()
    {
        normalMusic = false;
        audioPlayer.clip = gameOverTrack;
        audioPlayer.loop = false;
        audioPlayer.Play();
    }

    public void ResetGameMusic()
    {
        if (!normalMusic)
        {
            trackCounter = 0;
            normalMusic = true;
            audioPlayer.clip = normalTracks[trackCounter];
            audioPlayer.Play();
            trackCounter++;
        }
    }

}
