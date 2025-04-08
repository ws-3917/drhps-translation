using System.Collections;
using UnityEngine;

public class TRB_TorielWakeKris : MonoBehaviour
{
    [Header("-= References =-")]
    [SerializeField]
    private CameraManager playerCamera;

    [SerializeField]
    private Animator Toriel;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private SpriteRenderer KrisCoversRenderer;

    [SerializeField]
    private Sprite KrisCovers_Open;

    [SerializeField]
    private INT_Chat CutsceneIntChat;

    [SerializeField]
    private GameObject[] ObjectsToEnableOnEnd;

    [Header("- Toriel Walk Positions -")]
    [SerializeField]
    private Vector2[] Toriel_WalkToKrisBed;

    [SerializeField]
    private Vector2[] Toriel_WalkToKrisBed_Directions;

    [SerializeField]
    private Vector2[] Toriel_ExitRoom;

    [SerializeField]
    private Vector2[] Toriel_ExitRoom_Directions;

    [Header("- Cutscene Sounds -")]
    [SerializeField]
    private AudioClip[] CutsceneSounds;

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        Toriel.SetBool("InCutscene", value: true);
        MusicManager.StopSong(Fade: false, 0f);
        RotateTorielToDirection(Vector2.up);
        PlayerManager.Instance._PMove.AnimationOverriden = true;
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        playerCamera.transform.position = new Vector3(0f, 0f, -10f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: false, CutsceneIntChat);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Toriel.Play("Walk");
        for (int i = 0; i < Toriel_WalkToKrisBed.Length; i++)
        {
            while ((Vector2)Toriel.transform.position != Toriel_WalkToKrisBed[i])
            {
                RotateTorielToDirection(Toriel_WalkToKrisBed_Directions[i]);
                Toriel.transform.position = Vector3.MoveTowards(Toriel.transform.position, Toriel_WalkToKrisBed[i], 3f * Time.deltaTime);
                yield return null;
            }
        }
        Toriel.Play("Idle");
        RotateTorielToDirection(Vector2.right);
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true, CutsceneIntChat);
        Toriel.Play("Sassy");
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        KrisCoversRenderer.sprite = KrisCovers_Open;
        CutsceneUtils.PlaySound(CutsceneSounds[0]);
        yield return new WaitForSeconds(1.25f);
        Toriel.Play("Idle");
        RotateTorielToDirection(Vector2.right);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true, CutsceneIntChat);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        Toriel.Play("Walk");
        for (int i = 0; i < Toriel_ExitRoom.Length; i++)
        {
            while ((Vector2)Toriel.transform.position != Toriel_ExitRoom[i])
            {
                RotateTorielToDirection(Toriel_ExitRoom_Directions[i]);
                Toriel.transform.position = Vector3.MoveTowards(Toriel.transform.position, Toriel_ExitRoom[i], 4.5f * Time.deltaTime);
                yield return null;
            }
        }
        Toriel.transform.position = new Vector3(32f, 0f, 0f);
        CutsceneUtils.PlaySound(CutsceneSounds[1]);
        yield return new WaitForSeconds(0.25f);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        PlayerManager.Instance._PMove.AnimationOverriden = true;
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
        CutsceneUtils.MoveTransformLinear(PlayerManager.Instance.transform, new Vector3(2.97f, 0.25f, 0f), 1f);
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.down);
        PlayerManager.Instance._PMove.AnimationOverriden = false;
        PlayerManager.Instance.ResetToGameState();
        LightworldMenu.Instance.CanOpenMenu = true;
        GameObject[] objectsToEnableOnEnd = ObjectsToEnableOnEnd;
        for (int j = 0; j < objectsToEnableOnEnd.Length; j++)
        {
            objectsToEnableOnEnd[j].SetActive(value: true);
        }
    }

    private void RotateTorielToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Toriel, "VelocityX", "VelocityY", direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.gameObject)
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(FinishingCutscene());
        }
    }

    private IEnumerator FinishingCutscene()
    {
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        LightworldMenu.Instance.CanOpenMenu = false;
        UI_FADE.Instance.StartFadeIn(-1, 1f);
        CutsceneUtils.PlaySound(CutsceneSounds[2]);
        yield return new WaitForSeconds(2.5f);
        playerCamera.transform.position = new Vector3(-32f, 0f, -10f);
        UI_FADE.Instance.StartFadeOut(5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: true, CutsceneIntChat);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        UI_FADE.Instance.StartFadeIn(46, 1f);
    }
}
