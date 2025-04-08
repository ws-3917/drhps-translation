using System;
using System.Collections;
using UnityEngine;

public class TRB_Projects_Shared : MonoBehaviour
{
    [Header("-= Cutscene References =-")]
    public Animator Alphys;

    public Animator Susie;

    public Animator Berdly;

    public Animator Noelle;

    public Animator Catti;

    public Animator Jockington;

    public Animator MonsterKid;

    public Animator Snowdrake;

    public Animator Temmie;

    public Animator Kris;

    public Transform Egg;

    public Animator Toriel;

    [Space(5f)]
    [SerializeField]
    private CHATBOXTEXT AfterProjectsText;

    [Space(5f)]
    [SerializeField]
    private GameObject ShadowMask;

    [SerializeField]
    private Transform Shadow;

    public SpriteRenderer DoorRenderer;

    public Sprite Sprite_DoorClosed;

    public Sprite Sprite_DoorOpen;

    [Header("-= Cutscene Sounds =-")]
    [SerializeField]
    private AudioClip[] CutsceneSounds;

    public static TRB_Projects_Shared instance;

    [Header("-= Project Scripts =-")]
    [SerializeField]
    private int CurrentProjectIndex;

    [SerializeField]
    private Component[] ProjectComponents;

    [SerializeField]
    private AudioClip[] temmieTalkSounds;

    private void Awake()
    {
        instance = this;
        ChatboxManager chatboxManager = ChatboxManager.Instance;
        chatboxManager.Event_OnLetterTyped = (Action)Delegate.Combine(chatboxManager.Event_OnLetterTyped, new Action(OnTemmieSpeak));
    }

    private void Start()
    {
        Alphys.SetBool("InCutscene", value: true);
        Alphys.Play("IdleNeutral");
        Susie.SetBool("InCutscene", value: true);
        Berdly.SetBool("InCutscene", value: true);
        Noelle.SetBool("InCutscene", value: true);
        MonsterKid.SetBool("InCutscene", value: true);
        Snowdrake.SetBool("InCutscene", value: true);
        Temmie.SetBool("InCutscene", value: true);
        Catti.SetBool("InCutscene", value: true);
        Jockington.SetBool("InCutscene", value: true);
        Toriel.SetBool("InCutscene", value: true);
        LightworldMenu.Instance.CanOpenMenu = false;
        if (CurrentProjectIndex <= 0)
        {
            Temmie.Play("Idle");
            Catti.Play("Catti_SitAtTable");
            Jockington.Play("Jockington_SitAtTable");
            CurrentProjectIndex = 0;
            ProjectComponents[CurrentProjectIndex].gameObject.SetActive(value: true);
        }
    }

    public void CreateNewLightShadow(Vector2 Position, Vector2 Scale)
    {
        CutsceneUtils.PlaySound(CutsceneSounds[0], CutsceneUtils.DRH_MixerChannels.Effect, 0.5f);
        ShadowMask.transform.position = Position;
        ShadowMask.transform.localScale = new Vector3(Scale.x, Scale.y, 1f);
        Shadow.gameObject.SetActive(value: true);
        ShadowMask.SetActive(value: true);
    }

    public void CreateNewLightShadow_NoSound(Vector2 Position, Vector2 Scale)
    {
        ShadowMask.transform.position = Position;
        ShadowMask.transform.localScale = new Vector3(Scale.x, Scale.y, 1f);
        Shadow.gameObject.SetActive(value: true);
        ShadowMask.SetActive(value: true);
    }

    public void RemoveLightShadow()
    {
        CutsceneUtils.PlaySound(CutsceneSounds[0], CutsceneUtils.DRH_MixerChannels.Effect, 0.5f);
        Shadow.gameObject.SetActive(value: false);
        ShadowMask.gameObject.SetActive(value: false);
    }

    public void ResetAllClassmates()
    {
        CutsceneUtils.StopAllEffects();
        Temmie.transform.position = new Vector2(3.15f, -1.3f);
        Temmie.Play("Temmie_Sit");
        Egg.position = new Vector2(4f, -0.175f);
        MonsterKid.transform.position = new Vector2(3.3f, -2.85f);
        MonsterKid.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(MonsterKid, "VelocityX", "VelocityY", Vector2.up);
        Snowdrake.transform.position = new Vector2(3.3f, -4.6f);
        Snowdrake.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(Snowdrake, "VelocityX", "VelocityY", Vector2.up);
        Catti.Play("Catti_SitAtTable");
        Jockington.Play("Jockington_SitAtTable");
        Catti.transform.position = new Vector2(-3.6f, -3.15f);
        Jockington.transform.position = new Vector2(-3.6f, -4.6f);
        Noelle.transform.position = new Vector2(-3.575f, -1.5f);
        Berdly.transform.position = new Vector2(-0.1f, -1.5f);
        Susie.transform.position = new Vector2(-0.125f, -3.25f);
        Kris.transform.position = new Vector2(-0.13f, -4.6f);
        Berdly.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(Berdly, "VelocityX", "VelocityY", Vector2.up);
        Noelle.Play("Idle");
        Susie.Play("Idle");
        Alphys.Play("IdleNeutral");
        CutsceneUtils.RotateCharacterToDirection(Kris, "MOVEMENTX", "MOVEMENTY", Vector2.up);
    }

    public void NextProject()
    {
        StartCoroutine(SwapProjectTimed());
    }

    private IEnumerator SwapProjectTimed()
    {
        UI_FADE.Instance.StartFadeIn(-1, 1f);
        yield return new WaitForSeconds(1f);
        ResetAllClassmates();
        if (CurrentProjectIndex >= 0)
        {
            ProjectComponents[CurrentProjectIndex].gameObject.SetActive(value: false);
        }
        CurrentProjectIndex++;
        if (CurrentProjectIndex < 5)
        {
            ProjectComponents[CurrentProjectIndex].gameObject.SetActive(value: true);
        }
        yield return null;
        if (CurrentProjectIndex >= 5)
        {
            CameraManager.instance.transform.position = Vector3.zero;
            UI_FADE.Instance.StartFadeOut();
            yield return new WaitForSeconds(1f);
            CutsceneUtils.RunFreshChat(AfterProjectsText, 0, ForcePosition: true, OnBottom: true);
            while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
            {
                yield return null;
            }
            UI_FADE.Instance.StartFadeIn(49, 1f);
        }
        else
        {
            UI_FADE.Instance.StartFadeOut();
        }
    }

    private void OnTemmieSpeak()
    {
        if (ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex].name == "Temmie")
        {
            ChatboxManager.Instance.TextVoiceEmitter.PlayOneShot(temmieTalkSounds[UnityEngine.Random.Range(0, temmieTalkSounds.Length)], 0.45f);
        }
    }

    private void OnDestroy()
    {
        ChatboxManager chatboxManager = ChatboxManager.Instance;
        chatboxManager.Event_OnLetterTyped = (Action)Delegate.Remove(chatboxManager.Event_OnLetterTyped, new Action(OnTemmieSpeak));
    }

    public void RotateAlphysToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Alphys, "VelocityX", "VelocityY", direction);
    }

    public void RotateAlphysToPosition(Vector2 position)
    {
        CutsceneUtils.RotateCharacterTowardsPosition(Alphys, "VelocityX", "VelocityY", position);
    }

    public void RotateTorielToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Toriel, "VelocityX", "VelocityY", direction);
    }

    public void Alphys_Smile()
    {
        Alphys.Play("Idle");
    }

    public void Alphys_Neutral()
    {
        Alphys.Play("IdleNeutral");
    }

    public void Alphys_Left()
    {
        RotateAlphysToDirection(Vector2.left);
    }

    public void Alphys_Right()
    {
        RotateAlphysToDirection(Vector2.right);
    }

    public void Alphys_Up()
    {
        RotateAlphysToDirection(Vector2.up);
    }

    public void Alphys_Down()
    {
        RotateAlphysToDirection(Vector2.down);
    }

    public void Toriel_Left()
    {
        RotateTorielToDirection(Vector2.left);
    }

    public void Toriel_Right()
    {
        RotateTorielToDirection(Vector2.right);
    }

    public void Toriel_Up()
    {
        RotateTorielToDirection(Vector2.up);
    }

    public void Toriel_Down()
    {
        RotateTorielToDirection(Vector2.down);
    }
}
