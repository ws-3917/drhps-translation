using System;
using System.Collections;
using UnityEngine;

public class Susie_CouchCutscene : MonoBehaviour
{
    [SerializeField]
    private ActivePartyMember SusiePartyMember;

    [SerializeField]
    private SpriteRenderer SusieSpriteRenderer;

    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private Animator SusieAnimator;

    [SerializeField]
    private AudioSource CutsceneAudioSource;

    [SerializeField]
    private AudioClip[] CutsceneAudioClips;

    [SerializeField]
    private Animator SpringAnimator;

    private int cutsceneIndex;

    [SerializeField]
    private Vector3 StoredOriginalSusiePosition;

    [SerializeField]
    private Vector3 SusieLandPosition;

    [Header("Arc Utility")]
    [SerializeField]
    private float JumpArcHeight = 3f;

    [SerializeField]
    private float JumpDuration = 0.5f;

    [SerializeField]
    private Vector3 JumpStartPos;

    [SerializeField]
    private Vector3 JumpEndPos;

    private float timeElapsed;

    private bool isMoving;

    private bool FlippedVarient;

    public void StartCutscene()
    {
        SusiePartyMember = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie);
        SusieAnimator = SusiePartyMember.PartyMemberFollowSettings.SusieAnimator;
        SusieSpriteRenderer = SusiePartyMember.PartyMemberFollowSettings.GetComponentInChildren<SpriteRenderer>();
        StoredOriginalSusiePosition = SusiePartyMember.PartyMemberTransform.position;
        cutsceneIndex = 1;
        if (SusiePartyMember.PartyMemberTransform.position.x <= -0.3f)
        {
            FlippedVarient = true;
        }
        StartCoroutine(SusiePrepareJump());
    }

    public void RevertSusie()
    {
        SusieAnimator.Play("Idle");
        SusiePartyMember.PartyMemberFollowSettings.RotateSusieToDirection(Vector2.left);
        SusiePartyMember.PartyMemberFollowSettings.FollowingEnabled = true;
        SusiePartyMember.PartyMemberFollowSettings.currentState = Susie_Follower.MemberFollowerStates.CopyingInputs;
    }

    public void EndCutscene()
    {
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        LightworldMenu.Instance.CanOpenMenu = true;
    }

    private void Update()
    {
        if (cutsceneIndex != 0)
        {
            ArcUpdate();
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
            SusiePartyMember.PartyMemberFollowSettings.FollowingEnabled = false;
        }
    }

    public void IncrementCutsceneIndex()
    {
        cutsceneIndex++;
    }

    private void ArcUpdate()
    {
        if (isMoving)
        {
            timeElapsed += Time.deltaTime;
            float num = timeElapsed / JumpDuration;
            Vector2 vector = CalculateArcPosition(num);
            SusiePartyMember.PartyMemberTransform.position = vector;
            if (num >= 0.75f && cutsceneIndex == 4 && FlippedVarient)
            {
                SusieAnimator.Play("Susie_Couch_Rebound_Flipped");
            }
            if (num >= 1f)
            {
                isMoving = false;
                timeElapsed = 0f;
                SusieSpriteRenderer.flipX = false;
                SusiePartyMember.PartyMemberTransform.position = JumpEndPos;
                IncrementCutsceneIndex();
            }
        }
    }

    public void StartArcMovement()
    {
        SusiePartyMember.PartyMemberTransform.position = JumpStartPos;
        timeElapsed = 0f;
        isMoving = true;
    }

    private Vector2 CalculateArcPosition(float t)
    {
        Vector2 vector = Vector2.Lerp(JumpStartPos, JumpEndPos, t);
        float num = JumpArcHeight * Mathf.Sin(MathF.PI * t);
        return new Vector2(vector.x, vector.y + num);
    }

    private void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom)
    {
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

    private IEnumerator SusiePrepareJump()
    {
        yield return new WaitForSeconds(0.5f);
        if (FlippedVarient)
        {
            SusieAnimator.Play("Susie_Couch_Crouch_Flipped");
        }
        else
        {
            SusieAnimator.Play("Susie_Couch_Crouch");
        }
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[0]);
        yield return new WaitForSeconds(1.2f);
        JumpStartPos = StoredOriginalSusiePosition;
        JumpEndPos = SusieLandPosition;
        if (FlippedVarient)
        {
            SusieAnimator.Play("Susie_Couch_Jump_Flipped");
        }
        else
        {
            SusieAnimator.Play("Susie_Couch_Jump");
        }
        SusieAnimator.GetComponent<SPR_YSorting>().enabled = false;
        SusieSpriteRenderer.sortingOrder = 500;
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[1]);
        StartArcMovement();
        IncrementCutsceneIndex();
        while (cutsceneIndex != 3)
        {
            yield return null;
        }
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[2]);
        SusieAnimator.Play("Susie_Couch_Sit");
        yield return new WaitForSeconds(1.5f);
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[3]);
        SusieAnimator.Play("Susie_Couch_SitPanic");
        yield return new WaitForSeconds(1.427f);
        JumpStartPos = SusieLandPosition;
        JumpEndPos = StoredOriginalSusiePosition;
        StartArcMovement();
        IncrementCutsceneIndex();
        SusieAnimator.Play("Susie_Couch_Rebound");
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[4]);
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[6]);
        SpringAnimator.Play("LivingRoom_SpringBounce");
        while (cutsceneIndex != 5)
        {
            yield return null;
        }
        CutsceneAudioSource.PlayOneShot(CutsceneAudioClips[5]);
        SusieAnimator.Play("Susie_Couch_Land");
        SusieAnimator.GetComponent<SPR_YSorting>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        cutsceneIndex = 0;
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: false, OnBottom: false);
    }
}
