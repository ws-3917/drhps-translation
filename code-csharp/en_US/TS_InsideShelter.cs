using System.Collections;
using UnityEngine;

public class TS_InsideShelter : MonoBehaviour
{
    public enum TakingShelter_InsideState
    {
        Intro = 0,
        Setup = 1,
        Kris = 2,
        Ending = 3
    }

    [Header("-- References --")]
    [SerializeField]
    private CameraManager playerCamera;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Animator KrisAnimator;

    [SerializeField]
    private TS_VesselController Vessel;

    [SerializeField]
    private RuntimeAnimatorController Kris_TenseController;

    private Vector3 krisOriginalPos;

    private bool krisCanShake = true;

    [Header("- Camera Controls -")]
    [SerializeField]
    private float CameraFollowBB_Top;

    [SerializeField]
    private float CameraFollowBB_Bottom;

    [SerializeField]
    private bool CameraFollowingEnabled = true;

    [Header("-- Cutscene --")]
    [SerializeField]
    private TakingShelter_InsideState CurrentInsideShelterState;

    private TakingShelter_InsideState previousInsideState;

    [SerializeField]
    private int CutsceneIndex;

    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private INT_Chat KrisChatter;

    [Space(5f)]
    [SerializeField]
    private GameObject blackFade;

    [SerializeField]
    private GameObject KrisCollisionBlock;

    [SerializeField]
    private GameObject KrisCollisionBlock2;

    [Header("- Sound and Music -")]
    [SerializeField]
    private AudioClip shelterWind;

    [SerializeField]
    private AudioClip ReluctantGuest1;

    [SerializeField]
    private AudioClip ReluctantGuest2;

    [SerializeField]
    private AudioClip ReluctantGuest3;

    [SerializeField]
    private AudioSource SecondaryMusicSource;

    [Space(5f)]
    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] HeartRipClips;

    private void Awake()
    {
        shelterWind.LoadAudioData();
        ReluctantGuest1.LoadAudioData();
        ReluctantGuest2.LoadAudioData();
        ReluctantGuest3.LoadAudioData();
    }

    private void Start()
    {
        playerTransform = PlayerManager.Instance.transform;
        PlayerManager.Instance._PAnimation.FootstepsEnabled = true;
        PlayerManager.Instance._PMove.AllowSprint = false;
        krisOriginalPos = KrisAnimator.transform.position;
        LightworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.AllowInput = false;
        StartCoroutine(TSIntro());
        StartCoroutine(KrisShake_BeforeVessel());
    }

    private void Update()
    {
        if (CameraFollowingEnabled)
        {
            CameraFollowDefault();
        }
        if (previousInsideState != CurrentInsideShelterState)
        {
            previousInsideState = CurrentInsideShelterState;
            onCutsceneStateSwitch();
        }
        LightworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.AllowInput = false;
    }

    private void CameraFollowDefault()
    {
        if (playerTransform.position.y < CameraFollowBB_Top && playerTransform.position.y > CameraFollowBB_Bottom)
        {
            if (CurrentInsideShelterState == TakingShelter_InsideState.Intro)
            {
                playerCamera.FollowPlayerY = true;
            }
        }
        else
        {
            if (playerCamera.transform.position.y != CameraFollowBB_Top && playerCamera.transform.position.y != CameraFollowBB_Bottom)
            {
                if (Vector2.Distance(playerCamera.transform.position, Vector2.up * CameraFollowBB_Top) < Vector2.Distance(playerCamera.transform.position, Vector2.up * CameraFollowBB_Bottom))
                {
                    playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, CameraFollowBB_Top, playerCamera.transform.position.z);
                    if (CurrentInsideShelterState == TakingShelter_InsideState.Intro)
                    {
                        CurrentInsideShelterState = TakingShelter_InsideState.Setup;
                        CameraFollowingEnabled = false;
                    }
                }
                else
                {
                    playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, CameraFollowBB_Bottom, playerCamera.transform.position.z);
                }
            }
            playerCamera.FollowPlayerY = false;
        }
        if (!SecondaryMusicSource.isPlaying && Vector2.Distance(SecondaryMusicSource.transform.position, Vessel.transform.position) < SecondaryMusicSource.maxDistance)
        {
            SecondaryMusicSource.Play();
        }
    }

    private void onCutsceneStateSwitch()
    {
        if (CurrentInsideShelterState == TakingShelter_InsideState.Setup)
        {
            KrisCollisionBlock.SetActive(value: true);
            StartCoroutine(TSKris_Reveal());
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

    private IEnumerator TSIntro()
    {
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        yield return new WaitForSeconds(3f);
        CutsceneSource.PlayOneShot(HeartRipClips[0]);
        yield return new WaitForSeconds(1.65f);
        CutsceneSource.PlayOneShot(HeartRipClips[0]);
        yield return new WaitForSeconds(1.5f);
        CutsceneSource.PlayOneShot(HeartRipClips[1]);
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_RipOutSoul");
        yield return new WaitForSeconds(7.5f);
        CutsceneSource.PlayOneShot(HeartRipClips[2]);
        yield return new WaitForSeconds(1.25f);
        MusicManager.PlaySong(HeartRipClips[3], FadePreviousSong: false, 0f);
        yield return new WaitForSeconds(3.5f);
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
        PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        while (CutsceneIndex != 1)
        {
            yield return null;
        }
        MusicManager.StopSong(Fade: true, 2f);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);
        blackFade.SetActive(value: false);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        MusicManager.PlaySong(shelterWind, FadePreviousSong: false, 0f);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        KrisCollisionBlock2.SetActive(value: true);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        KrisCollisionBlock2.SetActive(value: false);
        MusicManager.StopSong(Fade: true, 2.5f, SecondaryMusicSource);
        while (PlayerManager.Instance.transform.position.y > 30.86173f)
        {
            yield return null;
        }
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        yield return new WaitForSeconds(1f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        KrisChatter.CurrentIndex = 0;
        KrisChatter.FirstTextPlayed = true;
        KrisChatter.Text = CutsceneChats[1];
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Crying);
        SecondaryMusicSource.clip = ReluctantGuest2;
        SecondaryMusicSource.Play();
        while (CutsceneIndex != 7)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[5], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 8)
        {
            yield return null;
        }
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        yield return new WaitForSeconds(1.5f);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        RunFreshChat(CutsceneChats[2], 0, ForcePosition: true, OnBottom: false);
    }

    public void ComfortKris()
    {
        StartCoroutine(BeginComfortKris());
    }

    private IEnumerator BeginComfortKris()
    {
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        Vessel.CopyTransformsEnabled = false;
        while ((Vector2)Vessel.transform.position != new Vector2(0.475f, 35.6f))
        {
            yield return null;
            PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
            Vessel.transform.position = Vector2.MoveTowards(Vessel.transform.position, new Vector2(0.475f, 35.6f), 1.5f * Time.deltaTime);
        }
        yield return new WaitForSeconds(1f);
        Vessel.CopyAnimationsEnabled = false;
        Vessel.SetVesselToHug();
        krisCanShake = false;
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        Vessel.transform.position = new Vector2(0.475f, 35.6f);
        CutsceneUtils.ShakeTransform(Vessel.transform, 0.1f, 1f);
        CutsceneSource.PlayOneShot(HeartRipClips[4]);
        yield return new WaitForSeconds(3f);
        StartCoroutine(KrisAfterComfortOrNot());
    }

    public void DoNotComfortKris()
    {
        StartCoroutine(BeginDoNotComfortKris());
    }

    private IEnumerator BeginDoNotComfortKris()
    {
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(KrisAfterComfortOrNot());
    }

    private IEnumerator KrisAfterComfortOrNot()
    {
        CutsceneIndex = 0;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        RunFreshChat(CutsceneChats[3], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 1)
        {
            yield return null;
        }
        MusicManager.StopSong(Fade: true, 2.5f, SecondaryMusicSource);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        ChatboxManager.Instance.EndText();
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        PlayerManager.Instance.transform.position = new Vector2(0f, 33.5f);
        Vessel.CopyAnimationsEnabled = true;
        Vessel.CopyTransformsEnabled = false;
        while ((Vector2)Vessel.transform.position != new Vector2(0f, 33.5f))
        {
            yield return null;
            PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
            Vessel.transform.position = Vector2.MoveTowards(Vessel.transform.position, new Vector2(0f, 33.5f), 2f * Time.deltaTime);
        }
        Vessel.CopyTransformsEnabled = true;
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        yield return new WaitForSeconds(1f);
        krisCanShake = false;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        RunFreshChat(CutsceneChats[4], 0, ForcePosition: true, OnBottom: false);
        SecondaryMusicSource.clip = ReluctantGuest3;
        SecondaryMusicSource.Play();
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        KrisAnimator.Play("Kris_TS_GetUp");
        CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 0.5f);
        KrisAnimator.runtimeAnimatorController = Kris_TenseController;
        KrisAnimator.Play("TENSE_KRIS_IDLE");
        KrisAnimator.SetBool("MOVING", value: false);
        CutsceneUtils.RotateCharacterToDirection(KrisAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RotateCharacterToDirection(KrisAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        RunFreshChat(CutsceneChats[4], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        PlayerManager.Instance.transform.position = new Vector2(0.6f, 33.5f);
        CutsceneUtils.RotateCharacterToDirection(KrisAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        PlayerManager.Instance._PAnimation.FootstepsEnabled = false;
        Vessel.CopyAnimationsEnabled = true;
        Vessel.CopyTransformsEnabled = false;
        while ((Vector2)Vessel.transform.position != new Vector2(0.6f, 33.5f) || (Vector2)KrisAnimator.transform.position != new Vector2(-0.6f, 32.6f))
        {
            yield return null;
            if ((Vector2)Vessel.transform.position != new Vector2(0.6f, 33.5f))
            {
                PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                Vessel.transform.position = Vector2.MoveTowards(Vessel.transform.position, new Vector2(0.6f, 33.5f), 0.5f * Time.deltaTime);
            }
            KrisAnimator.SetBool("MOVING", value: true);
            KrisAnimator.transform.position = Vector2.MoveTowards(KrisAnimator.transform.position, new Vector2(-0.6f, 32.6f), 1f * Time.deltaTime);
        }
        CutsceneUtils.RotateCharacterToDirection(KrisAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        KrisAnimator.SetBool("MOVING", value: false);
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.down);
        MusicManager.StopSong(Fade: true, 3f);
        CutsceneUtils.RotateCharacterToDirection(KrisAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        while ((Vector2)Vessel.transform.position != new Vector2(0.6f, 22f) || (Vector2)KrisAnimator.transform.position != new Vector2(-0.6f, 21.5f))
        {
            yield return null;
            PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
            Vessel.transform.position = Vector2.MoveTowards(Vessel.transform.position, new Vector2(0.6f, 22f), 2f * Time.deltaTime);
            KrisAnimator.SetBool("MOVING", value: true);
            KrisAnimator.transform.position = Vector2.MoveTowards(KrisAnimator.transform.position, new Vector2(-0.6f, 21.5f), 2f * Time.deltaTime);
        }
        yield return new WaitForSeconds(2.5f);
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_Horns_GainSoul");
        yield return new WaitForSeconds(3f);
        PlayerManager.Instance.transform.position = new Vector2(999f, 999f);
        PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        LightworldMenu.Instance.CanOpenMenu = true;
        LightworldMenu.Instance.AllowInput = true;
        PlayerManager.Instance._PMove._anim.GetComponent<SpriteRenderer>().enabled = true;
        PlayerManager.Instance._PMove._anim.GetComponent<SpriteRenderer>().color = Color.white;
        PlayerManager.Instance._PMove._anim.speed = 1f;
        PlayerManager.Instance._PMove._anim.transform.localPosition = new Vector2(0f, -0.9f);
        PlayerManager.Instance._PMove.AllowSprint = true;
        UI_FADE.Instance.StartFadeIn(35, 1f);
    }

    private IEnumerator TSKris_Reveal()
    {
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        PlayerManager.Instance._PMove._rb.velocity = Vector2.zero;
        PlayerManager.Instance._PMove._MoveDIR = Vector2.zero;
        yield return new WaitForSeconds(1f);
        while (playerCamera.transform.position.y != 35.45f)
        {
            yield return null;
            Vector3 target = new Vector3(playerCamera.transform.position.x, 35.45f, playerCamera.transform.position.z);
            playerCamera.transform.position = Vector3.MoveTowards(playerCamera.transform.position, target, 2f * Time.deltaTime);
        }
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector2.up * CameraFollowBB_Top, new Vector3(16.125f, 12f, 1f));
        Gizmos.DrawWireSphere(Vector2.up * CameraFollowBB_Top, 0.25f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector2.up * CameraFollowBB_Bottom, new Vector3(16.125f, 12f, 1f));
        Gizmos.DrawWireSphere(Vector2.up * CameraFollowBB_Bottom, 0.25f);
    }

    public void Vessel_IdleUp()
    {
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
    }

    private IEnumerator KrisShake_BeforeVessel()
    {
        yield return new WaitForSeconds(Random.Range(1.2f, 3f));
        if (krisCanShake)
        {
            KrisAnimator.transform.position = krisOriginalPos;
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
        StartCoroutine(KrisShake_BeforeVessel());
    }

    public void Kris_HeadDown()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_HeadDown");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }

    public void Kris_Look()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_Look");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }

    public void Kris_LookArmOut()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_LookArmOut");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }

    public void Kris_WaveAway()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_WaveAway");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }

    public void Kris_Confused()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_Confused");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }

    public void Kris_KneelHeadDown()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_KneelHeadDown");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }

    public void Kris_KneelLook()
    {
        KrisAnimator.transform.position = krisOriginalPos;
        KrisAnimator.Play("Kris_TS_Kneel");
        if (krisCanShake)
        {
            CutsceneUtils.ShakeTransform(KrisAnimator.transform, 0.1f, 1f);
        }
    }
}
