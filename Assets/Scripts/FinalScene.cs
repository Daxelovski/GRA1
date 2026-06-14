using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{
    private void Awake()
    {
        GameSave.ApplyAudioSettings();
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
