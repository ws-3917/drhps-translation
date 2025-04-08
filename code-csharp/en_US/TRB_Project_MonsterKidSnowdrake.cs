using System.Collections;
using UnityEngine;

public class TRB_Project_MonsterKidSnowdrake : MonoBehaviour
{
    [Header("-= Cutscene Chats =-")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private AudioClip[] CutsceneClips;

    private Animator MonsterKid;

    private Animator Snowdrake;

    private void Start()
    {
        StartCoroutine(ProjectCutscene());
        MonsterKid = TRB_Projects_Shared.instance.MonsterKid;
        Snowdrake = TRB_Projects_Shared.instance.Snowdrake;
        RotateMonsterKidToDirection(Vector2.down);
        RotateSnowdrakeToDirection(Vector2.down);
        MonsterKid.transform.position = new Vector2(0.15f, 0.7f);
        Snowdrake.transform.position = new Vector2(2.45f, 0.7f);
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
        CutsceneUtils.MoveTransformLinear(MonsterKid.transform, MonsterKid.transform.position + Vector3.left / 2f, 0.5f);
        CutsceneUtils.MoveTransformLinear(Snowdrake.transform, Snowdrake.transform.position + Vector3.right / 2f, 0.5f);
        RotateMonsterKidToDirection(Vector2.right);
        RotateSnowdrakeToDirection(Vector2.left);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        RotateMonsterKidToDirection(Vector2.down);
        RotateSnowdrakeToDirection(Vector2.down);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        CutsceneUtils.MoveTransformLinear(MonsterKid.transform, MonsterKid.transform.position - Vector3.left / 2f, 0.5f);
        CutsceneUtils.MoveTransformLinear(Snowdrake.transform, Snowdrake.transform.position - Vector3.right / 2f, 0.5f);
        RotateMonsterKidToDirection(Vector2.up);
        RotateSnowdrakeToDirection(Vector2.up);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        RotateMonsterKidToDirection(Vector2.up);
        RotateSnowdrakeToDirection(Vector2.up);
        CutsceneUtils.MoveTransformLinear(Snowdrake.transform, Snowdrake.transform.position - Vector3.right * 1.25f, 0.5f);
        yield return new WaitForSeconds(0.85f);
        CutsceneUtils.PlaySound(CutsceneClips[0]);
        yield return new WaitForSeconds(0.25f);
        CutsceneUtils.MoveTransformLinear(Snowdrake.transform, Snowdrake.transform.position + Vector3.right * 1.25f, 0.5f);
        yield return new WaitForSeconds(1f);
        MonsterKid.Play("IdleSunglasses");
        RotateMonsterKidToDirection(Vector2.down);
        RotateSnowdrakeToDirection(Vector2.down);
        CutsceneUtils.PlaySound(CutsceneClips[1]);
        yield return new WaitForSeconds(1.25f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.25f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 6, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.RemoveLightShadow();
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 7, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.MoveTransformLinear(Snowdrake.transform, Snowdrake.transform.position + Vector3.right * 1.5f + Vector3.down * 0.5f, 1.5f);
        Snowdrake.Play("Walk");
        RotateSnowdrakeToDirection(Vector2.right);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.MoveTransformLinear(MonsterKid.transform, MonsterKid.transform.position + Vector3.right * 1f, 0.5f);
        MonsterKid.Play("WalkSunglasses");
        RotateMonsterKidToDirection(Vector2.right);
        yield return new WaitForSeconds(0.5f);
        RotateSnowdrakeToDirection(Vector2.left);
        MonsterKid.Play("MonsterKid_ProjectTrip_BeginTrip");
        CutsceneUtils.ShakeTransform(MonsterKid.transform, 0.25f, 0.5f);
        Snowdrake.Play("Idle");
        CutsceneUtils.PlaySound(CutsceneClips[2]);
        yield return new WaitForSeconds(0.75f);
        MonsterKid.Play("MonsterKid_ProjectTrip_Trip");
        yield return new WaitForSeconds(3f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 8, ForcePosition: true, OnBottom: true);
        RotateSnowdrakeToDirection(Vector2.down);
        TRB_Projects_Shared.instance.RotateAlphysToDirection(Vector2.down);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        MonsterKid.Play("MonsterKid_ProjectTrip_TripReveal");
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 9, ForcePosition: true, OnBottom: true);
        TRB_Projects_Shared.instance.RotateAlphysToDirection(Vector2.left);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.25f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 10, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.NextProject();
    }

    public void RotateMonsterKidToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.MonsterKid, "VelocityX", "VelocityY", Direction);
    }

    public void RotateSnowdrakeToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Snowdrake, "VelocityX", "VelocityY", Direction);
    }

    public void PlaySplatSFX()
    {
        CutsceneUtils.PlaySound(CutsceneClips[3]);
    }

    public void MonsterKid_Down()
    {
        RotateMonsterKidToDirection(Vector2.down);
    }

    public void Snowdrake_Down()
    {
        RotateSnowdrakeToDirection(Vector2.down);
    }

    public void MonsterKid_Right()
    {
        RotateMonsterKidToDirection(Vector2.right);
    }

    public void Snowdrake_Right()
    {
        RotateSnowdrakeToDirection(Vector2.right);
    }

    public void MonsterKid_Left()
    {
        RotateMonsterKidToDirection(Vector2.left);
    }

    public void Snowdrake_Left()
    {
        RotateSnowdrakeToDirection(Vector2.left);
    }

    public void MonsterKid_Up()
    {
        RotateMonsterKidToDirection(Vector2.up);
    }

    public void Snowdrake_Up()
    {
        RotateSnowdrakeToDirection(Vector2.up);
    }
}
