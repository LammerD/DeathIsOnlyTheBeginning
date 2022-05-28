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
       BossRoom
    }

    public RoomTypes roomType;
    public List<GameObject> enemiesInRoom = new List<GameObject>();
    public List<GameObject> blockers = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.EnterNewRoom(this);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
