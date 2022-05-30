using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private String bossText;
    [SerializeField] private GameObject bossTextBox;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotSpeed;
    [SerializeField] private List<GameObject> boots = new List<GameObject>();
    [SerializeField] private List<Animator> bootsAnimators = new List<Animator>();
    [SerializeField] private List<GameObject> addEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> addEnemieCircles = new List<GameObject>();
    [System.Serializable]
    public class fistListList
    {
        public List<Animator> fistList;
    }
    public List<fistListList> fistList = new List<fistListList>();
    
    private PlayerHealth _playerHealth;

    private int _bootCount;
    private int _fistListCounter;
    void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        foreach (GameObject boot in boots)
        {
            bootsAnimators.Add(boot.GetComponent<Animator>());
        }
    }

    public void SwordThrow()
    {
        Vector2 dir = (Vector2) _playerHealth.transform.position - (Vector2) transform.position;
        dir.Normalize();
        GameObject currentProjectile = Instantiate(projectile, transform.position,Quaternion.identity);
        Rigidbody2D currentrb= currentProjectile.GetComponent<Rigidbody2D>();
        currentrb.velocity = dir*shotSpeed;
        Destroy(currentProjectile,5f);
    }

    public IEnumerator BootAttack()
    {
        boots[_bootCount].transform.position = _playerHealth.transform.position;
        bootsAnimators[_bootCount].SetTrigger("isBooting");
        _bootCount++;
        yield return new WaitForSeconds(.5f);
        boots[_bootCount].transform.position = _playerHealth.transform.position;
        bootsAnimators[_bootCount].SetTrigger("isBooting");
        _bootCount++;
        if (_bootCount >= 3)
        {
            _bootCount = 0;
        }
    }

    public void HeartAttack()
    {
        for (int i = 0; i < addEnemies.Count; i++)
        {
            StartCoroutine(HeartAttackSpawnDelay(addEnemieCircles[i],addEnemies[i]));
        }
    }

    private IEnumerator HeartAttackSpawnDelay(GameObject marker, GameObject add)
    {
        marker.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        marker.SetActive(false);
        add.SetActive(true);
    }

    private IEnumerator FistAttack()
    {
        String triggerString;
        if (_fistListCounter == 0)
        {
            triggerString = "isFistAttackingHorizontally";
        }
        else
        {
            triggerString = "isFistAttackingVertically";
        }
        foreach (Animator fist in fistList[_fistListCounter].fistList)
        {
            fist.SetTrigger(triggerString);
            yield return new WaitForSeconds(0.5f);
        }
        _fistListCounter++;
        if (_fistListCounter > 1)
        {
            _fistListCounter = 0;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.GetComponent<PlayerHealth>().TakeDamage(1);
    //     }
    // }
    public IEnumerator textAppear()
    {
        bossTextBox.SetActive(true);
        _playerHealth.TogglePlayerControl(false);
        string currentText = "";
        for (int i = 0; i < bossText.Length; i++)
        {
            currentText = bossText.Substring(0, i);
            bossTextBox.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(0.07f);
        }
    }
    public void TextOff()
    {
        _playerHealth.TogglePlayerControl(true);
        bossTextBox.SetActive(false);
    }
}
