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
        coins = Mathf.Max(0, coins + value);
        GameSave.Crystals = coins;
        UpdateText();
    }

    private void Load()
    {
        coins = GameSave.Crystals;
        UpdateText();
    }

    public void Save()
    {
        GameSave.Crystals = coins;
    }

    public void RefreshFromSave()
    {
        coins = GameSave.Crystals;
        UpdateText();
    }

    public void UpdateText()
    {
        if(coinsText != null)
        {
            coinsText.text = "Crystals: " + coins;
        }
    }
}
