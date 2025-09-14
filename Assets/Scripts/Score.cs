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
        scoreText.text = "Score: " + score; 
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score; 
    }

    private void Load()
    {
        score = PlayerPrefs.GetInt("Score", 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Score", score);
    }
}
