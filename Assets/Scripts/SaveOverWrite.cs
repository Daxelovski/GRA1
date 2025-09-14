using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOverWrite : MonoBehaviour
{
    void Awake()
    {
        OverWrite();
    }

    private void OverWrite()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetFloat("Health", 100);
    }
}
