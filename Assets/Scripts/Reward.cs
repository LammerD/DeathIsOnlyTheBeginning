using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [Serializable]
    public enum RewardType
    {
        HeartPickup,
        SpeedBoost,
        MaxHPBoost,
        AttackSpeedBoost,
        DamageBoost,
        Weapon,
        Key
    }

    [SerializeField] private RewardType ownType;
    [SerializeField] private GameObject textPopUp;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            PlayerController playerController = other.GetComponent<PlayerController>();
            PlayerSword playerSword = other.GetComponent<PlayerSword>();
            GameObject currentText;
            switch (ownType)
            {
                case RewardType.HeartPickup:
                    if (playerHealth.playerCurrentHealth < playerHealth.playerMaxHealth)
                    {
                        playerHealth.Heal();
                        Destroy(gameObject);
                    }
                    break;
                case RewardType.SpeedBoost:
                    playerController.moveSpeed += 0.2f;
                    currentText = Instantiate(textPopUp, playerController.transform.position, Quaternion.identity);
                    currentText.GetComponentInChildren<TextMeshPro>().text = "+WLK SPD";
                    Destroy(currentText,1f);
                    Destroy(gameObject);
                    break;
                case RewardType.MaxHPBoost:
                    playerHealth.GainMaxHealth();
                    currentText = Instantiate(textPopUp, playerController.transform.position, Quaternion.identity);
                    currentText.GetComponentInChildren<TextMeshPro>().text = "+MAX HP";
                    Destroy(currentText,1f);
                    Destroy(gameObject);
                    break;
                case RewardType.AttackSpeedBoost:
                    playerSword.cooldownTime = Mathf.Max(0, playerSword.cooldownTime - 0.2f);
                    currentText = Instantiate(textPopUp, playerController.transform.position, Quaternion.identity);
                    currentText.GetComponentInChildren<TextMeshPro>().text = "+ATK SPD";
                    Destroy(currentText,1f);
                    Destroy(gameObject);
                    break;  
                case RewardType.DamageBoost:
                    playerSword.playerDamage++;
                    currentText = Instantiate(textPopUp, playerController.transform.position, Quaternion.identity);
                    currentText.GetComponentInChildren<TextMeshPro>().text = "+ATK DMG";
                    Destroy(currentText,1f);
                    Destroy(gameObject);
                    break;
                case RewardType.Weapon:
                    playerSword.LevelUpSword();
                    Destroy(gameObject);
                    break;
                case RewardType.Key:
                    playerController.hasKey = true;
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
