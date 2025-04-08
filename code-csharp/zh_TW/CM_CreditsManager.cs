using System.Collections;
using TMPro;
using UnityEngine;

public class CM_CreditsManager : MonoBehaviour
{
    [Header("Intro Disclaimer")]
    [SerializeField]
    private TextMeshPro DisclaimerText;

    [SerializeField]
    private AudioClip DenySFX;

    [SerializeField]
    private AudioClip ConfirmSFX;

    [SerializeField]
    private AudioClip FinishConfirmSFX;

    private string originalDisclaimerText;

    private int remainingConfirmPresses = 3;

    private bool canBeginConfirming;

    [Header("Credits Intro")]
    [SerializeField]
    private SpriteRenderer Piano;

    [SerializeField]
    private Transform PianoDuplicate;

    [SerializeField]
    private AudioClip NoiseSFX;

    [SerializeField]
    private AudioClip LeafSFX;

    [SerializeField]
    private AudioClip PlinkSFX;

    [SerializeField]
    private AudioClip SusieLaughSFX;

    [SerializeField]
    private AudioClip Music_CreditSong;

    [SerializeField]
    private CHATBOXTEXT AfterCreditsDialogue;

    private CameraManager PlayerCamera;

    private PlayerManager Kris;

    [SerializeField]
    private CM_KrisPianoAnimation KrisPianoController;

    [Header("Ending Cutscene")]
    [SerializeField]
    private Animator SusieAnimator;

    [SerializeField]
    private Animator RalseiAnimator;

    [Header("Debug")]
    private Vector3 pos1 = new Vector3(0f, 0f, 0f);

    [SerializeField]
    private Vector3 pos2 = new Vector3(0f, -13f, 0f);

    [SerializeField]
    private float ScrollSpeed;

    private void Start()
    {
        Music_CreditSong.LoadAudioData();
        originalDisclaimerText = DisclaimerText.text;
        DisclaimerText.text = originalDisclaimerText;
        if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
        {
            DisclaimerText.font = SettingsManager.Instance.DyslexicFont;
            DisclaimerText.extraPadding = false;
        }
        StartCoroutine(Disclaimer());
        PlayerCamera = CameraManager.instance;
        Kris = PlayerManager.Instance;
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }

    private void Update()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        if (canBeginConfirming)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && remainingConfirmPresses > 0)
            {
                CutsceneUtils.PlaySound(ConfirmSFX);
                remainingConfirmPresses--;
                DisclaimerText.text = originalDisclaimerText + string.Format("\n請再按 <color=yellow>{1}</color> 次 <color=yellow>{0}</color> 鍵確認繼續。", PlayerInput.Instance.Key_Confirm, remainingConfirmPresses);
            }
            else
            {
                DisclaimerText.text = originalDisclaimerText + string.Format("\n請再按 <color=yellow>{1}</color> 次 <color=yellow>{0}</color> 鍵確認繼續。", PlayerInput.Instance.Key_Confirm, remainingConfirmPresses);
            }
        }
        if (GonerMenu.Instance.GonerMenuOpen)
        {
            if (MusicManager.Instance.source.isPlaying)
            {
                MusicManager.PauseMusic();
            }
        }
        else if (!MusicManager.Instance.source.isPlaying)
        {
            MusicManager.ResumeMusic();
        }
    }

    private IEnumerator Disclaimer()
    {
        yield return new WaitForSeconds(1f);
        CutsceneUtils.PlaySound(DenySFX);
        CutsceneUtils.FadeInText3D(DisclaimerText, 0.5f);
        yield return new WaitForSeconds(5f);
        canBeginConfirming = true;
        while (remainingConfirmPresses > 0)
        {
            yield return null;
        }
        CutsceneUtils.PlaySound(FinishConfirmSFX);
        CutsceneUtils.FadeOutText3D(DisclaimerText);
        yield return new WaitForSeconds(3f);
        StartCoroutine(Credits());
    }

    private IEnumerator Credits()
    {
        CutsceneUtils.FadeInSprite(Piano, 0.5f);
        yield return new WaitForSeconds(1.5f);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        while (Kris.transform.position.y < 1f)
        {
            yield return null;
            Kris.transform.position = Vector3.MoveTowards(Kris.transform.position, new Vector3(0f, 1f, 0f), 4f * Time.deltaTime);
            Kris._PMove._anim.SetBool("MOVING", value: true);
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.PlaySound(NoiseSFX);
        Kris.transform.position = new Vector2(0f, 2.25f);
        Kris._PMove._anim.Play("Kris_CM_LWPianoNote3");
        yield return new WaitForSeconds(2.5f);
        if (!DRHDebugManager.instance.DebugModeEnabled)
        {
            KrisPianoController.PlayRecording();
        }
        GonerMenu.Instance.ShowMusicCredit("memphis", "puzzlerat");
        MusicManager.Instance.source.loop = false;
        MusicManager.PlaySong(Music_CreditSong, FadePreviousSong: false, 0f);
        yield return new WaitForSeconds(9f);
        while (PlayerCamera.transform.position.y != pos2.y)
        {
            yield return null;
            PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, new Vector3(0f, pos2.y, -10f), ScrollSpeed * Time.deltaTime);
            if (PlayerCamera.transform.position.y <= -30f && (Vector2)Kris.transform.position != new Vector2(0f, -87.6f))
            {
                Kris.transform.position = new Vector2(0f, -87.6f);
                Kris._PMove.InDarkworld = true;
                KrisPianoController.IsDarkworldVarient = true;
                ChatboxManager.Instance.InDarkworld = true;
                Kris._PMove._anim.Play("Kris_CM_LWPianoNote3");
            }
        }
        yield return new WaitForSeconds(8f);
        Kris.transform.position = new Vector2(0.325f, -88.9f);
        CutsceneUtils.PlaySound(LeafSFX);
        Kris._PMove._anim.Play("DARKWORLD_KRIS_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        yield return new WaitForSeconds(1.5f);
        Kris._PMove.RotatePlayerAnim(Vector2.down);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(AfterCreditsDialogue, 0, ForcePosition: true, OnBottom: false);
        MonoBehaviour.print(MusicManager.Instance.source.time);
    }

    public void BeginSusieWalkToPiano()
    {
        StartCoroutine(SusieWalkToPiano());
        CutsceneUtils.RotateCharacterToDirection(RalseiAnimator, "VelocityX", "VelocityY", Vector2.up);
    }

    public void RalseiSusieLookEachother()
    {
        SusieAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(SusieAnimator, "VelocityX", "VelocityY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(RalseiAnimator, "VelocityX", "VelocityY", Vector2.left);
    }

    public void SusieScratchHead()
    {
        SusieAnimator.Play("SusieDarkworld_Scratch");
    }

    public void RalseiGiggle()
    {
        RalseiAnimator.Play("Ralsei_Laugh");
        Kris._PMove.RotatePlayerAnim(Vector2.down);
    }

    public void FinishHypothetical()
    {
        LightworldMenu.Instance.CanOpenMenu = true;
        DarkworldMenu.Instance.CanOpenMenu = true;
        UI_FADE.Instance.StartFadeIn(37, 0.25f, UnpauseOnEnd: false, NewMainMenuManager.MainMenuStates.Hypothetical);
    }

    private IEnumerator SusieWalkToPiano()
    {
        while (SusieAnimator.transform.position.y != -89.85f)
        {
            yield return null;
            SusieAnimator.Play("Walk");
            SusieAnimator.SetFloat("VelocityMagnitude", 1f);
            CutsceneUtils.RotateCharacterToDirection(SusieAnimator, "VelocityX", "VelocityY", Vector2.up);
            SusieAnimator.transform.position = Vector3.MoveTowards(SusieAnimator.transform.position, new Vector2(-0.75f, -89.85f), 2f * Time.deltaTime);
            Kris._PMove.RotatePlayerAnimTowardsPosition(SusieAnimator.transform.position);
        }
        SusieAnimator.Play("Idle");
        SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.PlaySound(PlinkSFX);
        yield return new WaitForSeconds(1.5f);
        CutsceneUtils.PlaySound(SusieLaughSFX);
        SusieAnimator.Play("SusieDarkworld_LaughRight");
        yield return new WaitForSeconds(2f);
        CutsceneUtils.RunFreshChat(AfterCreditsDialogue, 1, ForcePosition: true, OnBottom: false);
        SusieAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(SusieAnimator, "VelocityX", "VelocityY", Vector2.down);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos1, new Vector3(16.125f, 12f, 1f));
        Gizmos.DrawWireCube(pos2, new Vector3(16.125f, 12f, 1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos1, 0.5f);
        Gizmos.DrawWireSphere(pos2, 0.5f);
        float num = Mathf.Max(pos1.y + 6f, pos2.y + 6f);
        float num2 = Mathf.Min(pos1.y - 6f, pos2.y - 6f);
        float y = num - num2;
        Vector3 center = new Vector3((pos1.x + pos2.x) / 2f, (num + num2) / 2f, (pos1.z + pos2.z) / 2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(16.125f, y, 1f));
    }
}
