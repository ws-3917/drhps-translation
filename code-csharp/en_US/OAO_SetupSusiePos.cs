using System.Collections;
using UnityEngine;

public class OAO_SetupSusiePos : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetPos;

    private void Start()
    {
        StartCoroutine(SetupSusiePos());
    }

    private IEnumerator SetupSusiePos()
    {
        yield return null;
        ActivePartyMember activePartyMember = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie);
        activePartyMember.PartyMemberFollowSettings.FollowingEnabled = false;
        activePartyMember.PartyMemberFollowSettings.RotateSusieToDirection(Vector2.up);
        activePartyMember.PartyMemberTransform.position = targetPos;
    }

    public void EnableSusie()
    {
        PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie).PartyMemberFollowSettings.FollowingEnabled = true;
    }
}
