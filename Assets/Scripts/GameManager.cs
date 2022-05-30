using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<GrowthCircle> growthCirclesInCurrentRoom = new List<GrowthCircle>();

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;

    [SerializeField] private GameObject textBox;
    [SerializeField] private RawImage fadeOut;
    [SerializeField] private List<string> startText;
    [SerializeField] private List<string> tutorialText;
    [SerializeField] private List<string> winText;
    [SerializeField] private List<string> loseText;

    //Just Health pickup
    [SerializeField] private GameObject tierOneReward;

    //Max Health or MoveSpeed
    [SerializeField] private List<GameObject> tierTwoRewards = new List<GameObject>();

    //Damage or AttackSpeed
    [SerializeField] private List<GameObject> tierThreeRewards = new List<GameObject>();

    //Weapon
    [SerializeField] private List<GameObject> tierFourRewards = new List<GameObject>();
    public List<GameObject> startScreens;
    [SerializeField] private Animator blueBoss;
    [SerializeField] private AnimatorOverrideController redBoss;
    [SerializeField] private Animator redPlayer;
    [SerializeField] private AnimatorOverrideController bluePlayer;
    public GameObject currentStartScreens;

    private int _enemiesInCurrentRoom;
    public enum toDoAfterText
    {
        spawnTutorialEnemy,
        reloadGame,
        openStartMenu,
        openDeathMenu,
    }
    private Room _currentRoom;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            blueBoss = boss.GetComponent<Animator>();
            redPlayer = player.GetComponent<Animator>();
        }
    }

    void Start()
    {
        Instance = this;
        StartCoroutine(FadeFromBlack());

        currentStartScreens = !PersistencyHandler.Instance.isBluePlayer ? startScreens[0] : startScreens[1];
        boss.GetComponent<Animator>().runtimeAnimatorController =
            !PersistencyHandler.Instance.isBluePlayer ? blueBoss.runtimeAnimatorController : redBoss;
        player.GetComponent<Animator>().runtimeAnimatorController =
            !PersistencyHandler.Instance.isBluePlayer ? redPlayer.runtimeAnimatorController : bluePlayer;
        currentStartScreens.SetActive(true);
        fadeOut.color = new Color(0, 0, 0, 100);
    }

    public void EnemyDied()
    {
        _enemiesInCurrentRoom--;
        if (_enemiesInCurrentRoom <= 0)
        {
            Invoke(nameof(AllEnemiesDied), .5f);
        }else if (_currentRoom.roomType == Room.RoomTypes.TutorialRoom)
        {
            _currentRoom.enemiesInRoom[1].SetActive(true);
        }
    }

    public void EnterNewRoom(Room currentRoom)
    {
        _currentRoom = currentRoom;
        foreach (GameObject blocker in _currentRoom.blockers)
        {
            blocker.SetActive(true);
        }

        CineMachineShake.Instance.ShakeCamera(1, .2f);
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
        }
        else if (_currentRoom.roomType == Room.RoomTypes.BossRoom)
        {
            int bossHP = Mathf.Min(player.GetComponent<PlayerSword>().playerDamage * 20, 100);
            boss.GetComponent<BaseEnemy>().maxHealth = bossHP;
            boss.GetComponent<BaseEnemy>().health = bossHP;
            StartCoroutine(boss.GetComponent<BaseEnemy>().UpdateMaxHealth());
            boss.GetComponent<Animator>().SetTrigger("isAppearing");
            MusicHandler.instance.PlayBossTrack();
        }else if (_currentRoom.roomType == Room.RoomTypes.TutorialRoom)
        {
            StartCoroutine(TextAppear(tutorialText, toDoAfterText.spawnTutorialEnemy));
            _enemiesInCurrentRoom = 2;
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
                    Instantiate(tierOneReward, growthCircle.transform.position, Quaternion.identity);
                    break;
                case 1:
                    //Speed or MaxHP;
                    if (_currentRoom.roomType == Room.RoomTypes.TutorialRoom)
                    {
                        Instantiate(tierFourRewards[0], growthCircle.transform.position, Quaternion.identity);
                        tierFourRewards.RemoveAt(0);
                        break;
                    }
                    Instantiate(tierTwoRewards[Random.Range(0, tierTwoRewards.Count)], growthCircle.transform.position,
                        Quaternion.identity);
                    break;
                case 2:
                    //Damage or AttackSpeed;
                    Instantiate(tierThreeRewards[Random.Range(0, tierThreeRewards.Count)],
                        growthCircle.transform.position, Quaternion.identity);
                    break;
                case 3:
                    //Weapon
                    if (tierFourRewards.Count != 0)
                    {
                        Instantiate(tierFourRewards[0], growthCircle.transform.position, Quaternion.identity);
                        tierFourRewards.RemoveAt(0);
                    }
                    else
                    {
                        Instantiate(tierThreeRewards[Random.Range(0, tierThreeRewards.Count)],
                            growthCircle.transform.position, Quaternion.identity);
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
                        position + (Vector3) (Vector2.one * 0.2f), Quaternion.identity);
                    Instantiate(tierTwoRewards[0],
                        position + (Vector3) (Vector2.right * 0.2f), Quaternion.identity);
                    Instantiate(tierTwoRewards[1],
                        position + (Vector3) (Vector2.left * 0.2f), Quaternion.identity);
                    Instantiate(tierThreeRewards[0],
                        position + (Vector3) (Vector2.down * 0.2f), Quaternion.identity);
                    Instantiate(tierThreeRewards[1],
                        position + (Vector3) (Vector2.up * 0.2f), Quaternion.identity);
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
        player.GetComponent<PlayerHealth>().TogglePlayerControl(false);
        foreach (GameObject baseEnemy in _currentRoom.enemiesInRoom)
        {
            baseEnemy.SetActive(false);
        }

        MusicHandler.instance.PlayVictoryTrack();
        PersistencyHandler.Instance.ChangePlayerColor();
        StartCoroutine(WaitForBossDeath());
    }

    IEnumerator WaitForBossDeath()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(TextAppear(winText, toDoAfterText.reloadGame));
    }

    public void PlayerDied()
    {
        StartCoroutine(TextAppear(loseText, toDoAfterText.reloadGame));
    }

    private IEnumerator TextAppear(List<string> texts,toDoAfterText todo)
    {
        textBox.SetActive(true);
        foreach (string text in texts)
        {
            string currentText = "";
            for (int i = 0; i < text.Length; i++)
            {
                currentText = text.Substring(0, i);
                currentText += "<color=#00000000>" + text.Substring(i) + "</color>";
                textBox.GetComponent<TextMeshProUGUI>().text = currentText;
                yield return new WaitForSeconds(0.05f);
            }
            if (todo == toDoAfterText.openStartMenu)
            {
                MenuHandler.Instance.OpenStartMenu();
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
        switch (todo)
        {
            case toDoAfterText.spawnTutorialEnemy:
                textBox.SetActive(false);
                _currentRoom.enemiesInRoom[0].SetActive(true);
                yield break;
            case toDoAfterText.openDeathMenu:
                MenuHandler.Instance.OpenDeathMenu();
                yield break;
            case toDoAfterText.reloadGame:
                StartCoroutine(FadeToBlack());
                yield break;
        }
    }

    public void StartGame()
    {
        StartCoroutine(FadeImagePlay());
    }
    private IEnumerator FadeToBlack()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime) 
        {
            fadeOut.color = new Color(0, 0, 0, i);
            yield return null;
        }
        MenuHandler.Instance.ReloadGame();
    }
    private IEnumerator FadeFromBlack()
    {
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            fadeOut.color = new Color(0, 0, 0, i);
            yield return null;
        }

        StartCoroutine(TextAppear(startText, toDoAfterText.openStartMenu));
    }
    // private IEnumerator FadeImageStartScreen()
    // {
    //     for (float i = 0; i <= 1; i += Time.deltaTime) 
    //     {
    //         fadeOut.color = new Color(0, 0, 0, i);
    //         yield return null;
    //     }
    //     PersistencyHandler.Instance.currentStartImage.SetActive(true);
    //     for (float i = 1; i >= 0; i -= Time.deltaTime)
    //     {
    //         fadeOut.color = new Color(0, 0, 0, i);
    //         yield return null;
    //     }
    //
    //     StartCoroutine(TextAppear(startText, toDoAfterText.openStartMenu));
    // }
    private IEnumerator FadeImagePlay()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime) 
        {
            fadeOut.color = new Color(0, 0, 0, i);
            yield return null;
        }
        textBox.SetActive(false);
        currentStartScreens.SetActive(false);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            fadeOut.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }
}
