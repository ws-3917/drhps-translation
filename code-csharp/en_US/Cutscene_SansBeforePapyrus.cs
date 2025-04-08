using System.Collections;
using UnityEngine;

public class Cutscene_SansBeforePapyrus : MonoBehaviour
{
    public Vector3[] SansWalkPositions;

    public float[] SansWalkspeeds;

    public Susie_Follower Susie;

    public NPC Sans;

    public INT_Chat[] ChatsToDisable;

    public INT_Chat SansCutsceneChat;

    private int cutsceneIndex;

    private int SansWalkIndex;

    [Space(5f)]
    public TRIG_LEVELTRANSITION PapyrusRoomLevelPosition;

    public bool ActivatePrunselVarient;

    [Header("Before Cutscene Stuff")]
    public bool InLivingRoomMode;

    public bool WalkToKitchen;

    private bool previousWTK;

    public Vector2 LivingRoomPosition;

    public Vector2 KitchenPosition;

    private int walkamount;

    [SerializeField]
    private INT_Chat sansFinishedChat;

    [Header("Cutscene Stuff")]
    public AudioSource SansCutsceneSource;

    public AudioClip[] SansCutsceneSounds;

    public AudioReverbFilter PlayerCameraReverb;

    [Space(5f)]
    public SpriteRenderer PapyrusDoor;

    public Sprite PapyrusDoor_Open;

    [Space(5f)]
    public CHATBOXTEXT Papyrus_BaitnSwitch;

    public CHATBOXTEXT Susie_ConfirmKnight;

    public CHATBOXTEXT Susie_CountDown3;

    public CHATBOXTEXT Susie_CountDown2;

    public CHATBOXTEXT Susie_CountDown1;

    public CHATBOXTEXT Susie_CountDownGO;

    public CHATBOXTEXT Susie_ReplyToSans;

    public Vector3[] KrisCutscenePositions;

    public Vector3[] SusieCutscenePositions;

    [Space(5f)]
    [SerializeField]
    private HypothesisGoal Goal_AmbushPapyrus;

    [SerializeField]
    private HypothesisGoal Goal_GreetPapyrus;

    [SerializeField]
    private HypothesisGoal Goal_SusieSnack;

    [SerializeField]
    private HypothesisGoal Goal_PrunselAnomalies;

    private void Start()
    {
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie).PartyMemberFollowSettings;
    }

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
        PlayerPrefs.SetInt("PapyrusMeet_AmbushCutscene", 0);
    }

    private void Update()
    {
        if (cutsceneIndex != 0)
        {
            CutsceneUpdate();
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
        }
        else if (InLivingRoomMode)
        {
            LivingRoomModeUpdate();
        }
    }

    private void LivingRoomModeUpdate()
    {
        if (previousWTK != WalkToKitchen)
        {
            previousWTK = WalkToKitchen;
            if (Sans.CurrentWalkRoutine != null)
            {
                Sans.StopCoroutine(Sans.CurrentWalkRoutine);
                Sans.CurrentWalkRoutine = null;
            }
            walkamount++;
            if (walkamount > 6)
            {
                sansFinishedChat.IndexToLoop = 1;
                sansFinishedChat.FirstTextPlayed = true;
                sansFinishedChat.CurrentIndex = 1;
            }
        }
        if (WalkToKitchen && (Vector2)Sans.transform.position != KitchenPosition && Sans.CurrentWalkRoutine == null)
        {
            Sans.WalkToPosition(KitchenPosition, 9f);
        }
        if (!WalkToKitchen && (Vector2)Sans.transform.position != LivingRoomPosition && Sans.CurrentWalkRoutine == null)
        {
            Sans.WalkToPosition(LivingRoomPosition, 9f);
        }
        if ((Vector2)Sans.transform.position == KitchenPosition)
        {
            Sans.RotateNPC(Vector2.up);
            Collider2D[] collidersToDisableOnWalk = Sans.CollidersToDisableOnWalk;
            for (int i = 0; i < collidersToDisableOnWalk.Length; i++)
            {
                collidersToDisableOnWalk[i].enabled = false;
            }
        }
        if ((Vector2)Sans.transform.position == LivingRoomPosition)
        {
            Sans.RotateNPC(Vector2.right);
            sansFinishedChat.CanUse = true;
        }
        else
        {
            sansFinishedChat.CanUse = false;
        }
    }

    private void CutsceneUpdate()
    {
        switch (cutsceneIndex)
        {
            case 1:
                PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
                PlayerManager.Instance._PMove._rb.simulated = false;
                Susie.FollowingEnabled = false;
                if (SansWalkIndex < SansWalkPositions.Length && !Sans.ismoving)
                {
                    Sans.WalkToPosition(SansWalkPositions[SansWalkIndex], SansWalkspeeds[SansWalkIndex]);
                    Sans.ismoving = true;
                }
                if (Sans.FinishedMoveTo && SansWalkIndex < SansWalkPositions.Length)
                {
                    SansWalkIndex++;
                    Sans.FinishedMoveTo = false;
                    Sans.ismoving = false;
                }
                PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[0])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[0], 5f * Time.deltaTime);
                }
                if (Susie.transform.position != SusieCutscenePositions[0])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[0], 5f * Time.deltaTime);
                }
                if (Sans.transform.position == SansWalkPositions[SansWalkPositions.Length - 1])
                {
                    cutsceneIndex = 26;
                    StartCoroutine(SansKnockScene());
                }
                break;
            case 2:
                cutsceneIndex = 3;
                break;
            case 4:
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[1])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[1], 2f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                }
                if (Susie.transform.position != SusieCutscenePositions[1])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[1], 2f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                if (Susie.transform.position == SusieCutscenePositions[1] && PlayerManager.Instance.transform.position == KrisCutscenePositions[1])
                {
                    Susie.AnimationOverriden = false;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove.AnimationOverriden = false;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                    SetupCutscene4();
                }
                break;
            case 7:
                PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
                if (Susie.transform.position != SusieCutscenePositions[2])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[2], 6f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                if (Susie.transform.position == SusieCutscenePositions[2])
                {
                    Susie.AnimationOverriden = false;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(-1f, 0f));
                    SansCutsceneSource.PlayOneShot(SansCutsceneSounds[1]);
                    Susie.SusieAnimator.Play("Susie_TalkKris");
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(1f, 0f));
                    StartCoroutine(Cutscene8());
                }
                break;
            case 9:
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[2])
                {
                    PlayerManager.Instance._PMove.transform.position = Vector3.MoveTowards(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[2], 5f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                }
                if (Susie.transform.position != SusieCutscenePositions[3])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[3], 4f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                    Susie.SusieAnimator.Play("Walk");
                }
                if (Vector2.Distance(Susie.transform.position, SusieCutscenePositions[3]) < 0.5f)
                {
                    Vector2.Distance(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[2]);
                    _ = 0.5f;
                }
                break;
            case 10:
                if (PlayerManager.Instance._PMove.transform.position != KrisCutscenePositions[3])
                {
                    PlayerManager.Instance._PMove.transform.position = Vector3.MoveTowards(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[3], 2f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                }
                if (Susie.transform.position != SusieCutscenePositions[4])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[4], 2f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                    Susie.SusieAnimator.Play("Walk");
                }
                if (Vector2.Distance(Susie.transform.position, SusieCutscenePositions[4]) < 0.5f && Vector2.Distance(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[3]) < 0.5f)
                {
                    cutsceneIndex = 11;
                    Susie.AnimationOverriden = false;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove.AnimationOverriden = false;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                }
                break;
            case 11:
                cutsceneIndex = 12;
                ChatboxManager.Instance.CurrentTextIndex = 0;
                SansCutsceneChat.Text = Susie_CountDown3;
                SansCutsceneChat.OnBottom = false;
                SansCutsceneChat.CurrentIndex = 0;
                SansCutsceneChat.FirstTextPlayed = false;
                SansCutsceneChat.RUN();
                break;
            case 13:
                if (PlayerManager.Instance._PMove.transform.position != KrisCutscenePositions[4])
                {
                    PlayerManager.Instance._PMove.transform.position = Vector3.MoveTowards(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[4], 2f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                }
                if (Susie.transform.position != SusieCutscenePositions[5])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[5], 2f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                    Susie.SusieAnimator.Play("Walk");
                }
                if (Vector2.Distance(Susie.transform.position, SusieCutscenePositions[5]) < 0.5f && Vector2.Distance(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[4]) < 0.5f)
                {
                    cutsceneIndex = 14;
                    Susie.AnimationOverriden = false;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove.AnimationOverriden = false;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                }
                ChatboxManager.Instance.CurrentTextIndex = 0;
                SansCutsceneChat.Text = Susie_CountDown2;
                SansCutsceneChat.OnBottom = false;
                SansCutsceneChat.CurrentIndex = 0;
                SansCutsceneChat.FirstTextPlayed = false;
                break;
            case 15:
                if (PlayerManager.Instance._PMove.transform.position != KrisCutscenePositions[5])
                {
                    PlayerManager.Instance._PMove.transform.position = Vector3.MoveTowards(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[5], 2f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                }
                if (Susie.transform.position != SusieCutscenePositions[6])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[6], 2f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                    Susie.SusieAnimator.Play("Walk");
                }
                if (Vector2.Distance(Susie.transform.position, SusieCutscenePositions[6]) < 0.5f && Vector2.Distance(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[5]) < 0.5f)
                {
                    cutsceneIndex = 16;
                    Susie.AnimationOverriden = false;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove.AnimationOverriden = false;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                }
                ChatboxManager.Instance.CurrentTextIndex = 0;
                SansCutsceneChat.Text = Susie_CountDown1;
                SansCutsceneChat.OnBottom = false;
                SansCutsceneChat.CurrentIndex = 0;
                SansCutsceneChat.FirstTextPlayed = false;
                break;
            case 17:
                if (PlayerManager.Instance._PMove.transform.position != KrisCutscenePositions[6])
                {
                    PlayerManager.Instance._PMove.transform.position = Vector3.MoveTowards(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[6], 2f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                }
                if (Susie.transform.position != SusieCutscenePositions[7])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[7], 2f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                    Susie.SusieAnimator.Play("Walk");
                }
                if (Vector2.Distance(Susie.transform.position, SusieCutscenePositions[7]) < 0.5f && Vector2.Distance(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[6]) < 0.5f)
                {
                    cutsceneIndex = 18;
                    Susie.AnimationOverriden = false;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                    PlayerManager.Instance._PMove.AnimationOverriden = false;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                }
                ChatboxManager.Instance.CurrentTextIndex = 0;
                SansCutsceneChat.Text = Susie_CountDownGO;
                SansCutsceneChat.OnBottom = false;
                SansCutsceneChat.CurrentIndex = 0;
                SansCutsceneChat.FirstTextPlayed = false;
                break;
            case 19:
                if (PlayerManager.Instance._PMove.transform.position != KrisCutscenePositions[2])
                {
                    PlayerManager.Instance._PMove.transform.position = Vector3.MoveTowards(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[2], 8f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.Play("Kris_SprintUp");
                }
                if (Susie.transform.position != SusieCutscenePositions[3])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[3], 7f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.Play("Susie_SprintUp");
                }
                if (Vector2.Distance(Susie.transform.position, SusieCutscenePositions[3]) < 0.5f && Vector2.Distance(PlayerManager.Instance._PMove.transform.position, KrisCutscenePositions[2]) < 0.5f)
                {
                    cutsceneIndex = 420;
                }
                break;
            case 3:
            case 5:
            case 6:
            case 8:
            case 12:
            case 14:
            case 16:
            case 18:
                break;
        }
    }

    private IEnumerator Cutscene()
    {
        cutsceneIndex = 1;
        yield return new WaitForSeconds(1f);
    }

    public void SetupCutscene2()
    {
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
        cutsceneIndex = 4;
    }

    private IEnumerator Cutscene4()
    {
        MusicManager.StopSong(Fade: true, 0.5f);
        PlayerCameraReverb.reverbPreset = AudioReverbPreset.Arena;
        PapyrusDoor.sprite = PapyrusDoor_Open;
        SansCutsceneSource.PlayOneShot(SansCutsceneSounds[0]);
        yield return new WaitForSeconds(2f);
        SansCutsceneChat.Text = Papyrus_BaitnSwitch;
        SansCutsceneChat.CurrentIndex = 0;
        SansCutsceneChat.FirstTextPlayed = false;
        SansCutsceneChat.RUN();
    }

    public void SetupCutscene4()
    {
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
        cutsceneIndex = 5;
        StartCoroutine(Cutscene4());
    }

    public void SetupCutscene6()
    {
        PlayerCameraReverb.reverbPreset = AudioReverbPreset.Off;
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
        cutsceneIndex = 6;
        StartCoroutine(Cutscene6());
    }

    public void Susie_Countdown()
    {
        cutsceneIndex++;
        StartCoroutine(CounterDelay());
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
    }

    public void Susie_CountdownFinale()
    {
        SansCutsceneSource.PlayOneShot(SansCutsceneSounds[2]);
        cutsceneIndex++;
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
        PapyrusRoomLevelPosition.BeginTransition(2.45f);
    }

    private IEnumerator Cutscene6()
    {
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
        yield return new WaitForSeconds(1f);
        Sans.AnimateAutomatically = false;
        PlayerCameraReverb.reverbPreset = AudioReverbPreset.Off;
        Sans.PlayAnimation("Idle");
        Sans.RotateNPC(Vector2.right);
        yield return new WaitForSeconds(0.5f);
        Sans.PlayAnimation("Idle");
        Sans.RotateNPC(Vector2.down);
        yield return new WaitForSeconds(1.2f);
        SansCutsceneChat.Text = Susie_ConfirmKnight;
        SansCutsceneChat.CurrentIndex = 0;
        SansCutsceneChat.FirstTextPlayed = false;
        SansCutsceneChat.ManualTextboxPosition = true;
        SansCutsceneChat.OnBottom = true;
        SansCutsceneChat.RUN();
        cutsceneIndex = 999;
    }

    public void StartChat_SusieReplyToSans()
    {
        StartCoroutine(DelayForSusieReply());
    }

    private IEnumerator DelayForSusieReply()
    {
        yield return new WaitForSeconds(0.5f);
        SansCutsceneChat.Text = Susie_ReplyToSans;
        SansCutsceneChat.CurrentIndex = 0;
        SansCutsceneChat.FirstTextPlayed = false;
        SansCutsceneChat.ManualTextboxPosition = true;
        SansCutsceneChat.OnBottom = false;
        SansCutsceneChat.RUN();
    }

    public void EndCutscene()
    {
        PlayerManager.Instance._PMove.Manager._PlayerState = PlayerManager.PlayerState.Cutscene;
        PapyrusRoomLevelPosition.BeginTransition();
    }

    private IEnumerator Cutscene8()
    {
        if (!ActivatePrunselVarient)
        {
            cutsceneIndex = 8;
            yield return new WaitForSeconds(0.45f);
            ChatboxManager.Instance.CurrentTextIndex = 0;
            SansCutsceneChat.Text = Susie_ConfirmKnight;
            SansCutsceneChat.OnBottom = false;
            SansCutsceneChat.CurrentIndex = 0;
            SansCutsceneChat.FirstTextPlayed = false;
            SansCutsceneChat.RUN();
        }
        else
        {
            cutsceneIndex = 19;
            PapyrusRoomLevelPosition.BeginTransition();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void BeginWalkToDoor()
    {
        StartCoroutine(WalkToDoorTimed());
        HypotheticalGoalManager.Instance.CompleteGoal(Goal_GreetPapyrus);
    }

    public void SprintToDoor()
    {
        cutsceneIndex = 10;
        StartCoroutine(SprintToDoorTimed());
        PlayerPrefs.SetInt("PapyrusMeet_AmbushCutscene", 1);
        HypotheticalGoalManager.Instance.CompleteGoal(Goal_AmbushPapyrus);
    }

    public void FaceSusieToSans()
    {
        Susie.RotateSusieTowardsPosition(base.transform.position);
    }

    private IEnumerator WalkToDoorTimed()
    {
        PapyrusRoomLevelPosition.BeginTransition();
        yield return new WaitForSeconds(0.25f);
        cutsceneIndex = 9;
    }

    private IEnumerator SprintToDoorTimed()
    {
        yield return new WaitForSeconds(0.5f);
        SansCutsceneChat.RUN();
    }

    private IEnumerator CounterDelay()
    {
        yield return new WaitForSeconds(1f);
        SansCutsceneChat.RUN();
    }

    private IEnumerator SansKnockScene()
    {
        yield return new WaitForSeconds(0.5f);
        PapyrusDoor.sprite = PapyrusDoor_Open;
        SansCutsceneSource.PlayOneShot(SansCutsceneSounds[0]);
        MusicManager.StopSong(Fade: true, 0.5f);
        PlayerCameraReverb.reverbPreset = AudioReverbPreset.Arena;
        yield return new WaitForSeconds(1.2f);
        SansCutsceneChat.Text = Papyrus_BaitnSwitch;
        SansCutsceneChat.CurrentIndex = 0;
        SansCutsceneChat.FirstTextPlayed = false;
        SansCutsceneChat.ManualTextboxPosition = true;
        SansCutsceneChat.OnBottom = true;
        SansCutsceneChat.RUN();
        yield return new WaitForSeconds(0.3f);
        cutsceneIndex = 2;
    }

    public void CompleteSusieSnack()
    {
        HypotheticalGoalManager.Instance.CompleteGoal(Goal_SusieSnack);
    }

    public void IncrementPrunselAnomaly()
    {
        HypotheticalGoalManager.Instance.IncrementGoal(Goal_PrunselAnomalies, 1);
    }

    public void EnableSansLivingRoomMode()
    {
        InLivingRoomMode = true;
    }
}
