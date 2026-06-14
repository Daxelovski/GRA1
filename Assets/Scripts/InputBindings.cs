using UnityEngine;

public enum GameInputAction
{
    None = 0,
    MoveLeft = 1,
    MoveRight = 2,
    Jump = 3,
    Attack = 4
}

public static class InputBindings
{
    private const string Prefix = "Keybind_";

    public static readonly GameInputAction[] RebindableActions =
    {
        GameInputAction.MoveLeft,
        GameInputAction.MoveRight,
        GameInputAction.Jump,
        GameInputAction.Attack
    };

    public static KeyCode GetKey(GameInputAction action)
    {
        return (KeyCode)PlayerPrefs.GetInt(GetPrefsKey(action), (int)GetDefaultKey(action));
    }

    public static bool GetKeyDown(GameInputAction action)
    {
        return Input.GetKeyDown(GetKey(action));
    }

    public static bool GetKeyUp(GameInputAction action)
    {
        return Input.GetKeyUp(GetKey(action));
    }

    public static float GetHorizontalRaw()
    {
        float horizontal = 0f;

        if(Input.GetKey(GetKey(GameInputAction.MoveLeft)))
        {
            horizontal -= 1f;
        }

        if(Input.GetKey(GetKey(GameInputAction.MoveRight)))
        {
            horizontal += 1f;
        }

        return Mathf.Clamp(horizontal, -1f, 1f);
    }

    public static void SetKey(GameInputAction action, KeyCode key)
    {
        if(action == GameInputAction.None || IsAllowedRebindKey(key) == false)
        {
            return;
        }

        PlayerPrefs.SetInt(GetPrefsKey(action), (int)key);
        PlayerPrefs.Save();
    }

    public static void ResetDefaults()
    {
        foreach(GameInputAction action in RebindableActions)
        {
            PlayerPrefs.DeleteKey(GetPrefsKey(action));
        }

        PlayerPrefs.Save();
    }

    public static GameInputAction GetActionUsingKey(KeyCode key, GameInputAction exceptAction)
    {
        foreach(GameInputAction action in RebindableActions)
        {
            if(action != exceptAction && GetKey(action) == key)
            {
                return action;
            }
        }

        return GameInputAction.None;
    }

    public static bool IsAllowedRebindKey(KeyCode key)
    {
        if(key == KeyCode.None || IsReservedKey(key))
        {
            return false;
        }

        string keyName = key.ToString();
        return keyName.StartsWith("Mouse") == false && keyName.StartsWith("Joystick") == false;
    }

    public static bool IsReservedKey(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.Escape:
            case KeyCode.E:
            case KeyCode.G:
            case KeyCode.F3:
            case KeyCode.LeftShift:
            case KeyCode.RightShift:
                return true;
            default:
                return false;
        }
    }

    public static string GetActionLabel(GameInputAction action)
    {
        switch(action)
        {
            case GameInputAction.MoveLeft:
                return "Move Left";
            case GameInputAction.MoveRight:
                return "Move Right";
            case GameInputAction.Jump:
                return "Jump";
            case GameInputAction.Attack:
                return "Attack";
            default:
                return "Unknown";
        }
    }

    public static string GetKeyLabel(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.Space:
                return "Space";
            case KeyCode.LeftArrow:
                return "Left Arrow";
            case KeyCode.RightArrow:
                return "Right Arrow";
            case KeyCode.UpArrow:
                return "Up Arrow";
            case KeyCode.DownArrow:
                return "Down Arrow";
            case KeyCode.LeftShift:
                return "Left Shift";
            case KeyCode.RightShift:
                return "Right Shift";
            default:
                return key.ToString();
        }
    }

    private static string GetPrefsKey(GameInputAction action)
    {
        return Prefix + action;
    }

    private static KeyCode GetDefaultKey(GameInputAction action)
    {
        switch(action)
        {
            case GameInputAction.MoveLeft:
                return KeyCode.A;
            case GameInputAction.MoveRight:
                return KeyCode.D;
            case GameInputAction.Jump:
                return KeyCode.Space;
            case GameInputAction.Attack:
                return KeyCode.F;
            default:
                return KeyCode.None;
        }
    }
}
