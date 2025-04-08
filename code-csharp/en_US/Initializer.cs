using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    public GameObject DiscordCont;

    public bool DeleteAllData;

    public DRHDebugManager debug;

    private bool canEnableDebugMode = true;

    private void Awake()
    {
        Screen.fullScreen = false;
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Object.DontDestroyOnLoad(DiscordCont);
        int num = SecurePlayerPrefs.GetSecureInt("TotalCash");
        if (num % 10 != 0)
        {
            num = (num / 10 + 1) * 10;
        }
        SecurePlayerPrefs.SetSecureInt("TotalCash", num);
        if (SecurePlayerPrefs.HasDetectedFiltyHacker)
        {
            StartCoroutine(InitializePunishment());
        }
        else if (SceneManager.sceneCount < 2)
        {
            StartCoroutine(InitializeDelay());
        }
        else
        {
            StartCoroutine(InitializeSceneUnload());
        }
        if (canEnableDebugMode && Input.GetKeyDown(KeyCode.B))
        {
            debug.DebugModeEnabled = true;
        }
        CheckAndSetDefaultGonerSettings();
    }

    private void OnDestroy()
    {
        MonoBehaviour.print("FOUND YOU FUCKER");
    }

    private void Start()
    {
        UI_LoadingIcon.ToggleLoadingIcon(showIcon: true);
    }

    private void CheckAndSetDefaultGonerSettings()
    {
        if (!PlayerPrefs.HasKey("Setting_MasterVolume"))
        {
            SettingsManager.Instance.SaveFloatSetting("MasterVolume", 1f);
            SettingsManager.Instance.SaveFloatSetting("DialogueVolume", 0.9f);
            SettingsManager.Instance.SaveFloatSetting("EffectVolume", 0.8f);
            SettingsManager.Instance.SaveFloatSetting("MusicVolume", 0.64f);
            SettingsManager.Instance.SaveFloatSetting("Bumpscosity", GetEvenRandomFloat(0.01f));
            PlayerInput.RevertToDefaults();
            SettingsManager.Instance.ApplySettings();
        }
    }

    public float GetEvenRandomFloat(float stepSize)
    {
        if (stepSize <= 0f || stepSize > 1f)
        {
            Debug.LogError("step size outside of bounds idiot");
            return 0f;
        }
        int num = Mathf.FloorToInt(1f / stepSize);
        return (float)Random.Range(0, num + 1) * stepSize;
    }

    private IEnumerator InitializeDelay()
    {
        yield return new WaitForSeconds(1f);
        SettingsManager.Instance.ApplySettings();
        canEnableDebugMode = false;
        SceneManager.LoadScene(48);
    }

    private IEnumerator InitializeSceneUnload()
    {
        yield return null;
        SettingsManager.Instance.ApplySettings();
        SceneManager.UnloadSceneAsync("Initializer", UnloadSceneOptions.None);
    }

    private IEnumerator InitializePunishment()
    {
        yield return new WaitForSeconds(1f);
        Object.Destroy(debug.gameObject);
        SceneManager.LoadScene(41);
    }
}
