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
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private Image healthImg, manaImg;
    [SerializeField] private Transform player;
    [SerializeField] private AudioSystem audioSystem;

    private float baseMaxHealth;
    private float baseMaxMana;
    private float baseManaRegen;
    private int baseDamage;
    private bool baseStatsCaptured;
    private bool isDead;

    void Awake()
    {
        CaptureBaseStats();
    }

    void Start()
    {
        Load();
        if(audioSystem == null)
        {
            audioSystem = FindObjectOfType<AudioSystem>();
        }

        EnsureLivesText();
        UpdateHealthText();
        UpdateManaText();
        UpdateLivesText();
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

        if(manaImg != null)
        {
            manaImg.fillAmount = mana / maxMana;
        }

        UpdateManaText();
    }

    public void UpdateHealth(float value)
    {
        bool receivedDamage = value < 0f;

        if(receivedDamage && GameSave.GodMode)
        {
            return;
        }

        heatlh += value;

        if(heatlh > maxHealth)
        {
            heatlh = maxHealth;
        }

        if(heatlh < 0)
        {
            heatlh = 0;
        }

        if(healthImg != null)
        {
            healthImg.fillAmount = heatlh / maxHealth;
        }

        UpdateHealthText();

        if(receivedDamage)
        {
            if(heatlh <= 0f)
            {
                Dead();
            }
            else if(audioSystem != null)
            {
                audioSystem.PlayPlayerDamage(1f);
            }
        }
    }

    private void UpdateManaText()
    {
        if(manaText != null)
        {
            manaText.text = (int)mana + " / " + (int)maxMana;
        }
    }

    private void UpdateHealthText()
    {
        if(healthText != null)
        {
            healthText.text = (int)heatlh + " / " + (int)maxHealth;
        }
    }

    private void UpdateLivesText()
    {
        if(livesText != null)
        {
            livesText.text = "Lives: " + GameSave.CurrentLives;
        }
    }

    private void EnsureLivesText()
    {
        if(livesText != null)
        {
            return;
        }

        Canvas canvas = healthText != null ? healthText.GetComponentInParent<Canvas>() : FindObjectOfType<Canvas>();
        if(canvas == null)
        {
            return;
        }

        GameObject livesObject = new GameObject("Lives Text");
        livesObject.transform.SetParent(canvas.transform, false);

        livesText = livesObject.AddComponent<TextMeshProUGUI>();
        livesText.fontSize = 30f;
        livesText.color = Color.white;
        livesText.alignment = TextAlignmentOptions.Left;

        RectTransform rect = livesText.rectTransform;
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 0.5f);
        rect.anchoredPosition = new Vector2(610f, -130f);
        rect.sizeDelta = new Vector2(260f, 50f);
    }

    private void CheckHeight()
    {
        if(GameSave.GodMode)
        {
            return;
        }

        if(player != null && player.position.y < - 15f)
        {
            Dead();
        }
    }

    private void Dead()
    {
        if(GameSave.GodMode || isDead)
        {
            return;
        }

        isDead = true;
        Time.timeScale = 0f;
        bool hasLivesLeft = GameSave.SpendLife();
        UpdateLivesText();
        float delay = audioSystem != null ? audioSystem.PlayPlayerDeath(1f) : 0f;

        if(hasLivesLeft)
        {
            StartCoroutine(ReloadLevel(delay));
        }
        else
        {
            StartCoroutine(LoadDeathScreen(delay));
        }
    }

    private IEnumerator ReloadLevel(float delay)
    {
        if(delay > 0f)
        {
            yield return new WaitForSecondsRealtime(delay);
        }

        GameSave.SaveCurrentHealth(maxHealth);
        GameSave.SaveProgress(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadDeathScreen(float delay)
    {
        if(delay > 0f)
        {
            yield return new WaitForSecondsRealtime(delay);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Death Screen");
    }

    public void Save()
    {
        GameSave.SaveCurrentHealth(heatlh);
    }

    public void SaveFullHealth()
    {
        GameSave.SaveCurrentHealth(maxHealth);
    }

    public void RefreshUpgrades(UpgradeType upgradedType)
    {
        float previousMaxHealth = maxHealth;
        float previousMaxMana = maxMana;

        ApplyUpgradeStats();

        if(upgradedType == UpgradeType.Health)
        {
            heatlh += maxHealth - previousMaxHealth;
        }

        if(upgradedType == UpgradeType.Mana)
        {
            mana += maxMana - previousMaxMana;
        }

        if(heatlh > maxHealth)
        {
            heatlh = maxHealth;
        }

        if(mana > maxMana)
        {
            mana = maxMana;
        }

        if(healthImg != null)
        {
            healthImg.fillAmount = heatlh / maxHealth;
        }

        if(manaImg != null)
        {
            manaImg.fillAmount = mana / maxMana;
        }

        UpdateHealthText();
        UpdateManaText();
    }

    private void Load()
    {
        ApplyUpgradeStats();
        heatlh = GameSave.LoadCurrentHealth(maxHealth, maxHealth);
        mana = maxMana;
    }

    private void CaptureBaseStats()
    {
        if(baseStatsCaptured)
        {
            return;
        }

        baseMaxHealth = maxHealth;
        baseMaxMana = maxMana;
        baseManaRegen = manaRegen;
        baseDamage = damage;
        baseStatsCaptured = true;
    }

    private void ApplyUpgradeStats()
    {
        CaptureBaseStats();

        maxHealth = baseMaxHealth + GameSave.GetHealthBonus();
        maxMana = baseMaxMana + GameSave.GetManaBonus();
        manaRegen = baseManaRegen + GameSave.GetManaRegenBonus();
        damage = baseDamage + GameSave.GetDamageBonus();
    }
}
