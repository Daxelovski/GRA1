using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private GameObject audioWindow;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private bool showShop;
    private const string KeyboardControlsSceneName = "Keyboard Controls";
    private const string KeyboardControlsButtonName = "Keyboard Controls Button";

    void Start()
    {
        GameSave.ApplyAudioSettings();
        GameSave.ApplyDisplaySettings();
        RefreshSettingsControls();
        HideShopWindow();
        WireKeyboardControlsButton();
    }

    public void NewGame()
    {
        GameSave.MarkNewRun();
        SceneManager.LoadScene("First Level");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(GameSave.SavedLevel);
    }

    public void Options()
    {
        RefreshSettingsControls();

        if(optionsWindow != null)
        {
            optionsWindow.SetActive(true);
        }

        if(audioWindow != null)
        {
            audioWindow.SetActive(false);
        }

        HideShopWindow();
    }

    public void Audio()
    {
        if(audioWindow != null)
        {
            audioWindow.SetActive(true);
        }

        if(optionsWindow != null)
        {
            optionsWindow.SetActive(false);
        }

        HideShopWindow();
    }

    public void Shop()
    {
        showShop = !showShop;

        if(shopWindow != null)
        {
            shopWindow.SetActive(showShop);
        }

        if(optionsWindow != null)
        {
            optionsWindow.SetActive(false);
        }

        if(audioWindow != null)
        {
            audioWindow.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void KeyboardControls()
    {
        SceneManager.LoadScene(KeyboardControlsSceneName);
    }

    public void Fullscreen()
    {
        SetFullscreen(!GameSave.Fullscreen);
    }

    public void ToggleFPS()
    {
        GameSave.ShowFps = !GameSave.ShowFps;
        PlayerPrefs.Save();
    }

    public void SetFPS(bool value)
    {
        GameSave.ShowFps = value;
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool value)
    {
        GameSave.Fullscreen = value;
        PlayerPrefs.Save();
    }

    public void ToggleAudio()
    {
        GameSave.AudioEnabled = !GameSave.AudioEnabled;
        PlayerPrefs.Save();
    }

    public void SetAudioVolume(float value)
    {
        GameSave.AudioVolume = value;
        GameSave.AudioEnabled = value > 0.001f;
        PlayerPrefs.Save();
    }

    public void ToggleGodMode()
    {
        GameSave.GodMode = !GameSave.GodMode;
        PlayerPrefs.Save();
    }

    private void HideShopWindow()
    {
        showShop = false;

        if(shopWindow != null)
        {
            shopWindow.SetActive(false);
        }
    }

    private void RefreshSettingsControls()
    {
        if(fpsToggle != null)
        {
            fpsToggle.SetIsOnWithoutNotify(GameSave.ShowFps);
        }

        if(fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(GameSave.Fullscreen);
        }

        if(audioSlider != null)
        {
            audioSlider.SetValueWithoutNotify(GameSave.AudioEnabled ? GameSave.AudioVolume : 0f);
        }
    }

    private void WireKeyboardControlsButton()
    {
        Button keyboardControlsButton = FindKeyboardControlsButton();

        if(keyboardControlsButton == null)
        {
            return;
        }

        keyboardControlsButton.onClick = new Button.ButtonClickedEvent();
        keyboardControlsButton.onClick.AddListener(KeyboardControls);
    }

    private Button FindKeyboardControlsButton()
    {
        if(optionsWindow != null)
        {
            Button[] optionButtons = optionsWindow.GetComponentsInChildren<Button>(true);

            foreach(Button button in optionButtons)
            {
                if(button.name == KeyboardControlsButtonName)
                {
                    return button;
                }
            }
        }

        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();

        foreach(Button button in allButtons)
        {
            if(button.name == KeyboardControlsButtonName && button.gameObject.scene.IsValid())
            {
                return button;
            }
        }

        return null;
    }
}
