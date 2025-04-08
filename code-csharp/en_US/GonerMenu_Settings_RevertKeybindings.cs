using System.Collections;
using TMPro;
using UnityEngine;

public class GonerMenu_Settings_RevertKeybindings : MonoBehaviour
{
    [Header("-= References =-")]
    [SerializeField]
    private GonerMenu_Settings_Element AssosciatedElement;

    [SerializeField]
    private TextMeshProUGUI SettingText;

    [SerializeField]
    private GonerMenu_Settings_Keybind[] Keybindings;

    [SerializeField]
    private Animator Explosion;

    [SerializeField]
    private bool IgnoreGonerMenuOpenState;

    [Header("Only used for the main menu settings, keep null to use global reference")]
    [SerializeField]
    private GonerMenu_Settings IgnoredGonerSettingsManager;

    private bool InputEnabled;

    private int CurrentPresses;

    private int NeededPresses = 3;

    private void OnEnable()
    {
        SettingText.text = "-- Revert to Default --";
    }

    private void Update()
    {
        if (!GonerMenu.Instance.GonerMenuOpen && !IgnoreGonerMenuOpenState)
        {
            AssosciatedElement.CurrentlySelected = false;
        }
        if (AssosciatedElement.CurrentlySelected)
        {
            if (InputEnabled)
            {
                if (CurrentPresses == NeededPresses)
                {
                    PlayerInput.RevertToDefaults();
                    SettingText.text = "-- Revert to Default --";
                    InputEnabled = false;
                    CurrentPresses = 0;
                    Explosion.Play("GonerMenu_Explosion", 0, 0f);
                    if (IgnoredGonerSettingsManager == null)
                    {
                        GonerMenu_Settings.Instance.source.PlayOneShot(GonerMenu_Settings.Instance.SettingSound_Explosion);
                        GonerMenu_Settings.Instance.CancelOutOfCurrentSetting();
                    }
                    else
                    {
                        IgnoredGonerSettingsManager.source.PlayOneShot(IgnoredGonerSettingsManager.SettingSound_Explosion);
                        IgnoredGonerSettingsManager.CancelOutOfCurrentSetting();
                    }
                    StartCoroutine(DelayFromExplosion());
                }
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
                {
                    CurrentPresses++;
                    if (IgnoredGonerSettingsManager == null)
                    {
                        GonerMenu_Settings.Instance.source.PlayOneShot(GonerMenu_Settings.Instance.SettingSound_Tick);
                    }
                    else
                    {
                        IgnoredGonerSettingsManager.source.PlayOneShot(IgnoredGonerSettingsManager.SettingSound_Tick);
                    }
                }
                SettingText.text = $"-- Press {NeededPresses - CurrentPresses} more times to confirm --";
            }
            else
            {
                StartCoroutine(DebounceFromInput());
            }
        }
        else
        {
            SettingText.text = "-- Revert to Default --";
            InputEnabled = false;
            CurrentPresses = 0;
        }
    }

    private IEnumerator DelayFromExplosion()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        GonerMenu_Settings_Keybind[] keybindings = Keybindings;
        for (int i = 0; i < keybindings.Length; i++)
        {
            keybindings[i].UpdateKeybindText();
        }
    }

    private IEnumerator DebounceFromInput()
    {
        yield return new WaitForSecondsRealtime(0f);
        InputEnabled = true;
    }
}
