using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;

    [SerializeField]
    private AudioMixer MasterMixer;

    public TMP_FontAsset DyslexicFont;

    public static SettingsManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(instance);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
    }

    private void Update()
    {
        Application.targetFrameRate = 30;
    }

    public void SaveBoolSetting(string SettingName, bool Value)
    {
        PlayerPrefs.SetInt("Setting_" + SettingName, BoolToInt(Value));
    }

    public void SaveIntSetting(string SettingName, int Value)
    {
        PlayerPrefs.SetInt("Setting_" + SettingName, Value);
    }

    public void SaveFloatSetting(string SettingName, float Value)
    {
        PlayerPrefs.SetFloat("Setting_" + SettingName, Value);
    }

    public void SaveStringSetting(string SettingName, string Value)
    {
        PlayerPrefs.SetString("Setting_" + SettingName, Value);
    }

    public bool GetBoolSettingValue(string SettingName, int DefaultValue = 0)
    {
        int @int = PlayerPrefs.GetInt("Setting_" + SettingName, DefaultValue);
        return IntToBool(@int);
    }

    public float GetFloatSettingValue(string SettingName, float DefaultValue = 0f)
    {
        return PlayerPrefs.GetFloat("Setting_" + SettingName, DefaultValue);
    }

    public string GetStringSettingValue(string SettingName, string DefaultValue = "")
    {
        return PlayerPrefs.GetString("Setting_" + SettingName, DefaultValue);
    }

    public int GetIntSettingValue(string SettingName, int DefaultValue = 0)
    {
        return PlayerPrefs.GetInt("Setting_" + SettingName, DefaultValue);
    }

    public void ApplySettings()
    {
        if (GetBoolSettingValue("Fullscreen"))
        {
            Screen.SetResolution(1280, 960, FullScreenMode.FullScreenWindow, new RefreshRate
            {
                numerator = 30u,
                denominator = 1u
            });
        }
        else
        {
            Screen.SetResolution(1280, 960, FullScreenMode.Windowed, new RefreshRate
            {
                numerator = 30u,
                denominator = 1u
            });
        }
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
        if (GetBoolSettingValue("VSYNC"))
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
        }
        MasterMixer.SetFloat("MasterVolume", Mathf.Log10(GetFloatSettingValue("MasterVolume", 100f)) * 20f);
        MasterMixer.SetFloat("DialogueVolume", Mathf.Log10(GetFloatSettingValue("DialogueVolume", 100f)) * 20f);
        MasterMixer.SetFloat("EffectVolume", Mathf.Log10(GetFloatSettingValue("EffectVolume", 100f)) * 20f);
        MasterMixer.SetFloat("MusicVolume", Mathf.Log10(GetFloatSettingValue("MusicVolume", 100f)) * 20f);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
    }

    private int BoolToInt(bool value)
    {
        if (!value)
        {
            return 0;
        }
        return 1;
    }

    private bool IntToBool(int value)
    {
        if (value <= 0)
        {
            return false;
        }
        return true;
    }
}
