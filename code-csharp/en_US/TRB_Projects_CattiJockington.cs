using System.Collections;
using UnityEngine;

public class TRB_Projects_CattiJockington : MonoBehaviour
{
    [Header("-= Cutscene Chats =-")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private AudioClip[] CutsceneClips;

    [SerializeField]
    private Sprite[] BoardSprites;

    [SerializeField]
    private SpriteRenderer Board;

    [SerializeField]
    private Animator BoardRollAnimator;

    private int currentIndex;

    [SerializeField]
    private Transform CattyJumpscare;

    [SerializeField]
    private Transform BrattyJumpscare;

    private Animator Catti;

    private Animator Jockington;

    private void Start()
    {
        StartCoroutine(ProjectCutscene());
        Catti = TRB_Projects_Shared.instance.Catti;
        Jockington = TRB_Projects_Shared.instance.Jockington;
        Catti.Play("Idle");
        Jockington.Play("Idle");
        RotateCattiToDirection(Vector2.down);
        RotateJockingtonToDirection(Vector2.left);
        Catti.transform.position = new Vector2(-0.45f, 0.7f);
        Jockington.transform.position = new Vector2(3.2f, 0.7f);
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Alphys, "VelocityX", "VelocityY", Vector2.left);
        TRB_Projects_Shared.instance.CreateNewLightShadow(new Vector2(1.4f, 1.875f), new Vector2(1.5f, 0.8f));
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
        RealIncrementBoardIndex();
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        RealIncrementBoardIndex();
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: true);
        RealIncrementBoardIndex();
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.Noelle.Play("ShockUp");
        CutsceneUtils.PlaySound(CutsceneClips[7]);
        CutsceneUtils.ShakeTransform(TRB_Projects_Shared.instance.Noelle.transform);
        TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(new Vector2(-3.55f, -0.275f), new Vector2(0.8f, 0.8f));
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Susie, "VelocityX", "VelocityY", Vector2.left);
        RotateJockingtonToDirection(Vector2.left);
        RotateCattiToDirection(Vector2.left);
        yield return new WaitForSeconds(3f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 11, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        TRB_Projects_Shared.instance.Noelle.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Vector2.up);
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Susie, "VelocityX", "VelocityY", Vector2.up);
        RotateJockingtonToDirection(Vector2.down);
        RotateCattiToDirection(Vector2.down);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: true);
        TRB_Projects_Shared.instance.CreateNewLightShadow(new Vector2(1.4f, 1.875f), new Vector2(1.5f, 0.8f));
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.75f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 6, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.25f);
        TRB_Projects_Shared.instance.RemoveLightShadow();
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 7, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(new Vector2(1.4f, 2.95f), new Vector2(1.5f, 1.37f));
        CutsceneUtils.MoveTransformLinear(CattyJumpscare, new Vector3(2f, 3f), 0.25f);
        CutsceneUtils.MoveTransformLinear(BrattyJumpscare, new Vector3(0.45f, 3f), 0.25f);
        CutsceneUtils.PlaySound(CutsceneClips[1]);
        CutsceneUtils.PlaySound(CutsceneClips[2]);
        CutsceneUtils.PlaySound(CutsceneClips[3], CutsceneUtils.DRH_MixerChannels.Effect, 0.7f);
        CutsceneUtils.PlaySound(CutsceneClips[4], CutsceneUtils.DRH_MixerChannels.Effect, 0.25f);
        CutsceneUtils.PlaySound(CutsceneClips[5]);
        if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX"))
        {
            CutsceneUtils.ShakeTransform(CameraManager.instance.transform, 0.2f);
        }
        TRB_Projects_Shared.instance.Susie.Play("Susie_Shock_Up");
        TRB_Projects_Shared.instance.Berdly.Play("ShockUp");
        TRB_Projects_Shared.instance.Noelle.Play("ShockUp");
        TRB_Projects_Shared.instance.Temmie.Play("ShockUp");
        TRB_Projects_Shared.instance.MonsterKid.Play("Sunglasses_ShockUp");
        TRB_Projects_Shared.instance.Snowdrake.Play("ShockUp");
        TRB_Projects_Shared.instance.Alphys.Play("Shock");
        yield return new WaitForSeconds(4f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 8, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 10, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.NextProject();
    }

    public void RotateJockingtonToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Jockington, "VelocityX", "VelocityY", Direction);
    }

    public void RotateCattiToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Catti, "VelocityX", "VelocityY", Direction);
    }

    public void BerdlyCheckNoelle()
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Alphys, "VelocityX", "VelocityY", Vector2.down);
    }

    public void BerdlyUp()
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Vector2.up);
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Alphys, "VelocityX", "VelocityY", Vector2.left);
        RotateJockingtonToDirection(Vector2.right);
    }

    public void Jockington_Left()
    {
        RotateJockingtonToDirection(Vector2.left);
    }

    public void Jockington_Right()
    {
        RotateJockingtonToDirection(Vector2.right);
    }

    public void Catti_Down()
    {
        RotateCattiToDirection(Vector2.down);
    }

    public void Catti_Right()
    {
        RotateCattiToDirection(Vector2.right);
    }

    public void Catti_Smile()
    {
        Catti.Play("Catti_IdleSmile_Down");
    }

    public void IncrementBoardIndex()
    {
    }

    public void RealIncrementBoardIndex()
    {
        currentIndex++;
        CutsceneUtils.PlaySound(CutsceneClips[6], CutsceneUtils.DRH_MixerChannels.Effect, 0.5f);
        BoardRollAnimator.Play("TRB_Project_CattiJockington_RollUpSheet", -1, 0f);
        Board.sprite = BoardSprites[currentIndex];
    }
}
