using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GonerMenu_Settings_VolumeSlider : MonoBehaviour
{
    [Header("-= References =-")]
    [SerializeField]
    private GonerMenu_Settings_Element AssosciatedElement;

    [SerializeField]
    private TextMeshProUGUI VolumeText;

    [SerializeField]
    private AudioMixer TargetMixer;

    [SerializeField]
    private string VolumeExposedParam;

    [Header("-= Settings =-")]
    [Header("Is saved as Setting_<SettingName>")]
    [SerializeField]
    private string SettingName;

    [SerializeField]
    private float Minimum = 1E-05f;

    [SerializeField]
    private float Maximum = 1f;

    [SerializeField]
    private float Stepsize = 0.02f;

    private GonerMenu_Settings settings;

    private Coroutine InputCoroutine;

    private bool LeftHeld;

    private bool RightHeld;

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
        VolumeText.text = Mathf.Round(SettingsManager.Instance.GetFloatSettingValue(SettingName) * 100f) + "%";
    }

    private void Update()
    {
        if (!GonerMenu.Instance.GonerMenuOpen && !IgnoreGonerMenuOpenState)
        {
            AssosciatedElement.CurrentlySelected = false;
        }
        if (AssosciatedElement.CurrentlySelected)
        {
            if (InputCoroutine == null)
            {
                InputCoroutine = StartCoroutine(InputLoop());
            }
            LeftHeld = Input.GetKey(PlayerInput.Instance.Key_Left);
            RightHeld = Input.GetKey(PlayerInput.Instance.Key_Right);
        }
        else if (InputCoroutine != null)
        {
            StopCoroutine(InputCoroutine);
            InputCoroutine = null;
        }
    }

    private IEnumerator InputLoop()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        if (AssosciatedElement.CurrentlySelected)
        {
            if (LeftHeld)
            {
                SettingsManager.Instance.SaveFloatSetting(SettingName, Mathf.Clamp(SettingsManager.Instance.GetFloatSettingValue(SettingName) - Stepsize, Minimum, Maximum));
                SetMixerLevel(SettingsManager.Instance.GetFloatSettingValue(SettingName));
            }
            if (RightHeld)
            {
                SettingsManager.Instance.SaveFloatSetting(SettingName, Mathf.Clamp(SettingsManager.Instance.GetFloatSettingValue(SettingName) + Stepsize, Minimum, Maximum));
                SetMixerLevel(SettingsManager.Instance.GetFloatSettingValue(SettingName));
            }
            if ((RightHeld || LeftHeld) && SettingsManager.Instance.GetFloatSettingValue(SettingName) < Maximum && SettingsManager.Instance.GetFloatSettingValue(SettingName) > Minimum)
            {
                settings.source.PlayOneShot(settings.SettingSound_Blip, Mathf.Clamp(SettingsManager.Instance.GetFloatSettingValue(SettingName), 0f, 2f));
            }
        }
        StartCoroutine(InputLoop());
    }

    private void SetMixerLevel(float value)
    {
        if (VolumeExposedParam != "")
        {
            TargetMixer.SetFloat(VolumeExposedParam, Mathf.Log10(value) * 20f);
        }
        VolumeText.text = Mathf.Round(SettingsManager.Instance.GetFloatSettingValue(SettingName) * 100f) + "%";
    }
}
