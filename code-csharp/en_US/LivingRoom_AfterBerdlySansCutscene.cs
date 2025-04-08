using System.Collections;
using UnityEngine;

public class LivingRoom_AfterBerdlySansCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private int cutsceneIndex;

    [SerializeField]
    private Vector3[] KrisCutscenePositions;

    [SerializeField]
    private Vector3[] SusieCutscenePositions;

    [SerializeField]
    private Vector3[] SansCutscenePositions;

    [Space(5f)]
    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private GameObject Sans;

    [SerializeField]
    private Animator SansAnimator;

    [Space(5f)]
    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Space(5f)]
    [SerializeField]
    private SpriteRenderer ComicBookRenderer;

    [SerializeField]
    private Sprite ComicSwanSprite;

    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    private void Start()
    {
        SetComicBook();
    }

    private void Update()
    {
        if (cutsceneIndex != 0)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
            CutsceneUpdate();
        }
    }

    private void SetComicBook()
    {
        if (PlayerPrefs.GetInt("PapyrusMeet_ComicSwan", 0) == 1)
        {
            ComicBookRenderer.sprite = ComicSwanSprite;
        }
    }

    private void CutsceneUpdate()
    {
        switch (cutsceneIndex)
        {
            case 1:
                Susie.FollowingEnabled = false;
                Susie.AnimationOverriden = true;
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[0])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[0], 1f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", -1f);
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
                }
                if (Susie.transform.position != SusieCutscenePositions[0])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[0], 1f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                if (Susie.transform.position == SusieCutscenePositions[0])
                {
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(1f, 0f));
                }
                if (PlayerManager.Instance.transform.position == KrisCutscenePositions[0])
                {
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                    PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(1f, 0f));
                }
                if (Susie.transform.position == SusieCutscenePositions[0] && PlayerManager.Instance.transform.position == KrisCutscenePositions[0])
                {
                    SansAnimator.Play("Idle");
                    SansAnimator.SetFloat("MOVEMENTX", -1f);
                    SansAnimator.SetFloat("MOVEMENTY", 0f);
                    CutsceneChatter.Text = CutsceneChats[0];
                    CutsceneChatter.CurrentIndex = 0;
                    CutsceneChatter.CanUse = true;
                    CutsceneChatter.FirstTextPlayed = false;
                    CutsceneChatter.FinishedText = false;
                    CutsceneChatter.RUN();
                    cutsceneIndex = 2;
                }
                break;
            case 3:
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[1])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[1], 3f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", -1f);
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, -1f));
                }
                if (Susie.transform.position != SusieCutscenePositions[1])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[1], 3f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", -1f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                if (Susie.transform.position == SusieCutscenePositions[1])
                {
                    Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                }
                if (PlayerManager.Instance.transform.position == KrisCutscenePositions[1])
                {
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                    PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                }
                break;
            case 4:
                PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: false);
                PlayerManager.Instance._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
                PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(0f, 1f));
                Susie.SusieAnimator.SetFloat("VelocityX", 0f);
                Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                Susie.RotateSusieToDirection(new Vector2(0f, 1f));
                cutsceneIndex = 5;
                break;
            case 6:
                if (PlayerManager.Instance.transform.position != KrisCutscenePositions[2])
                {
                    PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, KrisCutscenePositions[2], 3f * Time.deltaTime);
                    PlayerManager.Instance._PMove.AnimationOverriden = true;
                    PlayerManager.Instance._PMove._anim.SetBool("MOVING", value: true);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTX", 1f);
                    PlayerManager.Instance._PMove._anim.SetFloat("MOVEMENTY", 0f);
                    PlayerManager.Instance._PMove.RotatePlayerAnim(new Vector2(1f, 0f));
                }
                if (Susie.transform.position != SusieCutscenePositions[1])
                {
                    Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, SusieCutscenePositions[2], 3f * Time.deltaTime);
                    Susie.AnimationOverriden = true;
                    Susie.SusieAnimator.SetFloat("VelocityX", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 100f);
                }
                break;
            case 2:
            case 5:
                break;
        }
    }

    public void BeginKrisSusieWalk()
    {
        cutsceneIndex = 3;
        StartCoroutine(DelayUntilSecondSansText());
    }

    public void EndCutscene()
    {
        cutsceneIndex = 6;
        LevelTransition.BeginTransition();
    }

    private void OnDestroy()
    {
        PlayerManager.Instance._PMove.AnimationOverriden = false;
    }

    private IEnumerator DelayUntilSecondSansText()
    {
        SansAnim_Idle_Right();
        yield return new WaitForSeconds(1.5f);
        SansAnim_Idle_Down();
        cutsceneIndex = 4;
        CutsceneChatter.Text = CutsceneChats[1];
        CutsceneChatter.CurrentIndex = 0;
        CutsceneChatter.CanUse = true;
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.FinishedText = false;
        CutsceneChatter.RUN();
    }

    public void SansAnim_Idle_Down()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetFloat("MOVEMENTX", 0f);
        SansAnimator.SetFloat("MOVEMENTY", -1f);
    }

    public void SansAnim_Idle_Left()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetFloat("MOVEMENTX", -1f);
        SansAnimator.SetFloat("MOVEMENTY", 0f);
    }

    public void SansAnim_Idle_Right()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetFloat("MOVEMENTX", 1f);
        SansAnimator.SetFloat("MOVEMENTY", 0f);
    }
}
