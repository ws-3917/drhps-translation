using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GonerMenu : MonoBehaviour
{
    [SerializeField]
    public Vector2 TargetSoulPosition;

    [SerializeField]
    private float SoulLerpSpeed;

    [SerializeField]
    private Transform SoulTransform;

    [SerializeField]
    private Material UnselectedGonerMaterial;

    [SerializeField]
    private Material SelectedGonerMaterial;

    private SettingsManager Settings;

    public AudioSource GonerMenuSource;

    private Animator GonerMenuAnimator;

    private AudioReverbPreset PreviousPreset;

    private MusicManager GameMusic;

    private AudioReverbFilter PlayerReverbFilter;

    private PlayerManager Kris;

    private bool PreviousAnimationState;

    private float PreviousMusicVolume;

    private ChatboxManager ChatboxManager;

    private bool PreviousChatboxInputState;

    public Image CMenu_SoulIcon;

    private LightworldMenu LWMenu;

    private bool PreviousLWMenuInputState;

    public bool gonerMenuWasOpen;

    private bool PreviousPlayerInteractionState;

    private TRIG_LEVELTRANSITION ReturnToMenu;

    [SerializeField]
    private AudioClip[] GonerMenuClips;

    [SerializeReference]
    private RuntimeAnimatorController DefaultLightworldKrisRuntimeController;

    [SerializeReference]
    private RuntimeAnimatorController DefaultDarkworldKrisRuntimeController;

    [SerializeField]
    private TextMeshProUGUI TipText;

    [SerializeField]
    private TextMeshProUGUI MusicText;

    [SerializeField]
    private TextMeshProUGUI CashText;

    [SerializeField]
    private AudioClip CashgainSFX;

    [SerializeField]
    private TextMeshProUGUI CurrentQuestsText;

    [Space(5f)]
    public static Hypothesis CurrentActiveHypothesis;

    [Space(5f)]
    public bool GonerMenuOpen;

    public bool CanOpenGonerMenu;

    public string CurrentMenuState = "Default";

    public int CurrentCursorPosition;

    private int CurrentCursorMax;

    private int CurrentCursorMin;

    private bool UseHorizontalControls;

    private bool XReturnDebounce;

    private bool HasRanXReturnDebounce;

    public static bool RecoveringFromGameOver;

    [Header("Base Menu References")]
    [SerializeField]
    private Transform[] BaseMenuOptions;

    [SerializeField]
    private Color[] BaseMenuOptionsColors;

    [SerializeField]
    private TextMeshProUGUI[] BaseMenuTexts;

    [SerializeField]
    private GameObject RestartBattleOption;

    [Header("Settings Menu References")]
    [SerializeField]
    private GonerMenu_Settings SettingsMenu;

    [Header("Return Menu References")]
    [SerializeField]
    private Transform ReturnWarningMenu;

    [SerializeField]
    private Transform[] ReturnWarningOptions;

    [SerializeField]
    private TextMeshProUGUI[] ReturnWarningText;

    [Header("Restart Menu References")]
    [SerializeField]
    private Transform RestartWarningMenu;

    [SerializeField]
    private Transform[] RestartWarningOptions;

    [SerializeField]
    private TextMeshProUGUI[] RestartWarningText;

    private static GonerMenu instance;

    public static GonerMenu Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        CurrentMenuState = "Default";
        GonerMenuSource = GetComponent<AudioSource>();
        GonerMenuAnimator = GetComponent<Animator>();
        ChatboxManager = Object.FindFirstObjectByType<ChatboxManager>();
        GameMusic = MusicManager.Instance;
        ReturnToMenu = GetComponent<TRIG_LEVELTRANSITION>();
    }

    private void Update()
    {
        if (PlayerReverbFilter == null && Camera.main != null)
        {
            PlayerReverbFilter = Camera.main.transform.GetComponent<AudioReverbFilter>();
        }
        if (Kris == null)
        {
            Kris = Object.FindFirstObjectByType<PlayerManager>();
        }
        if (LWMenu == null)
        {
            LWMenu = Object.FindFirstObjectByType<LightworldMenu>();
        }
        if (GonerMenuOpen)
        {
            SoulTransform.localPosition = Vector2.Lerp(SoulTransform.localPosition, TargetSoulPosition, SoulLerpSpeed * Time.unscaledDeltaTime);
            MenuUpdate();
        }
        else
        {
            SoulTransform.position = Vector2.one * -500f;
        }
        RestartBattleOption.SetActive(BattleSystem.CurrentlyInBattle);
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Pause) && CanOpenGonerMenu)
        {
            if (!GonerMenuOpen)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    private void MenuUpdate()
    {
        if (!UseHorizontalControls)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
            {
                CurrentCursorPosition++;
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Up))
            {
                CurrentCursorPosition--;
            }
        }
        else
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
            {
                CurrentCursorPosition++;
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
            {
                CurrentCursorPosition--;
            }
        }
        CurrentCursorPosition = Mathf.Clamp(CurrentCursorPosition, CurrentCursorMin, CurrentCursorMax);
        switch (CurrentMenuState)
        {
            case "Options":
                XReturnDebounce = false;
                HasRanXReturnDebounce = false;
                SettingsMenu.SettingsMenuOpen = true;
                SettingsMenu.gameObject.SetActive(value: true);
                break;
            case "ReturnWarning":
                {
                    XReturnDebounce = false;
                    HasRanXReturnDebounce = false;
                    CurrentCursorMin = 0;
                    CurrentCursorMax = 1;
                    UseHorizontalControls = true;
                    TargetSoulPosition = new Vector2(ReturnWarningOptions[CurrentCursorPosition].localPosition.x - 80f, ReturnWarningOptions[CurrentCursorPosition].localPosition.y);
                    SoulTransform.localPosition = new Vector2(ReturnWarningOptions[CurrentCursorPosition].localPosition.x - 80f, ReturnWarningOptions[CurrentCursorPosition].localPosition.y);
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
                    {
                        GonerMenuSource.PlayOneShot(GonerMenuClips[1]);
                        switch (CurrentCursorPosition)
                        {
                            case 0:
                                UI_LoadingIcon.ToggleLoadingIcon(showIcon: true);
                                UI_FADE.Instance.StartFadeIn(2, 1f, UnpauseOnEnd: true, NewMainMenuManager.MainMenuStates.Hypothetical);
                                CurrentActiveHypothesis = null;
                                ResetPlayer();
                                break;
                            case 1:
                                CurrentMenuState = "Default";
                                if (BattleSystem.CurrentlyInBattle)
                                {
                                    CurrentCursorMax = 3;
                                    CurrentCursorPosition = 2;
                                }
                                else
                                {
                                    CurrentCursorMax = 2;
                                    CurrentCursorPosition = 2;
                                }
                                break;
                        }
                    }
                    for (int k = 0; k < ReturnWarningOptions.Length; k++)
                    {
                        if (k == CurrentCursorPosition)
                        {
                            ReturnWarningText[k].color = Color.yellow;
                        }
                        else
                        {
                            ReturnWarningText[k].color = Color.white;
                        }
                    }
                    ReturnWarningMenu.position = new Vector2(645f, 480f);
                    break;
                }
            case "RestartBattleWarning":
                {
                    XReturnDebounce = false;
                    HasRanXReturnDebounce = false;
                    bool flag = false;
                    CurrentCursorMin = 0;
                    CurrentCursorMax = 1;
                    UseHorizontalControls = true;
                    TargetSoulPosition = new Vector2(RestartWarningOptions[CurrentCursorPosition].localPosition.x - 80f, RestartWarningOptions[CurrentCursorPosition].localPosition.y);
                    SoulTransform.localPosition = new Vector2(RestartWarningOptions[CurrentCursorPosition].localPosition.x - 80f, RestartWarningOptions[CurrentCursorPosition].localPosition.y);
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
                    {
                        GonerMenuSource.PlayOneShot(GonerMenuClips[1]);
                        switch (CurrentCursorPosition)
                        {
                            case 0:
                                StartCoroutine(Delay_RestartBattle());
                                flag = true;
                                RestartWarningMenu.position = new Vector2(645f, 5760f);
                                break;
                            case 1:
                                CurrentMenuState = "Default";
                                if (BattleSystem.CurrentlyInBattle)
                                {
                                    CurrentCursorMax = 3;
                                    CurrentCursorPosition = 3;
                                    flag = true;
                                    RestartWarningMenu.position = new Vector2(645f, 5760f);
                                }
                                else
                                {
                                    CurrentCursorMax = 2;
                                    CurrentCursorPosition = 2;
                                }
                                break;
                        }
                    }
                    for (int j = 0; j < RestartWarningOptions.Length; j++)
                    {
                        if (j == CurrentCursorPosition)
                        {
                            RestartWarningText[j].color = Color.yellow;
                        }
                        else
                        {
                            RestartWarningText[j].color = Color.white;
                        }
                    }
                    if (!flag)
                    {
                        RestartWarningMenu.position = new Vector2(645f, 480f);
                    }
                    break;
                }
            case "Default":
                {
                    CurrentCursorMin = 0;
                    if (BattleSystem.CurrentlyInBattle)
                    {
                        CurrentCursorMax = 3;
                    }
                    else
                    {
                        CurrentCursorMax = 2;
                    }
                    UseHorizontalControls = false;
                    TargetSoulPosition = new Vector2(BaseMenuOptions[CurrentCursorPosition].localPosition.x - 500f, BaseMenuOptions[CurrentCursorPosition].localPosition.y);
                    if (!HasRanXReturnDebounce && !XReturnDebounce)
                    {
                        StartCoroutine(XReturn());
                        HasRanXReturnDebounce = true;
                    }
                    SettingsMenu.gameObject.SetActive(value: false);
                    ReturnWarningMenu.position = new Vector2(645f, 5760f);
                    RestartWarningMenu.position = new Vector2(645f, 5760f);
                    for (int i = 0; i < BaseMenuOptions.Length; i++)
                    {
                        if (i == CurrentCursorPosition)
                        {
                            BaseMenuTexts[i].color = Color.yellow;
                            BaseMenuTexts[i].fontSharedMaterial = SelectedGonerMaterial;
                        }
                        else
                        {
                            BaseMenuTexts[i].color = Color.white;
                            BaseMenuTexts[i].fontSharedMaterial = UnselectedGonerMaterial;
                        }
                    }
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
                    {
                        GonerMenuSource.PlayOneShot(GonerMenuClips[1]);
                        switch (CurrentCursorPosition)
                        {
                            case 0:
                                UnpauseGame();
                                CurrentMenuState = "Default";
                                break;
                            case 1:
                                CurrentCursorPosition = 0;
                                CurrentMenuState = "Options";
                                break;
                            case 2:
                                CurrentMenuState = "ReturnWarning";
                                CurrentCursorPosition = 1;
                                break;
                            case 3:
                                CurrentMenuState = "RestartBattleWarning";
                                CurrentCursorPosition = 1;
                                break;
                        }
                    }
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && XReturnDebounce)
                    {
                        UnpauseGame();
                        CurrentMenuState = "Default";
                    }
                    break;
                }
        }
    }

    private IEnumerator Delay_RestartBattle()
    {
        UI_FADE.Instance.StartFadeIn(-1, 3f);
        Battle storedBattle = BattleSystem.Instance.CurrentBattle;
        UnpauseGame();
        BattleSystem.EndBattle(BattleSystem.EndBattleTypes.Instant);
        BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
        BattleSystem.BattleChatbox.TextVisible = true;
        foreach (ActivePartyMember activePartyMember in PartyMemberSystem.Instance.ActivePartyMembers)
        {
            activePartyMember.CurrentHealth = activePartyMember.PartyMemberDescription.MaximumHealth;
        }
        yield return new WaitForSeconds(0.1f);
        BattleSystem.StartBattle(storedBattle, CameraManager.instance.transform.position, SkipIntro: true);
    }

    public void PauseGame()
    {
        if (PlayerReverbFilter != null)
        {
            PreviousPreset = PlayerReverbFilter.reverbPreset;
            PlayerReverbFilter.reverbPreset = AudioReverbPreset.Cave;
        }
        GonerMenuAnimator.Play("GonerMenu_Open");
        ChatboxManager.StartCoroutine(ChatboxManager.Instance.InputDebounceDelay());
        PreviousChatboxInputState = ChatboxManager.AllowInput;
        ChatboxManager.AllowInput = false;
        CMenu_SoulIcon.enabled = false;
        Vector3 position = Camera.main.WorldToScreenPoint(Kris.transform.position);
        SoulTransform.position = position;
        CurrentQuestsText.color = Color.white;
        if (CurrentActiveHypothesis != null)
        {
            CurrentQuestsText.text = "";
            HypothesisGoal[] hypothesisGoals = CurrentActiveHypothesis.HypothesisGoals;
            foreach (HypothesisGoal hypothesisGoal in hypothesisGoals)
            {
                int @int = PlayerPrefs.GetInt("HypothesisGoal_" + hypothesisGoal.GoalPlayprefName, 0);
                if (hypothesisGoal.RequiredTasks > 1)
                {
                    if (@int < hypothesisGoal.RequiredTasks)
                    {
                        TextMeshProUGUI currentQuestsText = CurrentQuestsText;
                        currentQuestsText.text = currentQuestsText.text + "<color=white>" + hypothesisGoal.GoalHint + " (" + @int + "/" + hypothesisGoal.RequiredTasks + ")</color>\n";
                    }
                    else
                    {
                        TextMeshProUGUI currentQuestsText2 = CurrentQuestsText;
                        currentQuestsText2.text = currentQuestsText2.text + "<color=yellow>" + hypothesisGoal.GoalHint + " (" + @int + "/" + hypothesisGoal.RequiredTasks + ")</color>\n";
                    }
                }
                else if (@int != 0)
                {
                    TextMeshProUGUI currentQuestsText3 = CurrentQuestsText;
                    currentQuestsText3.text = currentQuestsText3.text + "<color=yellow>" + hypothesisGoal.GoalHint + " (已完成)</color>\n";
                }
                else
                {
                    TextMeshProUGUI currentQuestsText4 = CurrentQuestsText;
                    currentQuestsText4.text = currentQuestsText4.text + "<color=white>" + hypothesisGoal.GoalHint + " (未完成)</color>\n";
                }
            }
        }
        else
        {
            CurrentQuestsText.text = "";
        }
        PreviousAnimationState = Kris._PMove.FreezeAnimation;
        Kris._PMove.FreezeAnimation = true;
        GonerMenuOpen = true;
        PreviousMusicVolume = MusicManager.Instance.source.volume;
        MusicManager.Instance.source.volume = 0.1f;
        GameMusic.GetComponent<AudioLowPassFilter>().enabled = true;
        PreviousLWMenuInputState = LWMenu.AllowInput;
        LWMenu.AllowInput = false;
        PreviousPlayerInteractionState = Kris._PInteract.CanInteract;
        Kris._PInteract.CanInteract = false;
        Time.timeScale = 0f;
    }

    public void Pause_OpenOptionMenu()
    {
        PauseGame();
        CurrentMenuState = "Options";
    }

    public void ResetPlayer()
    {
        ChatboxManager.EndText();
        ChatboxManager.AllowInput = true;
        BattleSystem.EndBattle(BattleSystem.EndBattleTypes.Instant);
        PlayerManager.Instance.transform.GetComponent<Collider2D>().enabled = true;
        PlayerManager.Instance.transform.GetComponent<Collider2D>().isTrigger = false;
        PlayerManager.Instance.transform.GetComponent<Rigidbody2D>().simulated = true;
        PlayerManager.Instance.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        PlayerManager.Instance.GetComponentInChildren<SpriteRenderer>().enabled = true;
        PlayerManager.Instance._PAnimation.FootstepsEnabled = false;
        PlayerManager.Instance._PMove.AnimationOverriden = false;
        PlayerManager.Instance._PInteract.CanInteract = true;
        PlayerManager.Instance._PMove.AllowSprint = true;
        MusicManager.Instance.source.loop = true;
        MusicManager.Instance.source.volume = 0.65f;
        MusicManager.Instance.source.pitch = 1f;
        LightworldMenu.Instance.CanOpenMenu = true;
        LightworldMenu.Instance.AllowInput = true;
        DarkworldMenu.Instance.CanOpenMenu = true;
        if (PlayerManager.Instance._PMove.InDarkworld)
        {
            PlayerManager.Instance._PAnimation.GetComponent<Animator>().runtimeAnimatorController = DefaultDarkworldKrisRuntimeController;
            PlayerManager.Instance._PAnimation.GetComponent<Animator>().Play("DARKWORLD_KRIS_IDLE");
        }
        else
        {
            PlayerManager.Instance._PAnimation.GetComponent<Animator>().runtimeAnimatorController = DefaultLightworldKrisRuntimeController;
            PlayerManager.Instance._PAnimation.GetComponent<Animator>().Play("OVERWORLD_NOELLE_IDLE");
        }
    }

    public void UnpauseGame()
    {
        CurrentMenuState = "Default";
        StartCoroutine(InformPreviousClosure());
        SettingsMenu.SettingsMenuOpen = true;
        SettingsMenu.gameObject.SetActive(value: false);
        ReturnWarningMenu.position = new Vector2(645f, 5760f);
        RestartWarningMenu.position = new Vector2(645f, 5760f);
        GonerMenuAnimator.Play("GonerMenu_Close");
        CMenu_SoulIcon.enabled = true;
        GonerMenuOpen = false;
        Kris._PMove.FreezeAnimation = PreviousAnimationState;
        ChatboxManager.AllowInput = PreviousChatboxInputState;
        CutsceneUtils.FadeOutText(CurrentQuestsText, 4f);
        MusicManager.Instance.source.volume = PreviousMusicVolume;
        if (PlayerReverbFilter != null)
        {
            PlayerReverbFilter.reverbPreset = PreviousPreset;
        }
        GameMusic.GetComponent<AudioLowPassFilter>().enabled = false;
        LWMenu.AllowInput = PreviousLWMenuInputState;
        Kris._PInteract.CanInteract = PreviousPlayerInteractionState;
        Time.timeScale = 1f;
    }

    private IEnumerator XReturn()
    {
        yield return new WaitForSeconds(0f);
        XReturnDebounce = true;
    }

    private IEnumerator InformPreviousClosure()
    {
        gonerMenuWasOpen = true;
        yield return new WaitForSeconds(0.1f);
        gonerMenuWasOpen = false;
    }

    public void ShowTip(string tip)
    {
        TipText.text = "小贴士！" + tip;
        TipText.GetComponent<Animator>().Play("GonerMenu_Tip", -1, 0f);
    }

    public void ShowMusicCredit(string songName, string Credit)
    {
        MusicText.text = "<scale=0.5> </scale>~ " + songName + " ~ " + Credit;
        MusicText.GetComponent<Animator>().Play("GonerMenu_Music", -1, 0f);
    }

    public void ShowCashGain(float moneyGain)
    {
        CashText.text = $"+${moneyGain}";
        CashText.GetComponent<Animator>().Play("GonerMenu_Cash", -1, 0f);
        CutsceneUtils.PlaySound(CashgainSFX, CutsceneUtils.DRH_MixerChannels.Effect, 0.4f, Random.Range(0.9f, 1.1f));
    }
}
