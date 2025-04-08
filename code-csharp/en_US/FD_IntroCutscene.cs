using System.Collections;
using UnityEngine;

public class FD_IntroCutscene : MonoBehaviour
{
    [Header("- References -")]
    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Noelle;

    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private CameraManager PlayerCamera;

    [SerializeField]
    private GameObject Default_CameraTrigger;

    [SerializeField]
    private SpriteRenderer JuiceTable;

    [SerializeField]
    private SpriteRenderer JuiceBowl;

    [SerializeField]
    private GameObject SiplettCup;

    [SerializeField]
    private Animator Effect_Splash;

    [SerializeField]
    private Sprite JuiceTable_Mess;

    [SerializeField]
    private Sprite JuiceBowl_Mess;

    public FD_Intro_SiplettArc SiplettThrown;

    [SerializeField]
    private OverworldEnemy_Interaction SiplettBattleTrigger;

    [SerializeField]
    private GameObject Effect_CircleZoom;

    [SerializeField]
    private SpriteRenderer IceeCutout;

    [SerializeField]
    private Sprite Icee_Wink;

    [SerializeField]
    private ParticleSystem Icee_WinkParticle;

    [SerializeField]
    private HypothesisGoal Goal_FightPath;

    [SerializeField]
    private HypothesisGoal Goal_MercyPath;

    public HypothesisGoal Goal_NoHit;

    [Header("- Walk Positions -")]
    [SerializeField]
    private Vector2 WalkPos_Kris_ToDrinkSign;

    [SerializeField]
    private Vector2 WalkPos_Kris_SetupForDrinkPos;

    [SerializeField]
    private Vector2 WalkPos_Kris_DrinkTable;

    [SerializeField]
    private Vector2 WalkPos_Susie_DrinkTable;

    [SerializeField]
    private Vector2 WalkPos_Noelle_DrinkTable;

    [SerializeField]
    private Vector2 WalkPos_Susie_TowardsICEE;

    [SerializeField]
    private Vector2 WalkPos_Susie_TowardsCups;

    [Header("- Dialogue -")]
    [SerializeField]
    private CHATBOXTEXT Dialogue_NoelleSpotSign;

    [SerializeField]
    private CHATBOXTEXT Dialogue_FruitPunchTable;

    [SerializeField]
    private CHATBOXTEXT Dialogue_SusieApologize;

    [SerializeField]
    private CHATBOXTEXT Dialogue_SpotSiplett;

    [SerializeField]
    private CHATBOXTEXT Dialogue_Outro;

    [SerializeField]
    private CHATBOXTEXT Dialogue_Outro_IRememberYoureGenocides;

    [Header("- Sounds -")]
    [SerializeField]
    private AudioClip SFX_Noise;

    [SerializeField]
    private AudioClip SFX_Splash;

    [SerializeField]
    private AudioClip SFX_Throw;

    [SerializeField]
    private AudioClip SFX_NoelleScaredShort;

    [SerializeField]
    private AudioClip SFX_IceeWink;

    [Header("- Cutscene Info -")]
    [SerializeField]
    private int CutsceneIndex;

    [SerializeField]
    private Vector3 storedNoelleOriginalPos;

    private bool ViolenceUsed;

    private void Start()
    {
        StartCoroutine(IntroCutscene());
    }

    public void StartEndingCutscene(bool violenceUsed = false)
    {
        ViolenceUsed = violenceUsed;
        StartCoroutine(EndingCutscene());
    }

    private IEnumerator IntroCutscene()
    {
        yield return null;
        yield return null;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Noelle = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_NoelleDarkworld).PartyMemberFollowSettings;
        Kris = PlayerManager.Instance;
        PlayerCamera = CameraManager.instance;
        Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
        Kris._PMove.AllowSprint = false;
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        DarkworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.CanOpenMenu = false;
        Susie.delay /= 1.5f;
        Susie.AnimationOverriden = true;
        Noelle.delay /= 1.5f;
        Noelle.AnimationOverriden = true;
        Susie.SusieAnimator.SetBool("InCutscene", value: true);
        Noelle.SusieAnimator.SetBool("InCutscene", value: true);
        Susie.RotateSusieToDirection(Vector2.right);
        Susie.SusieAnimator.Play("Walk");
        Noelle.RotateSusieToDirection(Vector2.right);
        Noelle.SusieAnimator.Play("Walk");
        while ((Vector2)Kris.transform.position != WalkPos_Kris_ToDrinkSign)
        {
            yield return null;
            Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, WalkPos_Kris_ToDrinkSign, 4f * Time.deltaTime);
            Kris._PMove._anim.SetBool("MOVING", value: true);
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Susie.FollowingEnabled = false;
        Noelle.FollowingEnabled = false;
        Kris._PMove.RotatePlayerAnim(Vector2.left);
        Susie.RotateSusieToDirection(Vector2.left);
        Susie.SusieAnimator.Play("Idle");
        Noelle.RotateSusieToDirection(Vector2.up);
        Noelle.SusieAnimator.Play("Idle");
        CutsceneUtils.RunFreshChat(Dialogue_NoelleSpotSign, 0, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex < 1)
        {
            yield return null;
        }
        Susie.FollowingEnabled = true;
        Susie.currentState = Susie_Follower.MemberFollowerStates.CopyingInputs;
        Noelle.FollowingEnabled = true;
        Noelle.currentState = Susie_Follower.MemberFollowerStates.CopyingInputs;
        Susie.RotateSusieToDirection(Vector2.right);
        Susie.SusieAnimator.Play("Walk");
        Noelle.RotateSusieToDirection(Vector2.right);
        Noelle.SusieAnimator.Play("Walk");
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        Default_CameraTrigger.SetActive(value: false);
        while ((Vector2)Kris.transform.position != WalkPos_Kris_SetupForDrinkPos)
        {
            yield return null;
            Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, WalkPos_Kris_SetupForDrinkPos, 4f * Time.deltaTime);
            Kris._PMove._anim.SetBool("MOVING", value: true);
            Susie.RotateSusieToDirection(Vector2.right);
            Noelle.RotateSusieToDirection(Vector2.right);
            if (PlayerCamera.transform.position.x != 0f)
            {
                Vector3 target = new Vector3(0f, 0f, -10f);
                PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, target, 3.5f * Time.deltaTime);
                PlayerCamera.FollowPlayerX = false;
            }
        }
        Susie.FollowingEnabled = false;
        Noelle.FollowingEnabled = false;
        Kris._PMove.RotatePlayerAnim(Vector2.down);
        Susie.RotateSusieToDirection(Vector2.down);
        Noelle.RotateSusieToDirection(Vector2.down);
        Susie.SusieAnimator.SetBool("InCutscene", value: true);
        Noelle.SusieAnimator.SetBool("InCutscene", value: true);
        while ((Vector2)Kris.transform.position != WalkPos_Kris_DrinkTable || (Vector2)Susie.transform.position != WalkPos_Susie_DrinkTable || (Vector2)Noelle.transform.position != WalkPos_Noelle_DrinkTable)
        {
            yield return null;
            if ((Vector2)Kris.transform.position != WalkPos_Kris_DrinkTable)
            {
                Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, WalkPos_Kris_DrinkTable, 3f * Time.deltaTime);
                Kris._PMove._anim.SetBool("MOVING", value: true);
            }
            else
            {
                Kris._PMove._anim.SetBool("MOVING", value: false);
                Kris._PMove.RotatePlayerAnim(Vector2.left);
            }
            if ((Vector2)Susie.transform.position != WalkPos_Susie_DrinkTable)
            {
                Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, WalkPos_Susie_DrinkTable, 3f * Time.deltaTime);
                Susie.SusieAnimator.Play("Walk");
            }
            else
            {
                Susie.SusieAnimator.Play("Idle");
                Susie.RotateSusieToDirection(Vector2.down);
            }
            if ((Vector2)Noelle.transform.position != WalkPos_Noelle_DrinkTable)
            {
                Noelle.transform.position = Vector2.MoveTowards(Noelle.transform.position, WalkPos_Noelle_DrinkTable, 3f * Time.deltaTime);
                Noelle.SusieAnimator.Play("Walk");
            }
            else
            {
                Noelle.SusieAnimator.Play("Idle");
                Noelle.RotateSusieToDirection(Vector2.right);
            }
        }
        Noelle.SusieAnimator.Play("Idle");
        Noelle.RotateSusieToDirection(Vector2.right);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.down);
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove.RotatePlayerAnim(Vector2.left);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(Dialogue_FruitPunchTable, 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex < 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        Noelle.RotateSusieToDirection(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.up);
        Susie.SusieAnimator.speed = 0.9f;
        while ((Vector2)Susie.transform.position != WalkPos_Susie_TowardsICEE)
        {
            yield return null;
            Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, WalkPos_Susie_TowardsICEE, 2f * Time.deltaTime);
            Susie.SusieAnimator.Play("Walk");
        }
        Susie.SusieAnimator.Play("Idle");
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(Dialogue_FruitPunchTable, 1, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex < 3)
        {
            yield return null;
        }
        Susie.RotateSusieToDirection(Vector2.right);
        while ((Vector2)Susie.transform.position != WalkPos_Susie_TowardsCups)
        {
            yield return null;
            Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, WalkPos_Susie_TowardsCups, 2f * Time.deltaTime);
            Susie.SusieAnimator.Play("Walk");
        }
        Susie.SusieAnimator.speed = 1f;
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.PlaySound(SFX_Noise);
        SiplettCup.SetActive(value: false);
        Susie.SusieAnimator.Play("CupIdle");
        yield return new WaitForSeconds(1f);
        Susie.RotateSusieToDirection(Vector2.down);
        CutsceneUtils.RunFreshChat(Dialogue_FruitPunchTable, 2, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex < 4)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(Dialogue_FruitPunchTable, 3, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex < 5)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(Dialogue_FruitPunchTable, 4, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex < 6)
        {
            yield return null;
        }
        Susie.RotateSusieToDirection(Vector2.down);
        while ((Vector2)Susie.transform.position != WalkPos_Susie_DrinkTable)
        {
            yield return null;
            Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, WalkPos_Susie_DrinkTable, 3f * Time.deltaTime);
            Susie.SusieAnimator.Play("CupWalk");
            Kris._PMove.RotatePlayerAnimTowardsPosition(Susie.transform.position);
            Noelle.RotateSusieToDirection(Vector2.right);
        }
        Susie.SusieAnimator.Play("CupIdle");
        yield return new WaitForSeconds(0.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.CheekyGrin);
        Susie.SusieAnimator.Play("Susie_FD_CupSplash");
        JuiceTable.gameObject.SetActive(value: false);
        JuiceBowl.gameObject.SetActive(value: false);
        yield return new WaitForSeconds(3.35f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
        CutsceneUtils.PlaySound(SFX_Splash);
        Effect_Splash.Play("FD_Effect_CupSplash");
        yield return new WaitForSeconds(0.225f);
        CutsceneUtils.PlaySound(SFX_NoelleScaredShort);
        yield return new WaitForSeconds(2f);
        Susie.SusieAnimator.Play("Susie_FD_CupSplash_Look");
        yield return new WaitForSeconds(1.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock);
        SiplettThrown.gameObject.SetActive(value: true);
        CutsceneUtils.PlaySound(SFX_Throw);
        Susie.SusieAnimator.Play("Susie_FD_CupClean");
        Object.Destroy(Effect_Splash.gameObject);
        Noelle.SusieAnimator.GetComponent<SpriteRenderer>().enabled = false;
        CutsceneUtils.RunFreshChat(Dialogue_SusieApologize, 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex < 7)
        {
            yield return null;
        }
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Default);
        while (!SiplettThrown.FinishedThrowAnimation)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Noelle.transform.position = storedNoelleOriginalPos;
        Noelle.SusieAnimator.Play("NoelleDarkworld_FD_JuiceIdle");
        Noelle.RotateSusieToDirection(Vector2.right);
        Susie.RotateSusieToDirection(Vector2.right);
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        SiplettThrown.StartCoroutine(SiplettThrown.JumpUpAndDown());
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock);
        while (!SiplettThrown.FinishedJumpAnimation)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Annoyed);
        CutsceneUtils.RunFreshChat(Dialogue_SpotSiplett, 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex < 8)
        {
            yield return null;
        }
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
        SiplettBattleTrigger.ForceTriggerBattle();
        yield return new WaitForSeconds(0.9f);
        Noelle_LookRight();
        Kris._PMove.RotatePlayerAnim(Vector2.left);
        Susie_LookLeft();
    }

    private IEnumerator EndingCutscene()
    {
        Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
        Kris._PMove.AllowSprint = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        Noelle_LookRight();
        foreach (ActivePartyMember activePartyMember in PartyMemberSystem.Instance.ActivePartyMembers)
        {
            if (activePartyMember.PartyMemberTransform.transform.Find("Shadow") != null)
            {
                Object.Destroy(activePartyMember.PartyMemberTransform.transform.Find("Shadow").gameObject);
            }
            if (activePartyMember.PartyMemberTransform.transform.Find("Glow") != null)
            {
                Object.Destroy(activePartyMember.PartyMemberTransform.transform.Find("Glow").gameObject);
            }
        }
        yield return new WaitForSeconds(0.5f);
        Kris._PMove.RotatePlayerAnim(Vector2.left);
        CutsceneIndex = 0;
        if (ViolenceUsed)
        {
            CutsceneUtils.RunFreshChat(Dialogue_Outro_IRememberYoureGenocides, 0, ForcePosition: true, OnBottom: false);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Annoyed);
        }
        else
        {
            CutsceneUtils.RunFreshChat(Dialogue_Outro, 0, ForcePosition: true, OnBottom: false);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Default);
        }
        while (CutsceneIndex < 1)
        {
            yield return null;
        }
        if (ViolenceUsed)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject gameObject = Object.Instantiate(Effect_CircleZoom, Vector3.zero, Quaternion.identity);
        if (ViolenceUsed)
        {
            gameObject.transform.position = new Vector3(0f, 4.15f, 0f);
            gameObject.GetComponent<AudioSource>().enabled = false;
            yield return new WaitForSeconds(0.667f);
            IceeCutout.sprite = Icee_Wink;
            CutsceneUtils.PlaySound(SFX_IceeWink, CutsceneUtils.DRH_MixerChannels.Effect, 0.85f);
            Icee_WinkParticle.Play();
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
        }
        else
        {
            gameObject.transform.position = new Vector3(-1.35f, -1.3f, 0f);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.CheekyGrin);
        }
        yield return new WaitForSeconds(2f);
        if (ViolenceUsed)
        {
            HypotheticalGoalManager.Instance.CompleteGoal(Goal_FightPath);
        }
        else
        {
            HypotheticalGoalManager.Instance.CompleteGoal(Goal_MercyPath);
        }
        yield return new WaitForSeconds(1.5f);
        UI_FADE.Instance.StartFadeIn(37, 0.25f, UnpauseOnEnd: true, NewMainMenuManager.MainMenuStates.Hypothetical);
    }

    public void IncrementCutscene()
    {
        CutsceneIndex++;
    }

    public void Kris_LookUp()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void Kris_LookRight()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.right);
    }

    public void Susie_Proud()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Proud_Right");
    }

    public void Susie_LookUp()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
    }

    public void Susie_LookRight()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.right);
    }

    public void Susie_LookLeft()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.left);
    }

    public void Susie_HeadScratch()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Scratch");
    }

    public void Noelle_CoverBlushDown()
    {
        Noelle.SusieAnimator.Play("NoelleDarkworld_Cover_BlushDown");
    }

    public void Noelle_Scared()
    {
        Noelle.SusieAnimator.Play("NoelleDarkworld_FD_JuiceScared");
    }

    public void Noelle_NormalScared()
    {
        Noelle.SusieAnimator.Play("NoelleDarkworld_Scared");
    }

    public void Noelle_Laugh()
    {
        Noelle.SusieAnimator.Play("NoelleDarkworld_Laugh");
    }

    public void Noelle_LookUp()
    {
        Noelle.RotateSusieToDirection(Vector2.up);
        Noelle.SusieAnimator.Play("Idle");
    }

    public void Noelle_LookRight()
    {
        Noelle.RotateSusieToDirection(Vector2.right);
        Noelle.SusieAnimator.Play("Idle");
    }

    public void Noelle_SplashBlush()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.left);
        Noelle.SusieAnimator.GetComponent<SpriteRenderer>().enabled = true;
        Noelle.SusieAnimator.Play("NoelleDarkworld_FD_JuiceBlush");
        storedNoelleOriginalPos = Noelle.transform.position;
        Noelle.transform.position = Susie.transform.position;
        JuiceBowl.sprite = JuiceBowl_Mess;
        JuiceTable.sprite = JuiceTable_Mess;
        JuiceBowl.gameObject.SetActive(value: true);
        JuiceTable.gameObject.SetActive(value: true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(WalkPos_Kris_ToDrinkSign, 0.15f);
        Gizmos.DrawWireSphere(WalkPos_Kris_SetupForDrinkPos, 0.15f);
        Gizmos.DrawWireSphere(WalkPos_Kris_DrinkTable, 0.15f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(WalkPos_Susie_DrinkTable, 0.15f);
        Gizmos.DrawWireSphere(WalkPos_Susie_TowardsICEE, 0.15f);
        Gizmos.DrawWireSphere(WalkPos_Susie_TowardsCups, 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(WalkPos_Noelle_DrinkTable, 0.15f);
    }
}
