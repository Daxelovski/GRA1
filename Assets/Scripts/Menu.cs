using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private GameObject audioWindow;

    public void NewGame()
    {
        SceneManager.LoadScene("First Level");
    }

    public void Options()
    {
        optionsWindow.SetActive(true);
        audioWindow.SetActive(false);
    }

    public void Audio()
    {
        audioWindow.SetActive(true);
        optionsWindow.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Fullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
