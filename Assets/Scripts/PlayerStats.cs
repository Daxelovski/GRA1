using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] private float heatlh; 
    [SerializeField] private float maxMana, manaRegen;
    public float mana;
    public int damage;

    [SerializeField] private TextMeshProUGUI healthText, manaText;
    [SerializeField] private Image healthImg, manaImg;
    [SerializeField] private Transform player;

    void Start()
    {
        Load();
        UpdateHealthText();
        UpdateManaText();
    }

    void Update()
    {
        ManaRegeneration();
        CheckHeight();
    }

    private void ManaRegeneration()
    {
        mana += manaRegen * Time.deltaTime;

        if(mana > maxMana)
        {
            mana = maxMana;
        }

        if(mana < 0)
        {
            mana = 0;
        }

        manaImg.fillAmount = mana / maxMana;

        UpdateManaText();
    }

    public void UpdateHealth(float value)
    {
        heatlh += value;

        if(heatlh > maxHealth)
        {
            heatlh = maxHealth;
        }

        if(heatlh < 0)
        {
            heatlh = 0;
            Dead();
        }

        healthImg.fillAmount = heatlh / maxHealth;

        UpdateHealthText();
    }

    private void UpdateManaText()
    {
        manaText.text = (int)mana + " / " + maxMana; 
    }

    private void UpdateHealthText()
    {
        healthText.text = (int)heatlh + " / " + maxHealth; 
    }

    private void CheckHeight()
    {
        if(player.position.y < - 15f)
        {
            Dead();
        }
    }

    private void Dead()
    {
        SceneManager.LoadScene("Death Screen");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("Health", maxHealth);
    }

    private void Load()
    {
        maxHealth = PlayerPrefs.GetFloat("Health", maxHealth);
        heatlh = maxHealth;
    }
}
