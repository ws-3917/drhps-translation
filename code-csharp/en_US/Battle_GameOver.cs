using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle_GameOver : MonoBehaviour
{
    public static Battle_GameOver Instance;

    [Header("-- References --")]
    [SerializeField]
    private GameObject ObjectHolder;

    [SerializeField]
    private SpriteRenderer Soul;

    [Space(5f)]
    [SerializeField]
    private Sprite Soul_Regular;

    [SerializeField]
    private Sprite Soul_Break;

    [SerializeField]
    private ParticleSystem Soul_ShatterParticles;

    [SerializeField]
    private SpriteRenderer GameOverTitle;

    [Space(5f)]
    [SerializeField]
    private ChatboxSimpleText GameOverText;

    public CHATBOXTEXT[] PossibleDeathMessages;

    private int currentIndex;

    private CHATBOXTEXT targetText;

    [Header("-- Choices --")]
    [SerializeField]
    private TextMeshPro ContinueText;

    [Header("-- Choices --")]
    [SerializeField]
    private TextMeshPro GiveUpText;

    [SerializeField]
    private SpriteRenderer GhostSoul;

    private int CurrentSelectedChoice = -1;

    [Header("-- Sounds --")]
    [SerializeField]
    private AudioClip SoulSFX_Break;

    [SerializeField]
    private AudioClip SoulSFX_Shatter;

    [SerializeField]
    private AudioClip Music_FaintCourage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Music_FaintCourage.LoadAudioData();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void ResetGameOver()
    {
        GameOverTitle.color = Color.clear;
        ObjectHolder.SetActive(value: false);
        Soul.sprite = Soul_Regular;
        Soul.enabled = true;
        Soul_ShatterParticles.Stop();
        CurrentSelectedChoice = -1;
        ContinueText.color = Color.clear;
        GiveUpText.color = Color.clear;
        GhostSoul.transform.localPosition = new Vector2(0f, -3.65f);
        GhostSoul.color = Color.clear;
    }

    public void PlayGameOver(Vector2 SoulStartingPosition)
    {
        MusicManager.StopSong(Fade: false, 0f);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        GonerMenu.Instance.CanOpenGonerMenu = false;
        ObjectHolder.SetActive(value: true);
        Soul.transform.position = SoulStartingPosition;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        BattleSystem.PlayBattleSoundEffect(SoulSFX_Break);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock);
        Soul.sprite = Soul_Break;
        yield return new WaitForSeconds(1.5f);
        Soul.enabled = false;
        Soul_ShatterParticles.Play();
        BattleSystem.Instance.CloseBattleBox();
        BattleSystem.PlayBattleSoundEffect(SoulSFX_Shatter);
        yield return new WaitForSeconds(2.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Annoyed);
        MusicManager.PlaySong(Music_FaintCourage, FadePreviousSong: false, 0f);
        GameOverTitle.color = new Color(1f, 1f, 1f, 0f);
        CutsceneUtils.FadeInSprite(GameOverTitle, 0.5f);
        yield return new WaitForSeconds(1f);
        targetText = PossibleDeathMessages[Random.Range(0, PossibleDeathMessages.Length)];
        GameOverText.RunText(targetText, currentIndex);
    }

    public void ProgressGameoverDialogue()
    {
        StartCoroutine(ProgressDialogueDelay());
    }

    private IEnumerator ProgressDialogueDelay()
    {
        yield return new WaitForSeconds(1.75f);
        GameOverText.CurrentTextIndex++;
        GameOverText.RunText(targetText, currentIndex);
    }

    public void EndGameoverDialogue()
    {
        StartCoroutine(EndDialogueDelay());
    }

    private IEnumerator EndDialogueDelay()
    {
        bool hasSelectedChoice = false;
        yield return new WaitForSeconds(1.75f);
        GameOverText.EndText();
        ContinueText.color = new Color(1f, 1f, 1f, 0f);
        CutsceneUtils.FadeInText3D(ContinueText, 0.5f);
        GiveUpText.color = new Color(1f, 1f, 1f, 0f);
        CutsceneUtils.FadeInText3D(GiveUpText, 0.5f);
        GhostSoul.color = new Color(1f, 1f, 1f, 0f);
        CutsceneUtils.FadeInSprite(GhostSoul, 0.5f, 0.133f);
        yield return new WaitForSeconds(0.5f);
        while (!hasSelectedChoice)
        {
            yield return null;
            Vector2 b = GhostSoul.transform.localPosition;
            if (CurrentSelectedChoice != 0 && CurrentSelectedChoice != 1)
            {
                b = new Vector2(0f, -3.65f);
                ContinueText.color = Color.white;
                GiveUpText.color = Color.white;
            }
            else if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
            {
                hasSelectedChoice = true;
                continue;
            }
            if (CurrentSelectedChoice == 0)
            {
                b = new Vector2(-3.55f, -3.65f);
                ContinueText.color = Color.yellow;
                GiveUpText.color = Color.white;
            }
            if (CurrentSelectedChoice == 1)
            {
                b = new Vector2(3.55f, -3.65f);
                ContinueText.color = Color.white;
                GiveUpText.color = Color.yellow;
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
            {
                CurrentSelectedChoice = 0;
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
            {
                CurrentSelectedChoice = 1;
            }
            GhostSoul.transform.localPosition = Vector2.Lerp(GhostSoul.transform.localPosition, b, 4f * Time.deltaTime);
        }
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        PlayerManager.Instance._PlayerHealth = PlayerManager.Instance._PlayerMaxHealth;
        if (CurrentSelectedChoice == 0)
        {
            GonerMenu.RecoveringFromGameOver = true;
            UI_FADE.Instance.StartFadeIn(SceneManager.GetActiveScene().buildIndex, 0.5f);
        }
        else
        {
            UI_FADE.Instance.StartFadeIn(2, 0.5f, UnpauseOnEnd: false, NewMainMenuManager.MainMenuStates.Hypothetical);
        }
        GonerMenu.Instance.CanOpenGonerMenu = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        Debug.Log("PlayerPrefs saved!");
        PlayerPrefs.Save();
        ResetGameOver();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
