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
        ApplyVisibility();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            GameSave.ShowFps = !GameSave.ShowFps;
            PlayerPrefs.Save();
            ApplyVisibility();
        }

        if(!GameSave.ShowFps)
        {
            return;
        }

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

    public void ToggleFPS(bool value)
    {
        GameSave.ShowFps = value;
        PlayerPrefs.Save();
        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        if(text != null)
        {
            text.enabled = GameSave.ShowFps;
        }
    }
}
