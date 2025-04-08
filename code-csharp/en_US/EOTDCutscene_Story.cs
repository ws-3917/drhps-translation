using System.Collections;
using UnityEngine;

public class EOTDCutscene_Story : MonoBehaviour
{
    [SerializeField]
    private Animator PanelAnimator;

    [SerializeField]
    private Animator BackgroundAnimator;

    [SerializeField]
    private ChatboxSimpleText StoryText;

    [SerializeField]
    private CHATBOXTEXT StoryChatboxText;

    [SerializeField]
    private EOTD_ArgueCutscene ArgueCutscene;

    [SerializeField]
    private AudioClip fountainsong;

    [SerializeField]
    private AudioClip storysong_1;

    [SerializeField]
    private AudioClip storysong_2;

    private bool StoryPlaying;

    private int CutsceneSkipAmount;

    [SerializeField]
    private int CurrentStoryIndex;

    [SerializeField]
    private LegendStoryPanel[] StoryPanels;

    public void StartStory()
    {
        BackgroundAnimator.Play("EOTDSTORY_FadeIn");
        GonerMenu.Instance.CanOpenGonerMenu = false;
        StoryPlaying = true;
        StartCoroutine(WaitForFade());
    }

    public void EndStory()
    {
        BackgroundAnimator.Play("EOTDSTORY_FadeOut");
        PanelAnimator.Play("EOTDSTORY_LegendHidden");
        StoryText.EndText();
        MusicManager.StopSong(Fade: false, 0f);
        GonerMenu.Instance.CanOpenGonerMenu = true;
        MusicManager.Instance.source.loop = true;
        MusicManager.PlaySong(fountainsong, FadePreviousSong: true, 0.5f);
        StoryPlaying = false;
        StartCoroutine(FadeOutDelay());
    }

    private void Update()
    {
        if (StoryPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                CutsceneSkipAmount++;
            }
            if (CutsceneSkipAmount >= 3)
            {
                CutsceneSkipAmount = 0;
                StopCoroutine("StoryLoop");
                EndStory();
            }
        }
    }

    private IEnumerator FadeOutDelay()
    {
        yield return new WaitForSeconds(1.5f);
        ArgueCutscene.IncrementCutsceneIndex();
    }

    private IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(1.5f);
        MusicManager.PlaySong(storysong_1, FadePreviousSong: false, 0f);
        MusicManager.Instance.source.loop = false;
        StartCoroutine(StoryLoop());
    }

    private IEnumerator StoryLoop()
    {
        if (!StoryPlaying)
        {
            yield break;
        }
        if (CurrentStoryIndex < StoryPanels.Length)
        {
            LegendStoryPanel legendStoryPanel = StoryPanels[CurrentStoryIndex];
            if (legendStoryPanel.StoryPanelName != null)
            {
                PanelAnimator.Play(legendStoryPanel.StoryPanelName);
            }
            if (legendStoryPanel.StoryPanelTextIndex >= 0)
            {
                StoryText.RunText(StoryChatboxText, legendStoryPanel.StoryPanelTextIndex);
            }
            if (legendStoryPanel.StartSecondaryMusic)
            {
                MusicManager.PlaySong(storysong_2, FadePreviousSong: false, 0f);
            }
            yield return new WaitForSeconds(legendStoryPanel.PanelTimeLength);
            CurrentStoryIndex++;
            StartCoroutine(StoryLoop());
        }
        else
        {
            EndStory();
        }
    }
}
