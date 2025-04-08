using System.Collections;
using UnityEngine;

public class PapyrusRoom_AfterAdventureCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private int cutsceneIndex;

    [SerializeField]
    private Vector3[] KrisCutscenePositions;

    [SerializeField]
    private Vector3[] SusieCutscenePositions;

    [SerializeField]
    private Vector3[] PapyrusCutscenePositions;

    [SerializeField]
    private Vector3[] BerdlyCutscenePositions;

    [SerializeField]
    private float[] PapyrusWalkspeeds;

    [Space(5f)]
    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private GameObject Papyrus;

    [SerializeField]
    private Animator PapyrusAnimator;

    [SerializeField]
    private GameObject Berdly;

    [SerializeField]
    private GameObject BerdlySweatOverlay;

    [SerializeField]
    private Animator BerdlyAnimator;

    [Space(5f)]
    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private INT_Chat[] ChatsToDisable;

    [SerializeField]
    private CHATBOXTEXT NewTorielCall;

    [SerializeField]
    private CHATBOXTEXT NewBerdlyCall;

    [Space(5f)]
    [SerializeField]
    private GameObject PapyrusChairObject;

    [Space(5f)]
    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    [Space(15f)]
    [Header("Sounds")]
    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneAudioClips;

    private bool HasFinishedWalking;

    private bool Susie_FinishedWalkAnim;

    private int berdlywalkindex;

    public void StopMusic()
    {
        MusicManager.StopSong(Fade: true, 1f);
    }

    private void Update()
    {
        if (cutsceneIndex != 0)
        {
            if (cutsceneIndex != 3)
            {
                PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
                LightworldMenu.Instance.CanOpenMenu = false;
            }
            else
            {
                PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.NoPlayerMovement;
                LightworldMenu.Instance.CanOpenMenu = true;
            }
            CutsceneUpdate();
        }
    }

    private void CutsceneUpdate()
    {
        switch (cutsceneIndex)
        {
            case 2:
                Susie.FollowingEnabled = false;
                Susie.AnimationOverriden = true;
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[0])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[0], 3f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 1f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(1f, 0f));
                }
                if (Susie.transform.position != SusieCutscenePositions[0])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[0], 4f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                if (Susie.transform.position == SusieCutscenePositions[0] && !Susie_FinishedWalkAnim)
                {
                    Susie_FinishedWalkAnim = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, -1f));
                }
                if (PlayerManager.Instance.transform.position == KrisCutscenePositions[0])
                {
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                    PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(-1f, 0f));
                }
                if (Susie.transform.position == SusieCutscenePositions[0] && PlayerManager.Instance.transform.position == KrisCutscenePositions[0] && !HasFinishedWalking)
                {
                    HasFinishedWalking = true;
                    CutsceneChatter.Text = CutsceneChats[0];
                    CutsceneChatter.CurrentIndex = 0;
                    CutsceneChatter.CanUse = true;
                    CutsceneChatter.FirstTextPlayed = false;
                    CutsceneChatter.FinishedText = false;
                    CutsceneChatter.RUN();
                }
                break;
            case 7:
                PapyrusAnimator.Play("Papyrus_ComputerRight");
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[1])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[1], 3f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", -1f);
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
                }
                if (PlayerManager.Instance.transform.position == KrisCutscenePositions[1])
                {
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                    PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
                }
                if (Susie.transform.position != SusieCutscenePositions[1])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[1], 3f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                if (Susie.transform.position == SusieCutscenePositions[1])
                {
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.SusieAnimator.Play("Idle");
                    Susie.RotateSusieToDirection(new Vector2(0f, -1f));
                }
                if (Berdly.transform.position != BerdlyCutscenePositions[0] && berdlywalkindex == 0)
                {
                    Berdly.transform.position = Vector3.MoveTowards(Berdly.transform.position, BerdlyCutscenePositions[0], 8f * Time.deltaTime);
                    BerdlyAnimator.speed = 5f;
                    BerdlyAnimator.Play("berdly_walk_up");
                }
                else if (berdlywalkindex != 1)
                {
                    berdlywalkindex = 1;
                }
                if (Berdly.transform.position != BerdlyCutscenePositions[1] && berdlywalkindex == 1)
                {
                    Berdly.transform.position = Vector3.MoveTowards(Berdly.transform.position, BerdlyCutscenePositions[1], 8f * Time.deltaTime);
                    BerdlyAnimator.speed = 5f;
                    BerdlyAnimator.Play("berdly_walk_left");
                }
                if (Berdly.transform.position == BerdlyCutscenePositions[1] && berdlywalkindex == 1)
                {
                    BerdlyAnimator.Play("berdly_idle_up");
                    BerdlyAnimator.speed = 1f;
                    CutsceneChatter.Text = CutsceneChats[2];
                    CutsceneChatter.CurrentIndex = 0;
                    CutsceneChatter.CanUse = true;
                    CutsceneChatter.FirstTextPlayed = false;
                    CutsceneChatter.FinishedText = false;
                    CutsceneChatter.RUN();
                }
                if (Susie.transform.position == SusieCutscenePositions[1] && PlayerManager.Instance.transform.position == KrisCutscenePositions[1] && Berdly.transform.position == BerdlyCutscenePositions[1])
                {
                    cutsceneIndex = 8;
                }
                break;
            case 9:
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[2])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[2], 1f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 1f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
                }
                if (Susie.transform.position != SusieCutscenePositions[2])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[2], 1f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                break;
        }
    }

    public void BeginCutscene()
    {
        cutsceneIndex = 1;
        Papyrus.GetComponent<SPR_FaceTarget>().Active = false;
        PapyrusAnimator.Play("Papyrus_Idle_Right");
        StartCoroutine(DelayUntilWalkAway());
    }

    public void AfterBerdlyCall()
    {
        cutsceneIndex = 4;
        PlaySusieAnim_IdleDown();
        StartCoroutine(BerdlyDelay());
    }

    private IEnumerator BerdlyDelay()
    {
        yield return new WaitForSeconds(2f);
        cutsceneIndex = 5;
        CutsceneChatter.Text = CutsceneChats[1];
        CutsceneChatter.CurrentIndex = 0;
        CutsceneChatter.CanUse = true;
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.FinishedText = false;
        CutsceneChatter.RUN();
    }

    public void AfterSusieDotDotDot()
    {
        if (cutsceneIndex != 6)
        {
            cutsceneIndex = 6;
            StartCoroutine(DelayUntilBerdlyArrive());
        }
    }

    private IEnumerator DelayUntilBerdlyArrive()
    {
        yield return new WaitForSeconds(2f);
        Berdly.SetActive(value: true);
        BerdlySweatOverlay.SetActive(value: true);
        BerdlyAnimator.Play("berdly_walk_up");
        cutsceneIndex = 7;
        CutsceneSource.PlayOneShot(CutsceneAudioClips[0]);
    }

    private void PapyrusSitDown()
    {
        PapyrusAnimator.Play("Papyrus_ComputerIdle_Sad");
        PapyrusChairObject.SetActive(value: false);
        Papyrus.transform.position = PapyrusCutscenePositions[0];
    }

    public void PlaySusieAnim_IdleRight()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.RotateSusieToDirection(new Vector2(1f, 0f));
        Susie.SusieAnimator.Play("Idle");
    }

    public void PlaySusieAnim_IdleLeft()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.RotateSusieToDirection(new Vector2(-1f, 0f));
        Susie.SusieAnimator.Play("Idle");
    }

    public void PlaySusieAnim_IdleDown()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.RotateSusieToDirection(new Vector2(0f, -1f));
        Susie.SusieAnimator.Play("Idle");
    }

    public void PlaySusieAnim_Angry()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.SusieAnimator.Play("Susie_Angry_Right");
    }

    public void BeginCallSequence()
    {
        cutsceneIndex = 3;
        LightworldMenu.Instance.QueuedCharacterCalls[0] = NewTorielCall;
        LightworldMenu.Instance.QueuedCharacterCalls[1] = NewBerdlyCall;
        LightworldMenu.Instance.CallChatIndexes[0] = 0;
        LightworldMenu.Instance.CallChatIndexes[1] = 0;
    }

    public void RevertKrisControl()
    {
        cutsceneIndex = 5;
    }

    private IEnumerator DelayUntilWalkAway()
    {
        yield return new WaitForSeconds(0.5f);
        cutsceneIndex = 2;
        INT_Chat[] chatsToDisable = ChatsToDisable;
        for (int i = 0; i < chatsToDisable.Length; i++)
        {
            chatsToDisable[i].enabled = false;
        }
        PapyrusSitDown();
    }

    public void BerdlyKrisSusie_Left()
    {
        BerdlyAnim_Left();
        PlaySusieAnim_IdleLeft();
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(-1f, 0f));
    }

    public void PapyrusAnim_IdleUp()
    {
        PapyrusAnimator.Play("Papyrus_Idle_Up");
    }

    public void EndCutscene()
    {
        cutsceneIndex = 9;
        LevelTransition.BeginTransition();
        Susie.FollowingEnabled = true;
        Susie.AnimationOverriden = false;
        Susie.SusieAnimator.Play("Idle");
    }

    public void BerdlyAnim_Praise()
    {
        BerdlyAnimator.Play("berdly_praise");
        PlaySusieAnim_IdleDown();
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
    }

    public void BerdlyAnim_Shock()
    {
        BerdlyAnimator.Play("berdly_shock");
    }

    public void BerdlyAnim_Left()
    {
        BerdlyAnimator.Play("berdly_idle_left");
    }

    public void BerdlyAnim_Up()
    {
        BerdlyAnimator.Play("berdly_idle_up");
        BerdlySweatOverlay.SetActive(value: false);
    }

    public void BerdlyAnim_Down()
    {
        BerdlyAnimator.Play("berdly_idle_down");
    }
}
