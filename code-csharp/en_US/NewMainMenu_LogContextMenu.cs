using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewMainMenu_LogContextMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    [SerializeField]
    private NewMainMenu_LogSubMenu logSubMenu;

    [SerializeField]
    private TextMeshProUGUI optionText;

    private Coroutine currentMainTextWriting;

    [SerializeField]
    private GameObject AnnoyingAssLines;

    private GameObject prefabStored;

    [SerializeField]
    private GameObject prefabHolder;

    [SerializeField]
    private AudioSource[] BackgroundNoiseToDisable;

    private int CurrentSelected;

    private int previousSelected = -1;

    private bool simplesfx_on;

    public CollectibleLog CurrentLog;

    private bool AllTextShown;

    private bool wasEnabled;

    private bool AllowInput = true;

    private void Awake()
    {
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
    }

    private void Update()
    {
        if (AllowInput)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) || Input.GetKeyDown(PlayerInput.Instance.Key_Menu))
            {
                SelectOption();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
            {
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
                logSubMenu.AllowInput = true;
                base.gameObject.SetActive(value: false);
            }
        }
        if (!wasEnabled && base.gameObject.activeSelf)
        {
            previousSelected = -1;
            CurrentSelected = 0;
            wasEnabled = true;
            AllowInput = true;
            StartCoroutine(ShowBackgroundText());
            CheckUpdateScreenText();
            SetupExtraContent();
        }
        if (prefabStored != null)
        {
            prefabStored.transform.localPosition = Vector3.Lerp(prefabStored.transform.localPosition, Vector3.zero, 5f * Time.deltaTime);
        }
    }

    private void SelectOption()
    {
        AllowInput = false;
        previousSelected = -1;
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
        StartCoroutine(SelectOptionTimer());
        CheckUpdateScreenText();
    }

    private IEnumerator SelectOptionTimer()
    {
        yield return new WaitForSeconds(0.5f);
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
        AudioSource[] backgroundNoiseToDisable = BackgroundNoiseToDisable;
        for (int i = 0; i < backgroundNoiseToDisable.Length; i++)
        {
            backgroundNoiseToDisable[i].UnPause();
        }
        logSubMenu.AllowInput = true;
        base.gameObject.SetActive(value: false);
    }

    private void CheckUpdateScreenText()
    {
        if (previousSelected != CurrentSelected)
        {
            previousSelected = CurrentSelected;
            string text = "";
            string[] array = new string[1] { "Return" };
            for (int i = 0; i < array.Length; i++)
            {
                text = ((CurrentSelected != i) ? (text + array[i]) : (AllowInput ? (text + "<color=yellow>-> " + array[i] + "</color>") : (text + "<color=green>-> " + array[i] + "</color>")));
            }
            ChangeMainText(text, AllowAnimation: true);
        }
    }

    private void SetupExtraContent()
    {
        AnnoyingAssLines.SetActive(value: false);
        if (!(CurrentLog.LogPrefab != null))
        {
            return;
        }
        simplesfx_on = SettingsManager.Instance.GetBoolSettingValue("SimpleSFX");
        if (CurrentLog.LogPrefab != null)
        {
            prefabStored = Object.Instantiate(CurrentLog.LogPrefab, prefabHolder.transform.position, Quaternion.identity);
            prefabStored.transform.SetParent(prefabHolder.transform, worldPositionStays: false);
            prefabStored.transform.localPosition = new Vector3(0f, -999f, 0f);
            NewMainMenuManager.instance.MenuSource.PlayOneShot(CurrentLog.LogOpenSound);
        }
        if (CurrentLog.LogType == CollectibleLog.CollectibleLogType.Audio)
        {
            AudioSource[] backgroundNoiseToDisable = BackgroundNoiseToDisable;
            for (int i = 0; i < backgroundNoiseToDisable.Length; i++)
            {
                backgroundNoiseToDisable[i].Pause();
            }
        }
        previousSelected = -1;
        CheckUpdateScreenText();
    }

    private void OnDisable()
    {
        wasEnabled = false;
        AllTextShown = false;
        ChangeMainText("", AllowAnimation: false);
        AnnoyingAssLines.SetActive(value: true);
        CameraManager.instance.ReverbFilter.enabled = true;
        if (prefabStored != null)
        {
            Object.Destroy(prefabStored);
        }
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        currentMainTextWriting = null;
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
        optionText.text = text;
        if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX") && AllowAnimation)
        {
            optionText.maxVisibleCharacters = 0;
            if (currentMainTextWriting != null)
            {
                StopCoroutine(currentMainTextWriting);
            }
            currentMainTextWriting = StartCoroutine(ScrollMainText());
        }
        else
        {
            optionText.maxVisibleCharacters = text.Length;
        }
    }

    private IEnumerator ScrollMainText()
    {
        yield return null;
        while (optionText.maxVisibleCharacters < optionText.text.Length)
        {
            float num = 600 + Random.Range(-120, 0);
            optionText.maxVisibleCharacters += Mathf.CeilToInt(num * Time.deltaTime);
            optionText.maxVisibleCharacters = Mathf.Clamp(optionText.maxVisibleCharacters, 0, optionText.text.Length);
            yield return null;
        }
    }
}
