using System.Collections;
using UnityEngine;

public class EOTD_ArgueCutscene : MonoBehaviour
{
    public int CutsceneIndex;

    [Header("-- Cutscene References --")]
    [SerializeField]
    private CameraManager PlayerCamera;

    [SerializeField]
    private EOTDCutscene_Story StoryCutscene;

    [SerializeField]
    private GameObject SmokeCloudParticle;

    [SerializeField]
    private SpriteRenderer[] CharacterRenderers;

    [SerializeField]
    private GameObject MiddleCameraTrigger;

    [Header("Characters")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [SerializeField]
    private SpriteRenderer RalseiShadow;

    [Header("-- Character Move Positions --")]
    [Header("Kris, Susie and Ralsei")]
    [SerializeField]
    private Vector3[] KrisWalkPositions;

    [SerializeField]
    private Vector3[] SusieWalkPositions;

    [SerializeField]
    private Vector3[] RalseiWalkPositions;

    [Space(10f)]
    [SerializeField]
    private Vector3[] KrisWalkDirections;

    [SerializeField]
    private Vector3[] SusieWalkDirections;

    [SerializeField]
    private Vector3[] RalseiWalkDirections;

    [Space(10f)]
    [SerializeField]
    private int KrisWalkIndex;

    [SerializeField]
    private int SusieWalkIndex;

    [SerializeField]
    private int RalseiWalkIndex;

    [Space(10f)]
    [SerializeField]
    private int TargetKrisWalkIndex;

    [SerializeField]
    private int TargetSusieWalkIndex;

    [SerializeField]
    private int TargetRalseiWalkIndex;

    [Header("-- Cutscene Chats --")]
    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Header("-- Cutscene Audio --")]
    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private AudioClip MusicFinalFountain;

    [SerializeField]
    private AudioClip MusicGroupHug;

    [SerializeField]
    private AudioClip[] SmokeCloudSounds;

    private int SmokeCloudSoundIndex;

    private bool SmokeCloudPlayed;

    private void Start()
    {
        Kris = PlayerManager.Instance;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei).PartyMemberFollowSettings;
        Susie.transform.position = new Vector2(-8f, 0.092f);
        Ralsei.transform.position = new Vector2(-8f, -2.5f);
        if (Susie == null)
        {
            MonoBehaviour.print("Susie Missing?");
        }
        if (PlayerPrefs.GetInt("EOTD_FinishedArgueCutscene", 0) == 0)
        {
            Susie.FollowingEnabled = false;
            Ralsei.FollowingEnabled = false;
            CutsceneIndex = 1;
            PlayerPrefs.SetInt("EOTD_FinishedArgueCutscene", 1);
        }
        else
        {
            Susie.FollowingEnabled = false;
            Ralsei.FollowingEnabled = false;
            CutsceneIndex = 1;
            PlayerPrefs.SetInt("EOTD_FinishedArgueCutscene", 1);
        }
        MusicManager.PlaySong(MusicFinalFountain, FadePreviousSong: false, 1f);
    }

    private void Update()
    {
        if (CutsceneIndex > 0)
        {
            CutsceneUpdate();
            Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
        if (CutsceneIndex == -1)
        {
            Kris._PlayerState = PlayerManager.PlayerState.Game;
            DarkworldMenu.Instance.CanOpenMenu = true;
            base.enabled = false;
        }
    }

    private void CutsceneUpdate()
    {
        switch (CutsceneIndex)
        {
            case 1:
                if (KrisWalkIndex < TargetKrisWalkIndex)
                {
                    if (Kris.transform.position != KrisWalkPositions[KrisWalkIndex])
                    {
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[KrisWalkIndex], 3f * Time.deltaTime);
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: true);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", KrisWalkDirections[KrisWalkIndex].x);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", KrisWalkDirections[KrisWalkIndex].y);
                    }
                    else
                    {
                        KrisWalkIndex++;
                    }
                }
                else
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                }
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                if (RalseiWalkIndex < TargetRalseiWalkIndex)
                {
                    if (Ralsei.transform.position != RalseiWalkPositions[RalseiWalkIndex])
                    {
                        Ralsei.transform.position = Vector2.MoveTowards(Ralsei.transform.position, RalseiWalkPositions[RalseiWalkIndex], 3f * Time.deltaTime);
                        Ralsei.SusieAnimator.Play("SadWalk");
                        Ralsei.AnimationOverriden = true;
                        Ralsei.SusieAnimator.SetFloat("VelocityX", RalseiWalkDirections[RalseiWalkIndex].x);
                        Ralsei.SusieAnimator.SetFloat("VelocityY", RalseiWalkDirections[RalseiWalkIndex].y);
                        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        RalseiWalkIndex++;
                    }
                }
                else
                {
                    Ralsei.SusieAnimator.Play("Idle");
                    Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
                    Ralsei.SusieAnimator.SetFloat("VelocityY", 1f);
                    Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                if (Kris.transform.position == KrisWalkPositions[1] && Susie.transform.position == SusieWalkPositions[1] && Ralsei.transform.position == RalseiWalkPositions[1])
                {
                    Ralsei.SusieAnimator.Play("Idle");
                    Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
                    Ralsei.SusieAnimator.SetFloat("VelocityY", 1f);
                    Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                    IncrementCutsceneIndex();
                    StartCoroutine(AdmitFinalTaleDelay());
                }
                break;
            case 3:
                ChatboxManager.Instance.EndText();
                MusicManager.StopSong(Fade: true, 1f);
                StartCoroutine(StoryDelay());
                IncrementCutsceneIndex();
                break;
            case 5:
                RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: false);
                IncrementCutsceneIndex();
                break;
            case 7:
                StartCoroutine(KneelDelay());
                IncrementCutsceneIndex();
                break;
            case 9:
                TargetSusieWalkIndex = 3;
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                TargetKrisWalkIndex = 3;
                if (KrisWalkIndex < TargetKrisWalkIndex)
                {
                    if (Kris.transform.position != KrisWalkPositions[KrisWalkIndex])
                    {
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[KrisWalkIndex], 3f * Time.deltaTime);
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: true);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", KrisWalkDirections[KrisWalkIndex].x);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", KrisWalkDirections[KrisWalkIndex].y);
                    }
                    else
                    {
                        KrisWalkIndex++;
                    }
                }
                else
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", -1f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
                }
                if (Kris.transform.position == KrisWalkPositions[2] && Susie.transform.position == SusieWalkPositions[2])
                {
                    IncrementCutsceneIndex();
                    StartCoroutine(DelayUntilRalseiAdmits());
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", -1f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                break;
            case 11:
                RalseiAnim_GetUp();
                StartCoroutine(RalseiGetUpAnimation());
                IncrementCutsceneIndex();
                break;
            case 13:
                RunFreshChat(CutsceneChats[3], 0, ForcePosition: true, OnBottom: false);
                IncrementCutsceneIndex();
                break;
            case 15:
                SusieAnim_IdleHidden_Down();
                StartCoroutine(DelayUntilSusieRegress());
                IncrementCutsceneIndex();
                break;
            case 17:
                TargetSusieWalkIndex = 4;
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 2.5f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("WalkHidden");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                        Kris._PMove._anim.SetBool("MOVING", value: false);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", -1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("IdleHidden");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                if (Susie.transform.position == SusieWalkPositions[3])
                {
                    RunFreshChat(CutsceneChats[5], 0, ForcePosition: true, OnBottom: false);
                    IncrementCutsceneIndex();
                    Susie.SusieAnimator.Play("IdleHidden");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                break;
            case 19:
                TargetSusieWalkIndex = 5;
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 2f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("WalkHidden");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("IdleHidden");
                    Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                TargetRalseiWalkIndex = 3;
                if (RalseiWalkIndex < TargetRalseiWalkIndex)
                {
                    if (Ralsei.transform.position != RalseiWalkPositions[RalseiWalkIndex])
                    {
                        Ralsei.transform.position = Vector2.MoveTowards(Ralsei.transform.position, RalseiWalkPositions[RalseiWalkIndex], 4f * Time.deltaTime);
                        Ralsei.SusieAnimator.Play("WalkNeutral");
                        Ralsei.SusieAnimator.SetFloat("VelocityX", RalseiWalkDirections[RalseiWalkIndex].x);
                        Ralsei.SusieAnimator.SetFloat("VelocityY", RalseiWalkDirections[RalseiWalkIndex].y);
                        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        RalseiWalkIndex++;
                    }
                }
                else
                {
                    Ralsei.SusieAnimator.Play("Ralsei_Stand_Point");
                    Ralsei.SusieAnimator.SetFloat("VelocityX", -1f);
                    Ralsei.SusieAnimator.SetFloat("VelocityY", 0f);
                    Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                if (Susie.transform.position == SusieWalkPositions[4] && Ralsei.transform.position == RalseiWalkPositions[2])
                {
                    RunFreshChat(CutsceneChats[6], 0, ForcePosition: true, OnBottom: false);
                    IncrementCutsceneIndex();
                    Susie.SusieAnimator.Play("IdleHidden");
                    Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Ralsei.SusieAnimator.Play("Ralsei_Stand_Point");
                    Ralsei.SusieAnimator.SetFloat("VelocityX", -1f);
                    Ralsei.SusieAnimator.SetFloat("VelocityY", 0f);
                    Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                break;
            case 21:
                IncrementCutsceneIndex();
                StartCoroutine(DelayUntilSusieShock());
                break;
            case 22:
                IncrementCutsceneIndex();
                ChatboxManager.Instance.EndText();
                RalseiAnim_Argue_FoldedArms();
                break;
            case 23:
                TargetSusieWalkIndex = 6;
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 6f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("WalkHidden");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("SusieDarkworld_Hidden_Angry_Right");
                    Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    IncrementCutsceneIndex();
                }
                break;
            case 25:
                CutsceneSource.PlayOneShot(CutsceneSounds[6]);
                Susie.SusieAnimator.Play("SusieDarkworld_EOTDArgue_SurpriseHug");
                IncrementCutsceneIndex();
                StartCoroutine(SusieDelayUntilHug());
                break;
            case 27:
                TargetSusieWalkIndex = 7;
                Susie.SusieAnimator.GetComponent<SPR_YSorting>().YOffset = 0;
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 7f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("SusieDarkworld_EOTDArgue_HugBlur");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("SusieDarkworld_Hug");
                    Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Ralsei.SusieAnimator.Play("Ralsei_Argue7");
                    CutsceneSource.PlayOneShot(CutsceneSounds[3]);
                    IncrementCutsceneIndex();
                    StartCoroutine(RalseiReactHug());
                }
                break;
            case 29:
                TargetKrisWalkIndex = 5;
                if (KrisWalkIndex < TargetKrisWalkIndex)
                {
                    if (Kris.transform.position != KrisWalkPositions[KrisWalkIndex])
                    {
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[KrisWalkIndex], 3f * Time.deltaTime);
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: true);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", KrisWalkDirections[KrisWalkIndex].x);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", KrisWalkDirections[KrisWalkIndex].y);
                    }
                    else
                    {
                        KrisWalkIndex++;
                    }
                }
                else
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                    Kris._PMove._anim.Play("KrisDarkworld_Hug_Left");
                    CutsceneSource.PlayOneShot(CutsceneSounds[7]);
                    IncrementCutsceneIndex();
                }
                break;
            case 30:
                IncrementCutsceneIndex();
                MusicManager.Instance.source.loop = false;
                MusicManager.PlaySong(MusicGroupHug, FadePreviousSong: true, 1f);
                StartCoroutine(GroupHug());
                break;
            case 32:
                StartCoroutine(DelayUntilLeadTheWay());
                IncrementCutsceneIndex();
                break;
            case 34:
                {
                    Vector3 vector = new Vector3(Kris.transform.position.x, 0.75f, -10f);
                    if (PlayerCamera.transform.position != vector)
                    {
                        PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, vector, 4f * Time.deltaTime);
                    }
                    else
                    {
                        IncrementCutsceneIndex();
                    }
                    break;
                }
            case 35:
                EndCutscene();
                break;
            case 2:
            case 4:
            case 6:
            case 8:
            case 10:
            case 12:
            case 14:
            case 16:
            case 18:
            case 20:
            case 24:
            case 26:
            case 28:
            case 31:
            case 33:
                break;
        }
    }

    private void EndCutscene()
    {
        CutsceneIndex = -1;
        MiddleCameraTrigger.SetActive(value: false);
        Kris._PlayerState = PlayerManager.PlayerState.Game;
        Kris._PMove.AnimationOverriden = false;
        DarkworldMenu.Instance.CanOpenMenu = true;
        Susie.AnimationOverriden = false;
        Ralsei.AnimationOverriden = false;
        Susie.SusieAnimator.Play("Idle");
        Ralsei.SusieAnimator.Play("Idle");
        Susie.FollowingEnabled = true;
        Ralsei.FollowingEnabled = true;
        Susie.RotateSusieToDirection(new Vector2(1f, 0f));
        Ralsei.RotateSusieToDirection(new Vector2(0f, -1f));
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
    }

    public void IncrementCutsceneIndex_EndChat()
    {
        IncrementCutsceneIndex();
        ChatboxManager.Instance.EndText();
    }

    private void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom)
    {
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.CurrentIndex = index;
        CutsceneChatter.FinishedText = false;
        CutsceneChatter.Text = text;
        if (ForcePosition)
        {
            CutsceneChatter.ManualTextboxPosition = true;
            CutsceneChatter.OnBottom = OnBottom;
        }
        CutsceneChatter.RUN();
    }

    private IEnumerator AdmitFinalTaleDelay()
    {
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator StoryDelay()
    {
        yield return new WaitForSeconds(1f);
        StoryCutscene.StartStory();
    }

    private IEnumerator KneelDelay()
    {
        yield return new WaitForSeconds(1.5f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        SusieAnim_Shock_Up();
        RalseiAnim_Kneel();
        yield return new WaitForSeconds(0.5f);
        IncrementCutsceneIndex();
    }

    private IEnumerator DelayUntilRalseiAdmits()
    {
        yield return new WaitForSeconds(0.75f);
        RunFreshChat(CutsceneChats[2], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator RalseiGetUpAnimation()
    {
        yield return new WaitForSeconds(1.3666f);
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(7.3834f);
        Ralsei.SusieAnimator.Play("IdleNeutral");
        Ralsei.SusieAnimator.SetFloat("VelocityX", -1f);
        Ralsei.SusieAnimator.SetFloat("VelocityY", 0f);
        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        IncrementCutsceneIndex();
    }

    private IEnumerator DelayUntilSusieRegress()
    {
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[4], 0, ForcePosition: true, OnBottom: false);
        RalseiAnim_IdleNeutral_Left();
    }

    private IEnumerator DelayUntilSusieShock()
    {
        yield return new WaitForSeconds(1.25f);
        RunFreshChat(CutsceneChats[7], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator GroupHug()
    {
        yield return new WaitForSeconds(9.5f);
        MusicManager.PlaySong(MusicFinalFountain, FadePreviousSong: true, 1f);
        MusicManager.Instance.source.loop = true;
        yield return new WaitForSeconds(1f);
        Susie.SusieAnimator.GetComponent<SPR_YSorting>().YOffset = -1;
        RalseiAnim_Idle_Down();
        CutsceneSource.PlayOneShot(CutsceneSounds[5]);
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove._anim.Play("DARKWORLD_KRIS_IDLE");
        Kris._PMove._anim.SetFloat("MOVEMENTX", -1f);
        Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
        Kris.transform.position = new Vector3(3f, -3f, 0f);
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 1f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.transform.position = new Vector3(0f, -3f, 0f);
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[13], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator SmokeCloudPlaySounds()
    {
        if (SmokeCloudSoundIndex < SmokeCloudSounds.Length)
        {
            CutsceneSource.PlayOneShot(SmokeCloudSounds[SmokeCloudSoundIndex]);
            SmokeCloudSoundIndex++;
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(SmokeCloudPlaySounds());
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            IncrementCutsceneIndex();
        }
    }

    private IEnumerator DelayUntilAskKris()
    {
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[11], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator DelayUntilKrisHugReact()
    {
        ChatboxManager.Instance.EndText();
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[12], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator DelayUntilGetUp()
    {
        yield return new WaitForSeconds(1f);
        SpriteRenderer[] characterRenderers = CharacterRenderers;
        for (int i = 0; i < characterRenderers.Length; i++)
        {
            characterRenderers[i].enabled = true;
        }
        Kris._PAnimation.GetComponent<SpriteRenderer>().enabled = true;
        RalseiAnim_Idle_Down();
        CutsceneSource.PlayOneShot(CutsceneSounds[5]);
        RalseiShadow.enabled = true;
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove._anim.SetFloat("MOVEMENTX", -1f);
        Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
        Kris.transform.position = new Vector3(3f, -3f, 0f);
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 1f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.transform.position = new Vector3(0f, -3f, 0f);
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[13], 0, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator DelayUntilLeadTheWay()
    {
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[13], 1, ForcePosition: true, OnBottom: false);
    }

    private IEnumerator SusieDelayUntilHug()
    {
        yield return new WaitForSeconds(2.9f);
        IncrementCutsceneIndex();
    }

    private IEnumerator RalseiReactHug()
    {
        yield return new WaitForSeconds(1.5f);
        Ralsei.SusieAnimator.Play("Ralsei_Argue8");
        yield return new WaitForSeconds(1f);
        Ralsei.SusieAnimator.Play("Ralsei_Argue9");
        yield return new WaitForSeconds(1f);
        Ralsei.SusieAnimator.Play("Ralsei_Argue10");
        CutsceneSource.PlayOneShot(CutsceneSounds[7]);
        yield return new WaitForSeconds(2f);
        RunFreshChat(CutsceneChats[14], 0, ForcePosition: true, OnBottom: false);
    }

    public void RalseiAnim_Idle_Down()
    {
        Ralsei.SusieAnimator.Play("Idle");
        Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
        Ralsei.SusieAnimator.SetFloat("VelocityY", -1f);
        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void RalseiAnim_Idle_Left()
    {
        Ralsei.SusieAnimator.Play("Idle");
        Ralsei.SusieAnimator.SetFloat("VelocityX", -1f);
        Ralsei.SusieAnimator.SetFloat("VelocityY", 0f);
        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void RalseiAnim_Sad_Left()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Sad_Left");
    }

    public void RalseiAnim_Stand()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Stand_Left");
    }

    public void RalseiAnim_Argue1()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Argue1");
    }

    public void RalseiAnim_Argue2()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Argue2");
    }

    public void RalseiAnim_Argue3()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Argue3");
    }

    public void RalseiAnim_Argue4()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Argue4");
    }

    public void RalseiAnim_Argue5()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Argue5");
    }

    public void RalseiAnim_Argue6()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Argue6");
    }

    public void RalseiAnim_Shock_Left()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Shock_Left");
    }

    public void RalseiAnim_Shock()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Shock");
    }

    public void RalseiAnim_Blush_Down()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Blush_Down");
    }

    public void RalseiAnim_Sad_Down()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Sad_Down");
    }

    public void RalseiAnim_GetUp()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GetUp");
    }

    public void RalseiAnim_Laugh()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Laugh");
    }

    public void RalseiAnim_Kneel()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Kneel");
    }

    public void RalseiAnim_Kneel_Cry()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Kneel_Cry");
    }

    public void RalseiAnim_GroupHug_Idle()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GroupHug_Idle");
    }

    public void RalseiAnim_GroupHug_SusieFace()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GroupHug_SusieFace");
    }

    public void RalseiAnim_GroupHug_SusieDown()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GroupHug_SusieDown");
    }

    public void RalseiAnim_GroupHug_Blush()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GroupHug_Blush");
    }

    public void RalseiAnim_Argue_FoldedArms()
    {
        Ralsei.SusieAnimator.Play("Ralsei_ArgueFoldedArms");
    }

    public void RalseiAnim_GroupHug_LookKris()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GroupHug_LookKris");
    }

    public void RalseiAnim_GroupHug_HugKris()
    {
        Ralsei.SusieAnimator.Play("Ralsei_GroupHug_HugKris");
        StartCoroutine(DelayUntilKrisHugReact());
    }

    public void RalseiAnim_IdleNeutral_Down()
    {
        Ralsei.SusieAnimator.Play("IdleNeutral");
        Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
        Ralsei.SusieAnimator.SetFloat("VelocityY", -1f);
        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void RalseiAnim_IdleNeutral_Left()
    {
        Ralsei.SusieAnimator.Play("IdleNeutral");
        Ralsei.SusieAnimator.SetFloat("VelocityX", -1f);
        Ralsei.SusieAnimator.SetFloat("VelocityY", 0f);
        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_Idle_Up()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_IdleHidden_Down()
    {
        Susie.SusieAnimator.Play("IdleHidden");
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", -1f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_IdleHidden_Left()
    {
        Susie.SusieAnimator.Play("IdleHidden");
        Susie.SusieAnimator.SetFloat("VelocityX", -1f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_IdleHidden_Right()
    {
        Susie.SusieAnimator.Play("IdleHidden");
        Susie.SusieAnimator.SetFloat("VelocityX", 1f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_Scratch()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Scratch");
    }

    public void SusieAnim_HiddenConfused()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Hidden_Confused");
    }

    public void SusieAnim_HiddenAngry()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Hidden_Angry");
    }

    public void SusieAnim_HiddenAngryLeft()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Hidden_Angry_Left");
    }

    public void SusieAnim_TurnAway()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_TurnAway");
    }

    public void SusieAnim_Shock_Up()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Shock_Up");
    }
}
