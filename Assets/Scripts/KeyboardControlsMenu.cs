using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardControlsMenu : MonoBehaviour
{
    private GameInputAction waitingForAction = GameInputAction.None;
    private string message = "";

    private void Awake()
    {
        GameSave.ApplyAudioSettings();
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 18;
        GUI.skin.button.fontSize = 18;
        GUI.skin.box.fontSize = 18;

        HandleKeyCapture(Event.current);

        float width = 620f;
        float height = 640f;
        Rect area = new Rect((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);

        GUILayout.BeginArea(area, GUI.skin.box);
        GUILayout.Space(8f);
        DrawCenteredLabel("Keyboard Controls", 32);
        GUILayout.Space(20f);

        GUILayout.Label("Editable controls");
        GUILayout.Space(8f);

        foreach(GameInputAction action in InputBindings.RebindableActions)
        {
            DrawBindingRow(action);
        }

        GUILayout.Space(18f);
        GUILayout.Label("Static controls");
        GUILayout.Label("Sprint: Left Shift");
        GUILayout.Label("Pause: Escape");
        GUILayout.Label("Interact / portal: E");
        GUILayout.Label("Greyscale test: G");
        GUILayout.Label("FPS debug: F3");

        GUILayout.Space(18f);

        if(waitingForAction != GameInputAction.None)
        {
            GUILayout.Label("Press a key for: " + InputBindings.GetActionLabel(waitingForAction));
            GUILayout.Label("Escape cancels. Static controls cannot be assigned.");
        }
        else if(string.IsNullOrEmpty(message) == false)
        {
            GUILayout.Label(message);
        }

        GUILayout.FlexibleSpace();

        if(GUILayout.Button("Reset Defaults", GUILayout.Height(46f)))
        {
            InputBindings.ResetDefaults();
            waitingForAction = GameInputAction.None;
            message = "Default controls restored.";
        }

        if(GUILayout.Button("Back", GUILayout.Height(46f)))
        {
            SceneManager.LoadScene("Menu");
        }

        GUILayout.Space(8f);
        GUILayout.EndArea();
    }

    private void DrawBindingRow(GameInputAction action)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(InputBindings.GetActionLabel(action), GUILayout.Width(220f), GUILayout.Height(42f));

        string buttonLabel = waitingForAction == action ? "Press any key..." : InputBindings.GetKeyLabel(InputBindings.GetKey(action));
        if(GUILayout.Button(buttonLabel, GUILayout.Height(42f)))
        {
            waitingForAction = action;
            message = "";
        }

        GUILayout.EndHorizontal();
    }

    private void DrawCenteredLabel(string text, int fontSize)
    {
        int previousSize = GUI.skin.label.fontSize;
        TextAnchor previousAlignment = GUI.skin.label.alignment;

        GUI.skin.label.fontSize = fontSize;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(text, GUILayout.Height(50f));

        GUI.skin.label.fontSize = previousSize;
        GUI.skin.label.alignment = previousAlignment;
    }

    private void HandleKeyCapture(Event currentEvent)
    {
        if(waitingForAction == GameInputAction.None || currentEvent.type != EventType.KeyDown)
        {
            return;
        }

        KeyCode pressedKey = currentEvent.keyCode;

        if(pressedKey == KeyCode.Escape)
        {
            waitingForAction = GameInputAction.None;
            message = "Binding cancelled.";
            currentEvent.Use();
            return;
        }

        if(InputBindings.IsAllowedRebindKey(pressedKey) == false)
        {
            message = "This key is reserved or unsupported.";
            currentEvent.Use();
            return;
        }

        GameInputAction duplicateAction = InputBindings.GetActionUsingKey(pressedKey, waitingForAction);
        if(duplicateAction != GameInputAction.None)
        {
            message = "This key is already used by " + InputBindings.GetActionLabel(duplicateAction) + ".";
            currentEvent.Use();
            return;
        }

        InputBindings.SetKey(waitingForAction, pressedKey);
        message = InputBindings.GetActionLabel(waitingForAction) + " set to " + InputBindings.GetKeyLabel(pressedKey) + ".";
        waitingForAction = GameInputAction.None;
        currentEvent.Use();
    }
}
