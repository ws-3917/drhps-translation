using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AprilFools_Manager : MonoBehaviour
{
    public enum AprilFools25_State
    {
        Intro = 0,
        Fight = 1,
        EndingCutscene = 2,
        Hypoaster = 3
    }

    [Header("-= Stats =-")]
    public AprilFools25_State CurrentState;

    [Header("-= References =-")]
    [SerializeField]
    private CHATBOXTEXT IntroText;

    [SerializeField]
    private CHATBOXTEXT GameoverText;

    [SerializeField]
    private CHATBOXTEXT EndingText;

    public AprilFools_PlayerController Player;

    public AprilFools_Longshot Longshot;

    [SerializeField]
    private Image CutsceneFade;

    [SerializeField]
    private GameObject UIHypoaster;

    [SerializeField]
    private GameObject UIGameplay;

    [SerializeField]
    private GameObject UIEnding;

    [SerializeField]
    private Image UIAsgoreIcon;

    [SerializeField]
    private Sprite spr_AsgoreLockedIn;

    [SerializeField]
    private GameObject UILockIn;

    [SerializeField]
    private Transform EndingCutscene_CameraLongshotPos;

    [SerializeField]
    private Transform EndingCutscene_CameraKrisPos;

    [SerializeField]
    private Transform EndingCutscene_CameraAsgorePos;

    [SerializeField]
    private Animator EndingCutscene_Longshot;

    [SerializeField]
    private GameObject EndingCutscene_Rocket;

    [SerializeField]
    private GameObject EndingCutscene_Kris;

    [SerializeField]
    private GameObject EndingCutscene_Asgore;

    [SerializeField]
    private GameObject EndingCutscene_Tunnel;

    [SerializeField]
    private Rigidbody[] EndingCutscene_AsgoreGibs;

    [Header("-= Sounds =-")]
    [SerializeField]
    private AudioClip Song_MagicFrogeIntro;

    [SerializeField]
    private AudioClip Song_STANDOFF_If_It_Was_Good;

    [SerializeField]
    private AudioClip Song_FriendInNeed;

    [SerializeField]
    private AudioClip SFX_AsgoreBreak;

    [SerializeField]
    private AudioSource Song_FriendInNeed2;

    public static AprilFools_Manager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CurrentState = AprilFools25_State.Intro;
        Player.canMove = false;
        Song_MagicFrogeIntro.LoadAudioData();
        Song_STANDOFF_If_It_Was_Good.LoadAudioData();
        GonerMenu.Instance.ShowMusicCredit("A Stab in The Back", "MagicFroge");
        StartCoroutine(AprilFools());
    }

    public void PlayerGameOver()
    {
        MusicManager.Instance.source.pitch = 1f;
        Player.canMove = false;
        Battle_GameOver.Instance.PossibleDeathMessages = new CHATBOXTEXT[1] { GameoverText };
        Battle_GameOver.Instance.PlayGameOver(CameraManager.instance.transform.position);
        Longshot.gameObject.SetActive(value: false);
        Player.transform.position = new Vector3(100f, 100f, -10f);
        Player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        CameraManager.instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        UIGameplay.SetActive(value: false);
        CameraManager.instance.GetComponent<Camera>().orthographic = true;
        CameraManager.instance.GetComponent<Camera>().orthographicSize = 6f;
    }

    public void LockIn()
    {
        Player.walkingSpeed = 20f;
        Player.runningSpeed = 20f;
        Player.Health += 50;
        Player.Health = Mathf.Clamp(Player.Health, 0, 100);
        UILockIn.SetActive(value: true);
        Player.UI_UpdateHPGraphics();
        Player.UI_DisplayHeal();
        CameraManager.instance.GetComponent<Camera>().fieldOfView = 80f;
        Player.ShotgunAnimator.speed = 1.5f;
        UIAsgoreIcon.sprite = spr_AsgoreLockedIn;
        EndingCutscene_Tunnel.transform.position = new Vector3(0f, 4f, 1352f);
        AprilFools_EnvironmentScrolling[] components = GetComponents<AprilFools_EnvironmentScrolling>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].ScrolLSpeed *= 2f;
        }
    }

    public void EndFight()
    {
        Longshot.gameObject.SetActive(value: false);
        EndingCutscene_Longshot.transform.parent.gameObject.SetActive(value: true);
        Player.canMove = false;
        Player.Health = 999;
        UIGameplay.SetActive(value: false);
        UILockIn.SetActive(value: false);
        EndingCutscene_Tunnel.SetActive(value: false);
        AprilFools_EnvironmentScrolling[] components = GetComponents<AprilFools_EnvironmentScrolling>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].ScrolLSpeed *= 0f;
        }
        StartCoroutine(EndingCutscene());
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((bool)other.GetComponent<AprilFools_PlayerController>())
        {
            Player.Health = 999;
            PlayerGameOver();
        }
    }

    private IEnumerator AprilFools()
    {
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(IntroText, 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(IntroText, 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(IntroText, 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        MusicManager.Instance.source.loop = false;
        MusicManager.PlaySong(Song_MagicFrogeIntro, FadePreviousSong: false, 0f);
        UIHypoaster.SetActive(value: true);
        yield return new WaitForSeconds(4f);
        UIHypoaster.SetActive(value: false);
        CutsceneFade.color = Color.white;
        CutsceneUtils.FadeOutUIImage(CutsceneFade, 4f);
        MusicManager.Instance.source.loop = true;
        MusicManager.PlaySong(Song_STANDOFF_If_It_Was_Good, FadePreviousSong: false, 0f);
        GonerMenu.Instance.ShowMusicCredit("STANDOFF (Beta Mix)", "MagicFroge");
        Player.canMove = true;
        Longshot.BeginLongshotAI();
        CurrentState = AprilFools25_State.Fight;
    }

    private IEnumerator EndingCutscene()
    {
        MusicManager.Instance.source.pitch = 1f;
        MusicManager.StopSong(Fade: false, 0f);
        yield return null;
        Player.canMove = false;
        Player.playerCamera.transform.position = EndingCutscene_CameraLongshotPos.position;
        Player.playerCamera.transform.rotation = EndingCutscene_CameraLongshotPos.rotation;
        CutsceneUtils.RunFreshChat(EndingText, 0, ForcePosition: true, OnBottom: false);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        MusicManager.PlaySong(Song_FriendInNeed, FadePreviousSong: false, 0f);
        GonerMenu.Instance.ShowMusicCredit("Friend In Need", "Sooski");
        CutsceneUtils.MoveTransformLinear(Player.playerCamera.transform, new Vector3(16.25875f, 12.68045f, -6.437748f), 6f);
        yield return new WaitForSeconds(6f);
        MusicManager.StopSong(Fade: false, 0f);
        Player.playerCamera.transform.position = EndingCutscene_CameraLongshotPos.position;
        Player.playerCamera.transform.rotation = EndingCutscene_CameraLongshotPos.rotation;
        CutsceneUtils.RunFreshChat(EndingText, 1, ForcePosition: true, OnBottom: false);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        EndingCutscene_Rocket.SetActive(value: true);
        StartCoroutine(WaitForRocketBoom());
    }

    public void EndingCutscene_SwitchToKris()
    {
        Player.playerCamera.transform.position = EndingCutscene_CameraKrisPos.position;
        Player.playerCamera.transform.rotation = EndingCutscene_CameraKrisPos.rotation;
    }

    private IEnumerator WaitForRocketBoom()
    {
        while (EndingCutscene_Rocket != null)
        {
            yield return null;
        }
        EndingCutscene_Kris.SetActive(value: false);
        yield return new WaitForSeconds(3f);
        GonerMenu.Instance.CanOpenGonerMenu = false;
        Player.playerCamera.transform.position = EndingCutscene_CameraAsgorePos.position;
        Player.playerCamera.transform.rotation = EndingCutscene_CameraAsgorePos.rotation;
        EndingCutscene_Asgore.SetActive(value: true);
        yield return new WaitForSeconds(1f);
        Rigidbody[] endingCutscene_AsgoreGibs = EndingCutscene_AsgoreGibs;
        for (int i = 0; i < endingCutscene_AsgoreGibs.Length; i++)
        {
            endingCutscene_AsgoreGibs[i].isKinematic = false;
        }
        CutsceneUtils.PlaySound(SFX_AsgoreBreak);
        yield return new WaitForSeconds(5f);
        Song_FriendInNeed2.Play();
        CutsceneUtils.MoveTransformLinear(Player.playerCamera.transform, new Vector3(0.89f, 3.59f, 21.59f), 10f);
        yield return new WaitForSeconds(10f);
        Song_FriendInNeed2.GetComponent<AudioReverbFilter>().enabled = true;
        Song_FriendInNeed2.volume = 0.25f;
        UIEnding.SetActive(value: true);
        yield return new WaitForSeconds(7f);
        Song_FriendInNeed2.volume = 0.667f;
        while (Song_FriendInNeed2.isPlaying)
        {
            yield return null;
        }
        UIEnding.GetComponent<Animator>().Play("AF25_EndingUI_FadeOut");
        yield return new WaitForSeconds(2.5f);
        PlayerManager.Instance._PMove.InDarkworld = false;
        ChatboxManager.Instance.InDarkworld = false;
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(EndingText, 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        PlayerPrefs.SetInt("Trophy_CompletedApriLFools25", 1);
        UI_FADE.Instance.StartFadeIn(37, 1f, UnpauseOnEnd: true);
    }
}
