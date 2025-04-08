using System.Collections;
using UnityEngine;

public class EOTDCutscene_WalkTown : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex;

    [Header("-- Cutscene References --")]
    [SerializeField]
    private CameraManager PlayerCamera;

    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    [Header("Characters")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Animator JigsawJoe;

    [SerializeField]
    private SpriteRenderer Tasque;

    [SerializeField]
    private GameObject TasqueManager;

    [SerializeField]
    private Sprite TasqueMoveSprite;

    [SerializeField]
    private SpriteRenderer Rudinn_Dojo;

    [SerializeField]
    private SpriteRenderer Rudinn_Lancer;

    [SerializeField]
    private Animator Lancer;

    [SerializeField]
    private Transform Werewire;

    [SerializeField]
    private Animator Ralsei;

    [SerializeField]
    private SpriteRenderer Cafe;

    [SerializeField]
    private Sprite Cafe_Open;

    [Header("-- Character Move Positions --")]
    [Header("Kris and Susie")]
    [SerializeField]
    private Vector3[] KrisWalkPositions;

    [SerializeField]
    private Vector3[] SusieWalkPositions;

    [SerializeField]
    private int KrisWalkIndex;

    [SerializeField]
    private int TargetKrisWalkIndex;

    [SerializeField]
    private int SusieWalkIndex;

    [SerializeField]
    private int TargetSusieWalkIndex;

    [Header("Jigsaw Joe")]
    [SerializeField]
    private Vector3[] JJWalkPositions;

    [SerializeField]
    private int JJWalkIndex;

    [SerializeField]
    private bool JJHasBegunDelay;

    [Header("Tasque and Tasque Manager")]
    [SerializeField]
    private Vector3[] TasqueWalkPositions;

    [SerializeField]
    private Vector3[] TasqueManagerWalkPositions;

    [SerializeField]
    private int TasqueWalkIndex;

    [SerializeField]
    private int TargetTasqueWalkIndex;

    [SerializeField]
    private int TasqueManagerWalkIndex;

    [SerializeField]
    private int TargetTasqueManagerWalkIndex;

    [Header("Rudinn")]
    [SerializeField]
    private Vector3 RudinnWalkPositions;

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
    private AudioClip MusicCastleTown;

    [SerializeField]
    private AudioClip MusicLancer;

    private void Start()
    {
        Kris = PlayerManager.Instance;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        if (Susie == null)
        {
            MonoBehaviour.print("Susie Missing?");
        }
        Susie.FollowingEnabled = false;
        Susie.transform.position = (Vector2)Kris.transform.position + Vector2.down * 2.5f;
        CutsceneIndex = 1;
        MusicManager.PlaySong(MusicCastleTown, FadePreviousSong: true, 1f);
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            CutsceneUpdate();
            Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
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
                        Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                    }
                    else
                    {
                        KrisWalkIndex++;
                    }
                }
                else
                {
                    IncrementCutsceneIndex();
                    MusicManager.PauseMusic();
                    JigsawJoe.gameObject.SetActive(value: true);
                    Rudinn_Dojo.flipX = false;
                    CutsceneSource.PlayOneShot(CutsceneSounds[0]);
                }
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3.1f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                if (Kris.transform.position.y > PlayerCamera.transform.position.y)
                {
                    PlayerCamera.FollowPlayerY = true;
                }
                break;
            case 2:
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    Kris._PMove.RotatePlayerAnim(new Vector2(-1f, 0f));
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(-1f, 0f));
                    Vector3 vector3 = new Vector3(-4f, PlayerCamera.transform.position.y, -10f);
                    if (PlayerCamera.transform.position != vector3)
                    {
                        PlayerCamera.transform.position = Vector2.MoveTowards(PlayerCamera.transform.position, vector3, 4f * Time.deltaTime);
                        break;
                    }
                    IncrementCutsceneIndex();
                    RunFreshChat(CutsceneChats[0], 0, ForcePosition: false, OnBottom: false);
                    break;
                }
            case 4:
                JigsawJoe.Play("JJ_Dissapointed");
                if (JJWalkIndex < JJWalkPositions.Length)
                {
                    if (JigsawJoe.transform.position != JJWalkPositions[JJWalkIndex])
                    {
                        JigsawJoe.transform.position = Vector3.MoveTowards(JigsawJoe.transform.position, JJWalkPositions[JJWalkIndex], 8f * Time.deltaTime);
                    }
                    else
                    {
                        JJWalkIndex++;
                    }
                }
                if (!JJHasBegunDelay)
                {
                    JJHasBegunDelay = true;
                    CutsceneSource.PlayOneShot(CutsceneSounds[3]);
                    StartCoroutine(JJWalkDelay());
                }
                if (JJWalkIndex == 2)
                {
                    Kris._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
                    Susie.RotateSusieToDirection(new Vector2(0f, -1f));
                }
                break;
            case 6:
                {
                    Vector3 vector4 = new Vector3(Kris.transform.position.x, PlayerCamera.transform.position.y, -10f);
                    if (PlayerCamera.transform.position != vector4)
                    {
                        PlayerCamera.transform.position = Vector2.MoveTowards(PlayerCamera.transform.position, vector4, 4f * Time.deltaTime);
                        break;
                    }
                    MusicManager.ResumeMusic();
                    TargetKrisWalkIndex = 2;
                    TargetSusieWalkIndex = 2;
                    if (KrisWalkIndex < TargetKrisWalkIndex)
                    {
                        if (Kris.transform.position != KrisWalkPositions[KrisWalkIndex])
                        {
                            Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[KrisWalkIndex], 3f * Time.deltaTime);
                            Kris._PMove.AnimationOverriden = true;
                            Kris._PMove._anim.SetBool("MOVING", value: true);
                            Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                            Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                        }
                        else
                        {
                            KrisWalkIndex++;
                        }
                    }
                    else
                    {
                        IncrementCutsceneIndex();
                    }
                    if (SusieWalkIndex < TargetSusieWalkIndex)
                    {
                        if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                        {
                            Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3.1f * Time.deltaTime);
                            Susie.AnimationOverriden = true;
                            Susie.SusieAnimator.Play("Walk");
                            Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                            Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                            Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                        }
                        else
                        {
                            SusieWalkIndex++;
                        }
                    }
                    break;
                }
            case 7:
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    Kris._PMove.RotatePlayerAnim(new Vector2(1f, 0f));
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(1f, 0f));
                    Vector3 vector6 = new Vector3(2f, PlayerCamera.transform.position.y, -10f);
                    if (PlayerCamera.transform.position != vector6)
                    {
                        PlayerCamera.transform.position = Vector2.MoveTowards(PlayerCamera.transform.position, vector6, 4f * Time.deltaTime);
                        break;
                    }
                    IncrementCutsceneIndex();
                    RunFreshChat(CutsceneChats[1], 0, ForcePosition: false, OnBottom: false);
                    break;
                }
            case 9:
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    Kris._PMove.RotatePlayerAnim(new Vector2(1f, 0f));
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(1f, 0f));
                    PlayerCamera.FollowPlayerY = false;
                    PlayerCamera.FollowPlayerX = false;
                    Vector3 vector = new Vector3(2f, 18f, -10f);
                    if (PlayerCamera.transform.position != vector)
                    {
                        PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, vector, 4f * Time.deltaTime);
                        break;
                    }
                    IncrementCutsceneIndex();
                    StartCoroutine(TasqueCutscene());
                    break;
                }
            case 11:
                if (TasqueWalkIndex < TargetTasqueWalkIndex)
                {
                    if (Tasque.transform.position != TasqueWalkPositions[TasqueWalkIndex])
                    {
                        Tasque.transform.position = Vector2.MoveTowards(Tasque.transform.position, TasqueWalkPositions[TasqueWalkIndex], 10f * Time.deltaTime);
                        Tasque.sprite = TasqueMoveSprite;
                    }
                    else
                    {
                        TasqueWalkIndex++;
                    }
                }
                if (TasqueManager.activeSelf && TasqueManagerWalkIndex < TargetTasqueManagerWalkIndex)
                {
                    if (TasqueManager.transform.position != TasqueManagerWalkPositions[TasqueManagerWalkIndex])
                    {
                        TasqueManager.transform.position = Vector2.MoveTowards(TasqueManager.transform.position, TasqueManagerWalkPositions[TasqueManagerWalkIndex], 8f * Time.deltaTime);
                    }
                    else
                    {
                        TasqueManagerWalkIndex++;
                    }
                }
                break;
            case 12:
                TargetTasqueManagerWalkIndex = 2;
                if (TasqueManager.activeSelf)
                {
                    if (TasqueManagerWalkIndex < TargetTasqueManagerWalkIndex)
                    {
                        if (TasqueManager.transform.position != TasqueManagerWalkPositions[TasqueManagerWalkIndex])
                        {
                            TasqueManager.transform.position = Vector2.MoveTowards(TasqueManager.transform.position, TasqueManagerWalkPositions[TasqueManagerWalkIndex], 10f * Time.deltaTime);
                        }
                        else
                        {
                            TasqueManagerWalkIndex++;
                        }
                    }
                    else
                    {
                        IncrementCutsceneIndex();
                        TasqueManager.SetActive(value: false);
                    }
                }
                if (TasqueWalkIndex < TargetTasqueWalkIndex)
                {
                    if (Tasque.transform.position != TasqueWalkPositions[TasqueWalkIndex])
                    {
                        Tasque.transform.position = Vector2.MoveTowards(Tasque.transform.position, TasqueWalkPositions[TasqueWalkIndex], 8f * Time.deltaTime);
                    }
                    else
                    {
                        TasqueWalkIndex++;
                    }
                }
                else
                {
                    Tasque.gameObject.SetActive(value: false);
                }
                break;
            case 13:
                {
                    Vector3 vector2 = new Vector3(Kris.transform.position.x, Kris.transform.position.y, -10f);
                    if (PlayerCamera.transform.position != vector2)
                    {
                        PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, vector2, 6f * Time.deltaTime);
                        break;
                    }
                    PlayerCamera.FollowPlayerY = true;
                    RunFreshChat(CutsceneChats[3], 0, ForcePosition: false, OnBottom: false);
                    IncrementCutsceneIndex();
                    break;
                }
            case 15:
                TargetKrisWalkIndex = 4;
                TargetSusieWalkIndex = 4;
                MusicManager.ResumeMusic();
                if (KrisWalkIndex < TargetKrisWalkIndex)
                {
                    if (Kris.transform.position != KrisWalkPositions[KrisWalkIndex])
                    {
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[KrisWalkIndex], 3f * Time.deltaTime);
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: true);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                    }
                    else
                    {
                        KrisWalkIndex++;
                    }
                }
                else
                {
                    IncrementCutsceneIndex();
                    RunFreshChat(CutsceneChats[4], 0, ForcePosition: true, OnBottom: true);
                    Kris._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    Susie.SusieAnimator.Play("Idle");
                }
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3.1f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                break;
            case 16:
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3.1f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    Susie.SusieAnimator.Play("Idle");
                }
                Werewire.position = Vector3.MoveTowards(Werewire.position, Werewire.position + Vector3.down * 40f, 10f * Time.deltaTime);
                break;
            case 17:
                if (Rudinn_Lancer.transform.position != RudinnWalkPositions)
                {
                    Rudinn_Lancer.transform.position = Vector3.MoveTowards(Rudinn_Lancer.transform.position, RudinnWalkPositions, 4f * Time.deltaTime);
                }
                else
                {
                    IncrementCutsceneIndex();
                    Rudinn_Lancer.gameObject.SetActive(value: false);
                    RunFreshChat(CutsceneChats[5], 0, ForcePosition: true, OnBottom: true);
                }
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 3.1f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    Susie.SusieAnimator.Play("Idle");
                }
                Werewire.position = Vector3.MoveTowards(Werewire.position, Werewire.position + Vector3.down * 40f, 10f * Time.deltaTime);
                break;
            case 19:
                TargetSusieWalkIndex = 5;
                if (SusieWalkIndex < TargetSusieWalkIndex)
                {
                    if (Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 6f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Idle");
                        Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                        Lancer.Play("Lancer_Idle_Right");
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                if (Susie.transform.position == SusieWalkPositions[SusieWalkIndex])
                {
                    IncrementCutsceneIndex();
                    StartCoroutine(LancerSusieHighFive());
                }
                Werewire.position = Vector3.MoveTowards(Werewire.position, Werewire.position + Vector3.down * 40f, 10f * Time.deltaTime);
                break;
            case 21:
                IncrementCutsceneIndex();
                StartCoroutine(RalseiChatDelay());
                break;
            case 23:
                {
                    Vector3 vector5 = new Vector3(0f, Kris.transform.position.y + 4f, -10f);
                    PlayerCamera.FollowPlayerY = false;
                    PlayerCamera.FollowPlayerX = false;
                    if (PlayerCamera.transform.position != vector5)
                    {
                        PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, vector5, 4f * Time.deltaTime);
                    }
                    if (Ralsei.transform.position != new Vector3(0f, 26.65f, 0f))
                    {
                        Ralsei.transform.position = Vector3.MoveTowards(Ralsei.transform.position, new Vector3(0f, 26.65f, 0f), 6f * Time.deltaTime);
                        Ralsei.Play("Walk");
                        Ralsei.SetFloat("VelocityX", 0f);
                        Ralsei.SetFloat("VelocityY", -1f);
                        Ralsei.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        Ralsei.Play("Idle");
                        Ralsei.SetFloat("VelocityX", 0f);
                        Ralsei.SetFloat("VelocityY", -1f);
                        Ralsei.SetFloat("VelocityMagnitude", 0f);
                    }
                    if (Susie.transform.position != SusieWalkPositions[6])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[6], 6f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Idle");
                        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    }
                    if (Kris.transform.position != KrisWalkPositions[4])
                    {
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[4], 3f * Time.deltaTime);
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: false);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                        Kris._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    }
                    if (Kris.transform.position == KrisWalkPositions[4] && Susie.transform.position == SusieWalkPositions[6] && Ralsei.transform.position == new Vector3(0f, 26.65f, 0f) && PlayerCamera.transform.position == vector5)
                    {
                        IncrementCutsceneIndex();
                        StartCoroutine(RalseiWelcomeBackDelay());
                        RalseiAnim_Wave();
                    }
                    break;
                }
            case 25:
                if (LevelTransition != null)
                {
                    LevelTransition.BeginTransition();
                    LevelTransition = null;
                }
                if (Ralsei.transform.position != new Vector3(0f, 30f, 0f))
                {
                    Ralsei.transform.position = Vector3.MoveTowards(Ralsei.transform.position, new Vector3(0f, 30f, 0f), 1f * Time.deltaTime);
                    Ralsei.Play("Walk");
                    Ralsei.SetFloat("VelocityX", 0f);
                    Ralsei.SetFloat("VelocityY", 1f);
                    Ralsei.SetFloat("VelocityMagnitude", 1f);
                }
                else
                {
                    Ralsei.Play("Idle");
                    Ralsei.SetFloat("VelocityX", 0f);
                    Ralsei.SetFloat("VelocityY", 1f);
                    Ralsei.SetFloat("VelocityMagnitude", 0f);
                }
                if (Susie.transform.position != SusieWalkPositions[7])
                {
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[7], 1f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.Play("Walk");
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                }
                if (Kris.transform.position != KrisWalkPositions[5])
                {
                    Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[5], 1f * Time.deltaTime);
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                    Kris._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                }
                break;
            case 3:
            case 5:
            case 8:
            case 10:
            case 14:
            case 18:
            case 20:
            case 22:
            case 24:
                break;
        }
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
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

    private IEnumerator JJWalkDelay()
    {
        yield return new WaitForSeconds(3f);
        IncrementCutsceneIndex();
        RunFreshChat(CutsceneChats[0], 1, ForcePosition: false, OnBottom: false);
    }

    private IEnumerator TasqueCutscene()
    {
        yield return new WaitForSeconds(0.5f);
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        Cafe.sprite = Cafe_Open;
        Tasque.gameObject.SetActive(value: true);
        IncrementCutsceneIndex();
        MusicManager.PauseMusic();
        Kris._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
        Susie.RotateSusieToDirection(new Vector2(0f, 1f));
        yield return new WaitForSeconds(1f);
        TasqueManager.SetActive(value: true);
        yield return new WaitForSeconds(0.35f);
        RunFreshChat(CutsceneChats[2], 0, ForcePosition: true, OnBottom: true);
    }

    private IEnumerator LancerSusieHighFive()
    {
        MusicManager.StopSong(Fade: true, 0.5f);
        Susie.gameObject.SetActive(value: false);
        Susie.transform.position = SusieWalkPositions[5];
        Lancer.Play("Lancer_HighFive");
        yield return new WaitForSeconds(1f / 3f);
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        yield return new WaitForSeconds(1.3333334f);
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        yield return new WaitForSeconds(7f / 12f);
        CutsceneSource.PlayOneShot(CutsceneSounds[4]);
        yield return new WaitForSeconds(0.75f);
        CutsceneSource.PlayOneShot(CutsceneSounds[5]);
        yield return new WaitForSeconds(2f);
        Susie.gameObject.SetActive(value: true);
        Susie.RotateSusieToDirection(new Vector2(0f, -1f));
        Lancer.Play("Lancer_Idle_Right");
        yield return new WaitForSeconds(0.5f);
        Susie.RotateSusieToDirection(new Vector2(-1f, 0f));
        RunFreshChat(CutsceneChats[6], 0, ForcePosition: true, OnBottom: true);
        MusicManager.PlaySong(MusicLancer, FadePreviousSong: false, 1f);
    }

    private IEnumerator RalseiChatDelay()
    {
        MusicManager.StopSong(Fade: true, 0.5f);
        yield return new WaitForSeconds(0.5f);
        RunFreshChat(CutsceneChats[7], 0, ForcePosition: true, OnBottom: true);
        LancerAnim_Idle_Up();
        Susie.RotateSusieToDirection(new Vector2(0f, 1f));
    }

    private IEnumerator RalseiWelcomeBackDelay()
    {
        MusicManager.PlaySong(MusicCastleTown, FadePreviousSong: false, 1f);
        yield return new WaitForSeconds(0.5f);
        RunFreshChat(CutsceneChats[8], 0, ForcePosition: true, OnBottom: false);
    }

    public void LancerAnim_Idle_Down()
    {
        Lancer.Play("Lancer_Idle_Down");
    }

    public void LancerAnim_Idle_Right()
    {
        Lancer.Play("Lancer_Idle_Right");
    }

    public void LancerAnim_Idle_Up()
    {
        Lancer.Play("Lancer_Idle_Up");
    }

    public void LancerAnim_Concerned_Down()
    {
        Lancer.Play("Lancer_Concerned_Down");
    }

    public void LancerAnim_Concerned_Right()
    {
        Lancer.Play("Lancer_Concerned_Right");
    }

    public void RalseiAnim_Idle_Down()
    {
        Ralsei.Play("Idle");
        Ralsei.SetFloat("VelocityX", 0f);
        Ralsei.SetFloat("VelocityY", -1f);
        Ralsei.SetFloat("VelocityMagnitude", 0f);
    }

    public void RalseiAnim_Idle_Right()
    {
        Ralsei.Play("Idle");
        Ralsei.SetFloat("VelocityX", -1f);
        Ralsei.SetFloat("VelocityY", 0f);
        Ralsei.SetFloat("VelocityMagnitude", 0f);
    }

    public void RalseiAnim_Wave()
    {
        Ralsei.Play("Ralsei_Wave");
    }

    public void RalseiAnim_Laugh()
    {
        Ralsei.Play("Ralsei_Laugh");
    }

    public void RalseiAnim_Shock()
    {
        Ralsei.Play("Ralsei_Shock");
    }

    public void RalseiAnim_CrazyWalk()
    {
        Ralsei.Play("Ralsei_CrazyWalk");
    }

    public void RalseiAnim_Blush_Down()
    {
        Ralsei.Play("Ralsei_Blush_Down");
    }

    public void RalseiAnim_Sad_Down()
    {
        Ralsei.Play("Ralsei_Sad_Down");
    }

    public void RalseiAnim_Sad_Left()
    {
        Ralsei.Play("Ralsei_Sad_Left");
    }

    public void SusieAnim_Idle_Up()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_Scratch()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Scratch");
    }

    public void SusieAnim_Proud_Right()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Proud_Right");
    }
}
