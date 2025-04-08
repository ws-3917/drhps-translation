using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HypotheticalEndScreen : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    [Space(5f)]
    [SerializeField]
    private Animator HES_ScreenAnimator;

    [SerializeField]
    private Image LogPreviewRenderer;

    [SerializeField]
    private TextMeshProUGUI LogTitleText;

    [Space(5f)]
    [SerializeField]
    private AudioClip CompleteSong_part1;

    [SerializeField]
    private AudioClip CompleteSong_part2;

    private bool AllTextShown;

    private void Awake()
    {
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        CompleteSong_part1.LoadAudioData();
        CompleteSong_part2.LoadAudioData();
    }

    private void Start()
    {
        StartCoroutine(ShowBackgroundText());
        StartCoroutine(ScreenTime());
        SecurePlayerPrefs.SetSecureInt("TotalCash", SecurePlayerPrefs.GetSecureInt("TotalCash") + 120);
    }

    private void Update()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }

    private IEnumerator ScreenTime()
    {
        yield return null;
        MusicManager.PlaySong(CompleteSong_part1, FadePreviousSong: false, 0f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.CheekyGrin);
        yield return new WaitForSeconds(3f);
        if (GonerMenu.CurrentActiveHypothesis != null && GonerMenu.CurrentActiveHypothesis.LogRewardedOnCompletion != null && PlayerPrefs.GetInt(GonerMenu.CurrentActiveHypothesis.LogRewardedOnCompletion.LogPlayerPrefName, 0) == 0)
        {
            MusicManager.PlaySong(CompleteSong_part2, FadePreviousSong: false, 0f);
            PlayerPrefs.SetInt(GonerMenu.CurrentActiveHypothesis.LogRewardedOnCompletion.LogPlayerPrefName, 1);
            HES_ScreenAnimator.Play("HES_NewLog");
            LogPreviewRenderer.sprite = GonerMenu.CurrentActiveHypothesis.LogRewardedOnCompletion.LogPreview;
            LogTitleText.text = "新日誌已解鎖！\n" + GonerMenu.CurrentActiveHypothesis.LogRewardedOnCompletion.LogName;
            LogPreviewRenderer.rectTransform.sizeDelta = GonerMenu.CurrentActiveHypothesis.LogRewardedOnCompletion.LogPreviewScale;
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
            yield return new WaitForSeconds(3.5f);
        }
        PlayerPrefs.SetInt("TimesPlayed_" + GonerMenu.CurrentActiveHypothesis.HypothesisName, PlayerPrefs.GetInt("TimesPlayed_" + GonerMenu.CurrentActiveHypothesis.HypothesisName, 0) + 1);
        GonerMenu.Instance.ResetPlayer();
        UI_LoadingIcon.ToggleLoadingIcon(showIcon: true);
        UI_FADE.Instance.StartFadeIn(2, 0.25f, UnpauseOnEnd: true, NewMainMenuManager.MainMenuStates.Hypothetical);
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
}
