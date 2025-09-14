using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int current;

    public float time;
    public int frameCount;

    void Start()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = 0;

    }

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if(time > 1f)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            text.text = frameRate + " FPS";
            time -= 1f;
            frameCount = 0;
        }
    }
}