using System.Collections;
using UnityEngine;

public class TRB_Projects_SusieKris : MonoBehaviour
{
    [Header("-= Cutscene Chats =-")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private AudioClip[] CutsceneClips;

    [SerializeField]
    private AudioClip ProjectMusic;

    [SerializeField]
    private AudioClip Music_Legend;

    private Animator Susie;

    private Animator Kris;

    private bool CutoutFollowKris;

    private bool KrisChangingCutout;

    [Space(10f)]
    [SerializeField]
    private Transform CutoutTransform;

    [SerializeField]
    private SpriteRenderer CutoutRenderer;

    [SerializeField]
    private Sprite[] CutoutSprites;

    [Space(10f)]
    [SerializeField]
    private SpriteRenderer Desk;

    [Space(10f)]
    [SerializeField]
    private Transform SecondaryCutoutTransform;

    [SerializeField]
    private SpriteRenderer SecondaryCutoutRenderer;

    [Space(10f)]
    [SerializeField]
    private Transform Rose;

    private void Start()
    {
        StartCoroutine(ProjectCutscene());
        Susie = TRB_Projects_Shared.instance.Susie;
        Kris = TRB_Projects_Shared.instance.Kris;
        Susie.Play("Susie_TRB_NotepadWorry");
        MoveDeskToBack(MoveToBack: true);
        Susie.transform.position = new Vector2(0f, 0.7f);
        Kris.transform.position = new Vector2(2.025f, 1.65f);
        RotateSusieToDirection(Vector2.down);
        RotateKrisToDirection(Vector2.down);
        MusicManager.Instance.source.pitch = 1f;
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Alphys, "VelocityX", "VelocityY", Vector2.left);
        TRB_Projects_Shared.instance.CreateNewLightShadow(new Vector2(0.65f, 2.05f), new Vector2(1.5f, 1.08f));
    }

    private void LateUpdate()
    {
        if (CutoutFollowKris)
        {
            CutoutTransform.position = Kris.transform.position;
        }
    }

    public IEnumerator ProjectCutscene()
    {
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Susie.Play("Susie_TRB_NotepadShock");
        CutsceneUtils.ShakeTransform(Susie.transform, 0.25f, 1.25f);
        CutsceneUtils.PlaySound(CutsceneClips[0]);
        yield return new WaitForSeconds(1.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Susie_IdleRight();
        TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(new Vector2(1.55f, 2.05f), new Vector2(1.75f, 1.08f));
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.PlaySound(CutsceneClips[1]);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.PlaySound(CutsceneClips[2]);
        CutsceneUtils.MoveTransformOnArc(Kris.transform, new Vector2(3.8f, 1.05f), 0.75f, 0.75f);
        Kris.Play("Kris_Fall_Right");
        yield return new WaitForSeconds(0.75f);
        CutsceneUtils.PlaySound(CutsceneClips[3]);
        Kris.GetComponent<SpriteRenderer>().flipX = true;
        Kris.Play("Kris_KnockedOut");
        yield return new WaitForSeconds(1.5f);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        Kris.Play("Kris_Defeated");
        MoveDeskToBack(MoveToBack: false);
        yield return new WaitForSeconds(1f);
        Kris.GetComponent<SpriteRenderer>().flipX = false;
        Kris.Play("OVERWORLD_NOELLE_IDLE");
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.5f);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(-2.425f, 1.4f), 1.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if ((Vector2)Kris.transform.position == new Vector2(-2.425f, 1.4f))
            {
                Kris.SetBool("MOVING", value: false);
                RotateKrisToDirection(Vector2.up);
            }
            yield return null;
        }
        while ((Vector2)Kris.transform.position != new Vector2(-2.425f, 1.4f))
        {
            yield return null;
        }
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        yield return new WaitForSeconds(0.5f);
        CutoutRenderer.enabled = true;
        CutoutFollowKris = true;
        Kris.SetBool("MOVING", value: true);
        Kris.Play("TRB_KrisArmor_Walk");
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 1.25f);
        yield return new WaitForSeconds(1.25f);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.25f);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        CutoutFollowKris = false;
        CutoutTransform.position = new Vector2(2f, 1.9f);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("AbovePlayer");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = false;
        CutoutRenderer.sortingOrder = 5;
        yield return new WaitForSeconds(0.5f);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(3.75f, 1.4f), 0.35f);
        yield return new WaitForSeconds(0.35f);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.15f);
        GonerMenu.Instance.ShowMusicCredit("NULL", "Sooski");
        MusicManager.PlaySong(Music_Legend, FadePreviousSong: false, 0f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Kris_ChangeCardboardCutout(CutoutSprites[1]);
        while (KrisChangingCutout)
        {
            yield return null;
        }
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Kris_ChangeCardboardCutout(CutoutSprites[2], 1.35f);
        while (KrisChangingCutout)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 6, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Kris_ChangeCardboardCutout_HideSelf(CutoutSprites[1], 1.5f);
        while (KrisChangingCutout)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 7, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 8, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(3.75f, 1.4f), 0.35f);
        yield return new WaitForSeconds(0.35f);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 9, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Kris_ChangeCardboardCutout(CutoutSprites[2], 1.75f);
        while (KrisChangingCutout)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 10, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        StartCoroutine(Kris_GoToJail(CutoutSprites[3], 1.75f));
        KrisChangingCutout = true;
        while (KrisChangingCutout)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(new Vector2(1.55f, 2.05f), new Vector2(1.75f, 1.08f));
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 11, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        CutsceneUtils.MoveTransformLinear(CutoutTransform, new Vector2(CutoutTransform.position.x, 0.775f), 0.25f);
        CutsceneUtils.PlaySound(CutsceneClips[5]);
        yield return new WaitForSeconds(0.25f);
        CutoutTransform.position = new Vector2(1.95f, 0.775f);
        CutoutTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
        CutsceneUtils.ShakeTransform(CutoutTransform);
        CutoutRenderer.sprite = CutoutSprites[4];
        yield return new WaitForSeconds(1.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 12, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        StartCoroutine(Kris_ChangingCardboardCutout_JumpFromTable(CutoutSprites[5], 1.25f));
        KrisChangingCutout = true;
        while (KrisChangingCutout)
        {
            yield return null;
        }
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 13, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Kris_ChangeCardboardCutout(CutoutSprites[6], 2f);
        while (KrisChangingCutout)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 14, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 15, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        Susie_IdleRight();
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        Kris.Play("TRB_KrisArmor_BeginFall");
        yield return new WaitForSeconds(1f);
        CutsceneUtils.PlaySound(CutsceneClips[6], CutsceneUtils.DRH_MixerChannels.Effect, 1f, 2f);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.PlaySound(CutsceneClips[3]);
        Kris.transform.rotation = Quaternion.Euler(Vector3.zero);
        Kris.transform.position = new Vector3(4.35f, Kris.transform.position.y, Kris.transform.position.y);
        Kris.Play("TRB_KrisArmor_Fallen");
        yield return null;
        CutsceneUtils.ShakeTransform(Kris.transform, 0.25f, 1f);
        yield return new WaitForSeconds(2f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 16, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.PlaySound(CutsceneClips[1]);
        Kris.Play("TRB_KrisArmor_GetUp");
        CutsceneUtils.ShakeTransform(Kris.transform, 0.25f, 0.5f);
        yield return new WaitForSeconds(1f);
        Kris_IdleDown();
        Kris.Play("TRB_KrisArmor_Idle");
        yield return new WaitForSeconds(0.5f);
        Kris_ChangeCardboardCutout(CutoutSprites[1], 2f);
        while (KrisChangingCutout)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 17, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 18, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        MusicManager.StopSong(Fade: false, 0f);
        TRB_Projects_Shared.instance.CreateNewLightShadow(new Vector2(3.75f, 2.35f), new Vector2(0.75f, 0.75f));
        Susie_RevertToIdleAnim();
        Susie_IdleRight();
        yield return new WaitForSeconds(0.5f);
        Kris.Play("TRB_KrisArmor_Bow");
        yield return new WaitForSeconds(0.65f);
        CutsceneUtils.PlaySound(CutsceneClips[8]);
        BounceCharacter(TRB_Projects_Shared.instance.Jockington.transform);
        TRB_Projects_Shared.instance.Jockington.GetComponent<INT_TalkingAnimation>().enabled = false;
        TRB_Projects_Shared.instance.Jockington.SetBool("Talking", value: true);
        TRB_Projects_Shared.instance.Jockington.Play("Talking");
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Jockington, "VelocityX", "VelocityY", Vector2.right);
        yield return null;
        BounceCharacter(TRB_Projects_Shared.instance.Catti.transform);
        yield return null;
        BounceCharacter(TRB_Projects_Shared.instance.Berdly.transform);
        yield return null;
        BounceCharacter(TRB_Projects_Shared.instance.Snowdrake.transform);
        TRB_Projects_Shared.instance.Snowdrake.GetComponent<INT_TalkingAnimation>().enabled = false;
        TRB_Projects_Shared.instance.Snowdrake.SetBool("Talking", value: true);
        yield return null;
        BounceCharacter(TRB_Projects_Shared.instance.MonsterKid.transform);
        TRB_Projects_Shared.instance.MonsterKid.GetComponent<INT_TalkingAnimation>().enabled = false;
        TRB_Projects_Shared.instance.MonsterKid.SetBool("Talking", value: true);
        yield return null;
        BounceCharacter(TRB_Projects_Shared.instance.Temmie.transform);
        yield return null;
        BounceCharacter(TRB_Projects_Shared.instance.Egg);
        yield return new WaitForSeconds(1.75f);
        TRB_Projects_Shared.instance.Jockington.GetComponent<INT_TalkingAnimation>().enabled = true;
        TRB_Projects_Shared.instance.Jockington.SetBool("Talking", value: false);
        TRB_Projects_Shared.instance.Jockington.Play("Jockington_SitAtTable");
        TRB_Projects_Shared.instance.Snowdrake.GetComponent<INT_TalkingAnimation>().enabled = true;
        TRB_Projects_Shared.instance.Snowdrake.SetBool("Talking", value: false);
        TRB_Projects_Shared.instance.MonsterKid.GetComponent<INT_TalkingAnimation>().enabled = true;
        TRB_Projects_Shared.instance.MonsterKid.SetBool("Talking", value: false);
        CutsceneUtils.PlaySound(CutsceneClips[9]);
        CutsceneUtils.MoveTransformLinear(Rose, new Vector3(4.175f, 1.225f, 0f), 1f);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.PlaySound(CutsceneClips[10]);
        yield return new WaitForSeconds(0.35f);
        Kris.Play("TRB_KrisArmor_BowEnd");
        yield return new WaitForSeconds(1.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 19, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        TRB_Projects_Shared.instance.RemoveLightShadow();
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 20, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.NextProject();
    }

    private void Kris_ChangeCardboardCutout(Sprite newCutout, float Multiplier = 1f)
    {
        KrisChangingCutout = true;
        StartCoroutine(Kris_ChangingCardboardCutout(newCutout, Multiplier));
    }

    private void Kris_ChangeCardboardCutout_HideSelf(Sprite newCutout, float Multiplier = 1f)
    {
        KrisChangingCutout = true;
        StartCoroutine(Kris_ChangingCardboardCutout_HideSelf(newCutout, Multiplier));
    }

    private IEnumerator Kris_ChangingCardboardCutout(Sprite newCutout, float Multiplier = 1f)
    {
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 0.5f / Multiplier);
        yield return new WaitForSeconds(0.5f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.25f / Multiplier);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("Default");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = true;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutoutFollowKris = true;
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(-2.425f, 1.4f), 1.5f / Multiplier);
        yield return new WaitForSeconds(1.5f / Multiplier);
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        yield return new WaitForSeconds(0.5f / Multiplier);
        CutoutRenderer.enabled = true;
        CutoutRenderer.sprite = newCutout;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 1.25f / Multiplier);
        yield return new WaitForSeconds(1.25f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.15f / Multiplier);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        CutoutFollowKris = false;
        CutoutTransform.position = new Vector2(2f, 1.9f);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("AbovePlayer");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = false;
        CutoutRenderer.sortingOrder = 5;
        yield return new WaitForSeconds(0.25f / Multiplier);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(3.75f, 1.4f), 0.35f / Multiplier);
        yield return new WaitForSeconds(0.35f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        KrisChangingCutout = false;
    }

    private IEnumerator Kris_GoToJail(Sprite newCutout, float Multiplier = 1f)
    {
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 0.5f / Multiplier);
        yield return new WaitForSeconds(0.5f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.25f / Multiplier);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("Default");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = true;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutoutFollowKris = true;
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(-2.425f, 1.4f), 1.5f / Multiplier);
        yield return new WaitForSeconds(1.5f / Multiplier);
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        yield return new WaitForSeconds(0.5f / Multiplier);
        CutoutRenderer.enabled = true;
        CutoutRenderer.sprite = newCutout;
        CutoutFollowKris = true;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 1.25f / Multiplier);
        yield return new WaitForSeconds(1.25f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.15f / Multiplier);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        CutoutFollowKris = false;
        CutoutTransform.position = new Vector2(2f, 1.9f);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("AbovePlayer");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = false;
        CutoutRenderer.sortingOrder = 5;
        yield return new WaitForSeconds(0.25f / Multiplier);
        CutsceneUtils.PlaySound(CutsceneClips[1]);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2f, 1.6f), 0.35f / Multiplier);
        yield return new WaitForSeconds(0.35f / Multiplier / 2f);
        MoveDeskToBack(MoveToBack: true);
        yield return new WaitForSeconds(0.35f / Multiplier / 2f);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        KrisChangingCutout = false;
    }

    private IEnumerator Kris_ChangingCardboardCutout_JumpFromTable(Sprite newCutout, float Multiplier = 1f)
    {
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        CutsceneUtils.MoveTransformOnArc(Kris.transform, new Vector2(2.025f, 1.4f), 1f, 1f / Multiplier);
        yield return new WaitForSeconds(1f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        yield return new WaitForSeconds(0.25f / Multiplier);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(-2.425f, 1.4f), 1.5f / Multiplier);
        yield return new WaitForSeconds(1.5f / Multiplier);
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        yield return new WaitForSeconds(0.5f / Multiplier);
        CutoutRenderer = SecondaryCutoutRenderer;
        CutoutTransform = SecondaryCutoutTransform;
        CutoutRenderer.enabled = true;
        CutoutRenderer.sprite = newCutout;
        CutoutFollowKris = true;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 1.25f / Multiplier);
        yield return new WaitForSeconds(1.25f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.15f / Multiplier);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        CutoutFollowKris = false;
        CutoutTransform.position = new Vector2(2f, 1.9f);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("AbovePlayer");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = false;
        CutoutRenderer.sortingOrder = 5;
        yield return new WaitForSeconds(0.25f / Multiplier);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(3.75f, 1.4f), 0.35f / Multiplier);
        yield return new WaitForSeconds(0.35f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        KrisChangingCutout = false;
    }

    private IEnumerator Kris_ChangingCardboardCutout_HideSelf(Sprite newCutout, float Multiplier = 1f)
    {
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 0.5f / Multiplier);
        yield return new WaitForSeconds(0.5f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.25f / Multiplier);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("Default");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = true;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.left);
        CutoutFollowKris = true;
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(-2.425f, 1.4f), 1.5f / Multiplier);
        yield return new WaitForSeconds(1.5f / Multiplier);
        MoveDeskToBack(MoveToBack: false);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.up);
        yield return new WaitForSeconds(0.5f / Multiplier);
        CutoutRenderer.enabled = true;
        CutoutRenderer.sprite = newCutout;
        Kris.SetBool("MOVING", value: true);
        RotateKrisToDirection(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, new Vector2(2.025f, 1.4f), 1.25f / Multiplier);
        yield return new WaitForSeconds(1.25f / Multiplier);
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        yield return new WaitForSeconds(0.15f / Multiplier);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        CutoutFollowKris = false;
        CutoutTransform.position = new Vector2(2f, 1.9f);
        CutoutRenderer.sortingLayerID = SortingLayer.NameToID("AbovePlayer");
        CutoutTransform.GetComponent<SPR_YSorting>().enabled = false;
        CutoutRenderer.sortingOrder = 5;
        Kris.SetBool("MOVING", value: false);
        RotateKrisToDirection(Vector2.down);
        KrisChangingCutout = false;
    }

    public void RotateSusieToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Susie, "VelocityX", "VelocityY", Direction);
    }

    public void RotateKrisToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Kris, "MOVEMENTX", "MOVEMENTY", Direction);
    }

    public void MoveDeskToBack(bool MoveToBack)
    {
        if (!MoveToBack)
        {
            Desk.sortingLayerID = SortingLayer.NameToID("AbovePlayer");
        }
        else
        {
            Desk.sortingLayerID = SortingLayer.NameToID("BelowPlayer");
        }
    }

    public void Berdly_Exclaim()
    {
        TRB_Projects_Shared.instance.Berdly.Play("Exclaim");
    }

    public void Berdly_IdleUp()
    {
        TRB_Projects_Shared.instance.Berdly.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Vector2.up);
    }

    public void Susie_IdleDown()
    {
        RotateSusieToDirection(Vector2.down);
    }

    public void Susie_IdleUp()
    {
        RotateSusieToDirection(Vector2.up);
    }

    public void Susie_IdleLeft()
    {
        RotateSusieToDirection(Vector2.left);
    }

    public void Susie_IdleRight()
    {
        RotateSusieToDirection(Vector2.right);
    }

    public void Susie_RevertToIdleAnim()
    {
        Susie.Play("Susie_TRB_NotepadIdle");
    }

    public void Susie_ReadNotepad()
    {
        Susie.Play("Susie_TRB_NotepadRead");
    }

    public void Susie_NotepadWorry()
    {
        Susie.Play("Susie_TRB_NotepadWorry");
    }

    public void Susie_NotepadVictory()
    {
        Susie.Play("Susie_TRB_NotepadVictory");
    }

    public void Susie_NotepadSurprised()
    {
        Susie.Play("Susie_TRB_NotepadShock");
    }

    public void Kris_IdleDown()
    {
        RotateKrisToDirection(Vector2.down);
    }

    public void Kris_IdleUp()
    {
        RotateKrisToDirection(Vector2.up);
    }

    public void Kris_IdleLeft()
    {
        RotateKrisToDirection(Vector2.left);
    }

    public void Kris_IdleRight()
    {
        RotateKrisToDirection(Vector2.right);
    }

    public void Kris_EquipWeapon()
    {
        CutsceneUtils.PlaySound(CutsceneClips[7]);
        Kris.Play("TRB_KrisArmor_PullWeapon");
    }

    public void BounceCharacter(Transform character)
    {
        StartCoroutine(BounceCharacter_Timed(character));
    }

    private IEnumerator BounceCharacter_Timed(Transform character)
    {
        for (int i = 0; i < 4; i++)
        {
            CutsceneUtils.MoveTransformOnArc(character, character.transform.position, 0.25f, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
