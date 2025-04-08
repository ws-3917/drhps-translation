using System.Collections;
using UnityEngine;

public class Trailer_Chaos_NeoRuins : MonoBehaviour
{
    [SerializeField]
    private Transform SusieStartPos;

    [SerializeField]
    private Transform RalseiStartPos;

    [SerializeField]
    private PartyMember Susie;

    [SerializeField]
    private PartyMember Ralsei;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip puzzleComplete;

    [SerializeField]
    private AudioClip BridgeOpen;

    [SerializeField]
    private Animator BridgeAnimator;

    [SerializeField]
    private TheRoaringTitans_EyePuzzle_Eye[] Eyes;

    [SerializeField]
    private bool EyePuzzleComplete;

    private void Start()
    {
        PlayerManager.Instance._PAnimation.FootstepsEnabled = true;
        StartCoroutine(LateStart());
    }

    private void Update()
    {
        bool flag = true;
        TheRoaringTitans_EyePuzzle_Eye[] eyes = Eyes;
        for (int i = 0; i < eyes.Length; i++)
        {
            if (!eyes[i].EyeOn)
            {
                flag = false;
            }
        }
        if (flag && !EyePuzzleComplete)
        {
            EyePuzzleComplete = true;
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            StartCoroutine(EyeFinish());
        }
    }

    private IEnumerator EyeFinish()
    {
        source.PlayOneShot(puzzleComplete);
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.right);
        source.PlayOneShot(BridgeOpen);
        BridgeAnimator.Play("Trailer_Chaos_NeoPuzzle_BridgeOpen");
        yield return new WaitForSeconds(0.5f);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1f);
        ActivePartyMember activePartyMember = PartyMemberSystem.Instance.HasMemberInParty(Ralsei);
        ActivePartyMember activePartyMember2 = PartyMemberSystem.Instance.HasMemberInParty(Susie);
        activePartyMember.PartyMemberFollowSettings.FollowingEnabled = false;
        activePartyMember2.PartyMemberFollowSettings.FollowingEnabled = false;
        activePartyMember.PartyMemberFollowSettings.RotateSusieToDirection(Vector2.right);
        activePartyMember2.PartyMemberFollowSettings.RotateSusieTowardsPosition(Vector2.right);
        activePartyMember.PartyMemberTransform.position = RalseiStartPos.position;
        activePartyMember2.PartyMemberTransform.position = SusieStartPos.position;
    }
}
