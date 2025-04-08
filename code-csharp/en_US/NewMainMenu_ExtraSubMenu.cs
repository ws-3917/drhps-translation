using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewMainMenu_ExtraSubMenu : MonoBehaviour
{
    public enum ExtraMenuTypes
    {
        ReturnToConsoleMenu = 0,
        EnableGameobject = 1
    }

    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    public TextMeshProUGUI MainText;

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    private int CurrentSelected;

    private int previousSelected = -1;

    private bool wasEnabled;

    public ExtraMenuContent[] ExtraContent;

    public GameObject ConsoleMenu;

    public NewMainMenu_ExtraContext ExtraContextMenu;

    private bool AllTextShown;

    public bool AllowInput;

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
                SelectExtra();
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

    private void SelectExtra()
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
        switch (ExtraContent[CurrentSelected].ExtraType)
        {
            case ExtraMenuTypes.ReturnToConsoleMenu:
                ConsoleMenu.SetActive(value: true);
                base.gameObject.SetActive(value: false);
                break;
            case ExtraMenuTypes.EnableGameobject:
                ExtraContextMenu.CurrentExtraContent = ExtraContent[CurrentSelected];
                ExtraContextMenu.gameObject.SetActive(value: true);
                break;
        }
    }

    private void CheckCurrentSelectedOutsideBounds()
    {
        if (CurrentSelected < 0)
        {
            CurrentSelected = ExtraContent.Length - 1;
        }
        if (CurrentSelected > ExtraContent.Length - 1)
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
            for (int i = 0; i < ExtraContent.Length; i++)
            {
                text = ((CurrentSelected != i) ? (text + ExtraContent[i].ExtraName + "\n") : (AllowInput ? (text + "<color=yellow>-> " + ExtraContent[i].ExtraName + "</color>\n") : (text + "<color=green>-> " + ExtraContent[i].ExtraName + "</color>\n")));
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
