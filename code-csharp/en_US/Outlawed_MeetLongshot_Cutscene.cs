using System.Collections;
using UnityEngine;

public class Outlawed_MeetLongshot_Cutscene : MonoBehaviour
{
    [Header("-= Cutscene References =-")]
    [Header("Cutscene Logic")]
    [SerializeField]
    private CameraManager PlayerCamera;

    [SerializeField]
    private GameObject LevelCameraTrigger;

    [Header("Party Members")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [SerializeField]
    private Susie_Follower Asgore;

    [SerializeField]
    private PartyMember AsgorePartyMemberReference;

    [Header("Characters")]
    [SerializeField]
    private Animator Longshot;

    [SerializeField]
    private Susie_Follower Longshot_Follower;

    [Header("Objects")]
    [SerializeField]
    private SpriteRenderer LongshotExclamationMark;

    [SerializeField]
    private GameObject LongshotWrongDirectionTrigger;

    [Space(10f)]
    [Header("Dialogue")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneDialogue;

    [Header("Tracked Cutscene Variables")]
    [SerializeField]
    private bool CutsceneHasStarted;

    [Space(10f)]
    [Header("Sounds")]
    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private AudioClip mus_longshot;

    [SerializeField]
    private AudioClip mus_greenroom;

    [Space(10f)]
    [Header("- Target Cutscenes Positions -")]
    [Space(2f)]
    [Header("Walking Up To Longshot")]
    [SerializeField]
    private Vector2 walktolongshot_kris_targetPos;

    [SerializeField]
    private Vector2 walktolongshot_susie_targetPos;

    [SerializeField]
    private Vector2 walktolongshot_ralsei_targetPos;

    [SerializeField]
    private Vector2 walktolongshot_asgore_targetPos;

    [Header("Longshot introducing himself")]
    [SerializeField]
    private Vector2 introduceself_longshot_targetPos;

    private void Start()
    {
        Kris = PlayerManager.Instance;
        PlayerCamera = CameraManager.instance;
        DarkworldMenu.Instance.CanOpenMenu = true;
        LightworldMenu.Instance.CanOpenMenu = false;
        if (PlayerPrefs.GetInt("Game_PreviousVistedRoom", 0) == 43)
        {
            CutsceneHasStarted = true;
            LongshotWrongDirectionTrigger.SetActive(value: true);
            Longshot.Play("Idle");
            Longshot.SetBool("InCutscene", value: false);
            Longshot_Follower.FollowingEnabled = true;
            Longshot_Follower.currentState = Susie_Follower.MemberFollowerStates.SettingUpPosition;
            Longshot_Follower.transform.position = PlayerManager.Instance.transform.position;
            MusicManager.PlaySong(mus_greenroom, FadePreviousSong: false, 0f);
        }
        else
        {
            Longshot.SetBool("InCutscene", value: true);
            Longshot.Play("Pickinglock");
            MusicManager.StopSong(Fade: true, 1f);
        }
    }

    private void Update()
    {
        if (!CutsceneHasStarted && Kris.transform.position.x >= -7f)
        {
            CutsceneHasStarted = true;
            LevelCameraTrigger.SetActive(value: false);
            StartCoroutine(Cutscene());
        }
    }

    private IEnumerator Cutscene()
    {
        _ = Vector3.zero;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei).PartyMemberFollowSettings;
        Asgore = PartyMemberSystem.Instance.HasMemberInParty(AsgorePartyMemberReference).PartyMemberFollowSettings;
        DarkworldMenu.Instance.CanOpenMenu = false;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove.AnimationOverriden = true;
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove._anim.Play("DARKWORLD_KRIS_IDLE");
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.right);
        Susie.AdjustForCutscene(InCutscene: true);
        Ralsei.AdjustForCutscene(InCutscene: true);
        Asgore.AdjustForCutscene(InCutscene: true);
        PartyMemberSystem.Instance.AllPartyMemberPlayAnimation("Idle");
        PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.right);
        Longshot.Play("Pickinglock");
        yield return new WaitForSeconds(0.15f);
        PartyMemberSystem.Instance.AllPartyMemberPlayAnimation("Walk");
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
        CutsceneUtils.MoveTransformLinear(endPoint: new Vector3(-1.75f, -1.35f, -10f), target: PlayerCamera.transform, duration: 2f);
        CutsceneUtils.MoveTransformLinear(Kris.transform, walktolongshot_kris_targetPos, 2f);
        CutsceneUtils.MoveTransformLinear(Susie.transform, walktolongshot_susie_targetPos, 2f);
        CutsceneUtils.MoveTransformLinear(Ralsei.transform, walktolongshot_ralsei_targetPos, 2f);
        CutsceneUtils.MoveTransformLinear(Asgore.transform, walktolongshot_asgore_targetPos, 2f);
        yield return new WaitForSeconds(2f);
        PartyMemberSystem.Instance.AllPartyMemberPlayAnimation("Idle");
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Longshot.Play("LockpickBreak");
        CutsceneUtils.PlaySound(CutsceneSounds[1]);
        yield return new WaitForSeconds(1.5f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[0], 2, ForcePosition: true, OnBottom: true);
        Longshot.Play("StompAngry");
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Longshot.Play("StompIdle");
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[0], 3, ForcePosition: true, OnBottom: true);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        Ralsei.RotateSusieToDirection(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.down);
        Asgore.RotateSusieToDirection(Vector2.right);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.right);
        PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.right);
        PartyMemberSystem.Instance.AllPartyMemberPlayAnimation("Walk");
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
        CutsceneUtils.MoveTransformLinear(endPoint: new Vector3(3f, -1.35f, -10f), target: PlayerCamera.transform, duration: 4f);
        CutsceneUtils.MoveTransformLinear(Kris.transform, walktolongshot_kris_targetPos + Vector2.right * 12f, 4.5f);
        CutsceneUtils.MoveTransformLinear(Susie.transform, walktolongshot_susie_targetPos + Vector2.right * 12f, 4.5f);
        CutsceneUtils.MoveTransformLinear(Ralsei.transform, walktolongshot_ralsei_targetPos + Vector2.right * 12f, 4.5f);
        CutsceneUtils.MoveTransformLinear(Asgore.transform, walktolongshot_asgore_targetPos + Vector2.right * 12f, 4.5f);
        yield return new WaitForSeconds(3.5f);
        LongshotExclamationMark.enabled = true;
        Longshot.Play("BangDoor_NoticeRight");
        CutsceneUtils.PlaySound(CutsceneSounds[0], CutsceneUtils.DRH_MixerChannels.Effect, 0.8f);
        yield return new WaitForSeconds(1f);
        PartyMemberSystem.Instance.AllPartyMemberPlayAnimation("Idle");
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        LongshotExclamationMark.enabled = false;
        CutsceneUtils.RunFreshChat(CutsceneDialogue[0], 5, ForcePosition: true, OnBottom: true);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.left);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
            if (ChatboxManager.Instance.CurrentTextIndex == 4)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.up);
                Susie.RotateSusieToDirection(Vector2.down);
                Asgore.RotateSusieToDirection(Vector2.right);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 3)
            {
                Asgore.RotateSusieToDirection(Vector2.down);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 6)
            {
                Longshot.Play("Idle");
                RotateLongshotToDirection(Vector2.right);
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.left);
                Susie.RotateSusieToDirection(Vector2.left);
                Asgore.RotateSusieToDirection(Vector2.left);
            }
        }
        CutsceneUtils.MoveTransformLinear(Longshot.transform.parent.transform, introduceself_longshot_targetPos, 0.5f);
        Longshot.Play("Walk");
        yield return new WaitForSeconds(0.5f);
        Longshot.Play("Idle");
        yield return new WaitForSeconds(0.1f);
        Longshot.Play("TipHat");
        GonerMenu.Instance.ShowMusicCredit("Friend in Need", "Sooski");
        MusicManager.PlaySong(mus_longshot, FadePreviousSong: false, 0f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[1], 0, ForcePosition: false, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
            if (ChatboxManager.Instance.CurrentTextIndex == 3)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.up);
                Susie.RotateSusieToDirection(Vector2.down);
                Asgore.RotateSusieToDirection(Vector2.right);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 4)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.left);
                Susie.RotateSusieToDirection(Vector2.left);
                Asgore.RotateSusieToDirection(Vector2.left);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 9)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.right);
                Susie.RotateSusieToDirection(Vector2.down);
                Asgore.RotateSusieToDirection(Vector2.right);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 10)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.up);
                Susie.RotateSusieToDirection(Vector2.down);
                Asgore.RotateSusieToDirection(Vector2.right);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 11)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.left);
                Susie.RotateSusieToDirection(Vector2.left);
                Asgore.RotateSusieToDirection(Vector2.right);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 12 || ChatboxManager.Instance.CurrentTextIndex == 5)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.left);
                Susie.RotateSusieToDirection(Vector2.left);
                Asgore.RotateSusieToDirection(Vector2.left);
            }
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[2], 0, ForcePosition: false, OnBottom: true);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        Ralsei.RotateSusieToDirection(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.down);
        Asgore.RotateSusieToDirection(Vector2.right);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 6)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.left);
                Susie.RotateSusieToDirection(Vector2.left);
                Asgore.RotateSusieToDirection(Vector2.left);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 9)
            {
                Longshot.Play("Point_Right");
            }
            yield return null;
        }
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.right);
        PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.right);
        MusicManager.PauseMusic();
        CutsceneUtils.MoveTransformLinear(endPoint: new Vector3(6.45f, -1.35f, -10f), target: PlayerCamera.transform, duration: 2f);
        Longshot.Play("Idle");
        yield return new WaitForSeconds(3f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[2], 1, ForcePosition: false, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        MusicManager.ResumeMusic();
        Vector3 endPoint4 = new Vector3(3f, -1.35f, -10f);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.left);
        CutsceneUtils.MoveTransformLinear(PlayerCamera.transform, endPoint4, 1f);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[2], 2, ForcePosition: false, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 2)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.left);
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneDialogue[2], 3, ForcePosition: false, OnBottom: true);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        Ralsei.RotateSusieToDirection(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.down);
        Asgore.RotateSusieToDirection(Vector2.right);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 2)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                PartyMemberSystem.Instance.AllPartyMemberFaceDirection(Vector2.left);
            }
            if (ChatboxManager.Instance.CurrentTextIndex == 5)
            {
                PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
                Ralsei.RotateSusieToDirection(Vector2.up);
                Susie.RotateSusieToDirection(Vector2.down);
                Asgore.RotateSusieToDirection(Vector2.right);
            }
            yield return null;
        }
        CutsceneUtils.MoveTransformLinear(endPoint: new Vector3(PlayerManager.Instance.transform.position.x, -1.35f, -10f), target: PlayerCamera.transform, duration: 0.25f);
        yield return new WaitForSeconds(0.25f);
        Longshot.Play("Idle");
        Longshot.SetBool("InCutscene", value: false);
        Longshot_Follower.FollowingEnabled = true;
        Longshot_Follower.currentState = Susie_Follower.MemberFollowerStates.SettingUpPosition;
        DarkworldMenu.Instance.CanOpenMenu = true;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        PlayerManager.Instance._PMove.AnimationOverriden = false;
        PlayerManager.Instance._PMove._anim.Play("DARKWORLD_KRIS_IDLE");
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.down);
        PlayerCamera.FollowPlayerX = true;
        LevelCameraTrigger.SetActive(value: true);
        Susie.AdjustForCutscene(InCutscene: false);
        Ralsei.AdjustForCutscene(InCutscene: false);
        Asgore.AdjustForCutscene(InCutscene: false);
        LongshotWrongDirectionTrigger.SetActive(value: true);
        MusicManager.PlaySong(mus_greenroom, FadePreviousSong: true, 1f);
    }

    public void Longshot_PlayAnim_BangDoorIdle()
    {
        Longshot.Play("BangDoor_Idle");
    }

    public void Longshot_PlayAnim_PointRight()
    {
        Longshot.Play("Point_Right");
    }

    public void Longshot_PlayAnim_TipHat()
    {
        Longshot.Play("TipHat");
    }

    public void Longshot_PlayAnim_IdleRight()
    {
        Longshot.Play("Idle");
        RotateLongshotToDirection(Vector2.right);
    }

    public void Longshot_PlayAnim_IdleLeft()
    {
        Longshot.Play("Idle");
        RotateLongshotToDirection(Vector2.left);
    }

    public void Longshot_PlayAnim_IdleUpLeft()
    {
        Longshot.Play("IdleUp");
        RotateLongshotToDirection(Vector2.left);
    }

    public void Longshot_PlayAnim_IdleUpRight()
    {
        Longshot.Play("IdleUp");
        RotateLongshotToDirection(Vector2.right);
    }

    public void RotateLongshotToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Longshot, "VelocityX", "VelocityY", Direction);
    }
}
