using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private string[] texts;
    [SerializeField] private int current;
    [SerializeField] private float timer;
    [SerializeField] private float duration;
    
    void Update()
    {
        Work();
    }

    private void Work()
    {
        timer += Time.deltaTime;

        if(timer >= duration)
        {
            loadingText.text = texts[current];
            current ++;

            if(current == texts.Length)
            {
                current = 0;
            }

            timer = 0;
        }
    }
}
