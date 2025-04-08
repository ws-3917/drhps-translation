using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState
    {
        Disabled = 0,
        Intro = 1,
        PlayerTurn = 2,
        PlayerUseRound = 3,
        Dialogue = 4,
        Cutscene = 5,
        EnemyTurn = 6,
        Finished = 7
    }

    public enum BattlePlayerTurnState
    {
        ACT = 0,
        ITEM = 1,
        FIGHT = 2,
        SPARE = 3,
        FINISHED = 4
    }

    public enum EndBattleTypes
    {
        Default = 0,
        Instant = 1,
        GameOver = 2
    }

    public enum BattleEnemyRemoveAnimation
    {
        Flee = 0,
        Lost = 1,
        Spare = 2
    }

    public enum BottomBarWindows
    {
        None = 0,
        Fight_EnemySelection = 1,
        Fight_AttackBar = 2,
        Item_SelectItem = 3,
        Spare_EnemySelection = 4,
        Item_MemberSelection = 5,
        Act_SelectAction = 6,
        Act_EnemySelection = 7,
        Act_MemberSelection = 8
    }

    private static BattleSystem instance;

    [Header("- References -")]
    [SerializeField]
    private AudioSource audioSource;

    public static BattleChatbox BattleChatbox;

    public static BattlePartyMember CurrentTurnPartyMember;

    [SerializeField]
    private BattleAction DefaultAction_Check;

    public GameObject BattleBox;

    [SerializeField]
    private Animator BattleBox_Animator;

    [SerializeField]
    private GameObject BattleBox_Collisions;

    [SerializeField]
    private GameObject BattleBox_Soul;

    [Space(5f)]
    [SerializeField]
    private GameObject BattleUI;

    public string CurrentBattleScript;

    public GameObject CurrentBattleScriptGameobject;

    [Header("UI")]
    [Header("Battle Numbers")]
    public BattleNumberSprites BattleNumbers_SpecificText;

    public BattleNumberSprites BattleNumbers_Default;

    public BattleNumberSprites BattleNumbers_Golden;

    [Header("Bottom Bar")]
    [SerializeField]
    private RectTransform BottomBar_Transform;

    [Header("TP Bar")]
    [SerializeField]
    private RectTransform TPBar_Transform;

    [SerializeField]
    private Image TPBar_Bar;

    [SerializeField]
    private Image TPBar_BarDropper;

    [SerializeField]
    private Image TPBar_BarTopper;

    [SerializeField]
    private TextMeshProUGUI TPBar_TPAmount;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject BattlePrefab_PartyMember;

    [SerializeField]
    private GameObject BattlePrefab_PartyMemberStatus;

    [SerializeField]
    private GameObject BattlePrefab_EnemySelectionStatus;

    [SerializeField]
    private GameObject BattlePrefab_PlayerAttackBar;

    public GameObject BattlePrefab_Effect_PartyMemberAttack;

    [SerializeField]
    private GameObject BattlePrefab_BattleNumbers;

    [SerializeField]
    private Animator BehindBoxAttackFade_Animator;

    [Header("Holders")]
    [SerializeField]
    private GameObject Holder_BattlePartyMembers;

    [SerializeField]
    private GameObject Holder_BattlePartyMemberStatuses;

    [SerializeField]
    private GameObject Holder_Enemies;

    [SerializeField]
    private GameObject Holder_EnemySelection;

    [SerializeField]
    private GameObject Holder_AttackBars;

    [SerializeField]
    private GameObject Holder_Effects;

    public GameObject Holder_Bullets;

    [Header("Backgrounds")]
    [SerializeField]
    private SpriteRenderer DefaultBackgroundBox1;

    [SerializeField]
    private SpriteRenderer DefaultBackgroundBox2;

    [Header("UI Windows")]
    public GameObject BattleWindow_EnemySelection;

    [SerializeField]
    private GameObject BattleWindow_AttackBars;

    public GameObject BattleWindow_ActItemSelection;

    [SerializeField]
    private GameObject BattleWindow_PartyMemberSelection;

    [Header("- Dialogue -")]
    [SerializeField]
    private CHATBOXTEXT Dialogue_UseItem;

    [SerializeField]
    private CHATBOXTEXT Dialogue_SpareSuccessful;

    [SerializeField]
    private CHATBOXTEXT Dialogue_SpareFail;

    [SerializeField]
    private CHATBOXTEXT Dialogue_SpareFail_PossiblePacify;

    [SerializeField]
    private CHATBOXTEXT Dialogue_BattleWon;

    [Space(10f)]
    [SerializeField]
    private CHATBOXTEXT Dialogue_DefaultSpell_HealPrayer;

    [SerializeField]
    private CHATBOXTEXT Dialogue_DefaultSpell_IceShock;

    [SerializeField]
    private CHATBOXTEXT Dialogue_DefaultSpell_RudeBuster;

    [SerializeField]
    private CHATBOXTEXT Dialogue_DefaultSpell_UltimatHeal;

    [SerializeField]
    private CHATBOXTEXT Dialogue_DefaultSpell_FailedPacify;

    [SerializeField]
    private CHATBOXTEXT Dialogue_DefaultSpell_SleepMist;

    [Header("- Sounds -")]
    public AudioClip SFX_soul_damage;

    public AudioClip SFX_battle_start;

    public AudioClip SFX_menu_move;

    public AudioClip SFX_menu_select;

    public AudioClip SFX_menu_deny;

    public AudioClip SFX_fight_slash;

    public AudioClip SFX_fight_critical;

    public AudioClip SFX_intro_stomp;

    public AudioClip SFX_enemy_damage;

    public AudioClip sfx_enemy_spare;

    public AudioClip sfx_enemy_spareblink;

    public AudioClip SFX_heal;

    public AudioClip SFX_EnemyFlee;

    public AudioClip SFX_graze;

    public AudioClip SFX_StrongerThanYouSansResponse;

    [Header("- Default Spell Sounds -")]
    [SerializeField]
    private AudioClip SFX_RudeBuster_Fire;

    [SerializeField]
    private AudioClip SFX_RudeBuster_Hit;

    [SerializeField]
    private AudioClip SFX_SleepMist;

    [SerializeField]
    private AudioClip SFX_IceShock;

    public static BattleState CurrentBattleState;

    [Header("- Effects -")]
    public GameObject Effect_Heal;

    public GameObject Effect_Spare;

    public GameObject Effect_SoulRipOut;

    public GameObject Effect_SoulInsert;

    public GameObject Effect_RudeBuster_Projectile;

    public GameObject Effect_RudeBuster_Explosion;

    public GameObject Effect_SleepMist;

    public GameObject Effect_IceShock_Individual;

    [Header("- Battle Settings -")]
    public static bool CurrentlyInBattle;

    public BattleState PreviousBattleState;

    private BattleState LastBattleState;

    public Battle CurrentBattle;

    public int BattleSetting_TPAmount;

    public List<BattlePartyMemberUse> CurrentPlayerMoves = new List<BattlePartyMemberUse>();

    public List<BattlePartyMember> BattlePartyMembers = new List<BattlePartyMember>();

    public List<BattleActiveEnemy> BattleActiveEnemies = new List<BattleActiveEnemy>();

    public BattlePlayerTurnState CurrentBattlePlayerTurnState;

    public int CurrentPlayerTurnSelectionIndex;

    public float CurrentPossibleEXP;

    public float CurrentDDTotal;

    public List<BattleEnemyAttack> QueuedBattleAttacks = new List<BattleEnemyAttack>();

    private List<GameObject> CurrentlyActiveAttacks = new List<GameObject>();

    public List<BattleAction> QueuedBattleActions = new List<BattleAction>();

    private int PlayerturnCount;

    private int EnemyturnCount;

    private int TotalturnCount;

    [Header("- Stored Values -")]
    private bool LightworldMenu_CouldBeOpened;

    private bool DarkworldMenu_CouldBeOpened;

    public bool CurrentlyRunningAction;

    private Coroutine currentAttackTimerCoroutine;

    private CameraManager currentMainCamera;

    [Header("- Temporary Debug -")]
    public TextMeshProUGUI debugText;

    public CHATBOXTEXT testText;

    public GameObject dialogueBubble;

    private Vector2 BattlePartyMemberScrolling_TargetPos;

    private bool skipIntroForBattle;

    private int EnemyDamageBattleNumberOffset = 1;

    public static BattleSystem Instance => instance;

    public event Action<int> Event_OnPlayerTurn;

    public event Action<List<BattlePartyMemberUse>> Event_OnPlayerUseRound;

    public event Action<int> Event_OnEnemyAttackTurn;

    public event Action<Battle> Event_OnBattleStart;

    public event Action<Battle, EndBattleTypes> Event_OnBattleEnd;

    public event Action<BattleState, BattleState> Event_OnBattleStateChange;

    public event Action<BattleActiveEnemy, float, BattlePartyMember> Event_OnEnemyDamaged;

    public event Action<BattleActiveEnemy, float, BattlePartyMember> Event_OnEnemyKilled;

    public event Action<BattleActiveEnemy, bool, BattlePartyMember> Event_OnEnemySpared;

    public event Action<BattlePartyMember, float> Event_OnMemberDamaged;

    private void Awake()
    {
        instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        BattleWindow_EnemySelection.SetActive(value: false);
        BattleWindow_AttackBars.SetActive(value: false);
        BattleWindow_ActItemSelection.SetActive(value: false);
        BattleWindow_PartyMemberSelection.SetActive(value: false);
    }

    private void Start()
    {
        BattleChatbox = BattleChatbox.Instance;
        BattleBox_Soul.SetActive(value: false);
        BattleBox_Collisions.SetActive(value: false);
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void Update()
    {
        UpdateTPBarUI();
        UpdateBottomBarUI();
        UpdateDefaultBackground();
        BattleStateUpdate();
        UpdateBattleGraphicPositions();
        if (CurrentTurnPartyMember == null)
        {
            debugText.text = "current battle state: " + CurrentBattleState.ToString() + "\nCurrentTurnPartyMember: none";
        }
        else
        {
            debugText.text = "current battle state: " + CurrentBattleState.ToString() + "\nCurrentTurnPartyMember: " + CurrentTurnPartyMember.PartyMemberInBattle.PartyMemberName;
        }
        if (PreviousBattleState != CurrentBattleState)
        {
            OnBattleStateChange(CurrentBattleState, PreviousBattleState);
            LastBattleState = PreviousBattleState;
            PreviousBattleState = CurrentBattleState;
            BattleState_OnUpdate();
        }
    }

    private void UpdateBattleGraphicPositions()
    {
        if (currentMainCamera == null)
        {
            currentMainCamera = CameraManager.instance;
        }
        else
        {
            base.transform.position = (Vector2)currentMainCamera.transform.position;
        }
    }

    private void BattleStateUpdate()
    {
        switch (CurrentBattleState)
        {
            case BattleState.Intro:
                BattleChatbox.AllowInput = false;
                if (!skipIntroForBattle)
                {
                    if (Intro_AllPartyMembersInPosition() && Intro_AllEnemiesInPosition())
                    {
                        PlayBattleSoundEffect(SFX_intro_stomp);
                        BattleUI.SetActive(value: true);
                        MusicManager.PlaySong(CurrentBattle.BattleSong, FadePreviousSong: false, 0f);
                        CurrentBattleState = BattleState.PlayerTurn;
                    }
                    else
                    {
                        Intro_UpdatePartyMembersPositions();
                        Intro_UpdateEnemyPositions();
                    }
                }
                else
                {
                    DefaultBackgroundBox1.color = Color.white;
                    DefaultBackgroundBox2.color = Color.white;
                    UI_FADE.Instance.StartFadeOut(3f);
                    Intro_ForcefullySetPartyMembersPositions();
                    Intro_ForcefullySetEnemyPositions();
                    BattleUI.SetActive(value: true);
                    MusicManager.PlaySong(CurrentBattle.BattleSong, FadePreviousSong: false, 0f);
                    CurrentBattleState = BattleState.PlayerTurn;
                }
                break;
            case BattleState.PlayerTurn:
                BattleChatbox.AllowInput = false;
                break;
            case BattleState.Finished:
                BattleChatbox.AllowInput = true;
                break;
        }
    }

    private void BattleState_OnUpdate()
    {
        CheckForValidBattleEnd();
        switch (CurrentBattleState)
        {
            case BattleState.PlayerTurn:
                {
                    foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
                    {
                        battlePartyMember.PartyMemberStatus.ResetMemberIcon();
                    }
                    PerRound_HealDownedMembers();
                    for (int i = 0; i < BattlePartyMembers.Count; i++)
                    {
                        BattlePartyMembers[i].PartyMemberInBattle_AfterImageParticleRenderer.Stop();
                    }
                    if (LastBattleState == BattleState.Intro)
                    {
                        PartyMembers_AllPlayAnimations("Intro");
                    }
                    else
                    {
                        PartyMembers_AllPlayAnimations("Idle");
                    }
                    if (LastBattleState == BattleState.EnemyTurn)
                    {
                        CloseBattleBox();
                    }
                    foreach (BattlePartyMember battlePartyMember2 in BattlePartyMembers)
                    {
                        battlePartyMember2.IsDefending = false;
                        battlePartyMember2.SkippingTurn = false;
                    }
                    OpenBottomBarWindow(BottomBarWindows.None);
                    CurrentPlayerMoves.Clear();
                    CurrentBattlePlayerTurnState = BattlePlayerTurnState.ACT;
                    CurrentPlayerTurnSelectionIndex = 0;
                    BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
                    OnPlayerTurn();
                    break;
                }
            case BattleState.PlayerUseRound:
                EnemyDamageBattleNumberOffset = 1;
                ResetAllPartyMemberStatuses();
                CurrentBattlePlayerTurnState = BattlePlayerTurnState.ACT;
                BattleChatbox.EndText();
                StartCoroutine(BattlePlayerTurn_RunMoves());
                OnPlayerUseRound();
                break;
            case BattleState.Dialogue:
                {
                    bool flag = false;
                    foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
                    {
                        if (battleActiveEnemy == null)
                        {
                            MonoBehaviour.print("how?");
                        }
                        if (battleActiveEnemy.QueuedDialogue.Count != 0)
                        {
                            flag = true;
                        }
                    }
                    foreach (BattleActiveEnemy battleActiveEnemy2 in BattleActiveEnemies)
                    {
                        if (battleActiveEnemy2.Enemy_MercyAmount >= 100f)
                        {
                            ActiveEnemies_PlayAnimation(battleActiveEnemy2, "Spare");
                        }
                        else
                        {
                            ActiveEnemies_PlayAnimation(battleActiveEnemy2, "Idle");
                        }
                    }
                    if (flag)
                    {
                        StartCoroutine(BattleDialogue_RunEnemyDialogue());
                    }
                    else
                    {
                        CurrentBattleState = BattleState.EnemyTurn;
                    }
                    break;
                }
            case BattleState.EnemyTurn:
                OnEnemyAttackTurn();
                DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Default);
                OpenBattleBox();
                CreateAllQueuedAttacks();
                break;
            case BattleState.Cutscene:
                break;
        }
    }

    private IEnumerator BattlePlayerTurn_RunMoves()
    {
        yield return null;
        List<BattlePartyMemberUse> Moves = CurrentPlayerMoves;
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.CheekyGrin);
        List<BattlePartyMemberUse> list = new List<BattlePartyMemberUse>();
        List<BattlePartyMemberUse> fightMoves = new List<BattlePartyMemberUse>();
        foreach (BattlePartyMemberUse item in Moves)
        {
            if (item.BattleMove == BattlePlayerMove.Action || item.BattleMove == BattlePlayerMove.Item || item.BattleMove == BattlePlayerMove.Spare)
            {
                list.Add(item);
            }
            else if (item.BattleMove == BattlePlayerMove.Fight)
            {
                fightMoves.Add(item);
            }
        }
        foreach (BattlePartyMemberUse item2 in list)
        {
            CheckForValidBattleEnd();
            if (CurrentBattleState == BattleState.Finished)
            {
                continue;
            }
            if (item2.BattleMove == BattlePlayerMove.Item)
            {
                CurrentBattlePlayerTurnState = BattlePlayerTurnState.ITEM;
                MembersUseItems(new List<BattlePartyMemberUse> { item2 });
                while (CurrentBattlePlayerTurnState == BattlePlayerTurnState.ITEM)
                {
                    yield return new WaitForSeconds(0.015f);
                }
            }
            else if (item2.BattleMove == BattlePlayerMove.Action)
            {
                CurrentBattlePlayerTurnState = BattlePlayerTurnState.ACT;
                MemberUseActions(new List<BattlePartyMemberUse> { item2 });
                while (CurrentBattlePlayerTurnState == BattlePlayerTurnState.ACT || CurrentlyRunningAction)
                {
                    yield return new WaitForSeconds(0.25f);
                }
            }
            else if (item2.BattleMove == BattlePlayerMove.Spare)
            {
                CurrentBattlePlayerTurnState = BattlePlayerTurnState.SPARE;
                MembersSpare(new List<BattlePartyMemberUse> { item2 });
                while (CurrentBattlePlayerTurnState == BattlePlayerTurnState.SPARE)
                {
                    yield return new WaitForSeconds(0.015f);
                }
            }
        }
        CheckForValidBattleEnd();
        if (CurrentBattleState == BattleState.Finished)
        {
            yield break;
        }
        if (fightMoves.Count > 0)
        {
            CurrentBattlePlayerTurnState = BattlePlayerTurnState.FIGHT;
            CreateNewAttackBars(fightMoves);
            while (CurrentBattlePlayerTurnState == BattlePlayerTurnState.FIGHT)
            {
                yield return new WaitForSeconds(0.015f);
            }
        }
        CurrentBattleState = BattleState.Dialogue;
        PartyMembers_AllPlayAnimations("Idle");
        foreach (BattlePartyMemberUse item3 in Moves)
        {
            if (item3.BattleMove == BattlePlayerMove.Defend && item3.targetPartyMember.PartyMember_Health > 0f)
            {
                item3.targetPartyMember.IsDefending = true;
                PartyMembers_MemberPlayAnimation(item3.targetPartyMember, "Defend", 1f);
            }
        }
    }

    public void QueueBattleActions(IEnumerable<BattleAction> actions)
    {
        QueuedBattleActions.AddRange(actions);
    }

    private IEnumerator BattleDialogue_RunEnemyDialogue()
    {
        List<GameObject> createdBubbles = new List<GameObject>();
        bool allDialoguesCompleted;
        do
        {
            allDialoguesCompleted = true;
            List<BattleBubbleChatbox> activeBubbles = new List<BattleBubbleChatbox>();
            foreach (GameObject item in createdBubbles)
            {
                UnityEngine.Object.Destroy(item);
            }
            foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
            {
                if (battleActiveEnemy.QueuedDialogue.Count > 0 && battleActiveEnemy.TextIndex < battleActiveEnemy.QueuedDialogue[0].Textboxes[0].Text.Length)
                {
                    if (!(battleActiveEnemy.QueuedDialogue[0].Textboxes[0].Text[battleActiveEnemy.TextIndex] == "") && !(battleActiveEnemy.QueuedDialogue[0].Textboxes[0].Text[battleActiveEnemy.TextIndex] == ""))
                    {
                        BattleBubbleChatbox battleBubbleChatbox = NewDialogueForEnemy(battleActiveEnemy, battleActiveEnemy.QueuedDialogue[0], battleActiveEnemy.TextIndex, 0, battleActiveEnemy.QueuedDialogueBubble[0]);
                        activeBubbles.Add(battleBubbleChatbox);
                        createdBubbles.Add(battleBubbleChatbox.gameObject);
                    }
                    allDialoguesCompleted = false;
                }
                else
                {
                    Debug.Log(battleActiveEnemy.EnemyInBattle.EnemyName + " has no more dialogue.");
                    battleActiveEnemy.TextIndex = 0;
                }
            }
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Annoyed);
            if (activeBubbles.Count <= 0)
            {
                continue;
            }
            yield return new WaitUntil(() => activeBubbles.All((BattleBubbleChatbox bubble) => bubble.FinishedShowingText));
            float waitTime = 2f;
            float elapsedTime = 0f;
            bool playerPressedConfirm = false;
            while (elapsedTime < waitTime)
            {
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !GonerMenu.Instance.GonerMenuOpen && !GonerMenu.Instance.gonerMenuWasOpen)
                {
                    playerPressedConfirm = true;
                    break;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (playerPressedConfirm)
            {
                yield return new WaitForEndOfFrame();
            }
            foreach (BattleActiveEnemy battleActiveEnemy2 in BattleActiveEnemies)
            {
                battleActiveEnemy2.TextIndex++;
                if (battleActiveEnemy2.QueuedDialogue.Count > 0 && battleActiveEnemy2.TextIndex < battleActiveEnemy2.QueuedDialogue[0].Textboxes[0].Text.Length)
                {
                    allDialoguesCompleted = false;
                }
            }
        }
        while (!allDialoguesCompleted);
        foreach (GameObject item2 in createdBubbles)
        {
            UnityEngine.Object.Destroy(item2);
        }
        foreach (BattleActiveEnemy battleActiveEnemy3 in BattleActiveEnemies)
        {
            if (battleActiveEnemy3.QueuedDialogue.Count >= 1)
            {
                battleActiveEnemy3.QueuedDialogue.RemoveAt(0);
            }
            if (battleActiveEnemy3.QueuedDialogueBubble.Count >= 1)
            {
                battleActiveEnemy3.QueuedDialogueBubble.RemoveAt(0);
            }
            if (battleActiveEnemy3.SpecificTextIndexes.Count >= 1)
            {
                battleActiveEnemy3.SpecificTextIndexes.RemoveAt(0);
            }
        }
        Debug.Log("finished!");
        CurrentBattleState = BattleState.EnemyTurn;
    }

    public static void BattleState_PlayerTurn_UpdateCurrentlySelectedStatus()
    {
        if (CurrentBattleState != BattleState.PlayerTurn)
        {
            return;
        }
        bool flag = true;
        for (int i = 0; i < instance.BattlePartyMembers.Count; i++)
        {
            if (instance.BattlePartyMembers[i].PartyMember_Health > 0f)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            for (int j = 0; j < instance.BattlePartyMembers.Count; j++)
            {
                instance.BattlePartyMembers[j].PartyMemberStatus.CurrentlySelected = false;
                instance.BattlePartyMembers[j].PartyMemberStatus.StatusTabOpened = false;
            }
            return;
        }
        for (int k = 0; k < instance.BattlePartyMembers.Count; k++)
        {
            if (k == instance.CurrentPlayerTurnSelectionIndex)
            {
                if (!(instance.BattlePartyMembers[k].PartyMember_Health > 0f) || instance.BattlePartyMembers[k].SkippingTurn)
                {
                    IncrementCurrentPartyMemberStatusSelected();
                    break;
                }
                instance.BattlePartyMembers[k].PartyMemberStatus.CurrentlySelected = true;
                instance.BattlePartyMembers[k].PartyMemberStatus.StatusTabOpened = true;
            }
            else
            {
                instance.BattlePartyMembers[k].PartyMemberStatus.CurrentlySelected = false;
                instance.BattlePartyMembers[k].PartyMemberStatus.StatusTabOpened = false;
            }
        }
    }

    public static void IncrementCurrentPartyMemberStatusSelected()
    {
        instance.CurrentPlayerTurnSelectionIndex++;
        BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
        if (instance.CurrentPlayerTurnSelectionIndex >= instance.BattlePartyMembers.Count)
        {
            instance.CurrentPlayerTurnSelectionIndex = 0;
            instance.ResetAllPartyMemberStatuses();
            CurrentBattleState = BattleState.PlayerUseRound;
            CurrentTurnPartyMember = null;
        }
        else
        {
            CurrentTurnPartyMember = instance.BattlePartyMembers[instance.CurrentPlayerTurnSelectionIndex];
            if (CurrentTurnPartyMember != null && CurrentTurnPartyMember.PartyMember_Health <= 0f)
            {
                IncrementCurrentPartyMemberStatusSelected();
            }
        }
    }

    public static void UndoPreviousCurrentPartyMemberStatusSelected()
    {
        if (Instance.BattlePartyMembers[Instance.CurrentPlayerTurnSelectionIndex].PartyMember_Health > 0f)
        {
            Instance.PartyMembers_MemberPlayAnimation(Instance.BattlePartyMembers[Instance.CurrentPlayerTurnSelectionIndex], "Idle");
        }
        instance.CurrentPlayerTurnSelectionIndex--;
        while (instance.CurrentPlayerTurnSelectionIndex >= 0 && !(instance.BattlePartyMembers[instance.CurrentPlayerTurnSelectionIndex].PartyMember_Health > 0f))
        {
            instance.CurrentPlayerTurnSelectionIndex--;
        }
        if (instance.CurrentPlayerTurnSelectionIndex < 0)
        {
            for (int i = 0; i < instance.BattlePartyMembers.Count; i++)
            {
                if (instance.BattlePartyMembers[i].PartyMember_Health > 0f)
                {
                    instance.CurrentPlayerTurnSelectionIndex = i;
                    break;
                }
            }
        }
        instance.BattlePartyMembers[instance.CurrentPlayerTurnSelectionIndex].PartyMemberStatus.CurrentlySelected = true;
        if (instance.CurrentPlayerMoves.Count > 0)
        {
            if (instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].Item_ToUse != null)
            {
                if (instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].StoredItemOriginalIndex > -1)
                {
                    DarkworldInventory.Instance.PlayerInventory.Insert(instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].StoredItemOriginalIndex, instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].Item_ToUse);
                }
                else
                {
                    DarkworldInventory.Instance.PlayerInventory.Add(instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].Item_ToUse);
                }
            }
            if (instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].BattleMove == BattlePlayerMove.Defend)
            {
                Instance.AddTP(-16);
            }
            if (instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].Action_ToRun != null && instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].Action_ToRun.TPRequired > 0)
            {
                instance.AddTP(instance.CurrentPlayerMoves[instance.CurrentPlayerMoves.Count - 1].Action_ToRun.TPRequired);
            }
            instance.CurrentPlayerMoves.RemoveAt(instance.CurrentPlayerMoves.Count - 1);
        }
        CurrentTurnPartyMember = instance.BattlePartyMembers[instance.CurrentPlayerTurnSelectionIndex];
        BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
    }

    private void ResetAllPartyMemberStatuses()
    {
        foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
        {
            battlePartyMember.PartyMemberStatus.CurrentlySelected = false;
            battlePartyMember.PartyMemberStatus.SelectedIndex = 0;
        }
    }

    public void HideAllBattleWindows()
    {
        BattleWindow_EnemySelection.SetActive(value: false);
        BattleWindow_AttackBars.SetActive(value: false);
        BattleWindow_ActItemSelection.SetActive(value: false);
        BattleWindow_PartyMemberSelection.SetActive(value: false);
    }

    private void UpdateTPBarUI()
    {
        if (CurrentBattleState == BattleState.Disabled || CurrentBattleState == BattleState.Intro)
        {
            TPBar_Transform.anchoredPosition = Vector2.Lerp(TPBar_Transform.anchoredPosition, new Vector2(-300f, 0f), 12f * Time.fixedDeltaTime);
        }
        else
        {
            TPBar_Transform.anchoredPosition = Vector2.Lerp(TPBar_Transform.anchoredPosition, new Vector2(0f, 0f), 12f * Time.fixedDeltaTime);
        }
        float num = BattleSetting_TPAmount;
        TPBar_Bar.fillAmount = Mathf.Lerp(TPBar_Bar.fillAmount, num / 100f, 4f * Time.deltaTime);
        TPBar_BarDropper.fillAmount = Mathf.Lerp(TPBar_BarDropper.fillAmount, num / 100f - 0.015f, 3f * Time.deltaTime);
        TPBar_BarTopper.fillAmount = Mathf.Lerp(TPBar_BarTopper.fillAmount, num / 100f + 0.015f, 6f * Time.deltaTime);
        TPBar_TPAmount.text = num.ToString();
    }

    public void AddTP(int Amount)
    {
        BattleSetting_TPAmount = Mathf.Clamp(BattleSetting_TPAmount + Amount, 0, 100);
        UpdateTPBarUI();
    }

    private void UpdateBottomBarUI()
    {
        if (CurrentPlayerTurnSelectionIndex % 3 == 0 && CurrentPlayerTurnSelectionIndex != 0)
        {
            BattlePartyMemberScrolling_TargetPos = new Vector2(-1280 * (CurrentPlayerTurnSelectionIndex / 3), 0f);
        }
        ((RectTransform)Holder_BattlePartyMemberStatuses.transform).anchoredPosition = Vector2.Lerp(((RectTransform)Holder_BattlePartyMemberStatuses.transform).anchoredPosition, BattlePartyMemberScrolling_TargetPos, 15f * Time.fixedDeltaTime);
        switch (CurrentBattleState)
        {
            case BattleState.Disabled:
                BottomBar_Transform.anchoredPosition = Vector2.Lerp(BottomBar_Transform.anchoredPosition, new Vector2(0f, -400f), 12f * Time.fixedDeltaTime);
                break;
            case BattleState.PlayerTurn:
                BottomBar_Transform.anchoredPosition = Vector2.Lerp(BottomBar_Transform.anchoredPosition, new Vector2(0f, 0f), 12f * Time.fixedDeltaTime);
                break;
        }
    }

    private void UpdateDefaultBackground()
    {
        if (!CurrentlyInBattle)
        {
            DefaultBackgroundBox1.color = Color.Lerp(DefaultBackgroundBox1.color, new Color(1f, 1f, 1f, 0f), 12f * Time.fixedDeltaTime);
            DefaultBackgroundBox2.color = Color.Lerp(DefaultBackgroundBox2.color, new Color(1f, 1f, 1f, 0f), 12f * Time.fixedDeltaTime);
        }
        else
        {
            DefaultBackgroundBox1.color = Color.Lerp(DefaultBackgroundBox1.color, new Color(1f, 1f, 1f, 1f), 12f * Time.fixedDeltaTime);
            DefaultBackgroundBox2.color = Color.Lerp(DefaultBackgroundBox2.color, new Color(1f, 1f, 1f, 1f), 12f * Time.fixedDeltaTime);
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        BattleBox_Collisions.SetActive(value: false);
    }

    public static void PlayBattleSoundEffect(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (instance != null && instance.audioSource != null)
        {
            instance.audioSource.pitch = pitch;
            instance.audioSource.PlayOneShot(clip, volume);
        }
    }

    public static void StartBattle(Battle Battle, Vector2 EnemyStartPosition, bool SkipIntro = false)
    {
        CurrentlyInBattle = true;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Battle;
        PlayerManager.Instance.gameObject.SetActive(value: false);
        PartyMemberSystem.Instance.SetAllPartyMembersActive(ActiveSelf: false);
        instance.LightworldMenu_CouldBeOpened = LightworldMenu.Instance.CanOpenMenu;
        instance.DarkworldMenu_CouldBeOpened = DarkworldMenu.Instance.CanOpenMenu;
        DarkworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.CanOpenMenu = false;
        instance.SetupBattleScript(Battle);
        instance.skipIntroForBattle = SkipIntro;
        instance.ResetAllBattleEvents();
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
        instance.CurrentPossibleEXP = 0f;
        instance.CurrentDDTotal = 0f;
        MusicManager.StopSong(Fade: false, 0f);
        instance.BattlePartyMembers.Clear();
        instance.QueuedBattleActions.Clear();
        List<BattleAction> actions = new List<BattleAction> { instance.DefaultAction_Check };
        instance.QueueBattleActions(actions);
        instance.SetupPartyMembersInBattle(Battle);
        instance.CurrentBattle = Battle;
        for (int i = 0; i < instance.BattlePartyMembers.Count; i++)
        {
            foreach (BattleAction partyMember_DefaultSpell in instance.BattlePartyMembers[i].PartyMemberInBattle.PartyMember_DefaultSpells)
            {
                instance.QueuedBattleActions.Add(partyMember_DefaultSpell);
            }
        }
        instance.SetupEnemiesInBattle(Battle, EnemyStartPosition);
        CurrentBattleState = BattleState.Intro;
        PlayBattleSoundEffect(instance.SFX_battle_start);
    }

    public static void EndBattle(EndBattleTypes EndType = EndBattleTypes.Default)
    {
        switch (EndType)
        {
            case EndBattleTypes.Default:
                instance.StartCoroutine(instance.EndBattle_Default());
                DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Default);
                break;
            case EndBattleTypes.Instant:
                instance.StartCoroutine(instance.EndBattle_Instant());
                break;
            case EndBattleTypes.GameOver:
                instance.StartCoroutine(instance.EndBattle_GameOver());
                break;
        }
    }

    private IEnumerator EndBattle_Default()
    {
        yield return null;
        CurrentlyInBattle = false;
        instance.ResetAllBattleEvents();
        if (currentAttackTimerCoroutine != null)
        {
            StopCoroutine(currentAttackTimerCoroutine);
        }
        EndEnemyAttackTurn(RevertToPlayerTurn: false);
        UnityEngine.Object.Destroy(CurrentBattleScriptGameobject);
        Battle_PlayerSoul.Instance.CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        Battle_PlayerSoul.Instance.gameObject.SetActive(value: false);
        RefundAllMoveItems();
        MusicManager.StopSong(Fade: false, 0f);
        CurrentBattleState = BattleState.Disabled;
        instance.PreviousBattleState = BattleState.Disabled;
        bool allPartyMembersAtTarget = false;
        while (!allPartyMembersAtTarget)
        {
            allPartyMembersAtTarget = true;
            yield return null;
            for (int i = 0; i < BattlePartyMembers.Count; i++)
            {
                BattlePartyMembers[i].PartyMemberInBattle_AfterImageParticleRenderer.Stop();
                GameObject partyMemberInBattle_Gameobjects = BattlePartyMembers[i].PartyMemberInBattle_Gameobjects;
                Vector3 vector = BattlePartyMembers[i].StoredOriginalOverworldPosition;
                partyMemberInBattle_Gameobjects.transform.position = Vector3.MoveTowards(partyMemberInBattle_Gameobjects.transform.position, vector, 30f * Time.deltaTime);
                if (BattlePartyMembers[i].PartyMemberInBattle.StartFromPlayer)
                {
                    Animator anim = PlayerManager.Instance._PMove._anim;
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.runtimeAnimatorController = anim.runtimeAnimatorController;
                    if (PlayerManager.Instance._PMove.InDarkworld)
                    {
                        BattlePartyMembers[i].PartyMemberInBattle_Animator.Play("DARKWORLD_KRIS_IDLE");
                    }
                    else
                    {
                        BattlePartyMembers[i].PartyMemberInBattle_Animator.Play("OVERWORLD_NOELLE_IDLE");
                    }
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.SetFloat("MOVEMENTX", anim.GetFloat("MOVEMENTX"));
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.SetFloat("MOVEMENTY", anim.GetFloat("MOVEMENTY"));
                }
                else
                {
                    Animator anim = PartyMemberSystem.Instance.HasMemberInParty(BattlePartyMembers[i].PartyMemberInBattle).PartyMemberFollowSettings.SusieAnimator;
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.runtimeAnimatorController = anim.runtimeAnimatorController;
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.Play("Idle");
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.SetFloat("VelocityX", anim.GetFloat("VelocityX"));
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.SetFloat("VelocityY", anim.GetFloat("VelocityY"));
                    BattlePartyMembers[i].PartyMemberInBattle_Animator.SetFloat("VelocityMagnitude", 0f);
                }
                if (partyMemberInBattle_Gameobjects.transform.position != vector)
                {
                    allPartyMembersAtTarget = false;
                    continue;
                }
                BattlePartyMembers[i].PartyMemberInBattle_MainSpriteRenderer.enabled = false;
                if (BattlePartyMembers[i].PartyMemberInBattle.StartFromPlayer)
                {
                    PlayerManager.Instance.gameObject.SetActive(value: true);
                }
                else
                {
                    PartyMemberSystem.Instance.HasMemberInParty(BattlePartyMembers[i].PartyMemberInBattle)?.PartyMemberTransform.gameObject.SetActive(value: true);
                }
            }
        }
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        PlayerManager.Instance.gameObject.SetActive(value: true);
        PartyMemberSystem.Instance.SetAllPartyMembersActive(ActiveSelf: true);
        DarkworldMenu.Instance.CanOpenMenu = DarkworldMenu_CouldBeOpened;
        LightworldMenu.Instance.CanOpenMenu = LightworldMenu_CouldBeOpened;
        instance.OnBattleEnd(instance.CurrentBattle, EndBattleTypes.Default);
        instance.CurrentBattle = null;
        instance.BattlePartyMembers.Clear();
        ClearBattleSystem();
    }

    private IEnumerator EndBattle_Instant()
    {
        yield return null;
        CurrentlyInBattle = false;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        PlayerManager.Instance.gameObject.SetActive(value: true);
        PartyMemberSystem.Instance.SetAllPartyMembersActive(ActiveSelf: true);
        RefundAllMoveItems();
        DarkworldMenu.Instance.CanOpenMenu = DarkworldMenu_CouldBeOpened;
        LightworldMenu.Instance.CanOpenMenu = LightworldMenu_CouldBeOpened;
        CloseBattleBox(DisableSoulEffect: true);
        UnityEngine.Object.Destroy(CurrentBattleScriptGameobject);
        instance.ResetAllBattleEvents();
        if (currentAttackTimerCoroutine != null)
        {
            StopCoroutine(currentAttackTimerCoroutine);
        }
        EndEnemyAttackTurn(RevertToPlayerTurn: false);
        BattleUI.SetActive(value: false);
        Battle_PlayerSoul.Instance.CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        Battle_PlayerSoul.Instance.gameObject.SetActive(value: false);
        MusicManager.StopSong(Fade: false, 0f);
        instance.BattlePartyMembers.Clear();
        instance.OnBattleEnd(instance.CurrentBattle, EndBattleTypes.Instant);
        instance.CurrentBattle = null;
        CurrentBattleState = BattleState.Disabled;
        instance.PreviousBattleState = BattleState.Disabled;
        ClearBattleSystem();
    }

    private IEnumerator EndBattle_GameOver()
    {
        yield return null;
        CurrentlyInBattle = false;
        PlayerManager.Instance.gameObject.SetActive(value: false);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        DarkworldMenu.Instance.CanOpenMenu = DarkworldMenu_CouldBeOpened;
        LightworldMenu.Instance.CanOpenMenu = LightworldMenu_CouldBeOpened;
        CloseBattleBox(DisableSoulEffect: true);
        ResetAllBattleEvents();
        RefundAllMoveItems();
        if (currentAttackTimerCoroutine != null)
        {
            StopCoroutine(currentAttackTimerCoroutine);
        }
        EndEnemyAttackTurn(RevertToPlayerTurn: false);
        BattleUI.SetActive(value: false);
        instance.OnBattleEnd(instance.CurrentBattle, EndBattleTypes.GameOver);
        Battle_PlayerSoul.Instance.CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        Battle_PlayerSoul.Instance.gameObject.SetActive(value: false);
        UnityEngine.Object.Destroy(CurrentBattleScriptGameobject);
        MusicManager.StopSong(Fade: false, 0f);
        instance.CurrentBattle = null;
        BattlePartyMembers.Clear();
        CurrentBattle = null;
        CurrentBattleState = BattleState.Disabled;
        PreviousBattleState = BattleState.Disabled;
        ClearBattleSystem();
    }

    private void ClearBattleSystem()
    {
        StopAllCoroutines();
        BattleWindow_AttackBars.GetComponent<BattleAttackWindow>().ClearAttackBars();
        for (int i = 0; i < Holder_BattlePartyMembers.transform.childCount; i++)
        {
            UnityEngine.Object.Destroy(Holder_BattlePartyMembers.transform.GetChild(i).gameObject);
        }
        BattlePartyMembers.Clear();
        foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
        {
            UnityEngine.Object.Destroy(battleActiveEnemy.EnemyInBattle_Gameobject);
        }
        BattleBubbleChatbox[] array = UnityEngine.Object.FindObjectsByType<BattleBubbleChatbox>(FindObjectsSortMode.None);
        for (int j = 0; j < array.Length; j++)
        {
            UnityEngine.Object.Destroy(array[j].gameObject);
        }
        BattleSetting_TPAmount = 0;
        CurrentDDTotal = 0f;
        CurrentPossibleEXP = 0f;
        CurrentPlayerMoves.Clear();
        for (int k = 0; k < Holder_BattlePartyMemberStatuses.transform.childCount; k++)
        {
            UnityEngine.Object.Destroy(Holder_BattlePartyMemberStatuses.transform.GetChild(k).gameObject);
        }
        for (int l = 0; l < Holder_EnemySelection.transform.childCount; l++)
        {
            UnityEngine.Object.Destroy(Holder_EnemySelection.transform.GetChild(l).gameObject);
        }
        for (int m = 0; m < Holder_Effects.transform.childCount; m++)
        {
            UnityEngine.Object.Destroy(Holder_Effects.transform.GetChild(m).gameObject);
        }
        BattleActiveEnemies.Clear();
        CurrentBattlePlayerTurnState = BattlePlayerTurnState.ACT;
    }

    public void CheckForValidBattleEnd()
    {
        if (CurrentBattleState == BattleState.Finished || BattleActiveEnemies.Count > 0)
        {
            return;
        }
        CurrentBattleState = BattleState.Finished;
        StopCoroutine("BattlePlayerTurn_RunMoves");
        foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
        {
            if (battlePartyMember.PartyMember_Health <= 0f)
            {
                battlePartyMember.PartyMember_Health = 0f;
                HealPartyMember(battlePartyMember, Mathf.FloorToInt(battlePartyMember.PartyMember_MaxHealth / 12f));
            }
            if (battlePartyMember.PartyMemberInBattle.StartFromPlayer)
            {
                PlayerManager.Instance._PlayerHealth = Mathf.FloorToInt(battlePartyMember.PartyMember_Health);
            }
            else
            {
                battlePartyMember.ActiveMemberInBattle.CurrentHealth = Mathf.FloorToInt(battlePartyMember.PartyMember_Health);
            }
        }
        PartyMembers_AllPlayAnimations("Victory");
        BattleChatbox.StoredAdditiveValues.Clear();
        BattleChatbox.StoredAdditiveValues.Add(CurrentDDTotal.ToString());
        SecurePlayerPrefs.SetSecureInt("TotalCash", SecurePlayerPrefs.GetSecureInt("TotalCash") + Mathf.FloorToInt(CurrentDDTotal));
        if (CurrentPossibleEXP > 0f)
        {
            BattleChatbox.StoredAdditiveValues.Add(" ;你变得更强了。");
        }
        else
        {
            BattleChatbox.StoredAdditiveValues.Add(" ");
        }
        BattleChatbox.AllowInput = true;
        StartCoroutine(BattleEnd_DisplayVictoryDialogue());
    }

    private IEnumerator BattleEnd_DisplayVictoryDialogue()
    {
        yield return null;
        BattleChatbox.TextVisible = true;
        BattleChatbox.RunText(Dialogue_BattleWon, 0, null, ResetCurrentTextIndex: false);
        if (CurrentPossibleEXP > 0f)
        {
            PlayBattleSoundEffect(SFX_StrongerThanYouSansResponse, 0.75f, 2f);
        }
        while (BattleChatbox.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        EndBattle();
    }

    private void Intro_UpdatePartyMembersPositions()
    {
        for (int i = 0; i < BattlePartyMembers.Count; i++)
        {
            Vector2 vector = BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.transform.position;
            Vector2 storedPartyMembePositions = BattlePartyMembers[i].StoredPartyMembePositions;
            Vector2 normalized = (storedPartyMembePositions - vector).normalized;
            float num = 32f;
            Vector2 vector2 = vector + normalized * num * Time.fixedDeltaTime;
            if (Vector2.Distance(vector2, storedPartyMembePositions) < Vector2.Distance(vector, storedPartyMembePositions))
            {
                BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.transform.position = vector2;
                ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = BattlePartyMembers[i].PartyMemberInBattle_AfterImageParticleRenderer.textureSheetAnimation;
                if (textureSheetAnimation.GetSprite(0) != BattlePartyMembers[i].PartyMemberInBattle_MainSpriteRenderer.sprite)
                {
                    textureSheetAnimation.SetSprite(0, BattlePartyMembers[i].PartyMemberInBattle_MainSpriteRenderer.sprite);
                }
                BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.GetComponentInChildren<ParticleSystem>().Play();
            }
            else
            {
                BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.transform.position = storedPartyMembePositions;
                BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }
    }

    private void Intro_ForcefullySetPartyMembersPositions()
    {
        for (int i = 0; i < BattlePartyMembers.Count; i++)
        {
            Vector2 storedPartyMembePositions = BattlePartyMembers[i].StoredPartyMembePositions;
            BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.transform.position = storedPartyMembePositions;
            BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    private void Intro_UpdateEnemyPositions()
    {
        for (int i = 0; i < BattleActiveEnemies.Count; i++)
        {
            Vector2 vector = BattleActiveEnemies[i].EnemyInBattle_Gameobject.transform.position;
            Vector2 storedEnemyPositions = BattleActiveEnemies[i].StoredEnemyPositions;
            Vector2 normalized = (storedEnemyPositions - vector).normalized;
            float num = 32f;
            Vector2 vector2 = vector + normalized * num * Time.fixedDeltaTime;
            if (Vector2.Distance(vector2, storedEnemyPositions) < Vector2.Distance(vector, storedEnemyPositions))
            {
                BattleActiveEnemies[i].EnemyInBattle_Gameobject.transform.position = vector2;
                ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = BattleActiveEnemies[i].EnemyInBattle_AfterImageParticleRenderer.textureSheetAnimation;
                if (textureSheetAnimation.GetSprite(0) != BattleActiveEnemies[i].EnemyInBattle_MainSpriteRenderer.sprite)
                {
                    textureSheetAnimation.SetSprite(0, BattleActiveEnemies[i].EnemyInBattle_MainSpriteRenderer.sprite);
                }
                BattleActiveEnemies[i].EnemyInBattle_AfterImageParticleRenderer.GetComponentInChildren<ParticleSystem>().Play();
            }
            else
            {
                BattleActiveEnemies[i].EnemyInBattle_Gameobject.transform.position = storedEnemyPositions;
                BattleActiveEnemies[i].EnemyInBattle_AfterImageParticleRenderer.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }
    }

    private void Intro_ForcefullySetEnemyPositions()
    {
        for (int i = 0; i < BattleActiveEnemies.Count; i++)
        {
            Vector2 storedEnemyPositions = BattleActiveEnemies[i].StoredEnemyPositions;
            BattleActiveEnemies[i].EnemyInBattle_Gameobject.transform.position = storedEnemyPositions;
            BattleActiveEnemies[i].EnemyInBattle_AfterImageParticleRenderer.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    private bool Intro_AllPartyMembersInPosition()
    {
        for (int i = 0; i < BattlePartyMembers.Count; i++)
        {
            if (Vector2.Distance(BattlePartyMembers[i].PartyMemberInBattle_Gameobjects.transform.position, BattlePartyMembers[i].StoredPartyMembePositions) > 0.1f)
            {
                return false;
            }
        }
        return true;
    }

    private bool Intro_AllEnemiesInPosition()
    {
        for (int i = 0; i < BattleActiveEnemies.Count; i++)
        {
            if (Vector2.Distance(BattleActiveEnemies[i].EnemyInBattle_Gameobject.transform.position, BattleActiveEnemies[i].StoredEnemyPositions) > 0.1f)
            {
                return false;
            }
        }
        return true;
    }

    public void PartyMembers_AllPlayAnimations(string anim, float Time = 0f, bool PreventDownedAnimations = true)
    {
        for (int i = 0; i < BattlePartyMembers.Count; i++)
        {
            if (PreventDownedAnimations && BattlePartyMembers[i].PartyMember_Health <= 0f)
            {
                BattlePartyMembers[i].PartyMemberInBattle_Animator.Play("Down", 0, Time);
            }
            else
            {
                BattlePartyMembers[i].PartyMemberInBattle_Animator.Play(anim, 0, Time);
            }
        }
    }

    public void PartyMembers_MemberPlayAnimation(BattlePartyMember member, string anim, float Time = 0f)
    {
        if (member != null && member.PartyMemberInBattle_Gameobjects != null && member.PartyMemberInBattle_Animator != null)
        {
            member.PartyMemberInBattle_Animator.Play(anim, 0, Time);
        }
    }

    public void ActiveEnemies_PlayAnimation(BattleActiveEnemy target, string anim, float Time = 0f)
    {
        if (target != null && target.EnemyInBattle_Gameobject != null && target.EnemyInBattle_Animator != null)
        {
            target.EnemyInBattle_Animator.Play(anim, 0, Time);
        }
    }

    public void ActiveEnemies_AllPlayAnimation(string anim, float Time = 0f)
    {
        foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
        {
            ActiveEnemies_PlayAnimation(battleActiveEnemy, anim, Time);
        }
    }

    private void SetupPartyMembersInBattle(Battle Battle)
    {
        PartyMember[] partyMembers = Battle.PartyMembers;
        for (int i = 0; i < partyMembers.Length; i++)
        {
            AddPartyMemberToBattle(partyMembers[i], Battle.PartyMemberPositions[i] + (Vector2)base.transform.position);
        }
        CurrentTurnPartyMember = instance.BattlePartyMembers[instance.CurrentPlayerTurnSelectionIndex];
    }

    private void AddPartyMemberToBattle(PartyMember member, Vector2 pos)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(BattlePrefab_PartyMember, Holder_BattlePartyMembers.transform);
        gameObject.name = member.PartyMemberName;
        BattlePartyMember battlePartyMember = new BattlePartyMember();
        Animator componentInChildren = gameObject.GetComponentInChildren<Animator>();
        if (componentInChildren != null)
        {
            componentInChildren.runtimeAnimatorController = member.PartyMemberBattleAnimator;
        }
        else
        {
            Debug.LogWarning("Failed to get Animator in children of Battle Member!");
        }
        if (member.StartFromPlayer)
        {
            gameObject.transform.position = PlayerManager.Instance._PMove._anim.transform.position;
            battlePartyMember.StoredOriginalOverworldPosition = PlayerManager.Instance._PMove._anim.transform.position;
            battlePartyMember.PartyMember_Health = PlayerManager.Instance._PlayerHealth;
            battlePartyMember.PartyMember_MaxHealth = PlayerManager.Instance._PlayerMaxHealth;
        }
        else if (member != null)
        {
            ActivePartyMember activePartyMember = (battlePartyMember.ActiveMemberInBattle = PartyMemberSystem.Instance.HasMemberInParty(member));
            Transform specificMemberTransform = PartyMemberSystem.Instance.GetSpecificMemberTransform(member);
            if (specificMemberTransform != null)
            {
                gameObject.transform.position = specificMemberTransform.position;
                battlePartyMember.StoredOriginalOverworldPosition = activePartyMember.PartyMemberFollowSettings.SusieAnimator.transform.position;
            }
            else
            {
                gameObject.transform.position = PlayerManager.Instance._PMove._anim.transform.position;
                battlePartyMember.StoredOriginalOverworldPosition = PlayerManager.Instance._PMove._anim.transform.position;
            }
            battlePartyMember.PartyMember_Health = activePartyMember.CurrentHealth;
            battlePartyMember.PartyMember_MaxHealth = member.MaximumHealth;
        }
        else
        {
            gameObject.transform.position = PlayerManager.Instance._PMove._anim.transform.position;
            battlePartyMember.StoredOriginalOverworldPosition = PlayerManager.Instance._PMove._anim.transform.position;
            battlePartyMember.PartyMember_Health = 1f;
            battlePartyMember.PartyMember_MaxHealth = 1f;
            Debug.LogError("member in AddPartyMemberToBattle() is null!! WTF!!");
        }
        BattlePartyMembers.Add(battlePartyMember);
        battlePartyMember.PartyMemberInBattle_Gameobjects = gameObject;
        battlePartyMember.PartyMemberInBattle = member;
        battlePartyMember.StoredPartyMembePositions = pos;
        battlePartyMember.PartyMemberInBattle_Animator = componentInChildren;
        Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.name == "Sprite")
            {
                battlePartyMember.PartyMemberInBattle_MainSpriteRenderer = transform.GetComponent<SpriteRenderer>();
            }
        }
        componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform2 in componentsInChildren)
        {
            if (transform2.name == "AfterImage")
            {
                battlePartyMember.PartyMemberInBattle_AfterImageParticleRenderer = transform2.GetComponent<ParticleSystem>();
            }
        }
        DelayForMemberIntro(componentInChildren);
        SetupNewPartyMemberStatus(member, battlePartyMember);
    }

    private void SetupNewPartyMemberStatus(PartyMember member, BattlePartyMember battleMember)
    {
        GameObject obj = UnityEngine.Object.Instantiate(BattlePrefab_PartyMemberStatus, Holder_BattlePartyMemberStatuses.transform);
        BattlePartyMemberStatus component = obj.GetComponent<BattlePartyMemberStatus>();
        obj.name = member.PartyMemberName + "_Status";
        battleMember.PartyMemberStatus = component;
        component.TargetPartyMember = battleMember;
        component.MemberIcon.sprite = member.PartyMemberBattleIcon;
        component.MemberIcon.rectTransform.sizeDelta = new Vector2(member.PartyMemberBattleIcon.textureRect.width * 2f, member.PartyMemberBattleIcon.textureRect.height * 2f);
        component.MemberNameIcon.texture = member.PartyMemberBattle_NameIcon.texture;
        component.MemberNameIcon.rectTransform.sizeDelta = new Vector2(member.PartyMemberBattle_NameIcon.texture.width * 2, member.PartyMemberBattle_NameIcon.texture.height * 2);
        component.BoxOutline.color = member.PartyMemberColor;
        component.HealthBar.color = member.PartyMemberColor;
        component.SpecialLines1.color = member.PartyMemberColor;
        component.SpecialLines2.color = member.PartyMemberColor;
        component.HasActAbility = !member.HasMagic;
        component.UpdateButtonSprites();
    }

    private IEnumerator DelayForMemberIntro(Animator anim)
    {
        yield return new WaitForSeconds(0.01f);
        anim.Play("IntroIdle");
    }

    public void DamagePartyMember(BattlePartyMember Target, int Damage)
    {
        if (Target != null && Target.PartyMember_Health > 0f)
        {
            float partyMember_Health = Target.PartyMember_Health;
            this.Event_OnMemberDamaged(Target, Damage);
            Target.PartyMember_Health -= Damage;
            if (Target.PartyMember_Health <= 0f && partyMember_Health > 0f)
            {
                Target.PartyMember_Health = 0f - Mathf.Ceil(Target.PartyMember_MaxHealth / 2f);
                DisplayBattleNumbers(BattleNumbers_SpecificText, 0, Target.PartyMemberInBattle_Gameobjects.transform.position, Color.red);
                PartyMembers_MemberPlayAnimation(Target, "Down");
            }
            else
            {
                DisplayBattleNumbers(BattleNumbers_Default, Damage, Target.PartyMemberInBattle_Gameobjects.transform.position, Color.white);
                if (!Target.IsDefending)
                {
                    ShakePartyMember(Target, 0.25f, 2f);
                    StartCoroutine(PartyMember_HurtAnimDelay(Target));
                    PartyMembers_MemberPlayAnimation(Target, "Hurt");
                }
                else
                {
                    ShakePartyMember(Target, 0.17f);
                }
            }
            if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX"))
            {
                CutsceneUtils.ShakeTransform(currentMainCamera.transform, 0.125f, 0.25f);
            }
        }
        CheckAllPartyMemberDead();
    }

    private void CheckAllPartyMemberDead()
    {
        bool flag = true;
        foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
        {
            if (battlePartyMember.PartyMember_Health > 0f)
            {
                flag = false;
            }
        }
        if (flag)
        {
            Battle_GameOver.Instance.PossibleDeathMessages = CurrentBattle.GameOverDialogues;
            Battle_GameOver.Instance.PlayGameOver(BattleBox_Soul.transform.position);
            EndBattle(EndBattleTypes.GameOver);
        }
    }

    public void HealPartyMember(BattlePartyMember Target, int HealAmount, bool ForceNumbersOnly = false)
    {
        if (Target == null)
        {
            return;
        }
        float partyMember_Health = Target.PartyMember_Health;
        Target.PartyMember_Health += HealAmount;
        UnityEngine.Object.Instantiate(Effect_Heal, Target.PartyMemberInBattle_Gameobjects.transform.position, Quaternion.identity);
        FlashMemberLight(Target);
        if (Target.PartyMember_Health >= Target.PartyMember_MaxHealth && !ForceNumbersOnly)
        {
            Target.PartyMember_Health = Target.PartyMember_MaxHealth;
            DisplayBattleNumbers(BattleNumbers_SpecificText, 3, Target.PartyMemberInBattle_Gameobjects.transform.position, Color.green);
            if (partyMember_Health <= 0f)
            {
                PartyMembers_MemberPlayAnimation(Target, "Idle");
            }
        }
        else
        {
            DisplayBattleNumbers(BattleNumbers_Default, HealAmount, Target.PartyMemberInBattle_Gameobjects.transform.position, Color.green);
            if (partyMember_Health <= 0f && Target.PartyMember_Health > 0f)
            {
                PartyMembers_MemberPlayAnimation(Target, "Idle");
            }
        }
        PlayBattleSoundEffect(SFX_heal, 0.75f);
    }

    private void PerRound_HealDownedMembers()
    {
        foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
        {
            battlePartyMember.SkippingTurn = false;
            if (battlePartyMember.PartyMember_Health <= 0f)
            {
                HealPartyMember(battlePartyMember, Mathf.CeilToInt(battlePartyMember.PartyMember_MaxHealth / 8f));
            }
        }
    }

    private IEnumerator PartyMember_HurtAnimDelay(BattlePartyMember Target)
    {
        yield return new WaitForSeconds(1f);
        PartyMembers_MemberPlayAnimation(Target, "Idle");
    }

    private void SetupEnemiesInBattle(Battle Battle, Vector2 startPos)
    {
        BattleEnemy[] enemies = Battle.Enemies;
        for (int i = 0; i < enemies.Length; i++)
        {
            AddEnemyToBattle(enemies[i], startPos, Battle.EnemyPositions[i] + (Vector2)base.transform.position);
        }
    }

    private void SetupBattleScript(Battle Battle)
    {
        if (Battle != null && Battle.BattleScriptPrefab != null)
        {
            GameObject gameObject = (CurrentBattleScriptGameobject = UnityEngine.Object.Instantiate(Battle.BattleScriptPrefab));
            CurrentBattleScript = Battle.BattleScriptMainComponent;
            gameObject.name = Battle.BattleScriptPrefab.name;
        }
        else
        {
            Debug.LogError("SetupBattleScript FAILED! Either Battle or BattleScriptPrefab are null!");
        }
    }

    private void AddEnemyToBattle(BattleEnemy enemy, Vector2 startPos, Vector2 endPos)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(enemy.EnemyPrefab, Holder_Enemies.transform);
        gameObject.name = enemy.name;
        gameObject.transform.position = startPos;
        Animator component = gameObject.GetComponent<Animator>();
        BattleActiveEnemy battleActiveEnemy = new BattleActiveEnemy();
        BattleActiveEnemies.Add(battleActiveEnemy);
        battleActiveEnemy.EnemyInBattle_Gameobject = gameObject;
        battleActiveEnemy.EnemyInBattle = enemy;
        battleActiveEnemy.StoredEnemyPositions = endPos;
        battleActiveEnemy.EnemyInBattle_Animator = component;
        CurrentDDTotal += enemy.DarkDollars;
        Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.name == "Sprite")
            {
                battleActiveEnemy.EnemyInBattle_MainSpriteRenderer = transform.GetComponent<SpriteRenderer>();
            }
        }
        componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform2 in componentsInChildren)
        {
            if (transform2.name == "AfterImage")
            {
                battleActiveEnemy.EnemyInBattle_AfterImageParticleRenderer = transform2.GetComponent<ParticleSystem>();
            }
        }
        battleActiveEnemy.Enemy_Health = enemy.EnemyMaxHealth;
        battleActiveEnemy.Enemy_MaxHealth = enemy.EnemyMaxHealth;
        SetupEnemyStatus(enemy, battleActiveEnemy);
    }

    private void SetupEnemyStatus(BattleEnemy enemy, BattleActiveEnemy enemybattle)
    {
        GameObject obj = UnityEngine.Object.Instantiate(BattlePrefab_EnemySelectionStatus, Holder_EnemySelection.transform);
        BattleEnemyStatus component = obj.GetComponent<BattleEnemyStatus>();
        obj.name = enemy.name + "_Status";
        enemybattle.EnemyStatus = component;
        component.ActiveEnemy = enemybattle;
    }

    public void GlowEnemy(BattleActiveEnemy enemy, string SpecilizedAnim = "")
    {
        Enemy_TargetSelectionGlow[] array = UnityEngine.Object.FindObjectsOfType<Enemy_TargetSelectionGlow>();
        for (int i = 0; i < array.Length; i++)
        {
            array[i].CurrentlyTargetted = false;
        }
        Array.Clear(array, 0, array.Length);
        if (enemy == null)
        {
            return;
        }
        array = enemy.EnemyInBattle_Gameobject.GetComponentsInChildren<Enemy_TargetSelectionGlow>();
        for (int j = 0; j < array.Length; j++)
        {
            if (SpecilizedAnim == "")
            {
                array[j].CurrentlyTargetted = true;
            }
            else
            {
                array[j].PlayGlowSpecializedAnimation(SpecilizedAnim);
            }
        }
    }

    public void GlowMember(BattlePartyMember member, string SpecilizedAnim = "")
    {
        Enemy_TargetSelectionGlow[] array = UnityEngine.Object.FindObjectsOfType<Enemy_TargetSelectionGlow>();
        for (int i = 0; i < array.Length; i++)
        {
            array[i].CurrentlyTargetted = false;
        }
        Array.Clear(array, 0, array.Length);
        if (member == null)
        {
            return;
        }
        array = member.PartyMemberInBattle_Gameobjects.GetComponentsInChildren<Enemy_TargetSelectionGlow>();
        for (int j = 0; j < array.Length; j++)
        {
            if (SpecilizedAnim == "")
            {
                array[j].CurrentlyTargetted = true;
            }
            else
            {
                array[j].PlayGlowSpecializedAnimation(SpecilizedAnim);
            }
        }
    }

    public Enemy_TargetSelectionGlow[] GetEnemyGlows(BattleActiveEnemy enemy, string SpecilizedAnim = "")
    {
        return enemy.EnemyInBattle_Gameobject.GetComponentsInChildren<Enemy_TargetSelectionGlow>();
    }

    public void FlashMemberLight(BattlePartyMember member)
    {
        instance.GlowMember(member, "SingleGlow");
    }

    private void RemoveEnemyFromBattle(BattleActiveEnemy target, bool PlayRemoveAnimation = false, BattleEnemyRemoveAnimation animation = BattleEnemyRemoveAnimation.Flee)
    {
        if (target == null || !BattleActiveEnemies.Contains(target))
        {
            return;
        }
        StopCoroutine("ShakeEnemy");
        UnityEngine.Object.Destroy(target.EnemyStatus.gameObject);
        if (!PlayRemoveAnimation)
        {
            UnityEngine.Object.Destroy(target.EnemyInBattle_Gameobject);
        }
        else
        {
            if (animation == BattleEnemyRemoveAnimation.Flee)
            {
                BattleEnemyRemoveAnimation_Flee(target);
            }
            if (animation == BattleEnemyRemoveAnimation.Spare)
            {
                BattleEnemyRemoveAnimation_Spare(target);
            }
        }
        BattleActiveEnemies.Remove(target);
    }

    private void OpenBattleBox(bool DisableSoulEffect = false)
    {
        StartCoroutine(OpenBattleBox_Timed());
        if (BattlePartyMembers.Count > 0)
        {
            if (!DisableSoulEffect)
            {
                UnityEngine.Object.Instantiate(Effect_SoulRipOut).transform.position = BattlePartyMembers[0].StoredPartyMembePositions + new Vector2(0f, 0.6f);
            }
            StartCoroutine(LerpSoulToPosition(BattlePartyMembers[0].StoredPartyMembePositions + new Vector2(0f, 0.6f), BattleBox_Collisions.transform.position, 0.25f));
        }
        else
        {
            BattleBox_Soul.transform.position = BattleBox_Collisions.transform.position;
            BattleBox_Soul.GetComponent<Battle_PlayerSoul>().CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        }
    }

    public void CloseBattleBox(bool DisableSoulEffect = false)
    {
        StartCoroutine(CloseBattleBox_Timed(DisableSoulEffect));
        if (BattlePartyMembers.Count > 0)
        {
            if (BattlePartyMembers[0] != null)
            {
                StartCoroutine(LerpSoulToPosition(Battle_PlayerSoul.Instance.transform.position, BattlePartyMembers[0].StoredPartyMembePositions + new Vector2(0f, 0.6f), 0.25f));
                if (!DisableSoulEffect)
                {
                    UnityEngine.Object.Instantiate(Effect_SoulInsert).transform.position = BattlePartyMembers[0].StoredPartyMembePositions + new Vector2(0f, 0.6f);
                }
            }
            else
            {
                Debug.LogWarning("BattlePartyMembers[0] was null when trying to close battle box and lerp soul");
            }
        }
        else
        {
            BattleBox_Soul.transform.position = BattleBox_Collisions.transform.position;
            BattleBox_Soul.GetComponent<Battle_PlayerSoul>().CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        }
    }

    private IEnumerator LerpSoulToPosition(Vector3 start, Vector3 end, float time)
    {
        float elapsed = 0f;
        BattleBox_Soul.GetComponent<Battle_PlayerSoul>().CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        BattleBox_Soul.SetActive(value: true);
        while (elapsed < time)
        {
            BattleBox_Soul.transform.position = Vector3.Lerp(start, end, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        BattleBox_Soul.transform.position = end;
    }

    private IEnumerator OpenBattleBox_Timed()
    {
        BattleBox_Animator.Play("BattleBox_FadeIn");
        BehindBoxAttackFade_Animator.Play("BehindBoxFade_FadeIn", -1, 0f);
        yield return new WaitForSeconds(0.4f);
        BattleBox_Collisions.SetActive(value: true);
        BattleBox_Soul.GetComponent<Battle_PlayerSoul>().CurrentSoulState = Battle_PlayerSoul.SoulState.Active;
    }

    private IEnumerator CloseBattleBox_Timed(bool DisableBoxEffect = false)
    {
        if (!DisableBoxEffect)
        {
            BattleBox_Animator.Play("BattleBox_FadeOut");
        }
        else
        {
            BattleBox_Animator.Play("BattleBox_Hidden");
        }
        BehindBoxAttackFade_Animator.Play("BehindBoxFade_FadeOut", -1, 0f);
        FadePartyMembersAndEnemies(1f, 0.5f);
        BattleBox_Soul.GetComponent<Battle_PlayerSoul>().CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        yield return new WaitForSeconds(0.4f);
        BattleBox_Collisions.SetActive(value: false);
    }

    private void CreateAllQueuedAttacks()
    {
        float num = 395248f;
        foreach (BattleEnemyAttack queuedBattleAttack in QueuedBattleAttacks)
        {
            if (num == 395248f)
            {
                num = queuedBattleAttack.AttackTime;
            }
            if (queuedBattleAttack.AttackPrefab != null)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(queuedBattleAttack.AttackPrefab, BattleBox.transform.position, Quaternion.identity);
                gameObject.name = queuedBattleAttack.AttackPrefab.name;
                CurrentlyActiveAttacks.Add(gameObject);
            }
            if (queuedBattleAttack.FadeCharactersAway)
            {
                FadePartyMembersAndEnemies(0f, 0.5f);
            }
        }
        if (num != -1f)
        {
            currentAttackTimerCoroutine = StartCoroutine(EnemyAttackTimer(num));
        }
    }

    private IEnumerator EnemyAttackTimer(float AttackLength)
    {
        yield return new WaitForSeconds(AttackLength);
        EndEnemyAttackTurn();
        yield return new WaitForSeconds(0.2f);
        Battle_PlayerSoul.Instance.CurrentSoulState = Battle_PlayerSoul.SoulState.Disabled;
        BattleBox_Soul.SetActive(value: false);
        currentAttackTimerCoroutine = null;
    }

    public void EndEnemyAttackTurn(bool RevertToPlayerTurn = true)
    {
        foreach (GameObject currentlyActiveAttack in CurrentlyActiveAttacks)
        {
            UnityEngine.Object.Destroy(currentlyActiveAttack.gameObject);
        }
        foreach (BattleEnemyAttack queuedBattleAttack in QueuedBattleAttacks)
        {
            if (!queuedBattleAttack.ClearBulletHolderChildren)
            {
                continue;
            }
            Transform[] componentsInChildren = Holder_Bullets.GetComponentsInChildren<Transform>();
            foreach (Transform transform in componentsInChildren)
            {
                if (transform != Holder_Bullets.transform)
                {
                    UnityEngine.Object.Destroy(transform.gameObject);
                }
            }
        }
        CurrentlyActiveAttacks.Clear();
        QueuedBattleAttacks.Clear();
        if (RevertToPlayerTurn)
        {
            CurrentBattleState = BattleState.PlayerTurn;
        }
    }

    public static void OpenBottomBarWindow(BottomBarWindows Window)
    {
        switch (Window)
        {
            case BottomBarWindows.None:
                instance.HideAllBattleWindows();
                BattleChatbox.Instance.TextVisible = true;
                break;
            case BottomBarWindows.Fight_EnemySelection:
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().MenuContext = BattleEnemySelectionMenu.BattleEnemySelectionWindow_Context.Fight;
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().ResetSoulToRestPosition();
                instance.BattleWindow_EnemySelection.SetActive(value: true);
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().currentIndex = 0;
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().UpdateGraphics();
                BattleChatbox.Instance.TextVisible = false;
                break;
            case BottomBarWindows.Fight_AttackBar:
                instance.BattleWindow_AttackBars.SetActive(value: true);
                BattleChatbox.Instance.TextVisible = false;
                break;
            case BottomBarWindows.Item_SelectItem:
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().ResetBattleActItemWindow();
                instance.BattleWindow_ActItemSelection.SetActive(value: true);
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().AddDarkworldInventoryToSelectables();
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().Debounce_AllowInput();
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().UpdateCurrentSelection();
                BattleChatbox.Instance.TextVisible = false;
                break;
            case BottomBarWindows.Act_SelectAction:
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().ResetBattleActItemWindow();
                instance.BattleWindow_ActItemSelection.SetActive(value: true);
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().AddActionsToSelectables(instance.QueuedBattleActions);
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().Debounce_AllowInput();
                instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().UpdateCurrentSelection();
                BattleChatbox.Instance.TextVisible = false;
                break;
            case BottomBarWindows.Act_EnemySelection:
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().MenuContext = BattleEnemySelectionMenu.BattleEnemySelectionWindow_Context.Act;
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().ResetSoulToRestPosition();
                instance.BattleWindow_EnemySelection.SetActive(value: true);
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().currentIndex = 0;
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().UpdateGraphics();
                BattleChatbox.Instance.TextVisible = false;
                break;
            case BottomBarWindows.Act_MemberSelection:
                instance.BattleWindow_PartyMemberSelection.GetComponent<BattleMemberSelectionMenu>().MenuContext = BattleMemberSelectionMenu.BattleMemberSelectionWindow_Context.Act;
                instance.BattleWindow_PartyMemberSelection.SetActive(value: true);
                BattleChatbox.Instance.TextVisible = false;
                break;
            case BottomBarWindows.Item_MemberSelection:
                instance.BattleWindow_PartyMemberSelection.GetComponent<BattleMemberSelectionMenu>().MenuContext = BattleMemberSelectionMenu.BattleMemberSelectionWindow_Context.Item;
                instance.BattleWindow_PartyMemberSelection.SetActive(value: true);
                break;
            case BottomBarWindows.Spare_EnemySelection:
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().MenuContext = BattleEnemySelectionMenu.BattleEnemySelectionWindow_Context.Spare;
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().ResetSoulToRestPosition();
                instance.BattleWindow_EnemySelection.SetActive(value: true);
                instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().UpdateGraphics();
                BattleChatbox.Instance.TextVisible = false;
                break;
        }
    }

    public static void AddNewPlayerMove(BattlePlayerMove move, BattlePartyMember partyMember, int targetEnemy = 0, BattlePartyMember targetMember = null, BattleAction targetAction = null, InventoryItem targetItem = null, int storedItemIndex = 0)
    {
        BattlePartyMemberUse battlePartyMemberUse = new BattlePartyMemberUse();
        battlePartyMemberUse.BattleMove = move;
        battlePartyMemberUse.targetPartyMember = partyMember;
        battlePartyMemberUse.targetMember = targetMember;
        battlePartyMemberUse.Item_ToUse = targetItem;
        battlePartyMemberUse.StoredItemOriginalIndex = storedItemIndex;
        battlePartyMemberUse.Action_ToRun = targetAction;
        if (targetEnemy < 0 || targetEnemy > instance.BattleActiveEnemies.Count)
        {
            battlePartyMemberUse.targetEnemy = null;
        }
        else
        {
            battlePartyMemberUse.targetEnemy = instance.BattleActiveEnemies[targetEnemy];
        }
        if (battlePartyMemberUse.Action_ToRun != null && battlePartyMemberUse.Action_ToRun.TPRequired > 0)
        {
            instance.AddTP(-battlePartyMemberUse.Action_ToRun.TPRequired);
        }
        instance.CurrentPlayerMoves.Add(battlePartyMemberUse);
    }

    private void MembersUseItems(List<BattlePartyMemberUse> Moves)
    {
        StartCoroutine(UseMemberItemsTimed(Moves));
    }

    private IEnumerator UseMemberItemsTimed(List<BattlePartyMemberUse> Moves)
    {
        List<BattlePartyMemberUse> list = new List<BattlePartyMemberUse>();
        foreach (BattlePartyMemberUse Move in Moves)
        {
            if (Move.BattleMove == BattlePlayerMove.Item && Move.Item_ToUse != null)
            {
                MonoBehaviour.print(Move);
                list.Add(Move);
            }
        }
        foreach (BattlePartyMemberUse validmove in list)
        {
            PartyMembers_MemberPlayAnimation(validmove.targetPartyMember, "UseItem");
            OpenBottomBarWindow(BottomBarWindows.None);
            BattleChatbox.Instance.StoredAdditiveValues.Clear();
            BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.targetPartyMember.PartyMemberInBattle.PartyMemberName);
            BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.Item_ToUse.ItemName);
            BattleChatbox.AllowInput = true;
            BattleChatbox.RunText(Dialogue_UseItem, 0, null, ResetCurrentTextIndex: false);
            yield return new WaitForSeconds(0.33f);
            if (validmove.targetMember == null)
            {
                foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
                {
                    HealPartyMember(battlePartyMember, validmove.Item_ToUse.HealthAddition);
                }
            }
            else
            {
                HealPartyMember(validmove.targetMember, validmove.Item_ToUse.HealthAddition);
            }
            while (BattleChatbox.ChatIsCurrentlyRunning)
            {
                yield return null;
            }
        }
        Instance.CurrentBattlePlayerTurnState = BattlePlayerTurnState.FINISHED;
    }

    private void MemberUseActions(List<BattlePartyMemberUse> Moves)
    {
        StartCoroutine(UseMemberActionsTimed(Moves));
    }

    private IEnumerator UseMemberActionsTimed(List<BattlePartyMemberUse> Moves)
    {
        List<BattlePartyMemberUse> list = new List<BattlePartyMemberUse>();
        foreach (BattlePartyMemberUse Move in Moves)
        {
            if (Move.BattleMove == BattlePlayerMove.Action && Move.Action_ToRun != null)
            {
                MonoBehaviour.print(Move);
                list.Add(Move);
            }
        }
        foreach (BattlePartyMemberUse validmove in list)
        {
            instance.CurrentlyRunningAction = true;
            OpenBottomBarWindow(BottomBarWindows.None);
            BattleChatbox.Instance.StoredAdditiveValues.Clear();
            BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.targetPartyMember.PartyMemberInBattle.PartyMemberName);
            switch (validmove.Action_ToRun.ActionTarget)
            {
                case BattleAction.BattleActionTarget.PartyMember:
                    BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.targetMember.PartyMemberInBattle.PartyMemberName);
                    break;
                case BattleAction.BattleActionTarget.Enemy:
                    BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.targetEnemy.EnemyInBattle.EnemyName);
                    break;
            }
            yield return new WaitForSeconds(0.33f);
            if (validmove.Action_ToRun.BattleScript_FunctionToRun.Contains('>'))
            {
                string battleScript_FunctionToRun = validmove.Action_ToRun.BattleScript_FunctionToRun;
                battleScript_FunctionToRun = battleScript_FunctionToRun.Replace(">", "");
                Component component = this;
                component.GetType().GetMethod(battleScript_FunctionToRun).Invoke(component, new object[1] { validmove });
            }
            else
            {
                Component component2 = CurrentBattleScriptGameobject.GetComponent(CurrentBattleScript);
                component2.GetType().GetMethod(validmove.Action_ToRun.BattleScript_FunctionToRun).Invoke(component2, new object[1] { validmove });
            }
            while (instance.CurrentlyRunningAction)
            {
                yield return null;
            }
        }
        Instance.CurrentBattlePlayerTurnState = BattlePlayerTurnState.FINISHED;
    }

    private void MembersSpare(List<BattlePartyMemberUse> Moves)
    {
        StartCoroutine(AllMembersTrySpare(Moves));
    }

    private IEnumerator AllMembersTrySpare(List<BattlePartyMemberUse> Moves)
    {
        List<BattlePartyMemberUse> list = new List<BattlePartyMemberUse>();
        foreach (BattlePartyMemberUse Move in Moves)
        {
            if (Move.BattleMove == BattlePlayerMove.Spare && Move.targetEnemy != null)
            {
                MonoBehaviour.print(Move);
                list.Add(Move);
            }
        }
        foreach (BattlePartyMemberUse validmove in list)
        {
            PartyMembers_MemberPlayAnimation(validmove.targetPartyMember, "Spare");
            OpenBottomBarWindow(BottomBarWindows.None);
            BattleChatbox.Instance.StoredAdditiveValues.Clear();
            BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.targetPartyMember.PartyMemberInBattle.PartyMemberName);
            BattleChatbox.Instance.StoredAdditiveValues.Add(validmove.targetEnemy.EnemyInBattle.EnemyName);
            BattleChatbox.AllowInput = true;
            if (validmove.targetEnemy.Enemy_MercyAmount >= 100f)
            {
                BattleChatbox.RunText(Dialogue_SpareSuccessful, 0, null, ResetCurrentTextIndex: false);
            }
            else if (validmove.targetEnemy.Enemy_IsTired)
            {
                BattleChatbox.RunText(Dialogue_SpareFail_PossiblePacify, 0, null, ResetCurrentTextIndex: false);
            }
            else
            {
                BattleChatbox.RunText(Dialogue_SpareFail, 0, null, ResetCurrentTextIndex: false);
            }
            yield return new WaitForSeconds(0.33f);
            if (validmove.targetEnemy.Enemy_MercyAmount > 100f)
            {
                SpareEnemy(validmove.targetEnemy, 0f, validmove.targetPartyMember, validmove.targetEnemy.Enemy_Spareable);
            }
            else
            {
                SpareEnemy(validmove.targetEnemy, validmove.targetEnemy.EnemyInBattle.DefaultSpareAmount, validmove.targetPartyMember, validmove.targetEnemy.Enemy_Spareable);
            }
            while (BattleChatbox.ChatIsCurrentlyRunning)
            {
                yield return null;
            }
        }
        Instance.CurrentBattlePlayerTurnState = BattlePlayerTurnState.FINISHED;
    }

    public void RefundAllMoveItems()
    {
        if (Instance.CurrentPlayerMoves.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < Instance.CurrentPlayerMoves.Count; i++)
        {
            BattlePartyMemberUse battlePartyMemberUse = Instance.CurrentPlayerMoves[i];
            if (battlePartyMemberUse.Item_ToUse != null && CurrentPlayerTurnSelectionIndex <= i)
            {
                if (battlePartyMemberUse.StoredItemOriginalIndex > -1)
                {
                    DarkworldInventory.Instance.PlayerInventory.Insert(battlePartyMemberUse.StoredItemOriginalIndex, battlePartyMemberUse.Item_ToUse);
                }
                else
                {
                    DarkworldInventory.Instance.PlayerInventory.Add(battlePartyMemberUse.Item_ToUse);
                }
            }
        }
    }

    public static BattlePartyMember GetPartyMember(PartyMember member)
    {
        foreach (BattlePartyMember battlePartyMember in instance.BattlePartyMembers)
        {
            if (battlePartyMember.PartyMemberInBattle == member)
            {
                return battlePartyMember;
            }
        }
        return null;
    }

    private void CreateNewAttackBars(List<BattlePartyMemberUse> Moves)
    {
        BattleWindow_AttackBars.GetComponent<BattleAttackWindow>().ClearAttackBars();
        List<BattlePartyMemberUse> list = new List<BattlePartyMemberUse>();
        foreach (BattlePartyMemberUse Move in Moves)
        {
            if (Move.BattleMove == BattlePlayerMove.Fight && Move.targetPartyMember != null)
            {
                MonoBehaviour.print(Move);
                list.Add(Move);
            }
        }
        foreach (BattlePartyMemberUse item in list)
        {
            BattleAttackBar component = UnityEngine.Object.Instantiate(BattlePrefab_PlayerAttackBar, Holder_AttackBars.transform).GetComponent<BattleAttackBar>();
            component.TargetPartyMember = item.targetPartyMember;
            component.TargetActiveEnemy = item.targetEnemy;
            component.UpdateGraphics();
        }
        OpenBottomBarWindow(BottomBarWindows.Fight_AttackBar);
        BattleWindow_AttackBars.GetComponent<BattleAttackWindow>().SetupAttackBarPositions();
    }

    public void ShakePartyMember(BattlePartyMember target, float multiplier = 1f, float duration = 1f)
    {
        Transform target2 = null;
        if (target != null && target.PartyMemberInBattle_Gameobjects != null)
        {
            target2 = target.PartyMemberInBattle_Gameobjects.transform;
        }
        StartCoroutine(ShakeTarget(target2, multiplier, duration));
    }

    public void ShakeEnemy(BattleActiveEnemy target, float multiplier = 1f, float duration = 1f)
    {
        Transform target2 = null;
        if (target != null && target.EnemyInBattle_Gameobject != null)
        {
            target2 = target.EnemyInBattle_Gameobject.transform;
        }
        StartCoroutine(ShakeTarget(target2, multiplier, duration));
    }

    private IEnumerator ShakeTarget(Transform target, float multiplier = 1f, float duration = 1f)
    {
        if (target != null)
        {
            Vector3 originalPosition = target.position;
            float elapsedTime = 0f;
            while (multiplier > 0f && !(target == null))
            {
                float num = UnityEngine.Random.Range(-1f, 1f) * multiplier;
                target.position = new Vector2(originalPosition.x + num, originalPosition.y);
                elapsedTime += Time.fixedDeltaTime;
                multiplier -= Time.fixedDeltaTime * (1f / duration);
                yield return null;
            }
            if (target != null)
            {
                target.position = originalPosition;
            }
        }
    }

    public void DamageEnemy(BattleActiveEnemy target, float Damage, BattlePartyMember Inflictor, float posOffset = 0.6f, bool TransferRemainingDamage = false)
    {
        StartCoroutine(DamageEnemyAnimated(target, Damage, Inflictor, posOffset, TransferRemainingDamage));
    }

    public void SpareEnemy(BattleActiveEnemy target, float Amount, BattlePartyMember Inflictor, bool wasSpareable)
    {
        StartCoroutine(SpareEnemyAnimated(target, Amount, Inflictor, wasSpareable));
    }

    private BattleActiveEnemy GetNewTarget()
    {
        foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
        {
            if (battleActiveEnemy != null && battleActiveEnemy.EnemyInBattle != null && battleActiveEnemy.Enemy_Health > 0f)
            {
                return battleActiveEnemy;
            }
        }
        return null;
    }

    private IEnumerator DamageEnemyAnimated(BattleActiveEnemy target, float Damage, BattlePartyMember Inflictor, float posOffset = 0.6f, bool TransferRemainingDamage = false)
    {
        OnEnemyDamaged(target, Damage, Inflictor);
        if (target == null || !target.Enemy_ConsideredInBattle)
        {
            target = GetNewTarget();
            if (target == null || !target.Enemy_ConsideredInBattle)
            {
                MonoBehaviour.print("null");
                yield break;
            }
        }
        if (target.Enemy_Health - Damage <= 0f)
        {
            OnEnemyKilled(target, Damage, Inflictor);
            PlayBattleSoundEffect(SFX_enemy_damage, 0.5f);
            float num = Mathf.Max(Damage - target.Enemy_Health, 0f);
            if (target.EnemyInBattle_Gameobject != null)
            {
                Vector3 vector = target.EnemyInBattle_Gameobject.transform.position + new Vector3(0f, posOffset * (float)EnemyDamageBattleNumberOffset, 0f);
                DisplayBattleNumbers(BattleNumbers_SpecificText, 6, vector + new Vector3(0f, posOffset * (float)EnemyDamageBattleNumberOffset, 0f), Color.white, FlippedAnimation: true);
                DisplayBattleNumbers(BattleNumbers_Default, Mathf.CeilToInt(Damage - num), vector, Inflictor.PartyMemberInBattle.PartyMemberColor_Highlighted, FlippedAnimation: true);
                EnemyDamageBattleNumberOffset++;
            }
            CurrentPossibleEXP += target.EnemyInBattle.EXP;
            RemoveEnemyFromBattle(target, PlayRemoveAnimation: true);
            if (TransferRemainingDamage)
            {
                target = GetNewTarget();
                if (target != null)
                {
                    DamageEnemy(target, num, Inflictor);
                }
            }
            yield break;
        }
        if (target != null && target.EnemyInBattle_Animator != null)
        {
            if (Mathf.CeilToInt(Damage) <= 0)
            {
                DisplayBattleNumbers(BattleNumbers_SpecificText, 2, target.EnemyInBattle_Gameobject.transform.position + new Vector3(0f, posOffset * (float)EnemyDamageBattleNumberOffset, 0f), Inflictor.PartyMemberInBattle.PartyMemberColor_Highlighted, FlippedAnimation: true);
            }
            else
            {
                target.Enemy_Health -= Damage;
                target.Enemy_Health = Math.Clamp(target.Enemy_Health, 0f, target.Enemy_MaxHealth);
                PlayBattleSoundEffect(SFX_enemy_damage, 0.5f);
                target.EnemyInBattle_Animator.Play("Hurt", -1, 0f);
                if (target.Enemy_Health > 0f)
                {
                    ShakeEnemy(target, 0.25f, 2f);
                }
                DisplayBattleNumbers(BattleNumbers_Default, Mathf.CeilToInt(Damage), target.EnemyInBattle_Gameobject.transform.position + new Vector3(0f, posOffset * (float)EnemyDamageBattleNumberOffset, 0f), Inflictor.PartyMemberInBattle.PartyMemberColor_Highlighted, FlippedAnimation: true);
            }
            EnemyDamageBattleNumberOffset++;
        }
        yield return new WaitForSeconds(1f);
        if (target != null && target.EnemyInBattle_Animator != null)
        {
            if (target.Enemy_MercyAmount < 100f)
            {
                target.EnemyInBattle_Animator.Play("Idle");
            }
            else
            {
                target.EnemyInBattle_Animator.Play("Spare");
            }
        }
    }

    private IEnumerator SpareEnemyAnimated(BattleActiveEnemy target, float amount, BattlePartyMember inflictor, bool enemyWasSpareable)
    {
        if (target == null || target.Enemy_Health <= 0f || target.EnemyInBattle_Gameobject == null || !target.Enemy_ConsideredInBattle)
        {
            target = GetNewTarget();
            if (target == null)
            {
                Debug.LogWarning("No new targets available to spare?");
                yield break;
            }
        }
        _ = target.Enemy_MercyAmount;
        float newMercyAmount = Mathf.Clamp(target.Enemy_MercyAmount + amount, 0f, 100f);
        bool willBeSpareable = newMercyAmount >= 100f;
        if (willBeSpareable && !enemyWasSpareable)
        {
            target.EnemyInBattle_Animator.Play("Spare");
            target.Enemy_Spareable = true;
        }
        yield return null;
        OnEnemySpared(target, willBeSpareable, inflictor);
        target.Enemy_MercyAmount = newMercyAmount;
        PlayBattleSoundEffect(sfx_enemy_spareblink, 0.5f);
        if (newMercyAmount >= 100f)
        {
            if (!enemyWasSpareable)
            {
                if (target.EnemyInBattle_Gameobject != null)
                {
                    DisplayBattleNumbers(BattleNumbers_SpecificText, 4, target.EnemyInBattle_Gameobject.transform.position + new Vector3(-1f, 0.6f * (float)EnemyDamageBattleNumberOffset, 0f), Color.white, FlippedAnimation: true);
                }
            }
            else
            {
                RemoveEnemyFromBattle(target, PlayRemoveAnimation: true, BattleEnemyRemoveAnimation.Spare);
            }
        }
        else if (target.EnemyInBattle_Gameobject != null)
        {
            GlowEnemy(target, "FailedSpare");
            DisplayBattleNumbers(BattleNumbers_Golden, Mathf.CeilToInt(amount), target.EnemyInBattle_Gameobject.transform.position + new Vector3(-2f, 0.6f * (float)EnemyDamageBattleNumberOffset, 0f), Color.white, FlippedAnimation: true, IncludePercentSprite: true, IncludePlusSprite: true);
        }
    }

    public void DisplayBattleNumbers(BattleNumberSprites Numbers, int NumberToDisplay, Vector2 PositionToDisplay, Color NumberColors, bool FlippedAnimation = false, bool IncludePercentSprite = false, bool IncludePlusSprite = false)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(BattlePrefab_BattleNumbers, Holder_Effects.transform);
        gameObject.transform.position = PositionToDisplay;
        if (FlippedAnimation)
        {
            gameObject.GetComponent<Animator>().Play("BattleEffect_NumberPopIn_Flipped");
        }
        string text = NumberToDisplay.ToString();
        float num = Numbers.NumberSprites[0].textureRect.width / Numbers.NumberSprites[0].pixelsPerUnit;
        Vector2 vector = PositionToDisplay;
        Vector2 vector2 = Vector2.zero;
        if (IncludePlusSprite)
        {
            GameObject obj = new GameObject("Digit_Plus");
            obj.transform.SetParent(gameObject.transform);
            obj.transform.position = PositionToDisplay + new Vector2(num, 0f);
            vector += new Vector2(num * 2f, 0f);
            SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Numbers.NumberSprites[12];
            spriteRenderer.sortingLayerName = "Battle_AboveBox";
            spriteRenderer.color = NumberColors;
        }
        for (int i = 0; i < text.Length; i++)
        {
            GameObject obj2 = new GameObject("Digit_" + text[i]);
            obj2.transform.SetParent(gameObject.transform);
            vector2 = new Vector2(vector.x + (float)i * num, vector.y);
            obj2.transform.position = vector2;
            SpriteRenderer spriteRenderer2 = obj2.AddComponent<SpriteRenderer>();
            if (text[i] == '-')
            {
                spriteRenderer2.sprite = Numbers.NumberSprites[10];
            }
            else
            {
                int num2 = int.Parse(text[i].ToString());
                spriteRenderer2.sprite = Numbers.NumberSprites[num2];
            }
            spriteRenderer2.sortingLayerName = "Battle_AboveBox";
            spriteRenderer2.color = NumberColors;
        }
        if (IncludePercentSprite)
        {
            GameObject obj3 = new GameObject("Digit_Percentage");
            obj3.transform.SetParent(gameObject.transform);
            obj3.transform.position = vector2 + new Vector2(num, 0f);
            SpriteRenderer spriteRenderer3 = obj3.AddComponent<SpriteRenderer>();
            spriteRenderer3.sprite = Numbers.NumberSprites[11];
            spriteRenderer3.sortingLayerName = "Battle_AboveBox";
            spriteRenderer3.color = NumberColors;
        }
    }

    public BattleBubbleChatbox NewDialogueForEnemy(BattleActiveEnemy targetEnemy, CHATBOXTEXT text, int TextIndex, int AdditionalTextIndex, GameObject DialogueBubblePrefab)
    {
        GameObject obj = UnityEngine.Object.Instantiate(DialogueBubblePrefab);
        BattleBubbleChatbox component = obj.GetComponent<BattleBubbleChatbox>();
        obj.transform.position = targetEnemy.EnemyInBattle_Gameobject.transform.position + new Vector3(-1.25f, 0.75f);
        component.RunText(text, TextIndex, AdditionalTextIndex);
        return component;
    }

    private void FadePartyMembersAndEnemies(float targetOpacity, float duration)
    {
        foreach (BattlePartyMember battlePartyMember in BattlePartyMembers)
        {
            if (battlePartyMember.PartyMemberInBattle_MainSpriteRenderer != null)
            {
                StartCoroutine(FadeSpriteRenderer(battlePartyMember.PartyMemberInBattle_MainSpriteRenderer, targetOpacity, duration));
            }
        }
        foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
        {
            if (battleActiveEnemy.EnemyInBattle_Gameobject != null)
            {
                SpriteRenderer[] componentsInChildren = battleActiveEnemy.EnemyInBattle_Gameobject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer spriteRenderer in componentsInChildren)
                {
                    StartCoroutine(FadeSpriteRenderer(spriteRenderer, targetOpacity, duration));
                }
            }
        }
    }

    private IEnumerator FadeSpriteRenderer(SpriteRenderer spriteRenderer, float targetOpacity, float duration)
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            float startOpacity = originalColor.a;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float a = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / duration);
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);
                }
                yield return null;
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetOpacity);
            }
        }
        else
        {
            Debug.LogWarning("FadeSpriteRenderer Failed | spriteRenderer is missing or null?");
        }
    }

    public void DefaultAct_Check(BattlePartyMemberUse action)
    {
        Instance.PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultAct");
        CHATBOXTEXT enemyCheckText = action.targetEnemy.EnemyInBattle.EnemyCheckText;
        BattleChatbox.AllowInput = true;
        BattleChatbox.RunText(enemyCheckText, 0, null, ResetCurrentTextIndex: false);
    }

    public void BattleEnemyRemoveAnimation_Flee(BattleActiveEnemy target)
    {
        StartCoroutine(BattleEnemyRemoveAnimation_Flee_Coroutine(target));
    }

    private IEnumerator BattleEnemyRemoveAnimation_Flee_Coroutine(BattleActiveEnemy target)
    {
        target.Enemy_ConsideredInBattle = false;
        ParticleSystem enemyAfterImage = target.EnemyInBattle_AfterImageParticleRenderer.GetComponentInChildren<ParticleSystem>();
        GameObject enemyGameobject = target.EnemyInBattle_Gameobject;
        enemyGameobject.transform.Find("SweatDroplet").GetComponent<Animator>().Play("BattleEnemy_Sweat");
        PlayBattleSoundEffect(SFX_EnemyFlee, 1.5f);
        ActiveEnemies_PlayAnimation(target, "Hurt");
        yield return new WaitForSeconds(0.5f);
        Vector3 TargetPosition = enemyGameobject.transform.position + new Vector3(15f, 0f, 0f);
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = enemyAfterImage.textureSheetAnimation;
        ParticleSystem.EmissionModule emission = enemyAfterImage.emission;
        emission.rateOverTimeMultiplier = 120f;
        textureSheetAnimation.SetSprite(0, target.EnemyInBattle_MainSpriteRenderer.sprite);
        enemyAfterImage.Play();
        UnityEngine.Object.Destroy(enemyGameobject, 3f);
        while (enemyGameobject != null && enemyGameobject.transform.position != TargetPosition)
        {
            yield return null;
            if (enemyGameobject != null)
            {
                enemyGameobject.transform.position = Vector3.Lerp(enemyGameobject.transform.position, TargetPosition, 2f * Time.deltaTime);
            }
        }
    }

    public void BattleEnemyRemoveAnimation_Spare(BattleActiveEnemy target)
    {
        StartCoroutine(BattleEnemyRemoveAnimation_Spare_Coroutine(target));
    }

    private IEnumerator BattleEnemyRemoveAnimation_Spare_Coroutine(BattleActiveEnemy target)
    {
        target.Enemy_ConsideredInBattle = false;
        ParticleSystem particleSystem = null;
        ParticleSystem[] componentsInChildren = target.EnemyInBattle_AfterImageParticleRenderer.gameObject.transform.parent.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem2 in componentsInChildren)
        {
            _ = particleSystem2.name == "AfterImage";
            if (particleSystem2.name == "AfterImage_Spared")
            {
                particleSystem = particleSystem2;
            }
        }
        GameObject enemyGameobject = target.EnemyInBattle_Gameobject;
        UnityEngine.Object.Instantiate(Effect_Spare, target.EnemyInBattle_Gameobject.transform.position, Quaternion.identity);
        GlowEnemy(target, "Spared");
        PlayBattleSoundEffect(sfx_enemy_spare, 1.5f);
        ActiveEnemies_PlayAnimation(target, "Spare");
        Vector3 TargetPosition = enemyGameobject.transform.position + new Vector3(15f, 0f, 0f);
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = particleSystem.textureSheetAnimation;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.rateOverTimeMultiplier = 120f;
        textureSheetAnimation.SetSprite(0, target.EnemyInBattle_MainSpriteRenderer.sprite);
        particleSystem.Play();
        UnityEngine.Object.Destroy(enemyGameobject, 3f);
        while (enemyGameobject != null && enemyGameobject.transform.position != TargetPosition)
        {
            yield return null;
            if (enemyGameobject != null)
            {
                enemyGameobject.transform.position = Vector3.Lerp(enemyGameobject.transform.position, TargetPosition, 0.5f * Time.deltaTime);
            }
        }
    }

    public void Battle_FinishRunningActions()
    {
        CurrentlyRunningAction = false;
    }

    private void ResetAllBattleEvents()
    {
        PlayerturnCount = 0;
        EnemyturnCount = 0;
        TotalturnCount = 0;
    }

    private void OnPlayerTurn()
    {
        PlayerturnCount++;
        TotalturnCount++;
        this.Event_OnPlayerTurn?.Invoke(PlayerturnCount);
    }

    private void OnPlayerUseRound()
    {
        this.Event_OnPlayerUseRound?.Invoke(CurrentPlayerMoves);
    }

    private void OnEnemyAttackTurn()
    {
        EnemyturnCount++;
        this.Event_OnEnemyAttackTurn?.Invoke(EnemyturnCount);
    }

    private void OnBattleStart(Battle Battle)
    {
        MonoBehaviour.print("Battle has begun");
        this.Event_OnBattleStart?.Invoke(CurrentBattle);
    }

    private void OnBattleEnd(Battle Battle, EndBattleTypes EndType)
    {
        this.Event_OnBattleEnd?.Invoke(Battle, EndType);
    }

    private void OnBattleStateChange(BattleState NewState, BattleState OldState)
    {
        this.Event_OnBattleStateChange?.Invoke(NewState, OldState);
    }

    private void OnMemberDamaged(BattlePartyMember targetMember, float damage)
    {
        this.Event_OnMemberDamaged?.Invoke(targetMember, damage);
    }

    private void OnEnemyDamaged(BattleActiveEnemy targetEnemy, float damage, BattlePartyMember inflictor)
    {
        this.Event_OnEnemyDamaged?.Invoke(targetEnemy, damage, inflictor);
    }

    private void OnEnemyKilled(BattleActiveEnemy targetEnemy, float damage, BattlePartyMember inflictor)
    {
        this.Event_OnEnemyKilled?.Invoke(targetEnemy, damage, inflictor);
    }

    private void OnEnemySpared(BattleActiveEnemy targetEnemy, bool wasSpareable, BattlePartyMember inflictor)
    {
        this.Event_OnEnemySpared?.Invoke(targetEnemy, wasSpareable, inflictor);
    }

    public void DefaultSpell_HealPrayer(BattlePartyMemberUse action)
    {
        if (action != null && action.targetMember != null)
        {
            PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultSpell");
            StartCoroutine(DefaultSpell_HealPrayer_Delay(action.targetMember, action.targetPartyMember));
        }
    }

    private IEnumerator DefaultSpell_HealPrayer_Delay(BattlePartyMember target, BattlePartyMember inflictor)
    {
        yield return null;
        HealPartyMember(target, GetPartyMember(inflictor.PartyMemberInBattle).PartyMemberInBattle.MAGIC * 5);
        BattleChatbox.AllowInput = false;
        BattleChatbox.RunText(Dialogue_DefaultSpell_HealPrayer, 0, null, ResetCurrentTextIndex: false);
        yield return new WaitForSeconds(0.25f);
        BattleChatbox.AllowInput = true;
    }

    public void DefaultSpell_IceShock(BattlePartyMemberUse action)
    {
        if (action != null && action.targetEnemy != null)
        {
            PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultSpell");
            StartCoroutine(DefaultSpell_IceShock_Delay(action.targetEnemy, action.targetPartyMember));
        }
    }

    private IEnumerator DefaultSpell_IceShock_Delay(BattleActiveEnemy target, BattlePartyMember inflictor)
    {
        yield return null;
        BattleChatbox.AllowInput = false;
        BattleChatbox.RunText(Dialogue_DefaultSpell_IceShock, 0, null, ResetCurrentTextIndex: false);
        PlayBattleSoundEffect(SFX_IceShock);
        Vector3[] iceshock_offests = new Vector3[3]
        {
            new Vector3(-3f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(-2f, -1f, 0f)
        };
        List<Animator> iceshockAnimators = new List<Animator>();
        for (int i = 0; i < 3; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Effect_IceShock_Individual, Holder_Effects.transform);
            gameObject.transform.position = target.EnemyInBattle_Gameobject.transform.position + Vector3.one * 1.6f;
            gameObject.transform.position += iceshock_offests[i];
            iceshockAnimators.Add(gameObject.GetComponentInChildren<Animator>());
            yield return new WaitForSeconds(0.1f);
        }
        foreach (Animator item in iceshockAnimators)
        {
            if (item != null)
            {
                item.Play("Explode");
            }
        }
        float damage = 30 * (inflictor.PartyMemberInBattle.MAGIC - 10) + 90 + UnityEngine.Random.Range(1, 10);
        DamageEnemy(target, damage, inflictor);
        yield return new WaitForSeconds(0.3f);
        foreach (Animator item2 in iceshockAnimators)
        {
            UnityEngine.Object.Destroy(item2.transform.parent.gameObject);
        }
        iceshockAnimators.Clear();
        BattleChatbox.AllowInput = true;
    }

    public void DefaultSpell_Pacify(BattlePartyMemberUse action)
    {
        if (action != null && action.targetEnemy != null)
        {
            PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultSpell");
            StartCoroutine(DefaultSpell_Pacify_Delay(action.targetEnemy, action.targetPartyMember));
        }
    }

    private IEnumerator DefaultSpell_Pacify_Delay(BattleActiveEnemy target, BattlePartyMember inflictor)
    {
        yield return new WaitForSeconds(0.25f);
        GlowEnemy(target, "FailedPacify");
        BattleChatbox.AllowInput = false;
        BattleChatbox.RunText(Dialogue_DefaultSpell_FailedPacify, 0, null, ResetCurrentTextIndex: false);
        yield return new WaitForSeconds(0.25f);
        BattleChatbox.AllowInput = true;
    }

    public void DefaultSpell_SleepMist(BattlePartyMemberUse action)
    {
        if (action != null && action.targetEnemy != null)
        {
            PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultSpell");
            StartCoroutine(DefaultSpell_SleepMist_Delay(action.targetPartyMember));
        }
    }

    private IEnumerator DefaultSpell_SleepMist_Delay(BattlePartyMember inflictor)
    {
        yield return new WaitForSeconds(0.25f);
        foreach (BattleActiveEnemy battleActiveEnemy in BattleActiveEnemies)
        {
            UnityEngine.Object.Instantiate(Effect_SleepMist, battleActiveEnemy.EnemyInBattle_Gameobject.transform.position + new Vector3(0f, 0.4f, 0f), Quaternion.identity).transform.parent = Holder_Effects.transform;
        }
        PlayBattleSoundEffect(SFX_SleepMist);
        BattleChatbox.AllowInput = false;
        BattleChatbox.RunText(Dialogue_DefaultSpell_SleepMist, 0, null, ResetCurrentTextIndex: false);
        yield return new WaitForSeconds(0.25f);
        BattleChatbox.AllowInput = true;
    }

    public void SusieSpell_UltimatHeal(BattlePartyMemberUse action)
    {
        if (action != null && action.targetMember != null)
        {
            PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultSpell");
            StartCoroutine(SusieSpell_UltimateHeal_Delay(action.targetMember, action.targetPartyMember));
        }
    }

    private IEnumerator SusieSpell_UltimateHeal_Delay(BattlePartyMember target, BattlePartyMember inflictor)
    {
        yield return null;
        HealPartyMember(target, GetPartyMember(inflictor.PartyMemberInBattle).PartyMemberInBattle.MAGIC + 1, ForceNumbersOnly: true);
        BattleChatbox.AllowInput = false;
        BattleChatbox.RunText(Dialogue_DefaultSpell_UltimatHeal, 0, null, ResetCurrentTextIndex: false);
        yield return new WaitForSeconds(0.25f);
        BattleChatbox.AllowInput = true;
    }

    public void SusieSpell_RudeBuster(BattlePartyMemberUse action)
    {
        if (action != null && action.targetEnemy != null)
        {
            PartyMembers_MemberPlayAnimation(action.targetPartyMember, "DefaultSpell");
            StartCoroutine(DefaultSpell_RudeBuster_Delay(action.targetEnemy, action.targetPartyMember));
        }
    }

    private IEnumerator DefaultSpell_RudeBuster_Delay(BattleActiveEnemy target, BattlePartyMember inflictor)
    {
        BattleChatbox.AllowInput = false;
        yield return new WaitForSeconds(0.5f);
        BattleChatbox.RunText(Dialogue_DefaultSpell_RudeBuster, 0, null, ResetCurrentTextIndex: false);
        yield return new WaitForSeconds(0.1f);
        PartyMembers_MemberPlayAnimation(inflictor, "Spell_RudeBuster");
        yield return new WaitForSeconds(0.517f);
        PlayBattleSoundEffect(SFX_RudeBuster_Fire);
        GameObject RudeBusterProjectile = UnityEngine.Object.Instantiate(Effect_RudeBuster_Projectile, inflictor.PartyMemberInBattle_Gameobjects.transform.position + Vector3.up * 0.4f, Quaternion.identity);
        RudeBusterProjectile.transform.parent = Holder_Effects.transform;
        bool ProjectileCurrentlyActive = true;
        bool DealAdditionalDamage = false;
        CutsceneUtils.MoveTransformOnArc(RudeBusterProjectile.transform, target.EnemyInBattle_Gameobject.transform.position + Vector3.up * 0.4f, -1f, 0.433f, rotateAlongArc: true);
        while (ProjectileCurrentlyActive)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
            {
                DealAdditionalDamage = true;
            }
            if (RudeBusterProjectile.transform.position == target.EnemyInBattle_Gameobject.transform.position + Vector3.up * 0.4f)
            {
                ProjectileCurrentlyActive = false;
            }
            yield return null;
        }
        RudeBusterProjectile.GetComponentInChildren<ParticleSystem>().transform.parent = null;
        UnityEngine.Object.Destroy(RudeBusterProjectile);
        float num = 11 * inflictor.PartyMemberInBattle.ATK + 5 * inflictor.PartyMemberInBattle.MAGIC - 3 * target.EnemyInBattle.DF;
        if (DealAdditionalDamage)
        {
            num += 30f;
        }
        UnityEngine.Object.Instantiate(Effect_RudeBuster_Explosion, target.EnemyInBattle_Gameobject.transform.position + Vector3.up * 0.4f, Quaternion.identity).transform.parent = Holder_Effects.transform;
        PlayBattleSoundEffect(SFX_RudeBuster_Hit);
        DamageEnemy(target, num, inflictor);
        BattleChatbox.AllowInput = true;
    }
}
