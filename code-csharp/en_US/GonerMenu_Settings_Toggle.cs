using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GonerMenu_Settings_Toggle : MonoBehaviour
{
    [Header("-= References =-")]
    [SerializeField]
    private GonerMenu_Settings_Element AssosciatedElement;

    [SerializeField]
    private RawImage ToggleTickRenderer;

    [Header("-= Settings =-")]
    [Header("Is saved as Setting_<SettingName>")]
    [SerializeField]
    private string SettingName;

    private GonerMenu_Settings settings;

    private bool InputEnabled;

    [SerializeField]
    private bool UpdateDyslexicFontsOnChange;

    [SerializeField]
    private bool IgnoreGonerMenuOpenState;

    [Header("Only used for the main menu settings, keep null to use global reference")]
    [SerializeField]
    private GonerMenu_Settings IgnoredGonerSettingsManager;

    private void OnEnable()
    {
        if ((bool)IgnoredGonerSettingsManager)
        {
            settings = IgnoredGonerSettingsManager;
        }
        else
        {
            settings = GonerMenu_Settings.Instance;
        }
        ToggleTickRenderer.enabled = SettingsManager.Instance.GetBoolSettingValue(SettingName);
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
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
                {
                    SettingsManager.Instance.SaveBoolSetting(SettingName, !SettingsManager.Instance.GetBoolSettingValue(SettingName));
                    ToggleTickRenderer.enabled = SettingsManager.Instance.GetBoolSettingValue(SettingName);
                    settings.source.PlayOneShot(settings.SettingSound_Tick);
                    if (UpdateDyslexicFontsOnChange)
                    {
                        settings.UpdateSettingFontsDyslexic();
                    }
                    SettingsManager.Instance.ApplySettings();
                }
            }
            else
            {
                StartCoroutine(DebounceFromInput());
            }
        }
        else
        {
            InputEnabled = false;
        }
    }

    private IEnumerator DebounceFromInput()
    {
        yield return new WaitForSeconds(0f);
        InputEnabled = true;
    }
}
