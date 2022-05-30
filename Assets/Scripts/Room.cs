using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Serializable]
    public enum RoomTypes
    {
       NormalRoom,
       BossRoom,
       BossDoor,
       TutorialRoom
    }

    public RoomTypes roomType;
    public List<GameObject> enemiesInRoom = new List<GameObject>();
    public List<GameObject> blockers = new List<GameObject>();
    public AudioClip bossMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (roomType == RoomTypes.BossDoor)
            {
                if (other.GetComponent<PlayerController>().hasKey)
                {
                    blockers[0].SetActive(false);
                    GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            else
            {
                GameManager.Instance.EnterNewRoom(this);
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
