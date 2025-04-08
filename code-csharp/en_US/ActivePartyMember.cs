using System;
using UnityEngine;

[Serializable]
public class ActivePartyMember
{
    [Header("- Overworld -")]
    public PartyMember PartyMemberDescription;

    public Transform PartyMemberTransform;

    public Susie_Follower PartyMemberFollowSettings;

    [Header("- Stats -")]
    public int CurrentHealth;
}
