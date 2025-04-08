using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EOTDREWRITE_FountainCutscene : MonoBehaviour
{
    [Header("Cutscene References")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private GameObject soulShine;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [SerializeField]
    private EOTDREWRITE_FountainColor fountainColorManager;

    [SerializeField]
    private Transform startingSusiePosition;

    [SerializeField]
    private int CutsceneIndex;

    [SerializeField]
    private Animator[] fountainAnimators;

    [SerializeField]
    private GameObject FountainSealUI;

    [Header("Cutscene Chats")]
    [SerializeField]
    private ChatboxSusieFountain Chatter;

    [SerializeField]
    private INT_Chat ChatterDefault;

    [SerializeField]
    private CHATBOXTEXT susieText;

    [Header("Cutscene Sounds")]
    [SerializeField]
    private AudioSource cutsceneSource;

    [SerializeField]
    private AudioClip SFX_soulShine;

    [SerializeField]
    private AudioClip SFX_dontforget;

    [SerializeField]
    private AudioClip mus_rumble;

    [SerializeField]
    private AudioClip mus_finalfountain;

    private void Start()
    {
        Kris = PlayerManager.Instance;
        Kris.PlayerSpriteRenderer.color = Color.black;
        mus_rumble.LoadAudioData();
        mus_finalfountain.LoadAudioData();
        Ralsei.RotateSusieToDirection(Vector2.up);
        StartCoroutine(Cutscene());
    }

    public void IncrementCutscene()
    {
        CutsceneIndex++;
    }

    private IEnumerator Cutscene()
    {
        yield return null;
        DarkworldMenu.Instance.CanOpenMenu = false;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Susie.transform.Find("Shadow").gameObject.SetActive(value: true);
        Susie.transform.Find("Shadow").GetComponent<SpriteRenderer>().color = Color.black;
        Susie.transform.Find("FountainLight").gameObject.SetActive(value: true);
        fountainColorManager.spriteRenderers.Add(Susie.transform.Find("FountainLight").GetComponent<SpriteRenderer>());
        fountainColorManager.originalDarknesses.Add(1f);
        Susie.SusieAnimator.GetComponent<SpriteRenderer>().color = Color.black;
        Susie.FollowingEnabled = false;
        Susie.transform.position = startingSusiePosition.position;
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
        yield return new WaitForSeconds(1f);
        while (Kris.transform.position.y < -2f && Susie.transform.position.y < -3f)
        {
            yield return null;
            if (Kris.transform.position != new Vector3(Kris.transform.position.x, -2f, 0f))
            {
                Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, new Vector3(Kris.transform.position.x, -2f, 0f), 1.5f * Time.deltaTime);
                Kris._PMove.AnimationOverriden = true;
                Kris._PMove._anim.SetBool("MOVING", value: true);
                Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                Kris._PMove._anim.speed = 0.65f;
            }
            else
            {
                Kris._PMove._anim.SetBool("MOVING", value: false);
                Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
            }
            if (Susie.transform.position != new Vector3(Susie.transform.position.x, -3f, 0f))
            {
                Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, new Vector3(Susie.transform.position.x, -3f, 0f), 1.5f * Time.deltaTime);
                Susie.AnimationOverriden = true;
                Susie.SusieAnimator.Play("Walk");
                Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                Susie.SusieAnimator.speed = 0.65f;
            }
            else
            {
                Susie.SusieAnimator.Play("Idle");
                Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                Susie.SusieAnimator.SetFloat("VelocityY", 1f);
                Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
            }
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
        Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
        Kris._PMove._anim.speed = 1f;
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", 1f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        Susie.SusieAnimator.speed = 1f;
        yield return new WaitForSeconds(1.5f);
        Chatter.RunText(susieText, 0, null, ResetCurrentTextIndex: false);
        while (CutsceneIndex < 1)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (Kris.transform.position.y < -0.75f)
        {
            yield return null;
            if (Kris.transform.position != new Vector3(Kris.transform.position.x, -0.75f, 0f))
            {
                Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, new Vector3(Kris.transform.position.x, -0.75f, 0f), 1.5f * Time.deltaTime);
                Kris._PMove.AnimationOverriden = true;
                Kris._PMove._anim.SetBool("MOVING", value: true);
                Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                Kris._PMove._anim.speed = 0.65f;
            }
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
        Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
        Kris._PMove._anim.speed = 1f;
        yield return new WaitForSeconds(1f);
        Chatter.RunText(susieText, 1, null, ResetCurrentTextIndex: false);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Crying);
        while (CutsceneIndex < 2)
        {
            yield return null;
        }
        soulShine.transform.position = Kris.transform.position;
        soulShine.SetActive(value: true);
        cutsceneSource.PlayOneShot(SFX_soulShine);
        MusicManager.PlaySong(mus_rumble, FadePreviousSong: false, 0f);
        yield return new WaitForSeconds(2.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Annoyed);
        UI_FADE.Instance.StartFadeIn(-1, 0.5f);
        while (CameraManager.instance.transform.position.y < 3.35f)
        {
            yield return null;
            if (CameraManager.instance.transform.position != new Vector3(CameraManager.instance.transform.position.x, 3.35f, -10f))
            {
                CameraManager.instance.transform.position = Vector3.MoveTowards(CameraManager.instance.transform.position, new Vector3(CameraManager.instance.transform.position.x, 3.35f, -10f), 1.5f * Time.deltaTime);
            }
        }
        CameraManager.instance.transform.position = new Vector3(1.5f, 3.5f, -10f);
        UI_FADE.Instance.StartFadeOut(0.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock);
        MusicManager.PlaySong(mus_finalfountain, FadePreviousSong: true, 0.15f);
        yield return new WaitForSeconds(2f);
        ChatterDefault.RUN();
        Kris.PlayerSpriteRenderer.color = Color.white;
        Kris._PMove.AnimationOverriden = false;
        Kris._PMove.InDarkworld = false;
        LightworldMenu.Instance.CanOpenMenu = false;
        while (CutsceneIndex < 3)
        {
            yield return null;
        }
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Crying);
        StartCoroutine(songFountainSync());
        while (CameraManager.instance.transform.position.y < 42f)
        {
            yield return null;
            if (CameraManager.instance.transform.position != new Vector3(CameraManager.instance.transform.position.x, 42f, -10f))
            {
                CameraManager.instance.transform.position = Vector3.MoveTowards(CameraManager.instance.transform.position, new Vector3(CameraManager.instance.transform.position.x, 42f, -10f), 4f * Time.deltaTime);
            }
        }
    }

    private IEnumerator songFountainSync()
    {
        yield return new WaitForSeconds(1.5f);
        MusicManager.StopSong(Fade: true, 0.5f);
        cutsceneSource.PlayOneShot(SFX_dontforget);
        Animator[] array = fountainAnimators;
        for (int i = 0; i < array.Length; i++)
        {
            array[i].SetTrigger("PlayFountainSeal");
        }
        yield return new WaitForSeconds(3f);
        FountainSealUI.SetActive(value: true);
        yield return new WaitForSeconds(1.5f);
        UI_FADE.Instance.StartFadeIn(-1, 0.25f);
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(23);
    }
}
