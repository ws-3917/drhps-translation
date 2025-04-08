using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewMainMenu_QuitSubMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    public TextMeshProUGUI MainText;

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    public GameObject ComputerGreenLight;

    public GameObject ComputerOrangeLight;

    private int CurrentSelected = 1;

    private int previousSelected = -1;

    private Coroutine currentMainTextWriting;

    private bool wasEnabled;

    public GameObject ConsoleMenu;

    private bool AllTextShown;

    private bool AllowInput;

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
        if (!GonerMenu.Instance.GonerMenuOpen && AllowInput)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
            {
                CurrentSelected++;
                CheckCurrentSelectedOutsideBounds();
                CheckUpdateScreenText();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
            {
                CurrentSelected--;
                CheckCurrentSelectedOutsideBounds();
                CheckUpdateScreenText();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) || Input.GetKeyDown(PlayerInput.Instance.Key_Menu))
            {
                SelectOption();
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
            CurrentSelected = 1;
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

    private void SelectOption()
    {
        AllowInput = false;
        previousSelected = -1;
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
        StartCoroutine(SelectSubMenuTimer());
        CheckUpdateScreenText();
    }

    private IEnumerator SelectSubMenuTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (CurrentSelected == 0)
        {
            StartCoroutine(QuitSequence());
            yield break;
        }
        ConsoleMenu.SetActive(value: true);
        base.gameObject.SetActive(value: false);
    }

    public IEnumerator QuitSequence()
    {
        ChangeMainText("", AllowAnimation: false);
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.text = "";
        }
        NewMainMenuManager.instance.CameraAnimator.Play("MainMenu_Camera_MonitorQuitGame", -1, 0f);
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_ComputerBootOff);
        ComputerGreenLight.SetActive(value: false);
        ComputerOrangeLight.SetActive(value: true);
        yield return new WaitForSeconds(0.5f);
        UI_FADE.Instance.StartFadeIn(-1, 0.5f);
        ComputerOrangeLight.SetActive(value: false);
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }

    private void CheckCurrentSelectedOutsideBounds()
    {
        if (CurrentSelected < 0)
        {
            CurrentSelected = 1;
        }
        if (CurrentSelected > 1)
        {
            CurrentSelected = 0;
        }
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuMove);
    }

    private void CheckUpdateScreenText()
    {
        if (previousSelected != CurrentSelected)
        {
            previousSelected = CurrentSelected;
            string text = "";
            text = "<b>Are you sure you want to quit?</b>\n";
            string[] array = new string[2] { "Yes ", "No" };
            for (int i = 0; i < array.Length; i++)
            {
                text = ((CurrentSelected != i) ? (text + array[i]) : (AllowInput ? (text + "<color=yellow>-> " + array[i] + "</color>") : (text + "<color=green>-> " + array[i] + "</color>")));
            }
            ChangeMainText(text, AllowAnimation: true);
        }
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
        if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX") && AllowAnimation)
        {
            MainText.maxVisibleCharacters = 0;
            if (currentMainTextWriting != null)
            {
                StopCoroutine(currentMainTextWriting);
            }
            currentMainTextWriting = StartCoroutine(ScrollMainText());
        }
        else
        {
            MainText.maxVisibleCharacters = text.Length;
        }
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
