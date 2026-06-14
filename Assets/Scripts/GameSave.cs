using UnityEngine;

public enum UpgradeType
{
    Health,
    Damage,
    Mana,
    Speed
}

public static class GameSave
{
    public const int MaxLives = 3;

    private const string ScoreKey = "Score";
    private const string CrystalsKey = "Coins";
    private const string SavedLevelKey = "SavedLevel";
    private const string HasSaveKey = "HasSave";
    private const string ResetRunKey = "ResetRunOnFirstLevel";
    private const string CurrentHealthKey = "CurrentHealth";
    private const string CurrentLivesKey = "CurrentLives";

    private const string FpsKey = "Settings_ShowFPS";
    private const string FullscreenKey = "Settings_Fullscreen";
    private const string AudioEnabledKey = "Settings_AudioEnabled";
    private const string AudioVolumeKey = "Settings_AudioVolume";
    private const string GodModeKey = "Settings_GodMode";

    private const string HealthLevelKey = "Upgrade_HealthLevel";
    private const string DamageLevelKey = "Upgrade_DamageLevel";
    private const string ManaLevelKey = "Upgrade_ManaLevel";
    private const string SpeedLevelKey = "Upgrade_SpeedLevel";

    public static bool ShowFps
    {
        get { return PlayerPrefs.GetInt(FpsKey, 1) == 1; }
        set { PlayerPrefs.SetInt(FpsKey, value ? 1 : 0); }
    }

    public static bool Fullscreen
    {
        get { return PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0);
            Screen.fullScreen = value;
        }
    }

    public static bool AudioEnabled
    {
        get { return PlayerPrefs.GetInt(AudioEnabledKey, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(AudioEnabledKey, value ? 1 : 0);
            ApplyAudioSettings();
        }
    }

    public static bool GodMode
    {
        get { return PlayerPrefs.GetInt(GodModeKey, 0) == 1; }
        set { PlayerPrefs.SetInt(GodModeKey, value ? 1 : 0); }
    }

    public static float AudioVolume
    {
        get { return PlayerPrefs.GetFloat(AudioVolumeKey, 1f); }
        set
        {
            PlayerPrefs.SetFloat(AudioVolumeKey, Mathf.Clamp01(value));
            ApplyAudioSettings();
        }
    }

    public static int Crystals
    {
        get { return PlayerPrefs.GetInt(CrystalsKey, 0); }
        set { PlayerPrefs.SetInt(CrystalsKey, Mathf.Max(0, value)); }
    }

    public static int Score
    {
        get { return PlayerPrefs.GetInt(ScoreKey, 0); }
        set { PlayerPrefs.SetInt(ScoreKey, Mathf.Max(0, value)); }
    }

    public static bool HasSave
    {
        get { return PlayerPrefs.GetInt(HasSaveKey, 0) == 1; }
    }

    public static bool ShouldResetRunOnFirstLevel
    {
        get { return PlayerPrefs.GetInt(ResetRunKey, 0) == 1; }
    }

    public static string SavedLevel
    {
        get { return PlayerPrefs.GetString(SavedLevelKey, "First Level"); }
    }

    public static int CurrentLives
    {
        get { return Mathf.Clamp(PlayerPrefs.GetInt(CurrentLivesKey, MaxLives), 0, MaxLives); }
        set { PlayerPrefs.SetInt(CurrentLivesKey, Mathf.Clamp(value, 0, MaxLives)); }
    }

    public static void MarkNewRun()
    {
        ResetRunData(false);
    }

    public static void ResetRunData(bool keepCrystalsAndUpgrades)
    {
        PlayerPrefs.SetInt(ResetRunKey, 0);
        PlayerPrefs.SetInt(HasSaveKey, 1);
        PlayerPrefs.SetString(SavedLevelKey, "First Level");
        PlayerPrefs.DeleteKey(CurrentHealthKey);
        CurrentLives = MaxLives;
        Score = 0;

        if(!keepCrystalsAndUpgrades)
        {
            Crystals = 0;
            SetUpgradeLevel(UpgradeType.Health, 0);
            SetUpgradeLevel(UpgradeType.Damage, 0);
            SetUpgradeLevel(UpgradeType.Mana, 0);
            SetUpgradeLevel(UpgradeType.Speed, 0);
        }

        PlayerPrefs.Save();
    }

    public static bool SpendLife()
    {
        CurrentLives -= 1;
        PlayerPrefs.Save();
        return CurrentLives > 0;
    }

    public static void SaveProgress(string nextLevel)
    {
        PlayerPrefs.SetInt(HasSaveKey, 1);

        if(!string.IsNullOrEmpty(nextLevel))
        {
            PlayerPrefs.SetString(SavedLevelKey, nextLevel);
        }

        PlayerPrefs.Save();
    }

    public static void SaveCurrentHealth(float health)
    {
        PlayerPrefs.SetFloat(CurrentHealthKey, Mathf.Max(0f, health));
    }

    public static float LoadCurrentHealth(float fallbackHealth, float maxHealth)
    {
        float savedHealth = PlayerPrefs.GetFloat(CurrentHealthKey, fallbackHealth);
        return Mathf.Clamp(savedHealth, 1f, maxHealth);
    }

    public static void ApplyAudioSettings()
    {
        AudioListener.volume = AudioEnabled ? AudioVolume : 0f;
    }

    public static int GetUpgradeLevel(UpgradeType type)
    {
        return PlayerPrefs.GetInt(GetUpgradeKey(type), 0);
    }

    public static int GetUpgradeCost(UpgradeType type)
    {
        int level = GetUpgradeLevel(type);

        switch(type)
        {
            case UpgradeType.Health:
                return 30 + level * 20;
            case UpgradeType.Damage:
                return 35 + level * 25;
            case UpgradeType.Mana:
                return 25 + level * 20;
            case UpgradeType.Speed:
                return 30 + level * 25;
            default:
                return 999;
        }
    }

    public static bool TryBuyUpgrade(UpgradeType type, out string message)
    {
        int level = GetUpgradeLevel(type);

        if(level >= 5)
        {
            message = "Upgrade is already maxed.";
            return false;
        }

        int cost = GetUpgradeCost(type);

        if(Crystals < cost)
        {
            message = "Not enough crystals.";
            return false;
        }

        Crystals -= cost;
        SetUpgradeLevel(type, level + 1);
        PlayerPrefs.Save();
        message = "Upgrade bought.";
        return true;
    }

    public static float GetHealthBonus()
    {
        return GetUpgradeLevel(UpgradeType.Health) * 20f;
    }

    public static int GetDamageBonus()
    {
        return GetUpgradeLevel(UpgradeType.Damage) * 5;
    }

    public static float GetManaBonus()
    {
        return GetUpgradeLevel(UpgradeType.Mana) * 15f;
    }

    public static float GetManaRegenBonus()
    {
        return GetUpgradeLevel(UpgradeType.Mana) * 0.75f;
    }

    public static float GetSpeedBonus()
    {
        return GetUpgradeLevel(UpgradeType.Speed) * 0.55f;
    }

    public static float GetSprintBonus()
    {
        return GetUpgradeLevel(UpgradeType.Speed) * 0.9f;
    }

    public static string GetUpgradeLabel(UpgradeType type)
    {
        switch(type)
        {
            case UpgradeType.Health:
                return "Health";
            case UpgradeType.Damage:
                return "Damage";
            case UpgradeType.Mana:
                return "Mana";
            case UpgradeType.Speed:
                return "Speed";
            default:
                return "Upgrade";
        }
    }

    private static void SetUpgradeLevel(UpgradeType type, int level)
    {
        PlayerPrefs.SetInt(GetUpgradeKey(type), Mathf.Clamp(level, 0, 5));
    }

    private static string GetUpgradeKey(UpgradeType type)
    {
        switch(type)
        {
            case UpgradeType.Health:
                return HealthLevelKey;
            case UpgradeType.Damage:
                return DamageLevelKey;
            case UpgradeType.Mana:
                return ManaLevelKey;
            case UpgradeType.Speed:
                return SpeedLevelKey;
            default:
                return HealthLevelKey;
        }
    }
}
