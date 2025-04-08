using System.Collections;
using UnityEngine;

public class EOTD_TrainingArea_LancerByeCutscene : MonoBehaviour
{
    public int CutsceneIndex;

    [SerializeField]
    private Collider2D CutsceneCollider;

    [Header("References")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [SerializeField]
    private CameraManager PlayerCamera;

    [SerializeField]
    private Animator King;

    [SerializeField]
    private Animator Queen;

    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private InventoryItem ItemPostcard;

    [Header("Dialogue")]
    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Header("Character Animations")]
    [SerializeField]
    private Vector2[] SusieWalkPositions;

    [SerializeField]
    private Vector2[] SusieWalkDirections;

    [SerializeField]
    private int SusieWalkTarget;

    [SerializeField]
    private int SusieWalkIndex;

    [Space(5f)]
    [SerializeField]
    private Vector2[] KrisWalkPositions;

    [SerializeField]
    private Vector2[] KrisWalkDirections;

    [SerializeField]
    private int KrisWalkTarget;

    [SerializeField]
    private int KrisWalkIndex;

    [Space(5f)]
    [SerializeField]
    private Vector2[] RalseiWalkPositions;

    [SerializeField]
    private Vector2[] RalseiWalkDirections;

    [SerializeField]
    private int RalseiWalkTarget;

    [SerializeField]
    private int RalseiWalkIndex;

    [Space(5f)]
    [SerializeField]
    private Vector2[] CameraMovePositions;

    [SerializeField]
    private int CameraMoveTarget;

    [SerializeField]
    private int CameraMoveIndex;

    private void Start()
    {
        Kris = PlayerManager.Instance;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei).PartyMemberFollowSettings;
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
            Susie.FollowingEnabled = false;
            Ralsei.FollowingEnabled = false;
            PlayerCamera.FollowPlayerX = false;
            CutsceneUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int @int = PlayerPrefs.GetInt("EOTD_HasMetLancer", 0);
        if (other.tag == "Player")
        {
            if (@int == 0)
            {
                CutsceneIndex = 1;
            }
            CutsceneCollider.enabled = false;
        }
    }

    private void CutsceneUpdate()
    {
        switch (CutsceneIndex)
        {
            case 1:
                RunFreshChat(CutsceneChats[0], 0, ForcePosition: false, OnBottom: false);
                IncrementCutscene();
                break;
            case 3:
                if (KrisWalkIndex < KrisWalkTarget)
                {
                    if ((Vector2)Kris.transform.position != KrisWalkPositions[KrisWalkIndex])
                    {
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[KrisWalkIndex], 6f * Time.deltaTime);
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: true);
                        Kris._PMove._anim.SetFloat("MOVEMENTX", KrisWalkDirections[KrisWalkIndex].x);
                        Kris._PMove._anim.SetFloat("MOVEMENTY", KrisWalkDirections[KrisWalkIndex].y);
                    }
                    else
                    {
                        KrisWalkIndex++;
                    }
                }
                else
                {
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                }
                if (SusieWalkIndex < SusieWalkTarget)
                {
                    if ((Vector2)Susie.transform.position != SusieWalkPositions[SusieWalkIndex])
                    {
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[SusieWalkIndex], 6f * Time.deltaTime);
                        Susie.AnimationOverriden = true;
                        Susie.SusieAnimator.Play("Walk");
                        Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[SusieWalkIndex].x);
                        Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[SusieWalkIndex].y);
                        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        SusieWalkIndex++;
                    }
                }
                else
                {
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                if (RalseiWalkIndex < RalseiWalkTarget)
                {
                    if ((Vector2)Ralsei.transform.position != RalseiWalkPositions[RalseiWalkIndex])
                    {
                        Ralsei.transform.position = Vector2.MoveTowards(Ralsei.transform.position, RalseiWalkPositions[RalseiWalkIndex], 6f * Time.deltaTime);
                        Ralsei.SusieAnimator.Play("Walk");
                        Ralsei.AnimationOverriden = true;
                        Ralsei.SusieAnimator.SetFloat("VelocityX", RalseiWalkDirections[RalseiWalkIndex].x);
                        Ralsei.SusieAnimator.SetFloat("VelocityY", RalseiWalkDirections[RalseiWalkIndex].y);
                        Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
                    }
                    else
                    {
                        RalseiWalkIndex++;
                    }
                }
                else
                {
                    Ralsei.SusieAnimator.Play("Idle");
                    Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
                    Ralsei.SusieAnimator.SetFloat("VelocityY", 1f);
                    Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                }
                if (CameraMoveIndex < CameraMoveTarget)
                {
                    if ((Vector2)PlayerCamera.transform.position != CameraMovePositions[CameraMoveIndex])
                    {
                        PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, (Vector3)CameraMovePositions[CameraMoveIndex] + new Vector3(0f, 0f, -10f), 6f * Time.deltaTime);
                    }
                    else
                    {
                        CameraMoveIndex++;
                    }
                }
                if ((Vector2)Kris.transform.position == KrisWalkPositions[0] && (Vector2)Susie.transform.position == SusieWalkPositions[0] && (Vector2)Ralsei.transform.position == RalseiWalkPositions[0])
                {
                    Ralsei.SusieAnimator.Play("Idle");
                    Ralsei.SusieAnimator.SetFloat("VelocityX", 0f);
                    Ralsei.SusieAnimator.SetFloat("VelocityY", 1f);
                    Ralsei.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Susie.SusieAnimator.Play("Idle");
                    Susie.SusieAnimator.SetFloat("VelocityX", 1f);
                    Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                    Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
                    Kris._PMove._anim.SetBool("MOVING", value: false);
                    Kris._PMove._anim.SetFloat("MOVEMENTX", 0f);
                    Kris._PMove._anim.SetFloat("MOVEMENTY", 1f);
                    IncrementCutscene();
                    RunFreshChat(CutsceneChats[1], 0, ForcePosition: false, OnBottom: false);
                }
                break;
            case 5:
                ChatboxManager.Instance.AllowInput = false;
                StartCoroutine(QueenMorseCode());
                IncrementCutscene();
                break;
            case 7:
                StartCoroutine(PostcardEnding());
                IncrementCutscene();
                break;
            case 9:
                if (PlayerCamera.transform.position.x != Kris.transform.position.x)
                {
                    float x = Kris.transform.position.x;
                    PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, new Vector3(x, 3.5f, -10f), 6f * Time.deltaTime);
                }
                else
                {
                    IncrementCutscene();
                    EndCutscene();
                }
                break;
            case 2:
            case 4:
            case 6:
            case 8:
                break;
        }
    }

    private void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom)
    {
        CutsceneChatter.CanUse = true;
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.CurrentIndex = index;
        CutsceneChatter.FinishedText = false;
        CutsceneChatter.Text = text;
        if (ForcePosition)
        {
            CutsceneChatter.ManualTextboxPosition = true;
            CutsceneChatter.OnBottom = OnBottom;
        }
        CutsceneChatter.RUN();
    }

    public void IncrementCutscene()
    {
        CutsceneIndex++;
    }

    public void EndCutscene()
    {
        DarkworldInventory.Instance.PlayerKeyItems.Add(ItemPostcard);
        CutsceneIndex = 0;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        DarkworldMenu.Instance.CanOpenMenu = true;
        Susie.positions.Clear();
        Susie.rotations.Clear();
        Ralsei.positions.Clear();
        Ralsei.rotations.Clear();
        Susie.FollowingEnabled = true;
        Ralsei.FollowingEnabled = true;
        Susie.AnimationOverriden = false;
        Ralsei.AnimationOverriden = false;
        Susie.SusieAnimator.Play("Idle");
        Ralsei.SusieAnimator.Play("Idle");
        PlayerPrefs.SetInt("EOTD_HasMetLancer", 1);
        PlayerCamera.FollowPlayerX = true;
        Kris._PMove.AnimationOverriden = false;
    }

    public void KingAnim_CradleIdle()
    {
        float normalizedTime = King.GetCurrentAnimatorStateInfo(0).normalizedTime;
        King.Play("King_HoldLancer_Idle", 0, normalizedTime);
    }

    public void KingAnim_CradleIdle_Smile()
    {
        float normalizedTime = King.GetCurrentAnimatorStateInfo(0).normalizedTime;
        King.Play("King_HoldLancer_Smile", 0, normalizedTime);
    }

    public void KingAnim_CradleIdle_Neutral()
    {
        float normalizedTime = King.GetCurrentAnimatorStateInfo(0).normalizedTime;
        King.Play("King_HoldLancer_Neutral", 0, normalizedTime);
    }

    public void KingAnim_CradleIdle_Shock()
    {
        float normalizedTime = King.GetCurrentAnimatorStateInfo(0).normalizedTime;
        King.Play("King_HoldLancer_Shock", 0, normalizedTime);
    }

    public void KingAnim_CradleIdle_Embarrased()
    {
        float normalizedTime = King.GetCurrentAnimatorStateInfo(0).normalizedTime;
        King.Play("King_HoldLancer_Embarrased", 0, normalizedTime);
    }

    public void KingAnim_CradleShush()
    {
        King.Play("King_HoldLancer_Shush");
    }

    public void QueenAnim_IdleOutline_Left()
    {
        Queen.Play("Queen_IdleOutline_Left");
    }

    public void QueenAnim_IdleOutline_Down()
    {
        Queen.Play("Queen_IdleOutline_Down");
    }

    public void QueenAnim_LaughOutline()
    {
        Queen.Play("Queen_LaughOutline");
    }

    public void SusieAnim_ProudRight()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.Play("SusieDarkworld_Proud_Right");
    }

    public void SusieAnim_AngryRight()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.Play("SusieDarkworld_Angry_Right");
    }

    public void SusieAnim_Idle_Right()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 1f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    public void SusieAnim_Idle_Down()
    {
        Susie.AnimationOverriden = true;
        Susie.SusieAnimator.Play("Idle");
        Susie.SusieAnimator.SetFloat("VelocityX", 0f);
        Susie.SusieAnimator.SetFloat("VelocityY", -1f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
    }

    private IEnumerator QueenMorseCode()
    {
        CutsceneSource.pitch = 3f;
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        yield return new WaitForSeconds(5f);
        CutsceneSource.pitch = 1f;
        CutsceneSource.enabled = false;
        ChatboxManager.Instance.EndText();
        ChatboxManager.Instance.AllowInput = true;
        yield return new WaitForSeconds(0f);
        CutsceneSource.enabled = true;
        RunFreshChat(CutsceneChats[2], 0, ForcePosition: false, OnBottom: false);
    }

    private IEnumerator PostcardEnding()
    {
        CutsceneSource.pitch = 1f;
        CutsceneSource.enabled = true;
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(2f);
        RunFreshChat(CutsceneChats[3], 0, ForcePosition: false, OnBottom: false);
    }
}
