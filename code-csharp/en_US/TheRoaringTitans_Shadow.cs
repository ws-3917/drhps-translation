using UnityEngine;

public class TheRoaringTitans_Shadow : MonoBehaviour
{
    [SerializeField]
    private bool TargetPlayer;

    [SerializeField]
    private PartyMember TargetPartyMember;

    private Transform TargetTransform;

    private void LateUpdate()
    {
        if (TargetTransform != null)
        {
            base.transform.position = TargetTransform.position;
            return;
        }
        if (TargetPlayer)
        {
            TargetTransform = PlayerManager.Instance.PlayerSpriteRenderer.transform;
            return;
        }
        ActivePartyMember activePartyMember = PartyMemberSystem.Instance.HasMemberInParty(TargetPartyMember);
        if (activePartyMember != null)
        {
            TargetTransform = activePartyMember.PartyMemberFollowSettings.SusieAnimator.transform;
        }
    }
}
