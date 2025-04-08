using System.Collections;
using UnityEngine;

public class EOTDRouxlsRoom_Puzzle : MonoBehaviour
{
    [SerializeField]
    private PushBlock_PreasurePlate[] PreasurePlates;

    [SerializeField]
    private int AmountRequired = 2;

    [SerializeField]
    private bool PuzzleComplete;

    [SerializeField]
    private bool ConfettiDrop;

    public bool hasMovedPartyMembers;

    [SerializeField]
    private bool PlayerInputDisable;

    [SerializeField]
    private AudioClip CompleteSound;

    [SerializeField]
    private ParticleSystem ConfettiParticle;

    [SerializeField]
    private INT_Chat AngryRouxlsChat;

    [SerializeField]
    private HypothesisGoal RouxlsPuzzleGoal;

    [SerializeField]
    private Vector2 SusieMovePos;

    [SerializeField]
    private Vector2 RalseiMovePos;

    private void Update()
    {
        if (!PuzzleComplete)
        {
            int num = 0;
            PushBlock_PreasurePlate[] preasurePlates = PreasurePlates;
            for (int i = 0; i < preasurePlates.Length; i++)
            {
                if (preasurePlates[i].Complete)
                {
                    num++;
                }
            }
            if (num == AmountRequired)
            {
                CompletePuzzle();
                PuzzleComplete = true;
            }
        }
        else if (PlayerInputDisable)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
        if (ConfettiDrop)
        {
            ConfettiParticle.transform.position = ConfettiParticle.transform.position + Vector3.down * 10f * Time.deltaTime;
        }
    }

    private void CompletePuzzle()
    {
        PlayerManager.Instance.PlayerAudioSource.PlayOneShot(CompleteSound);
        ConfettiParticle.Play();
        PlayerInputDisable = true;
        StartCoroutine(DelayUntilConfettiMove());
        HypotheticalGoalManager.Instance.CompleteGoal(RouxlsPuzzleGoal);
    }

    private IEnumerator DelayUntilConfettiMove()
    {
        yield return new WaitForSeconds(3f);
        AngryRouxlsChat.RUN();
        ConfettiDrop = true;
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return null;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        DarkworldMenu.Instance.CanOpenMenu = true;
    }

    public void FUCKINGMOVE()
    {
        if (hasMovedPartyMembers)
        {
            return;
        }
        hasMovedPartyMembers = true;
        foreach (ActivePartyMember activePartyMember in PartyMemberSystem.Instance.ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.FollowingEnabled = false;
            activePartyMember.PartyMemberFollowSettings.currentState = Susie_Follower.MemberFollowerStates.Disabled;
            activePartyMember.PartyMemberFollowSettings.RotateSusieToDirection(Vector2.right);
        }
        StartCoroutine(FUCKINGMOVE_TIMED());
    }

    public void okaysorrycomeback()
    {
        foreach (ActivePartyMember activePartyMember in PartyMemberSystem.Instance.ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.FollowingEnabled = true;
            activePartyMember.PartyMemberFollowSettings.AnimationOverriden = false;
            activePartyMember.PartyMemberFollowSettings.SusieAnimator.SetBool("InCutscene", value: false);
            activePartyMember.PartyMemberFollowSettings.ClearAllMovementHistory();
            activePartyMember.PartyMemberFollowSettings.currentState = Susie_Follower.MemberFollowerStates.SettingUpPosition;
        }
    }

    private IEnumerator FUCKINGMOVE_TIMED()
    {
        ActivePartyMember susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld);
        ActivePartyMember ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei);
        while ((Vector2)susie.PartyMemberTransform.position != SusieMovePos || (Vector2)ralsei.PartyMemberTransform.position != RalseiMovePos)
        {
            yield return null;
            susie.PartyMemberTransform.position = Vector2.MoveTowards(susie.PartyMemberTransform.position, SusieMovePos, 6f * Time.deltaTime);
            susie.PartyMemberFollowSettings.AnimationOverriden = true;
            susie.PartyMemberFollowSettings.SusieAnimator.Play("Idle");
            susie.PartyMemberFollowSettings.SusieAnimator.SetBool("InCutscene", value: true);
            ralsei.PartyMemberTransform.position = Vector2.MoveTowards(ralsei.PartyMemberTransform.position, RalseiMovePos, 6f * Time.deltaTime);
            ralsei.PartyMemberFollowSettings.AnimationOverriden = true;
            ralsei.PartyMemberFollowSettings.SusieAnimator.Play("Idle");
            ralsei.PartyMemberFollowSettings.SusieAnimator.SetBool("InCutscene", value: true);
        }
    }

    public void EndCutscene()
    {
        PlayerInputDisable = false;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        DarkworldMenu.Instance.CanOpenMenu = true;
        okaysorrycomeback();
    }
}
