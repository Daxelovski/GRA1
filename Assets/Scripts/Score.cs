using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        Load();
        UpdateText();
    }

    public void AddScore(int value)
    {
        score += value;
        GameSave.Score = score;
        UpdateText();
    }

    private void Load()
    {
        score = GameSave.Score;
    }

    public void Save()
    {
        GameSave.Score = score;
    }

    private void UpdateText()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
