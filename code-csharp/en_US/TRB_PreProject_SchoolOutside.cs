using System.Collections;
using UnityEngine;

public class TRB_PreProject_SchoolOutside : MonoBehaviour
{
    [Header("-= Cutscene References =-")]
    [SerializeField]
    private CameraManager playerCamera;

    [SerializeField]
    private Animator Toriel;

    [SerializeField]
    private Animator TorielCar;

    [SerializeField]
    private Animator Noelle;

    [SerializeField]
    private Animator Berdly;

    [SerializeField]
    private Animator Susie;

    [SerializeField]
    private PlayerManager Kris;

    [Header("- Dialogue -")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Header("- Sounds -")]
    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [Header("- Walking Positions -")]
    [SerializeField]
    private Vector2[] TorielCar_WalkPositions_PullIn;

    [SerializeField]
    private string[] TorielCar_WalkPositions_PullIn_Directions;

    [SerializeField]
    private Vector2[] Toriel_WalkPositions_WalkAroundCar;

    [SerializeField]
    private Vector2[] Toriel_WalkPositions_WalkAroundCar_Directions;

    [SerializeField]
    private Vector2[] NoelleBerdly_WalkPositions_HeadInside;

    [SerializeField]
    private Vector2[] NoelleBerdly_WalkPositions_HeadInside_Directions;

    [SerializeField]
    private Vector2 Susie_WalkPositions_RunToKris;

    private void Start()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        Kris = PlayerManager.Instance;
        Toriel.SetBool("InCutscene", value: true);
        Noelle.SetBool("InCutscene", value: true);
        Susie.SetBool("InCutscene", value: true);
        Kris._PMove.AnimationOverriden = true;
        Kris._PMove._anim.SetBool("MOVING", value: false);
        RotateNoelleToDirection(Vector2.down);
        Noelle.Play("IdleBooks");
        Susie.Play("TRBNotepad_Idle");
        Berdly.Play("IdlePosterBoard");
        RotateSusieToDirection(Vector2.up);
        RotateBerdlyToDirection(Vector2.right);
        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.MoveTransformSmooth(playerCamera.transform, new Vector3(10f, 2.025f, -10f), 5f);
        for (int i = 0; i < TorielCar_WalkPositions_PullIn.Length; i++)
        {
            while ((Vector2)TorielCar.transform.position != TorielCar_WalkPositions_PullIn[i])
            {
                TorielCar.Play(TorielCar_WalkPositions_PullIn_Directions[i]);
                TorielCar.transform.position = Vector3.MoveTowards(TorielCar.transform.position, TorielCar_WalkPositions_PullIn[i], 4.5f * Time.deltaTime);
                yield return null;
            }
        }
        CutsceneUtils.PlaySound(CutsceneSounds[0]);
        TorielCar.Play(TorielCar_WalkPositions_PullIn_Directions[1], -1, 0f);
        TorielCar.speed = 0f;
        RotateBerdlyToDirection(Vector2.left);
        yield return new WaitForSeconds(1f);
        Toriel.transform.position = TorielCar.transform.position + new Vector3(0f, -1f);
        RotateTorielToDirection(Vector2.left);
        Toriel.Play("WalkBag");
        Toriel.speed = 0.5f;
        Kris.transform.position = TorielCar.transform.position;
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        Kris._PMove._anim.SetBool("MOVING", value: true);
        Kris._PMove._anim.speed = 0.5f;
        CutsceneUtils.MoveTransformLinear(Kris.transform, TorielCar.transform.position + new Vector3(3f, 0f), 1f);
        CutsceneUtils.MoveTransformLinear(Toriel.transform, TorielCar.transform.position + new Vector3(-2f, -1f), 1f);
        yield return new WaitForSeconds(0.25f);
        CutsceneUtils.PlaySound(CutsceneSounds[1]);
        yield return new WaitForSeconds(0.75f);
        TorielCar.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("BelowPlayer");
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove._anim.speed = 1f;
        Toriel.speed = 1f;
        for (int i = 0; i < Toriel_WalkPositions_WalkAroundCar.Length; i++)
        {
            while ((Vector2)Toriel.transform.position != Toriel_WalkPositions_WalkAroundCar[i])
            {
                RotateTorielToDirection(Toriel_WalkPositions_WalkAroundCar_Directions[i]);
                Toriel.transform.position = Vector3.MoveTowards(Toriel.transform.position, Toriel_WalkPositions_WalkAroundCar[i], 3f * Time.deltaTime);
                yield return null;
            }
        }
        Toriel.Play("IdleBag");
        Toriel.speed = 1f;
        RotateTorielToDirection(Vector2.right);
        RotateBerdlyToDirection(Vector2.right);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 4)
            {
                RotateSusieToDirection(Vector2.left);
                RotateNoelleToDirection(Vector2.left);
            }
            else if (ChatboxManager.Instance.CurrentTextIndex == 5)
            {
                RotateBerdlyToDirection(Vector2.left);
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 2)
            {
                RotateBerdlyToDirection(Vector2.right);
                RotateNoelleToDirection(Vector2.down);
            }
            else
            {
                RotateBerdlyToDirection(Vector2.left);
                RotateNoelleToDirection(Vector2.left);
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        RotateBerdlyToDirection(Vector2.right);
        RotateNoelleToDirection(Vector2.left);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 3)
            {
                RotateNoelleToDirection(Vector2.down);
                RotateSusieToDirection(Vector2.up);
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        Noelle.Play("WalkBooks");
        for (int i = 0; i < NoelleBerdly_WalkPositions_HeadInside.Length; i++)
        {
            while ((Vector2)Noelle.transform.position != NoelleBerdly_WalkPositions_HeadInside[i])
            {
                RotateNoelleToDirection(NoelleBerdly_WalkPositions_HeadInside_Directions[i]);
                Noelle.transform.position = Vector3.MoveTowards(Noelle.transform.position, NoelleBerdly_WalkPositions_HeadInside[i], 3f * Time.deltaTime);
                yield return null;
            }
        }
        Noelle.GetComponent<SpriteRenderer>().enabled = false;
        CutsceneUtils.PlaySound(CutsceneSounds[2]);
        Berdly.Play("WalkPosterBoard");
        CutsceneUtils.MoveTransformLinear(Berdly.transform, NoelleBerdly_WalkPositions_HeadInside[0], 3.5f);
        yield return new WaitForSeconds(3.5f);
        RotateBerdlyToDirection(Vector2.right);
        Berdly.Play("IdlePosterBoard");
        RotateSusieToDirection(Vector2.up);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Berdly.Play("WalkPosterBoard");
        CutsceneUtils.MoveTransformLinear(Berdly.transform, NoelleBerdly_WalkPositions_HeadInside[1], 2f);
        yield return new WaitForSeconds(2f);
        Berdly.GetComponent<SpriteRenderer>().enabled = false;
        CutsceneUtils.PlaySound(CutsceneSounds[2]);
        yield return new WaitForSeconds(2f);
        Susie.Play("Susie_TRB_NotepadShock");
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        RotateSusieToDirection(Vector2.left);
        Susie.Play("TRBNotepad_Walk");
        CutsceneUtils.MoveTransformLinear(Susie.transform, Susie_WalkPositions_RunToKris, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Susie.Play("TRBNotepad_Idle");
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            if (ChatboxManager.Instance.CurrentTextIndex == 3)
            {
                RotateSusieToDirection(Vector2.right);
            }
            yield return null;
        }
        Susie.Play("TRBNotepad_Idle");
        RotateSusieToDirection(Vector2.up);
        yield return new WaitForSeconds(1f);
        RotateSusieToDirection(Vector2.left);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 6, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        UI_FADE.Instance.StartFadeIn(-1, 1f);
        MusicManager.StopSong(Fade: true, 1f);
        yield return new WaitForSeconds(3f);
        CutsceneUtils.PlaySound(CutsceneSounds[3]);
        playerCamera.transform.position = new Vector3(64f, 0f, -10f);
        UI_FADE.Instance.StartFadeOut(5f);
        yield return new WaitForSeconds(1.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        CutsceneUtils.PlaySound(CutsceneSounds[4]);
        yield return new WaitForSeconds(1f);
        MusicManager.PlaySong(CutsceneSounds[5], FadePreviousSong: false, 0f);
        MusicManager.Instance.source.pitch = 1.2f;
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        MusicManager.StopSong(Fade: true, 2f);
        MusicManager.Instance.source.pitch = 1f;
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        UI_FADE.Instance.StartFadeIn(47, 3f);
    }

    private void RotateTorielToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Toriel, "VelocityX", "VelocityY", direction);
    }

    private void RotateSusieToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Susie, "VelocityX", "VelocityY", direction);
    }

    private void RotateBerdlyToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Berdly, "VelocityX", "VelocityY", direction);
    }

    private void RotateNoelleToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Noelle, "VelocityX", "VelocityY", direction);
    }
}
