using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private MoneySystem money;
    [SerializeField] private Score score;
    [SerializeField] private float timer;
    [SerializeField] private PlayerStats player;

    public bool nearPortal;

    [SerializeField] private GameObject portalWindow;
    [SerializeField] private Crystal[] crystals;
    [SerializeField] private bool holdKey;
    [SerializeField] private string nextScene;
    [SerializeField] private bool bossDefeated;
    [SerializeField] private bool sceneTransitionStarted;
    [SerializeField] private Material grayscaleMaterial;
    [SerializeField] private KeyCode grayscaleToggleKey = KeyCode.G;
    [SerializeField] private float grayscaleFadeSpeed = 8f;

    private readonly HashSet<Crystal> subscribedCrystals = new HashSet<Crystal>();
    private bool paused;
    private bool showShop;
    private string pauseMessage;
    private bool grayscaleEnabled;
    private const float GrayscaleOnBlend = 0f;
    private const float GrayscaleOffBlend = 1f;

    void Start()
    {
        Time.timeScale = 1f;
        GameSave.ApplyAudioSettings();
        GameSave.ApplyDisplaySettings();
        CacheSceneReferences();
        Find();
        SetEvents();
        grayscaleEnabled = false;
        SetGrayscaleBlend(GrayscaleOffBlend);
    }

    void Update()
    {
        HandleGrayscaleToggle();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if(paused)
        {
            return;
        }

        CheckPortal();
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        SetGrayscaleBlend(GrayscaleOffBlend);
    }

    private void HandleGrayscaleToggle()
    {
        if(grayscaleMaterial == null || grayscaleMaterial.HasProperty("_Blend") == false)
        {
            return;
        }

        if(Input.GetKeyDown(grayscaleToggleKey))
        {
            grayscaleEnabled = !grayscaleEnabled;
        }

        float current = grayscaleMaterial.GetFloat("_Blend");
        float target = grayscaleEnabled ? GrayscaleOnBlend : GrayscaleOffBlend;
        float speed = grayscaleFadeSpeed <= 0f ? 1000f : grayscaleFadeSpeed;
        SetGrayscaleBlend(Mathf.MoveTowards(current, target, speed * Time.unscaledDeltaTime));
    }

    private void SetGrayscaleBlend(float value)
    {
        if(grayscaleMaterial != null && grayscaleMaterial.HasProperty("_Blend"))
        {
            grayscaleMaterial.SetFloat("_Blend", value);
        }
    }

    private void CheckPortal()
    {
        if(sceneTransitionStarted)
        {
            return;
        }

        if(nearPortal == true)
        {
            if(IsBossScene() && !bossDefeated)
            {
                if(portalWindow != null && portalWindow.activeSelf == true)
                {
                    portalWindow.SetActive(false);
                }

                return;
            }

            if(portalWindow != null && portalWindow.activeSelf == false)
            {
                portalWindow.SetActive(true);
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                LoadNextScene();
            }
        }
        else if(portalWindow != null && portalWindow.activeSelf == true)
        {
            portalWindow.SetActive(false);
        }
    }

    private void LoadNextScene()
    {
        sceneTransitionStarted = true;
        Time.timeScale = 1f;
        string sceneToLoad = ResolveNextScene();

        SaveCurrentProgress(sceneToLoad, true);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SaveCurrentProgress(string sceneToSave)
    {
        SaveCurrentProgress(sceneToSave, false);
    }

    private void SaveCurrentProgress(string sceneToSave, bool refillHealthOnLoad)
    {
        CacheSceneReferences();

        if(player != null)
        {
            if(refillHealthOnLoad)
            {
                player.SaveFullHealth();
            }
            else
            {
                player.Save();
            }
        }

        if(score != null)
        {
            score.Save();
        }

        if(money != null)
        {
            money.Save();
        }

        GameSave.SaveProgress(sceneToSave);
    }

    private string ResolveNextScene()
    {
        if(!string.IsNullOrEmpty(nextScene))
        {
            return nextScene;
        }

        switch(SceneManager.GetActiveScene().name)
        {
            case "First Level":
                return "Second Level";
            case "Second Level":
                return "Third Level";
            case "Third Level":
                return "Fourth Level";
            case "Fourth Level":
                return "Fifth Level";
            case "Fifth Level":
                return "Boss Level";
            case "Boss Level":
                return "Final";
            default:
                return "Final";
        }
    }

    public void Find()
    {
        crystals = GameObject.FindObjectsOfType<Crystal>();
    }

    private void CacheSceneReferences()
    {
        if(audioSystem == null)
        {
            audioSystem = FindObjectOfType<AudioSystem>();
        }

        if(money == null)
        {
            money = FindObjectOfType<MoneySystem>();
        }

        if(score == null)
        {
            score = FindObjectOfType<Score>();
        }

        if(player == null)
        {
            player = FindObjectOfType<PlayerStats>();
        }
    }

    public void SetEvents()
    {
        foreach(var item in crystals)
        {
            if(item == null || subscribedCrystals.Contains(item))
            {
                continue;
            }

            item.pickupEvent += AddScorceAndCurrency;
            subscribedCrystals.Add(item);
        }
    }

    private void AddScorceAndCurrency()
    {
        if(score != null)
        {
            score.AddScore(50);
        }

        if(money != null)
        {
            money.AddCrystals(5);
        }

        if(audioSystem != null)
        {
            audioSystem.PlayAudio(1, 1);
        }
    }

    public void CollectPotion()
    {
        if(audioSystem != null)
        {
            audioSystem.PlayAudio(2, 1);
        }
    }

    public void BossDefeated()
    {
        bossDefeated = true;
        pauseMessage = "Boss defeated. Portal unlocked.";
    }

    private bool IsBossScene()
    {
        return SceneManager.GetActiveScene().name == "Boss Level";
    }

    private void TogglePause()
    {
        paused = !paused;
        showShop = false;
        Time.timeScale = paused ? 0f : 1f;
    }

    private void OnGUI()
    {
        if(!paused)
        {
            return;
        }

        GUI.skin.button.fontSize = 16;
        GUI.skin.label.fontSize = 15;

        float width = 330f;
        float height = showShop ? 460f : 310f;
        Rect area = new Rect((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);

        GUILayout.BeginArea(area, GUI.skin.box);
        GUILayout.Label("Paused");
        GUILayout.Label("Lives: " + GameSave.CurrentLives);
        GUILayout.Label("Crystals: " + GameSave.Crystals);

        if(GUILayout.Button("Resume"))
        {
            TogglePause();
        }

        if(GUILayout.Button("Save"))
        {
            SaveCurrentProgress(SceneManager.GetActiveScene().name);
            pauseMessage = "Game saved.";
        }

        if(GUILayout.Button("Shop"))
        {
            showShop = !showShop;
        }

        if(GUILayout.Button("FPS: " + (GameSave.ShowFps ? "ON" : "OFF")))
        {
            GameSave.ShowFps = !GameSave.ShowFps;
            PlayerPrefs.Save();
        }

        if(GUILayout.Button("Audio: " + (GameSave.AudioEnabled ? "ON" : "OFF")))
        {
            GameSave.AudioEnabled = !GameSave.AudioEnabled;
            PlayerPrefs.Save();
        }

        if(GUILayout.Button("God Mode: " + (GameSave.GodMode ? "ON" : "OFF")))
        {
            GameSave.GodMode = !GameSave.GodMode;
            PlayerPrefs.Save();
        }

        if(GUILayout.Button("Main Menu"))
        {
            Time.timeScale = 1f;
            SaveCurrentProgress(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Menu");
        }

        if(showShop)
        {
            DrawUpgradeButton(UpgradeType.Health);
            DrawUpgradeButton(UpgradeType.Damage);
            DrawUpgradeButton(UpgradeType.Mana);
            DrawUpgradeButton(UpgradeType.Speed);
        }

        if(!string.IsNullOrEmpty(pauseMessage))
        {
            GUILayout.Label(pauseMessage);
        }

        GUILayout.EndArea();
    }

    private void DrawUpgradeButton(UpgradeType type)
    {
        int level = GameSave.GetUpgradeLevel(type);
        string label = GameSave.GetUpgradeLabel(type) + " Lv. " + level + "/5 - " + GameSave.GetUpgradeCost(type) + " crystals";

        if(GUILayout.Button(label))
        {
            if(GameSave.TryBuyUpgrade(type, out pauseMessage))
            {
                ApplyPurchasedUpgrade(type);
            }
        }
    }

    private void ApplyPurchasedUpgrade(UpgradeType type)
    {
        CacheSceneReferences();

        if(player != null)
        {
            player.RefreshUpgrades(type);
        }

        Movement movement = player != null ? player.GetComponent<Movement>() : null;
        if(movement == null)
        {
            movement = FindObjectOfType<Movement>();
        }

        if(movement != null)
        {
            movement.RefreshUpgrades();
        }

        if(money != null)
        {
            money.RefreshFromSave();
        }
    }

}
