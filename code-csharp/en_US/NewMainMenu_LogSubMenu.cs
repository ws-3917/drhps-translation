using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu_LogSubMenu : MonoBehaviour
{
    public enum LogMenuType
    {
        ReturnToConsoleMenu = 0,
        Prefab = 1
    }

    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    public TextMeshProUGUI MainText;

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    private int CurrentSelected;

    private int previousSelected = -1;

    [SerializeField]
    private List<CollectibleLog> UnlockableLogs = new List<CollectibleLog>();

    private bool wasEnabled;

    public LogMenuType[] UnlockableLogMenuTypes;

    public GameObject ConsoleMenu;

    public NewMainMenu_LogContextMenu LogContextMenu;

    [SerializeField]
    private List<string> storedLogNames = new List<string>();

    private List<bool> storedUnlockedLogs = new List<bool>();

    [SerializeField]
    private TextMeshProUGUI LogName;

    [SerializeField]
    private TextMeshProUGUI LogTypeName;

    [SerializeField]
    private Image LogIcon;

    private bool AllTextShown;

    public bool AllowInput;

    private void Awake()
    {
        CachePrefabReturnedValues();
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
    }

    private void Update()
    {
        if (!GonerMenu.Instance.GonerMenuOpen && AllowInput)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
            {
                CurrentSelected++;
                CheckCurrentSelectedOutsideBounds();
                CheckUpdateScreenText();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Up))
            {
                CurrentSelected--;
                CheckCurrentSelectedOutsideBounds();
                CheckUpdateScreenText();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) || Input.GetKeyDown(PlayerInput.Instance.Key_Menu))
            {
                SelectLog();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
            {
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
                ConsoleMenu.SetActive(value: true);
                base.gameObject.SetActive(value: false);
            }
        }
        if (!wasEnabled && base.gameObject.activeSelf)
        {
            wasEnabled = true;
            AllowInput = true;
            StartCoroutine(ShowBackgroundText());
            CheckUpdateScreenText();
        }
    }

    private void OnDisable()
    {
        wasEnabled = false;
        previousSelected = -1;
        AllTextShown = false;
        ChangeMainText("", AllowAnimation: false);
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.maxVisibleCharacters = 0;
        }
    }

    private void CachePrefabReturnedValues()
    {
        storedLogNames.Clear();
        foreach (CollectibleLog unlockableLog in UnlockableLogs)
        {
            if (unlockableLog != null)
            {
                if (!unlockableLog.StartUnlocked)
                {
                    if (PlayerPrefs.GetInt(unlockableLog.LogPlayerPrefName, 0) == 0)
                    {
                        storedLogNames.Add("???");
                        storedUnlockedLogs.Add(item: false);
                    }
                    else
                    {
                        storedLogNames.Add(unlockableLog.LogName);
                        storedUnlockedLogs.Add(item: true);
                    }
                }
                else
                {
                    storedLogNames.Add(unlockableLog.LogName);
                    storedUnlockedLogs.Add(item: true);
                }
            }
            else
            {
                storedLogNames.Add("... Return");
                storedUnlockedLogs.Add(item: false);
            }
        }
    }

    private void SelectLog()
    {
        AllowInput = false;
        previousSelected = -1;
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
        StartCoroutine(SelectLogTimer());
        CheckUpdateScreenText();
    }

    private IEnumerator SelectLogTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (UnlockableLogs[CurrentSelected] != null)
        {
            PlayerPrefs.SetInt("readlog_" + UnlockableLogs[CurrentSelected].LogPlayerPrefName, 1);
        }
        switch (UnlockableLogMenuTypes[CurrentSelected])
        {
            case LogMenuType.ReturnToConsoleMenu:
                ConsoleMenu.SetActive(value: true);
                base.gameObject.SetActive(value: false);
                break;
            case LogMenuType.Prefab:
                if (UnlockableLogs[CurrentSelected].LogPrefab != null && storedUnlockedLogs[CurrentSelected])
                {
                    LogContextMenu.CurrentLog = UnlockableLogs[CurrentSelected];
                    LogContextMenu.gameObject.SetActive(value: true);
                    break;
                }
                AllowInput = true;
                previousSelected = -1;
                CheckUpdateScreenText();
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
                break;
        }
    }

    private void CheckCurrentSelectedOutsideBounds()
    {
        if (CurrentSelected < 0)
        {
            CurrentSelected = UnlockableLogs.Count - 1;
        }
        if (CurrentSelected > UnlockableLogs.Count - 1)
        {
            CurrentSelected = 0;
        }
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuMove);
    }

    private void CheckUpdateScreenText()
    {
        if (previousSelected == CurrentSelected)
        {
            return;
        }
        previousSelected = CurrentSelected;
        string text = "";
        for (int i = 0; i < storedLogNames.Count; i++)
        {
            if (CurrentSelected == i)
            {
                text = (AllowInput ? (text + "<color=yellow>-> " + storedLogNames[i] + "</color>\n") : (text + "<color=green>-> " + storedLogNames[i] + "</color>\n"));
                if (UnlockableLogMenuTypes[i] != 0 && storedUnlockedLogs[i])
                {
                    LogName.text = storedLogNames[i];
                    LogTypeName.text = UnlockableLogs[i].LogType.ToString();
                    LogIcon.sprite = UnlockableLogs[i].LogPreview;
                    LogIcon.color = new Color(1f, 1f, 1f, 1f);
                    LogIcon.rectTransform.sizeDelta = UnlockableLogs[i].LogPreviewScale;
                }
                else
                {
                    LogName.text = "";
                    LogTypeName.text = "";
                    LogIcon.sprite = null;
                    LogIcon.color = new Color(1f, 1f, 1f, 0f);
                }
            }
            else
            {
                text = ((!(UnlockableLogs[i] != null)) ? (text + storedLogNames[i] + "\n") : (UnlockableLogs[i].StartUnlocked ? ((PlayerPrefs.GetInt("readlog_" + UnlockableLogs[i].LogPlayerPrefName, 0) != 0) ? (text + storedLogNames[i] + "\n") : (text + "<color=yellow>" + storedLogNames[i] + "</color>\n")) : ((PlayerPrefs.GetInt(UnlockableLogs[i].LogPlayerPrefName, 0) != 1) ? (text + "<color=grey>" + storedLogNames[i] + "</color>\n") : ((PlayerPrefs.GetInt("readlog_" + UnlockableLogs[i].LogPlayerPrefName, 0) != 0) ? (text + storedLogNames[i] + "\n") : (text + "<color=yellow>" + storedLogNames[i] + "</color>\n")))));
            }
        }
        ChangeMainText(text, AllowAnimation: true);
        UpdateCreditUI();
    }

    private void UpdateCreditUI()
    {
        _ = UnlockableLogMenuTypes[CurrentSelected];
        _ = 1;
    }

    private IEnumerator ShowBackgroundText()
    {
        if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX"))
        {
            List<TextMeshProUGUI> finishedTexts = new List<TextMeshProUGUI>();
            while (!AllTextShown)
            {
                yield return null;
                foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
                {
                    if (!finishedTexts.Contains(backgroundTextLine) && backgroundTextLine.maxVisibleCharacters >= backgroundTextLine.text.Length)
                    {
                        finishedTexts.Add(backgroundTextLine);
                    }
                    else if (backgroundTextLine.maxVisibleCharacters < backgroundTextLine.text.Length)
                    {
                        float num = 100 + Random.Range(-60, -10);
                        backgroundTextLine.maxVisibleCharacters += Mathf.CeilToInt(num * Time.deltaTime);
                        backgroundTextLine.maxVisibleCharacters = Mathf.Clamp(backgroundTextLine.maxVisibleCharacters, 0, backgroundTextLine.text.Length);
                        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuClick);
                    }
                }
                if (CompareTextLists(finishedTexts, BackgroundTextLines))
                {
                    AllTextShown = true;
                    MonoBehaviour.print("all text shown");
                }
            }
            yield break;
        }
        foreach (TextMeshProUGUI backgroundTextLine2 in BackgroundTextLines)
        {
            backgroundTextLine2.maxVisibleCharacters = backgroundTextLine2.text.Length;
        }
    }

    private bool CompareTextLists(List<TextMeshProUGUI> list1, List<TextMeshProUGUI> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i].text != list2[i].text)
            {
                return false;
            }
        }
        return true;
    }

    public void ChangeMainText(string text, bool AllowAnimation)
    {
        MainText.text = text;
        MainText.maxVisibleCharacters = text.Length;
    }

    private IEnumerator ScrollMainText()
    {
        yield return null;
        while (MainText.maxVisibleCharacters < MainText.text.Length)
        {
            float num = 600 + Random.Range(-120, 0);
            MainText.maxVisibleCharacters += Mathf.CeilToInt(num * Time.deltaTime);
            MainText.maxVisibleCharacters = Mathf.Clamp(MainText.maxVisibleCharacters, 0, MainText.text.Length);
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuClick);
            yield return null;
        }
    }
}
