using System;
using System.IO;
using TMPro;
using UnityEngine;

public class DRHDebugManager : MonoBehaviour
{
    public bool DebugModeEnabled;

    [SerializeField]
    private GameObject DebugCanvas;

    [SerializeField]
    private TextMeshProUGUI DebugConsoleOutput;

    [SerializeField]
    private GameObject SpeedUpIcon;

    private string output;

    [TextArea(10, 10)]
    public string controls;

    [SerializeField]
    private TextMeshProUGUI DebugFPSCounter;

    [SerializeField]
    private AudioSource debugSource;

    public static DRHDebugManager instance;

    private bool hasToggledMenu;

    [SerializeField]
    private AudioClip sfx_fiddlesticks;

    private string lastStackTrace;

    private void Awake()
    {
        instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Update()
    {
        if (DebugModeEnabled)
        {
            DebugCanvas.SetActive(value: true);
            DebugFPSCounter.text = Mathf.Round(1f / Time.unscaledDeltaTime).ToString();
        }
        else
        {
            DebugCanvas.SetActive(value: false);
        }
        if (!Input.anyKey)
        {
            hasToggledMenu = false;
        }
        if (Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Return) && !hasToggledMenu)
        {
            hasToggledMenu = true;
            DebugModeEnabled = !DebugModeEnabled;
            if (DebugModeEnabled)
            {
                output = "";
                DebugConsoleOutput.text = "";
                Debug.Log("<color=green>DEBUG FREAKING MODE ENABLED!! WOAHH!!</color>");
            }
        }
        if (DebugModeEnabled && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.P) && !hasToggledMenu)
        {
            hasToggledMenu = true;
            DebugConsoleOutput.enabled = !DebugConsoleOutput.enabled;
            if (DebugModeEnabled)
            {
                Debug.Log("<color=green>Toggled output visibility</color>");
            }
        }
        if (DebugModeEnabled && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.G) && !hasToggledMenu)
        {
            hasToggledMenu = true;
            if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
                SpeedUpIcon.SetActive(value: false);
            }
            else
            {
                Time.timeScale = 100f;
                SpeedUpIcon.SetActive(value: true);
            }
        }
        if (DebugModeEnabled && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.H) && !hasToggledMenu)
        {
            hasToggledMenu = true;
            if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
                SpeedUpIcon.SetActive(value: false);
            }
            else
            {
                Time.timeScale = 0.1f;
                SpeedUpIcon.SetActive(value: true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F8) && !string.IsNullOrEmpty(lastStackTrace))
        {
            SaveStackTraceToFile();
        }
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
                PlayErrorSound();
                hasToggledMenu = true;
                DebugModeEnabled = true;
                output = output + "\n<color=red>" + logString + "</color>\n<color=purple>Oh fiddlesticks, what now?</color>\n<color=#00ffff>Press F8 to print stackTrace to .txt</color>";
                lastStackTrace = "Main Issue\n" + logString + "\nStack Trace\n" + stackTrace;
                break;
            case LogType.Warning:
                output = output + "\n<color=yellow>" + logString + "</color>\n";
                break;
            case LogType.Log:
                output = output + "\n<color=white>" + logString + "</color>";
                break;
        }
        if (DebugModeEnabled)
        {
            DebugConsoleOutput.text = output + controls;
        }
        else
        {
            DebugConsoleOutput.text = controls;
        }
    }

    private void PlayErrorSound()
    {
        if (!debugSource.isPlaying)
        {
            debugSource.PlayOneShot(sfx_fiddlesticks);
        }
    }

    private void SaveStackTraceToFile()
    {
        string text = DateTime.Now.ToString("hhmm");
        string text2 = Path.Combine(Application.persistentDataPath, text + "stacktrace.txt");
        File.WriteAllText(text2, lastStackTrace);
        Application.OpenURL("file://" + text2);
        Debug.Log("<color=green>Stack trace saved to " + text2 + "</color>");
    }
}
