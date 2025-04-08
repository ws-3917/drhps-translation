using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EOTDCutscene_Entrance : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex;

    [Header("-- Cutscene References --")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private PartyMember SusiePartyMember;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Animator BackgroundAnimator;

    [Header("Landing from the fall")]
    [SerializeField]
    private Vector3 KrisLandPos;

    [SerializeField]
    private Vector3 SusieLandPos;

    [SerializeField]
    private Vector3 KrisStartPos;

    [SerializeField]
    private Vector3 SusieStartPos;

    [Header("-- Cutscene Audio --")]
    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    private void Start()
    {
        StartCoroutine(SetupCutscene());
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            CutsceneUpdate();
            Susie.FollowingEnabled = false;
            Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
    }

    private void CutsceneUpdate()
    {
        switch (CutsceneIndex)
        {
            case 1:
                Kris.transform.position = KrisStartPos;
                Susie.transform.position = SusieStartPos;
                CutsceneSource.PlayOneShot(CutsceneSounds[2]);
                CutsceneIndex = 2;
                break;
            case 2:
                if (Kris.transform.position != KrisLandPos)
                {
                    Kris.transform.position = Vector3.MoveTowards(Kris.transform.position, KrisLandPos, 15f * Time.deltaTime);
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.Play("KrisDarkworld_Fall");
                }
                if (Susie.transform.position != SusieLandPos)
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieLandPos, 15f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.Play("SusieDarkworld_Fall");
                }
                if (Kris.transform.position == KrisLandPos && Susie.transform.position == SusieLandPos)
                {
                    CutsceneSource.PlayOneShot(CutsceneSounds[0]);
                    CutsceneIndex = 3;
                    Susie.SusieAnimator.Play("SusieDarkworld_Land");
                    Kris._PMove._anim.Play("KrisDarkworld_Land");
                    StartCoroutine(DelayBeforeBackgroundReveal());
                }
                break;
        }
    }

    private IEnumerator SetupCutscene()
    {
        yield return null;
        SceneManager.LoadScene(10);
    }

    private IEnumerator DelayBeforeBackgroundReveal()
    {
        yield return new WaitForSeconds(1.5f);
        BackgroundAnimator.Play("EOTD_CastletownEntrance_FadeIn");
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(1f);
        CutsceneIndex = 0;
        Kris._PlayerState = PlayerManager.PlayerState.Game;
        Kris.ResetToGameState();
        Susie.FollowingEnabled = true;
        Susie.AnimationOverriden = false;
        Kris._PMove.AnimationOverriden = false;
        Susie.SusieAnimator.Play("Idle");
        Kris._PMove._anim.Play("DARKWORLD_KRIS_IDLE");
        DarkworldMenu.Instance.CanOpenMenu = true;
        Kris._PMove.GetComponent<Collider2D>().enabled = true;
        Susie.RotateSusieToDirection(new Vector2(1f, 0f));
        Kris._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
    }
}
