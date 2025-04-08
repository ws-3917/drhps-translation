using System.Collections;
using UnityEngine;

public class EOTD_CastleFloor_LockedDoor : MonoBehaviour
{
    [SerializeField]
    private string LancerSpokenPlayerPref = "EOTD_HasMetLancer";

    [SerializeField]
    private INT_Chat DoorChat;

    [SerializeField]
    private CHATBOXTEXT Door_PreventLeave;

    [SerializeField]
    private CHATBOXTEXT Door_AllowChoice;

    [SerializeField]
    private CHATBOXTEXT DoorCutscene_RalseiBegin;

    [SerializeField]
    private CHATBOXTEXT DoorCutscene_RalseiFinale;

    [Header("Cutscene")]
    [SerializeField]
    private int CutsceneIndex;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [SerializeField]
    private CameraManager Camera;

    [SerializeField]
    private Vector3 KrisTargetPos;

    [SerializeField]
    private Vector3 SusieTargetPos;

    [SerializeField]
    private Vector3 RalseiTargetPos;

    [SerializeField]
    private SpriteRenderer backgroundRenderer;

    [SerializeField]
    private Sprite backgroundDoorOpenSprite;

    [SerializeField]
    private GameObject backgroundDoorGlow;

    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip DoorOpenSound;

    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    private void Start()
    {
        if (PlayerPrefs.GetInt(LancerSpokenPlayerPref, 0) == 0)
        {
            DoorChat.Text = Door_PreventLeave;
        }
        else
        {
            DoorChat.Text = Door_AllowChoice;
        }
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei).PartyMemberFollowSettings;
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            CutsceneUpdate();
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
            DarkworldMenu.Instance.CanOpenMenu = false;
            Camera.FollowPlayerX = false;
            Camera.FollowPlayerY = false;
        }
    }

    public void BeginEndCutscene()
    {
        CutsceneIndex = 1;
        MusicManager.StopSong(Fade: true, 1f);
        Ralsei.FollowingEnabled = false;
        Ralsei.RotateSusieToDirection(Vector2.down);
        Susie.FollowingEnabled = false;
        Susie.RotateSusieToDirection(Vector2.up);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        DarkworldMenu.Instance.CanOpenMenu = false;
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        ChatboxManager.Instance.EndText();
    }

    private IEnumerator DelayUntilRalseiTalk()
    {
        yield return new WaitForSeconds(1.5f);
        CutsceneIndex = 3;
        RunFreshChat(DoorCutscene_RalseiBegin, 0, ForcePosition: false, OnBottom: false);
    }

    private IEnumerator RalseiDoorOpen()
    {
        yield return new WaitForSeconds(0.5f);
        RalseiAnim_Idle_Left();
        yield return new WaitForSeconds(0.5f);
        RalseiAnim_Idle_Up();
        yield return new WaitForSeconds(2f);
        backgroundRenderer.sprite = backgroundDoorOpenSprite;
        backgroundDoorGlow.SetActive(value: false);
        CutsceneSource.PlayOneShot(DoorOpenSound);
        CutsceneUtils.ShakeTransform(Camera.transform);
        yield return new WaitForSeconds(1.5f);
        RalseiAnim_Idle_Down();
        RunFreshChat(DoorCutscene_RalseiFinale, 0, ForcePosition: false, OnBottom: false);
    }

    private void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom)
    {
        DoorChat.FirstTextPlayed = false;
        DoorChat.CurrentIndex = index;
        DoorChat.FinishedText = false;
        DoorChat.Text = text;
        if (ForcePosition)
        {
            DoorChat.ManualTextboxPosition = true;
            DoorChat.OnBottom = OnBottom;
        }
        DoorChat.RUN();
    }

    private void CutsceneUpdate()
    {
        switch (CutsceneIndex)
        {
            case 1:
                if (PlayerManager.Instance.transform.position != KrisTargetPos)
                {
                    PlayerManager.Instance.transform.position = Vector2.MoveTowards(PlayerManager.Instance.transform.position, KrisTargetPos, 3f * Time.deltaTime);
                }
                if (Susie.transform.position != SusieTargetPos)
                {
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieTargetPos, 3f * Time.deltaTime);
                }
                if (Ralsei.transform.position != RalseiTargetPos)
                {
                    Ralsei.transform.position = Vector2.MoveTowards(Ralsei.transform.position, RalseiTargetPos, 6f * Time.deltaTime);
                }
                if (Camera.transform.position.x != RalseiTargetPos.x)
                {
                    Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, new Vector3(RalseiTargetPos.x, Camera.transform.position.y, Camera.transform.position.z), 1.5f * Time.deltaTime);
                }
                if (Camera.transform.position.x == RalseiTargetPos.x && PlayerManager.Instance.transform.position == KrisTargetPos && Susie.transform.position == SusieTargetPos && Ralsei.transform.position == RalseiTargetPos)
                {
                    CutsceneIndex = 2;
                    StartCoroutine(DelayUntilRalseiTalk());
                }
                break;
            case 4:
                StartCoroutine(RalseiDoorOpen());
                IncrementCutsceneIndex();
                break;
            case 6:
                LevelTransition.BeginTransition(0.25f);
                RalseiAnim_Idle_Left();
                IncrementCutsceneIndex();
                break;
            case 7:
                if ((Vector2)PlayerManager.Instance.transform.position != (Vector2)KrisTargetPos + Vector2.up * 3f)
                {
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance.transform.position = Vector2.MoveTowards(PlayerManager.Instance.transform.position, (Vector2)KrisTargetPos + Vector2.up * 3f, 0.75f * Time.deltaTime);
                }
                if ((Vector2)Susie.transform.position != (Vector2)SusieTargetPos + Vector2.up * 3f)
                {
                    Susie.SusieAnimator.SetBool("InCutscene", value: true);
                    Susie.SusieAnimator.Play("Walk");
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, (Vector2)SusieTargetPos + Vector2.up * 3f, 0.75f * Time.deltaTime);
                }
                if ((Vector2)Ralsei.transform.position != (Vector2)RalseiTargetPos + Vector2.right * 3f)
                {
                    Ralsei.SusieAnimator.SetBool("InCutscene", value: true);
                    Ralsei.SusieAnimator.Play("Walk");
                    Ralsei.transform.position = Vector2.MoveTowards(Ralsei.transform.position, (Vector2)RalseiTargetPos + Vector2.right * 3f, 1f * Time.deltaTime);
                }
                break;
            case 2:
            case 3:
            case 5:
                break;
        }
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
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

    public void RalseiAnim_Idle_Up()
    {
        Ralsei.SusieAnimator.Play("Idle");
        Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
        Ralsei.SusieAnimator.SetFloat("VelocityY", 1f);
        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void RalseiAnim_Blush_Down()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Blush_Down");
    }

    public void RalseiAnim_Laugh()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Laugh");
    }

    public void SusieAnim_Scratch()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Scratch");
    }

    public void SusieAnim_Idle_Up()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }
}
