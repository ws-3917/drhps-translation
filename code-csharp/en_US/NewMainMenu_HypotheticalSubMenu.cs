using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu_HypotheticalSubMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    public TextMeshProUGUI MainText;

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    private int CurrentSelected;

    private int previousSelected = -1;

    [SerializeField]
    private List<Hypothesis> PlayableHypotheticals = new List<Hypothesis>();

    private bool wasEnabled;

    [SerializeField]
    private GameObject ConsoleMenu;

    [SerializeField]
    private NewMainMenu_Hypothetical_ContextMenu HypotheticalContextMenu;

    public Sprite noIconSprite;

    [SerializeField]
    private List<string> storedHypotheticalNames = new List<string>();

    [SerializeField]
    private TextMeshProUGUI HypotheticalName;

    [SerializeField]
    private TextMeshProUGUI HypotheticalTagline;

    [SerializeField]
    private Image HypotheticalIcon;

    private bool AllTextShown;

    public bool AllowInput;

    private void Awake()
    {
        PlayableHypotheticals.Insert(0, null);
        CachePrefabReturnedValues();
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        UpdateUnlockedGamblingState();
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
                SelectHypothetical();
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
            previousSelected = -1;
            StartCoroutine(ShowBackgroundText());
            CheckUpdateScreenText();
            UpdateUnlockedGamblingState();
        }
    }

    private void UpdateUnlockedGamblingState()
    {
        if (PlayerPrefs.GetInt("UnlockedGambling", 0) != 0)
        {
            return;
        }
        bool flag = true;
        foreach (Hypothesis playableHypothetical in PlayableHypotheticals)
        {
            if (playableHypothetical != null && playableHypothetical.HypotheticalReleased)
            {
                if (PlayerPrefs.GetInt("TimesPlayed_" + playableHypothetical.HypothesisName, 0) <= 0)
                {
                    flag = false;
                    break;
                }
                HypothesisGoal[] hypothesisGoals = playableHypothetical.HypothesisGoals;
                foreach (HypothesisGoal hypothesisGoal in hypothesisGoals)
                {
                    if (PlayerPrefs.GetInt("HypothesisGoal_" + hypothesisGoal.GoalPlayprefName, 0) < hypothesisGoal.RequiredTasks)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (!flag)
            {
                break;
            }
        }
        if (flag)
        {
            PlayerPrefs.SetInt("UnlockedGambling", 1);
            GonerMenu.Instance.ShowTip("Something has unlocked in the main menu...");
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
        storedHypotheticalNames.Clear();
        foreach (Hypothesis playableHypothetical in PlayableHypotheticals)
        {
            if (playableHypothetical != null)
            {
                storedHypotheticalNames.Add(playableHypothetical.HypothesisName);
            }
            else
            {
                storedHypotheticalNames.Add("... Return");
            }
        }
    }

    private void SelectHypothetical()
    {
        if (PlayableHypotheticals[CurrentSelected] != null && !PlayableHypotheticals[CurrentSelected].HypotheticalReleased)
        {
            if (PlayableHypotheticals[CurrentSelected].HypotheticalReleased)
            {
                AllowInput = false;
                previousSelected = -1;
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
                StartCoroutine(SelectHypotheticalTimer());
                CheckUpdateScreenText();
            }
            else
            {
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
            }
        }
        else
        {
            AllowInput = false;
            previousSelected = -1;
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
            StartCoroutine(SelectHypotheticalTimer());
            CheckUpdateScreenText();
        }
    }

    private IEnumerator SelectHypotheticalTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (PlayableHypotheticals[CurrentSelected] != null)
        {
            HypotheticalContextMenu.CurrentHypothesis = PlayableHypotheticals[CurrentSelected];
            HypotheticalContextMenu.gameObject.SetActive(value: true);
        }
        else
        {
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
            ConsoleMenu.SetActive(value: true);
            base.gameObject.SetActive(value: false);
        }
    }

    private void CheckCurrentSelectedOutsideBounds()
    {
        if (CurrentSelected < 0)
        {
            CurrentSelected = PlayableHypotheticals.Count - 1;
        }
        if (CurrentSelected > PlayableHypotheticals.Count - 1)
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
        for (int i = 0; i < storedHypotheticalNames.Count; i++)
        {
            if (PlayableHypotheticals[i] == null)
            {
                if (CurrentSelected == i)
                {
                    text += (AllowInput ? "<color=yellow>-> ... Return</color>\n" : "<color=green>-> ... Return</color>\n");
                    HypotheticalName.text = "";
                    HypotheticalTagline.text = "";
                    HypotheticalIcon.sprite = noIconSprite;
                    HypotheticalIcon.color = new Color(1f, 1f, 1f, 0f);
                }
                else
                {
                    text += "... Return\n";
                }
            }
            else
            {
                if (!PlayableHypotheticals[i].HypotheticalReleased)
                {
                    continue;
                }
                bool flag = IsAllQuestsCompleted(i);
                string text2 = (flag ? "yellow" : "white");
                if (PlayableHypotheticals[i].HypothesisGoals.Length == 0 && PlayerPrefs.GetInt("TimesPlayed_" + PlayableHypotheticals[i].HypothesisName, 0) > 0)
                {
                    text2 = "yellow";
                }
                if (CurrentSelected == i)
                {
                    string text3 = (flag ? "#ffc478" : "#fffc78");
                    if (PlayableHypotheticals[i].HypothesisGoals.Length == 0 && PlayerPrefs.GetInt("TimesPlayed_" + PlayableHypotheticals[i].HypothesisName, 0) > 0)
                    {
                        text3 = "#ffc478";
                    }
                    text += (AllowInput ? ("<color=" + text3 + ">-> " + storedHypotheticalNames[i] + "</color>\n") : ("<color=green>-> " + storedHypotheticalNames[i] + "</color>\n"));
                    HypotheticalName.text = storedHypotheticalNames[i];
                    HypotheticalTagline.text = PlayableHypotheticals[i].HypothesisTagline;
                    HypotheticalIcon.color = new Color(1f, 1f, 1f, 1f);
                    HypotheticalIcon.sprite = PlayableHypotheticals[i].HypothesisMenuSprite ?? noIconSprite;
                }
                else
                {
                    text = text + "<color=" + text2 + ">" + storedHypotheticalNames[i] + "</color>\n";
                }
            }
        }
        text += "\n<size=42><color=red>> Future Hypotheticals <</size><color=grey>\n\n";
        for (int j = 0; j < storedHypotheticalNames.Count; j++)
        {
            if (PlayableHypotheticals[j] != null && !PlayableHypotheticals[j].HypotheticalReleased)
            {
                if (CurrentSelected == j)
                {
                    text += (AllowInput ? ("<color=yellow>-> " + storedHypotheticalNames[j] + "</color>\n") : ("<color=green>-> " + storedHypotheticalNames[j] + "</color>\n"));
                    HypotheticalName.text = storedHypotheticalNames[j];
                    HypotheticalTagline.text = PlayableHypotheticals[j].HypothesisTagline;
                    HypotheticalIcon.sprite = noIconSprite;
                    HypotheticalIcon.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    text = text + "<color=grey>" + storedHypotheticalNames[j] + "</color>\n";
                }
            }
        }
        text += "\n<color=green>More to be revealed!</color>";
        ChangeMainText(text, AllowAnimation: true);
        bool IsAllQuestsCompleted(int hypotheticalIndex)
        {
            HypothesisGoal[] hypothesisGoals = PlayableHypotheticals[hypotheticalIndex].HypothesisGoals;
            if (hypothesisGoals == null || hypothesisGoals.Length == 0)
            {
                return false;
            }
            HypothesisGoal[] array = hypothesisGoals;
            foreach (HypothesisGoal hypothesisGoal in array)
            {
                if (PlayerPrefs.GetInt("HypothesisGoal_" + hypothesisGoal.GoalPlayprefName, 0) < hypothesisGoal.RequiredTasks)
                {
                    return false;
                }
            }
            return true;
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
