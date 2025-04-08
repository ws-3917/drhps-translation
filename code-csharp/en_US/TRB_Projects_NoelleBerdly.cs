using System.Collections;
using UnityEngine;

public class TRB_Projects_NoelleBerdly : MonoBehaviour
{
    [Header("-= Cutscene Chats =-")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private AudioClip[] CutsceneClips;

    private Animator Noelle;

    private Animator Berdly;

    private bool LightFollowDucttape;

    [SerializeField]
    private SpriteRenderer DuctTape;

    [SerializeField]
    private Sprite DuctTape_Hit;

    [SerializeField]
    private Vector2[] NoelleLeaveWalkPositions;

    [SerializeField]
    private Vector2[] NoelleLeaveWalkDirections;

    private int NoelleLeaveWalkIndex;

    private void Start()
    {
        StartCoroutine(ProjectCutscene());
        Noelle = TRB_Projects_Shared.instance.Noelle;
        Berdly = TRB_Projects_Shared.instance.Berdly;
        Noelle.Play("Idle");
        Berdly.Play("Idle");
        RotateNoelleToDirection(Vector2.down);
        RotateBerdlyToDirection(Vector2.down);
        TRB_Projects_Shared.instance.RotateTorielToDirection(Vector2.down);
        Noelle.transform.position = new Vector2(-0.45f, 0.7f);
        Berdly.transform.position = new Vector2(2.75f, 0.7f);
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Alphys, "VelocityX", "VelocityY", Vector2.left);
        TRB_Projects_Shared.instance.CreateNewLightShadow(new Vector2(1.1f, 1.875f), new Vector2(1.5f, 0.8f));
    }

    private void Update()
    {
        if (LightFollowDucttape)
        {
            TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(DuctTape.transform.position, new Vector2(0.8f, 0.8f));
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
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(new Vector2(-0.15f, -1.95f), new Vector2(0.8f, 0.8f));
        TRB_Projects_Shared.instance.Susie.Play("Susie_TRB_ThrowDucttape", -1, 0f);
        CutsceneUtils.PlaySound(CutsceneClips[1]);
        yield return new WaitForSeconds(1.183f);
        CutsceneUtils.PlaySound(CutsceneClips[2]);
        DuctTape.gameObject.SetActive(value: true);
        LightFollowDucttape = true;
        CutsceneUtils.MoveTransformOnArc(DuctTape.transform, Berdly.transform.position + Vector3.up, 2f, 1f);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.ShakeTransform(Berdly.transform);
        CutsceneUtils.PlaySound(CutsceneClips[3]);
        CutsceneUtils.PlaySound(CutsceneClips[4]);
        Berdly.Play("Shock");
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.MoveTransformLinear(DuctTape.transform, Berdly.transform.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        DuctTape.sprite = DuctTape_Hit;
        yield return new WaitForSeconds(0.75f);
        LightFollowDucttape = false;
        TRB_Projects_Shared.instance.CreateNewLightShadow_NoSound(new Vector2(1.1f, 1.875f), new Vector2(1.5f, 0.8f));
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        DuctTape.sortingLayerID = SortingLayer.NameToID("BelowPlayer");
        TRB_Projects_Shared.instance.Susie.Play("Idle", -1, 0f);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
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
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.DoorRenderer.sprite = TRB_Projects_Shared.instance.Sprite_DoorOpen;
        CutsceneUtils.PlaySound(CutsceneClips[0]);
        MusicManager.StopSong(Fade: false, 0f);
        TRB_Projects_Shared.instance.RemoveLightShadow();
        TRB_Projects_Shared.instance.RotateAlphysToDirection(Vector2.up);
        RotateBerdlyToDirection(Vector2.right);
        RotateNoelleToDirection(Vector2.right);
        yield return new WaitForSeconds(1f);
        CutsceneUtils.MoveTransformLinear(TRB_Projects_Shared.instance.Toriel.transform, new Vector2(4.6f, 1.35f), 1f);
        TRB_Projects_Shared.instance.Toriel.Play("WalkN");
        yield return new WaitForSeconds(1f);
        TRB_Projects_Shared.instance.Toriel.Play("IdleN");
        yield return new WaitForSeconds(0.25f);
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.RotateTorielToDirection(Vector2.up);
        CutsceneUtils.MoveTransformLinear(TRB_Projects_Shared.instance.Toriel.transform, new Vector2(4.6f, 2.35f), 0.5f);
        TRB_Projects_Shared.instance.Toriel.Play("WalkN");
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.Toriel.Play("IdleN");
        CutsceneUtils.FadeOutSprite(TRB_Projects_Shared.instance.Toriel.GetComponent<SpriteRenderer>(), 3f);
        yield return new WaitForSeconds(0.333f);
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Noelle.Play("WalkN");
        Noelle.speed = 1.85f;
        while ((Vector2)Noelle.transform.position != NoelleLeaveWalkPositions[4])
        {
            yield return null;
            if ((Vector2)Noelle.transform.position != NoelleLeaveWalkPositions[NoelleLeaveWalkIndex])
            {
                Noelle.transform.position = Vector2.MoveTowards(Noelle.transform.position, NoelleLeaveWalkPositions[NoelleLeaveWalkIndex], 5f * Time.deltaTime);
                RotateNoelleToDirection(NoelleLeaveWalkDirections[NoelleLeaveWalkIndex]);
                RotateBerdlyToPosition(Noelle.transform.position);
                TRB_Projects_Shared.instance.RotateAlphysToPosition(Noelle.transform.position);
            }
            else if (NoelleLeaveWalkIndex < NoelleLeaveWalkPositions.Length)
            {
                NoelleLeaveWalkIndex++;
            }
        }
        TRB_Projects_Shared.instance.DoorRenderer.sprite = TRB_Projects_Shared.instance.Sprite_DoorClosed;
        Noelle.GetComponent<SpriteRenderer>().enabled = false;
        Noelle.enabled = false;
        CutsceneUtils.PlaySound(CutsceneClips[5]);
        yield return new WaitForSeconds(2f);
        MusicManager.Instance.source.pitch = 0.9f;
        MusicManager.PlaySong(CutsceneClips[6], FadePreviousSong: false, 0f);
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        MusicManager.StopSong(Fade: false, 0f);
        MusicManager.Instance.source.pitch = 1f;
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.NextProject();
    }

    public void RotateBerdlyToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Direction);
    }

    public void RotateBerdlyToPosition(Vector2 Position)
    {
        CutsceneUtils.RotateCharacterTowardsPosition(TRB_Projects_Shared.instance.Berdly, "VelocityX", "VelocityY", Position);
    }

    public void RotateNoelleToDirection(Vector2 Direction)
    {
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Noelle, "VelocityX", "VelocityY", Direction);
    }

    public void Berdly_Exclaim()
    {
        Berdly.Play("Exclaim");
    }

    public void Berdly_Praise()
    {
        Berdly.Play("Praise");
    }

    public void Berdly_ShockRight()
    {
        Berdly.Play("ShockRight");
    }

    public void Berdly_IdleDown()
    {
        Berdly.Play("Idle");
        RotateBerdlyToDirection(Vector2.down);
    }

    public void Berdly_IdleUp()
    {
        Berdly.Play("Idle");
        RotateBerdlyToDirection(Vector2.up);
    }

    public void Berdly_IdleLeft()
    {
        Berdly.Play("Idle");
        RotateBerdlyToDirection(Vector2.left);
    }

    public void Berdly_IdleRight()
    {
        Berdly.Play("Idle");
        RotateBerdlyToDirection(Vector2.right);
    }

    public void Noelle_SwitchToNeutralAnimationSet()
    {
        Noelle.Play("IdleN");
    }

    public void Noelle_IdleDown()
    {
        RotateNoelleToDirection(Vector2.down);
    }

    public void Noelle_IdleUp()
    {
        RotateNoelleToDirection(Vector2.up);
    }

    public void Noelle_IdleLeft()
    {
        RotateNoelleToDirection(Vector2.left);
    }

    public void Noelle_IdleRight()
    {
        RotateNoelleToDirection(Vector2.right);
    }
}
