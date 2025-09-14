using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    public int coins;
    [SerializeField] private TextMeshProUGUI coinsText;

    void Start()
    {
        Load();
        UpdateText();
    }

    public void AddCrystals(int value)
    {
        coins += value;
        UpdateText();
    }

    private void Load()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateText();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void UpdateText()
    {
        coinsText.text = "Crystals: " + coins;
    }
}
