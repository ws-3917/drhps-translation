using System.Collections;
using UnityEngine;

public class EOTD_SchoolLobby : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex = 1;

    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private AudioSource CutsceneMusicSource;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    private Transform Kris;

    [SerializeField]
    private Animator Susie;

    [SerializeField]
    private RuntimeAnimatorController KrisHornController;

    [SerializeField]
    private Vector3[] KrisWalkPositions;

    [SerializeField]
    private GameObject DoorDarkness;

    [SerializeField]
    private AudioClip mus_bird;

    [SerializeField]
    private GameObject photo;

    private void Start()
    {
        Kris = PlayerManager.Instance.transform;
        PlayerManager.Instance._PMove._anim.runtimeAnimatorController = KrisHornController;
        Susie.Play("Susie_EOTD_SchoolLobbyPanic_Idle");
        MusicManager.PlaySong(mus_bird, FadePreviousSong: false, 0f);
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            CutsceneUpdate();
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
        }
    }

    private void CutsceneUpdate()
    {
        switch (CutsceneIndex)
        {
            case 1:
                StartCoroutine(DelayUntilKrisLeaveCloset());
                IncrementCutsceneIndex();
                break;
            case 3:
                if (Kris.position != KrisWalkPositions[1])
                {
                    PlayerManager.Instance.transform.position = Vector2.MoveTowards(PlayerManager.Instance.transform.position, KrisWalkPositions[1], 3f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", -1f);
                }
                else
                {
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", -1f);
                    IncrementCutsceneIndex();
                }
                break;
            case 5:
                StartCoroutine(textDelay());
                IncrementCutsceneIndex();
                break;
            case 7:
                photo.SetActive(value: true);
                CutsceneSource.PlayOneShot(CutsceneSounds[1]);
                StartCoroutine(EndingDelay());
                IncrementCutsceneIndex();
                break;
            case 2:
            case 4:
            case 6:
                break;
        }
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
    }

    private IEnumerator textDelay()
    {
        yield return new WaitForSeconds(0.5f);
        CutsceneChatter.Text = CutsceneChats[1];
        CutsceneChatter.CurrentIndex = 0;
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.CanUse = true;
        CutsceneChatter.RUN();
    }

    private IEnumerator EndingDelay()
    {
        MusicManager.StopSong(Fade: true, 0.25f);
        yield return new WaitForSeconds(0.25f);
        CutsceneMusicSource.PlayOneShot(CutsceneSounds[2]);
        yield return new WaitForSeconds(8f);
        UI_FADE.Instance.StartFadeIn(37, 0.25f, UnpauseOnEnd: true, NewMainMenuManager.MainMenuStates.Hypothetical);
    }

    private IEnumerator DelayUntilKrisLeaveCloset()
    {
        Susie.Play("Susie_EOTD_SchoolLobbyPanic_Idle");
        yield return new WaitForSeconds(2.5f);
        DoorDarkness.SetActive(value: true);
        Kris.position = KrisWalkPositions[0];
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.down);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        yield return new WaitForSeconds(0.25f);
        Susie.Play("Susie_EOTD_SchoolLobbyPanic");
        yield return new WaitForSeconds(0.5f);
        IncrementCutsceneIndex();
        yield return new WaitForSeconds(1.5f);
        PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", -1f);
        PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", 0f);
        yield return new WaitForSeconds(1f);
        Susie.Play("Susie_EOTD_SchoolLobbyPanic_Look");
        yield return new WaitForSeconds(0.5f);
        CutsceneChatter.Text = CutsceneChats[0];
        CutsceneChatter.RUN();
    }

    public void SusieAnim_Idle_Right()
    {
        Susie.Play("Idle");
        Susie.SetFloat("VelocityX", 1f);
        Susie.SetFloat("VelocityY", 0f);
        Susie.SetFloat("VelocityMagnitude", 0f);
    }

    public void EndHypothetical()
    {
    }
}
