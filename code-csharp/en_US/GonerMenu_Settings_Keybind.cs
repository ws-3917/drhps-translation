using System;
using TMPro;
using UnityEngine;

public class GonerMenu_Settings_Keybind : MonoBehaviour
{
    [Header("-= References =-")]
    [SerializeField]
    private GonerMenu_Settings_Element AssosciatedElement;

    [SerializeField]
    private TextMeshProUGUI KeybindText;

    [SerializeField]
    private bool IgnoreGonerMenuOpenState;

    [Header("Only used for the main menu settings, keep null to use global reference")]
    [SerializeField]
    private GonerMenu_Settings IgnoredGonerSettingsManager;

    [Header("-= Settings =-")]
    [Header("Is saved as Setting_<KeyBinding>")]
    [SerializeField]
    private string KeyBinding;

    [SerializeField]
    private KeyCode DefaultKeyBinding;

    private int currentKeyCode;

    private void OnEnable()
    {
        LoadKeybinding();
    }

    private void LoadKeybinding()
    {
        currentKeyCode = PlayerPrefs.GetInt("Setting_" + KeyBinding, (int)DefaultKeyBinding);
        TextMeshProUGUI keybindText = KeybindText;
        KeyCode keyCode = (KeyCode)currentKeyCode;
        keybindText.text = keyCode.ToString();
    }

    private void Update()
    {
        if (!GonerMenu.Instance.GonerMenuOpen && !IgnoreGonerMenuOpenState)
        {
            AssosciatedElement.CurrentlySelected = false;
        }
        if (!AssosciatedElement.CurrentlySelected)
        {
            return;
        }
        foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(value) && !PlayerInput.HoldingAnyImportantKeys())
            {
                currentKeyCode = (int)value;
                KeybindText.text = value.ToString();
                PlayerPrefs.SetInt("Setting_" + KeyBinding, currentKeyCode);
                if (IgnoredGonerSettingsManager != null)
                {
                    IgnoredGonerSettingsManager.CancelOutOfCurrentSetting();
                }
                else
                {
                    GonerMenu_Settings.Instance.CancelOutOfCurrentSetting();
                }
                SettingsManager.Instance.ApplySettings();
                Debug.Log("New keycode set: " + value);
                PlayerInput.LoadKeybinding();
            }
        }
    }

    public void UpdateKeybindText()
    {
        KeybindText.text = string.Format("{0}", (KeyCode)PlayerPrefs.GetInt("Setting_" + KeyBinding, (int)DefaultKeyBinding));
    }
}
