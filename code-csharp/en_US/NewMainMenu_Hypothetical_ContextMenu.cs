using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu_Hypothetical_ContextMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    [SerializeField]
    private NewMainMenu_HypotheticalSubMenu hypotheticalSubMenu;

    [SerializeField]
    private TextMeshProUGUI optionText;

    private Coroutine currentMainTextWriting;

    private int CurrentSelected;

    private int previousSelected = -1;

    public Hypothesis CurrentHypothesis;

    [SerializeField]
    private TextMeshProUGUI HypotheticalTitle;

    [SerializeField]
    private TextMeshProUGUI HypotheticalDescription;

    [SerializeField]
    private TextMeshProUGUI Warning;

    [SerializeField]
    private TextMeshProUGUI HypotheticalQuests;

    [SerializeField]
    private Image HypotheticalIcon;

    private bool AllTextShown;

    private bool wasEnabled;

    private bool AllowInput = true;

    private void Awake()
    {
        CurrentSelected = PlayerPrefs.GetInt("NewMainMenu_PreviousSelectedHypothetical", 0);
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
                hypotheticalSubMenu.AllowInput = true;
                base.gameObject.SetActive(value: false);
            }
        }
        if (!wasEnabled && base.gameObject.activeSelf)
        {
            previousSelected = -1;
            wasEnabled = true;
            AllowInput = true;
            StartCoroutine(ShowBackgroundText());
            CheckUpdateScreenText();
            SetupHypothetical();
        }
    }

    private void SelectOption()
    {
        PlayerPrefs.SetInt("NewMainMenu_PreviousSelectedHypothetical", CurrentSelected);
        AllowInput = false;
        previousSelected = -1;
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
        StartCoroutine(SelectOptionTimer());
        CheckUpdateScreenText();
    }

    private IEnumerator SelectOptionTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (CurrentSelected == 0)
        {
            GonerMenu.CurrentActiveHypothesis = CurrentHypothesis;
            UI_LoadingIcon.ToggleLoadingIcon(showIcon: true);
            UI_FADE.Instance.StartFadeIn(CurrentHypothesis.HypothesisStartingScene, 0.5f);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.CheekyGrin);
        }
        else
        {
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
            hypotheticalSubMenu.AllowInput = true;
            base.gameObject.SetActive(value: false);
        }
    }

    private void CheckUpdateScreenText()
    {
        if (previousSelected != CurrentSelected)
        {
            previousSelected = CurrentSelected;
            string text = "";
            string[] array = new string[2] { "Play ", "Return" };
            for (int i = 0; i < array.Length; i++)
            {
                text = ((CurrentSelected != i) ? (text + array[i]) : (AllowInput ? (text + "<color=yellow>-> " + array[i] + "</color>") : (text + "<color=green>-> " + array[i] + "</color>")));
            }
            ChangeMainText(text, AllowAnimation: true);
        }
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

    private void SetupHypothetical()
    {
        HypotheticalTitle.text = CurrentHypothesis.HypothesisName;
        HypotheticalDescription.text = "<color=grey>Hypothetically..\n\n</color>" + CurrentHypothesis.HypothesisDescription;
        if (CurrentHypothesis.HypothesisMenuSprite != null && CurrentHypothesis.HypotheticalReleased)
        {
            HypotheticalIcon.sprite = CurrentHypothesis.HypothesisMenuSprite;
        }
        else
        {
            HypotheticalIcon.sprite = hypotheticalSubMenu.noIconSprite;
        }
        Warning.text = CurrentHypothesis.TimeWhenWritten;
        string text = "";
        text = ((CurrentHypothesis.HypothesisGoals.Length == 0) ? (text + "<color=red>[ No Bonus Quests ]</color>") : (text + "<color=green>[ Bonus Quests ]</color>"));
        HypothesisGoal[] hypothesisGoals = CurrentHypothesis.HypothesisGoals;
        foreach (HypothesisGoal hypothesisGoal in hypothesisGoals)
        {
            int @int = PlayerPrefs.GetInt("HypothesisGoal_" + hypothesisGoal.GoalPlayprefName, 0);
            text += "\n";
            HypotheticalQuests.color = Color.white;
            if (hypothesisGoal.RequiredTasks > 1)
            {
                text = ((@int >= hypothesisGoal.RequiredTasks) ? (text + "<color=yellow>") : (text + "<color=white>"));
                text = text + "(" + @int + "/" + hypothesisGoal.RequiredTasks + ")";
            }
            else
            {
                text = ((@int == 0) ? (text + "<color=white>(-) ") : (text + "<color=yellow>(X)"));
            }
            text += hypothesisGoal.GoalHint;
        }
        HypotheticalQuests.text = text;
    }

    private void OnDisable()
    {
        wasEnabled = false;
        AllTextShown = false;
        ChangeMainText("", AllowAnimation: false);
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.maxVisibleCharacters = 0;
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
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuClick);
            yield return null;
        }
    }
}
