using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu_ConsoleMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    public TextMeshProUGUI MainText;

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    private int CurrentSelected;

    private int previousSelected = -1;

    private float DELHoldTime;

    [SerializeField]
    private Image DELStaticImage;

    [SerializeField]
    private AudioSource DELStaticSource;

    [SerializeField]
    private NewMainMenu_QuitSubMenu DELQuitMenu;

    [SerializeField]
    private TextMeshProUGUI VersionText;

    private bool UnlockedGambling;

    public Animator CameraAnimator;

    private bool wasEnabled;

    public GameObject[] SubMenuGameobjects;

    public GameObject[] SubMenuGameobjects_Gambling;

    private bool AllTextShown;

    private bool AllowInput;

    private void Awake()
    {
        VersionText.text = "汉化构建V8\n已汉化78%\n\n反馈群797416533";
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        if (PlayerPrefs.GetInt("TipShown_MainMenuControls", 0) == 0)
        {
            PlayerPrefs.SetInt("TipShown_MainMenuControls", 1);
            GonerMenu.Instance.ShowTip($"按<color=yellow>{PlayerInput.Instance.Key_Confirm}</color>确定, 按<color=yellow>{PlayerInput.Instance.Key_Cancel}</color>返回, 使用<color=yellow>方向键</color>选择条目。");
        }
        if (PlayerPrefs.GetInt("UnlockedGambling", 0) == 1)
        {
            UnlockedGambling = true;
        }
        DELHoldTime = 0f;
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
                SelectSubMenu();
            }
        }
        if (!wasEnabled && base.gameObject.activeSelf)
        {
            wasEnabled = true;
            AllowInput = true;
            if (CameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("MainMenu_Camera_MonitorZoomIn"))
            {
                CameraAnimator.Play("MainMenu_Camera_MonitorZoomOut");
            }
            if (PlayerPrefs.GetInt("UnlockedGambling", 0) == 1)
            {
                UnlockedGambling = true;
            }
            DELHoldTime = 0f;
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Default);
            StartCoroutine(ShowBackgroundText());
            CheckUpdateScreenText();
        }
        if (DELHoldTime < 5f && AllowInput)
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                DELHoldTime += Time.deltaTime;
            }
            else
            {
                DELHoldTime = Mathf.Lerp(DELHoldTime, 0f, 2f * Time.deltaTime);
            }
        }
        DELStaticImage.color = new Color(1f, 1f, 1f, DELHoldTime / 5f);
        DELStaticSource.volume = DELHoldTime / 5f;
        if (DELHoldTime >= 5f && AllowInput)
        {
            AllowInput = false;
            StopCoroutine("SelectSubMenuTimer");
            int @int = PlayerPrefs.GetInt("TimesPlayed", 0);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("TimesPLayed", @int);
            DELQuitMenu.gameObject.SetActive(value: true);
            DELQuitMenu.StartCoroutine(DELQuitMenu.QuitSequence());
        }
    }

    private void OnDisable()
    {
        wasEnabled = false;
        previousSelected = -1;
        AllTextShown = false;
        ChangeMainText("", AllowAnimation: false);
        DELHoldTime = 0f;
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.maxVisibleCharacters = 0;
        }
    }

    private void SelectSubMenu()
    {
        AllowInput = false;
        previousSelected = -1;
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
        StartCoroutine(SelectSubMenuTimer());
        CheckUpdateScreenText();
    }

    private IEnumerator SelectSubMenuTimer()
    {
        CameraAnimator.Play("MainMenu_Camera_MonitorZoomIn");
        yield return new WaitForSeconds(0.5f);
        if (UnlockedGambling)
        {
            SubMenuGameobjects_Gambling[CurrentSelected].SetActive(value: true);
        }
        else
        {
            SubMenuGameobjects[CurrentSelected].SetActive(value: true);
        }
        base.gameObject.SetActive(value: false);
    }

    private void CheckCurrentSelectedOutsideBounds()
    {
        if (UnlockedGambling)
        {
            if (CurrentSelected < 0)
            {
                CurrentSelected = 7;
            }
            if (CurrentSelected > 7)
            {
                CurrentSelected = 0;
            }
        }
        else
        {
            if (CurrentSelected < 0)
            {
                CurrentSelected = 6;
            }
            if (CurrentSelected > 6)
            {
                CurrentSelected = 0;
            }
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
        string text = "<b>>请选择一个条目</b>\r\n\n";
        string[] array = new string[7] { "模拟猜想", "查看日志", "游戏设置", "制作组", "额外内容", "Discord", "退出游戏" };
        string[] array2 = new string[8] { "模拟猜想", "查看日志", "进入赌博", "游戏设置", "制作组", "额外内容", "Discord", "退出游戏" };
        if (UnlockedGambling)
        {
            for (int i = 0; i < array2.Length; i++)
            {
                text = ((CurrentSelected != i) ? (text + array2[i] + "\n") : (AllowInput ? (text + "<color=yellow>-> " + array2[i] + "</color>\n") : (text + "<color=green>-> " + array2[i] + "</color>\n")));
            }
        }
        else
        {
            for (int j = 0; j < array.Length; j++)
            {
                text = ((CurrentSelected != j) ? (text + array[j] + "\n") : (AllowInput ? (text + "<color=yellow>-> " + array[j] + "</color>\n") : (text + "<color=green>-> " + array[j] + "</color>\n")));
            }
        }
        ChangeMainText(text, AllowAnimation: true);
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
