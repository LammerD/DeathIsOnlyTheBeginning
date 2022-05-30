using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public List<GrowthCircle> growthCirclesInCurrentRoom = new List<GrowthCircle>();
    //Just Health pickup
    [SerializeField] private GameObject tierOneReward;
    //Max Health or MoveSpeed
    [SerializeField] private List<GameObject> tierTwoRewards = new List<GameObject>();
    //Damage or AttackSpeed
    [SerializeField] private List<GameObject> tierThreeRewards = new List<GameObject>();
    //Weapon
    [SerializeField] private List<GameObject> tierFourRewards = new List<GameObject>();
    
    private int _enemiesInCurrentRoom;
    private AudioSource _audioSource;
    private Room _currentRoom;
    void Start()
    {
        Instance = this;
    }

    public void EnemyDied()
    {
        _enemiesInCurrentRoom--;
        if (_enemiesInCurrentRoom <= 0)
        {
            Invoke(nameof(AllEnemiesDied),.5f);
        }
    }

    public void EnterNewRoom(Room currentRoom)
    {
        _currentRoom = currentRoom;
        foreach (GameObject blocker in _currentRoom.blockers)
        {
            blocker.SetActive(true);
        }
        CineMachineShake.Instance.ShakeCamera(1,.2f);
        foreach (GrowthCircle growthCircle in growthCirclesInCurrentRoom)
        {
            Destroy(growthCircle);
        }
        growthCirclesInCurrentRoom = new List<GrowthCircle>();
        if (currentRoom.roomType == Room.RoomTypes.NormalRoom)
        {
            int i = 0;
            foreach (GameObject baseEnemy in currentRoom.enemiesInRoom)
            {
                baseEnemy.SetActive(true);
                i++;
            }
            _enemiesInCurrentRoom = i;
        }else if (_currentRoom.roomType == Room.RoomTypes.BossRoom)
        {
            //Fix this shit jesus
            FindObjectOfType<Boss>().GetComponent<Animator>().SetTrigger("isAppearing");
            FindObjectOfType<AudioSource>().clip = currentRoom.bossMusic;
            FindObjectOfType<AudioSource>().Play();
        }
    }

    private void AllEnemiesDied()
    {
        growthCirclesInCurrentRoom = growthCirclesInCurrentRoom.Where(item => item != null).ToList();
        foreach (GrowthCircle growthCircle in growthCirclesInCurrentRoom)
        {
            switch (growthCircle.countEnemiesDiedInCircle)
            {
                case 0:
                    //HeartPickup;
                    Instantiate(tierOneReward,growthCircle.transform.position,Quaternion.identity);
                    break;
                case 1:
                    //Speed or MaxHP;
                    Instantiate(tierTwoRewards[Random.Range(0, tierTwoRewards.Count)], growthCircle.transform.position,Quaternion.identity);
                    break;
                case 2:
                    //Damage or AttackSpeed;
                    Instantiate(tierThreeRewards[Random.Range(0, tierThreeRewards.Count)], growthCircle.transform.position,Quaternion.identity);
                    break;
                case 3:
                    //Weapon
                    if (tierFourRewards.Count != 0)
                    {
                        Instantiate(tierFourRewards[0],growthCircle.transform.position,Quaternion.identity);
                        tierFourRewards.RemoveAt(0);
                    }
                    else
                    {
                        Instantiate(tierThreeRewards[Random.Range(0, tierThreeRewards.Count)], growthCircle.transform.position,Quaternion.identity);
                    }
                    break;
                case >=4:
                    //All the Loot!
                    var position = growthCircle.transform.position;
                    if (tierFourRewards.Count != 0)
                    {
                        Instantiate(tierFourRewards[0], growthCircle.transform.position, Quaternion.identity);
                        tierFourRewards.RemoveAt(0);
                    }
                    Instantiate(tierOneReward,
                        position+(Vector3)(Vector2.one*0.2f), Quaternion.identity);
                    Instantiate(tierTwoRewards[0],
                        position+(Vector3)(Vector2.right*0.2f), Quaternion.identity);
                    Instantiate(tierTwoRewards[1],
                        position+(Vector3)(Vector2.left*0.2f), Quaternion.identity);
                    Instantiate(tierThreeRewards[0],
                        position+(Vector3)(Vector2.down*0.2f), Quaternion.identity);
                    Instantiate(tierThreeRewards[1],
                        position+(Vector3)(Vector2.up*0.2f), Quaternion.identity);
                    break;
            }
        }

        foreach (GameObject blocker in _currentRoom.blockers)
        {
            blocker.SetActive(false);
        }
    }

    public void BossDefeated()
    {
        StartCoroutine(WaitForSeconds());
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1.9f);
        MenuHandler.Instance.OpenDeathMenu();
    }
}
