using System.Collections;
using UnityEngine;

public class EOTDCutscene_EndingStory : MonoBehaviour
{
    public EOTDCutscene_EndingStory_Panel[] StoryPanels;

    [Header("References")]
    [SerializeField]
    private SpriteRenderer PanelRenderer;

    [SerializeField]
    private AudioSource Song;

    [SerializeField]
    private Animator ThreeCharacterPanel;

    [SerializeField]
    private Animator FadeAnimator;

    [SerializeField]
    private TRIG_LEVELTRANSITION levelTransition;

    private void Start()
    {
        StartCoroutine(StoryPlay());
        StartCoroutine(SongLength());
    }

    private void Update()
    {
        DarkworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.CanOpenMenu = false;
    }

    private IEnumerator SongLength()
    {
        yield return new WaitForSeconds(Song.clip.length + 1f);
        levelTransition.BeginTransition();
    }

    private IEnumerator StoryPlay()
    {
        yield return new WaitForSeconds(2f);
        MusicManager.StopSong(Fade: false, 0f);
        Song.Play();
        EOTDCutscene_EndingStory_Panel[] storyPanels = StoryPanels;
        foreach (EOTDCutscene_EndingStory_Panel panel in storyPanels)
        {
            FadeAnimator.GetComponent<SpriteRenderer>().color = new Color(panel.FadeColor.r, panel.FadeColor.g, panel.FadeColor.b, FadeAnimator.GetComponent<SpriteRenderer>().color.a);
            if (panel.FadeIn)
            {
                FadeAnimator.Play("EOTD_EndingStory_FadeIn", -1, 0f);
                FadeAnimator.speed = panel.FadeSpeedMultiplier;
            }
            else
            {
                FadeAnimator.Play("EOTD_EndingStory_Hidden", -1, 0f);
            }
            if (panel.IsThreeCharacterPanel)
            {
                ThreeCharacterPanel.Play("EOTD_EndingStory_Section2");
            }
            PanelRenderer.sprite = panel.PanelSprite;
            if (panel.FadeOut)
            {
                yield return new WaitForSeconds(ConvertCapCutTimeToUnityTime(panel.PanelLength) - 0.2f / panel.FadeSpeedMultiplier);
                FadeAnimator.Play("EOTD_EndingStory_FadeOut", -1, 0f);
                FadeAnimator.speed = panel.FadeSpeedMultiplier;
                yield return new WaitForSeconds(0.2f / panel.FadeSpeedMultiplier);
            }
            else
            {
                yield return new WaitForSeconds(ConvertCapCutTimeToUnityTime(panel.PanelLength));
            }
        }
    }

    public float ConvertCapCutTimeToUnityTime(float capCutTime)
    {
        int num = Mathf.FloorToInt(capCutTime);
        float num2 = (capCutTime - (float)num) * 100f;
        return ((float)(num * 60) + num2) / 60f;
    }
}
